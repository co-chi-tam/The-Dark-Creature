using UnityEngine;
using UnityEngine.EventSystems;
using FSM;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;

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

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	protected override void Start()
	{
		base.Start ();
			
		m_Inventory 	= UIInventory.GetInstance ();

		m_Inventory.SetPlayer(this);

		m_SkillSlot = new TDCSkillSlot(TDCEnum.EGameType.NormalMeleeSkill, this.GetEntity());
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		m_FSMManager.UpdateState();
		StateName = m_FSMManager.StateCurrentName;

		m_SkillSlot.UpdateSkill(Time.fixedDeltaTime);
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
		m_SkillSlot.ActiveSkill();
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
				case (int) TDCEnum.ELayer.LayerPlane:
					{
						SetTargetPosition(point);
						SetEnemyEntity(null);
					}
					break;
				case (int) TDCEnum.ELayer.LayerCreature:
				case (int) TDCEnum.ELayer.LayerEnviroment:
					{
						SetEnemyEntity(entity);
					}
					break;
				case (int) TDCEnum.ELayer.LayerGObject:
					{

					}
					break;
			}
		}
	}

	public override void OnSelectedItem (int itemIndex)
	{
		base.OnSelectedItem (itemIndex);
		var item = m_Entity.GetInventory()[itemIndex];
		item.ExcuteItem();
		if (item.GetData().Amount == 0)
		{
			m_Inventory.RemoveItem(itemIndex);
			m_Entity.GetInventory()[itemIndex] = null;
		}
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
				{
					if (indexItemInInventory != -1)
					{
						inventory[indexItemInInventory].GetData().Amount++;
						return indexItemInInventory;
					}
					else
					{
						inventory[emptySlot] = m_GameManager.CreateItem(gameType, itemType, this, amount);
					}
				}
				break;
			case TDCEnum.EItemType.GObject:
			case TDCEnum.EItemType.Weapon:
				{
					inventory[emptySlot] = m_GameManager.CreateItem(gameType, itemType, this, amount);
				}
				break;
		}
		if (emptySlot != -1)
		{
			var item = GetInventory()[emptySlot];
			m_Inventory.AddItem(emptySlot, item);
		}

		return emptySlot;
	}

	#endregion
}
