using UnityEngine;
using UnityEngine.EventSystems;
using FSM;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCPlayerController : TDCCreatureController
{
    #region Property

	private UIInventory m_Inventory;
	private bool m_TouchedUI = false;

    #endregion

    #region Implementation Monobehaviour

	public override void Init ()
	{
		base.Init ();

		var playerDeathState = new FSMPlayerDeathState(this);

		m_FSMManager.RegisterState("PlayerDeathState", playerDeathState);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	protected override void Start()
	{
		base.Start ();
			
		m_Inventory 	= UIInventory.GetInstance ();

		m_Inventory.SetPlayer(this);

		UIManager.Instance.Init(this);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		m_FSMManager.UpdateState();
	}

	protected override void LateUpdate () {
		base.LateUpdate();
#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo)) {
				if (!EventSystem.current.IsPointerOverGameObject()) { 
					ActiveAction(hitInfo);
				}
            }
        }
#elif UNITY_ANDROID
		if (Input.touchCount == 1) {
			var touchPhase = Input.GetTouch (0).phase;
			switch (touchPhase) {
			case TouchPhase.Began:
				m_TouchedUI = !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo)) {
					if (m_TouchedUI)
					{
						ActiveAction(hitInfo);
					}
				}
				break;
			case TouchPhase.Moved:
				break;
			case TouchPhase.Ended:
				break;
			}
		}
#endif
    }

    #endregion

    #region Main method

	public override void Activekill(int index)
	{
		base.Activekill(index);
		m_Entity.AddSkillChain(index);
	}

	private void ActiveAction(RaycastHit hitInfo) {
		var hitGameObject = hitInfo.collider.gameObject;
		var entity = m_GameManager.GetEntityByName(hitGameObject.name);
		if (entity != this.GetEntity())
		{
			var point = hitInfo.point;
			point.y = 0f;
			switch (hitGameObject.layer)
			{
				case (int) TDCEnum.ELayer.LayerPlane: {
					SetTargetPosition(point);
					SetEnemyEntity(null);
					break;
				}
				case (int) TDCEnum.ELayer.LayerCreature:
				case (int) TDCEnum.ELayer.LayerEnviroment:
				case (int) TDCEnum.ELayer.LayerGObject: 
				case (int) TDCEnum.ELayer.LayerItem: {
					SetEnemyEntity(entity);
					break;
				}
			}
		}
	}

	public override void ActiveAction(int index) {
		base.ActiveAction(index);
		if (GetEnemyEntity() != null)
			return;
		var colliders = Physics.OverlapSphere(m_Transform.position, GetDetectRange(), m_ColliderLayerMask);
		if (colliders.Length == 0)
			return;
		QuickSort.SimpleSort(colliders, (x, y) =>
			{
				var _xDist = (TransformPosition - x.transform.position).sqrMagnitude;
				var _yDist = (TransformPosition - y.transform.position).sqrMagnitude;
				return _xDist > _yDist ? 1 : _xDist < _yDist ? -1 : 0;
			});
			
		for (int i = 0; i < colliders.Length; i++)
		{
			var colliderObject = colliders[i].gameObject;
			var layer = colliderObject.layer;
			if (layer == (int) TDCEnum.ELayer.LayerPlane || 
				layer == (int) TDCEnum.ELayer.LayerGObject || 
				colliderObject.name == this.name)
				continue;
			var entity = m_GameManager.GetEntityByName(colliderObject.name);
			if (entity != this.GetEntity())
			{
				var point = colliderObject.transform.position;
				point.y = 0f;
				switch (layer)
				{
					case (int) TDCEnum.ELayer.LayerCreature:
						{
							if (index == 1)
							{
								SetEnemyEntity(entity);
								return;
							}
						}
						break;
					case (int) TDCEnum.ELayer.LayerEnviroment:
					case (int) TDCEnum.ELayer.LayerItem:
						{
							if (index == 0)
							{
								SetEnemyEntity(entity);
								return;
							}
						}
						break;
				}
			}
		}

	}

	public override void OnSelectedItem (int itemIndex)
	{
		base.OnSelectedItem (itemIndex);
		var item = m_Entity.GetInventory()[itemIndex];
		item.ExcuteItem();
	}

	public override int AddItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount)
	{
		var inventory = m_Entity.GetInventory();
		var indexItemInInventory = FindItemSlot(gameType);
		var emptySlot = FindEmptySlot();
		if (emptySlot == -1)
			return -1;
		switch (itemType)
		{
			case TDCEnum.EItemType.Food:
			case TDCEnum.EItemType.Item:
			case TDCEnum.EItemType.GObject: {
				if (indexItemInInventory != -1)
				{
					inventory[indexItemInInventory].GetData().Amount++;
					return indexItemInInventory;
				}
				else
				{
					inventory[emptySlot] = m_GameManager.CreateItem(gameType, itemType, this, amount);
				}
				break;
			}
			case TDCEnum.EItemType.Weapon: {
				inventory[emptySlot] = m_GameManager.CreateItem(gameType, itemType, this, amount);
				break;
			}
		}
		if (emptySlot != -1)
		{
			var item = GetInventory()[emptySlot];
			m_Inventory.AddItem(emptySlot, item);
		}

		return emptySlot;
	}

	#endregion

	#region Getter & Setter

	public override int GetSanity()
	{
		return m_Entity.GetSanity();
	}

	public override void SetSanity(int value)
	{
		m_Entity.SetSanity(value);
	}

	public override int GetMaxSanity()
	{
		return m_Entity.GetMaxSanity();
	}

	public override int GetHunger()
	{
		return m_Entity.GetHunger();
	}

	public override void SetHunger(int value)
	{
		m_Entity.SetHunger(value);
	}

	public override int GetMaxHunger()
	{
		return m_Entity.GetMaxHunger();
	}

	#endregion

	#region FSM

	internal override bool IsDeath()
	{
		return base.IsDeath() || m_Entity.GetHunger() < 1;
	}

	#endregion
}
