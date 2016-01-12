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

    #region Implementation Mono
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

		UIManager.Instance.Init(m_Entity);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		m_FSMManager.UpdateState();
	}

	protected override void LateUpdate () {
#if UNITY_EDITOR
		if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo)) {
				if (!EventSystem.current.IsPointerOverGameObject()) { 
					PlayerAction(hitInfo);
				}
            }
        }
#elif UNITY_ANDROID
		if (Input.touchCount == 1) {
			var touchPhase = Input.GetTouch (0).phase;
			switch (touchPhase) {
			case TouchPhase.Began:
				m_TouchedUI = !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
				break;
			case TouchPhase.Moved:
				break;
			case TouchPhase.Ended:
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo)) {
					if (m_TouchedUI)
                    {
						PlayerAction(hitInfo);
                    }
				}
				break;
			}
		}
#endif
    }

    #endregion

    #region Main method

	public override void ActiveSkill(int index)
	{
		base.ActiveSkill(index);
		m_Entity.ActiveSkill(index);
	}

	private void PlayerAction(RaycastHit hitInfo) {
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
				case (int) TDCEnum.ELayer.LayerGObject: {
					SetEnemyEntity(entity);
					break;
				}
			}
		}
	}

	public override void OnSelectedItem (int itemIndex)
	{
		base.OnSelectedItem (itemIndex);
		var item = m_Entity.GetItemInventory()[itemIndex];
		item.ExcuteItem();
		if (item.GetData().Amount == 0)
		{
			m_Inventory.RemoveItem(itemIndex);
			m_Entity.GetItemInventory()[itemIndex] = null;
		}
	}

	public override int AddItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount)
	{
		var inventory = m_Entity.GetItemInventory();
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

	#endregion

	#region FSM

	internal override bool IsDeath()
	{
		return base.IsDeath() || m_Entity.GetHunger() < 1;
	}

	#endregion
}
