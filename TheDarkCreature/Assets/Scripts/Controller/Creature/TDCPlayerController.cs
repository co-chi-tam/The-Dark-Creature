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

	private TDCPlayerData m_PlayerData;
	private UIInventory m_Inventory;
	private bool m_TouchedUI = false;

	[SerializeField]
	private float HeatPoint = 0f;

    #endregion

    #region Implementation Mono
	public override void Init ()
	{
		base.Init ();
	}

    public override void Start()
	{
		base.Start ();
			
		m_Inventory 	= UIInventory.GetInstance ();

		m_FSMManager.RegisterCondition("HaveEnemy", HaveEnemy);

        m_FSMManager.LoadFSM(m_PlayerData.FSMPath);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		m_FSMManager.UpdateState();
		StateName = m_FSMManager.StateCurrentName;
	}

	public override void LateUpdate () {
		HeatPoint = m_PlayerData.CurrentHeatPoint;
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

	private void PlayerAction(RaycastHit hitInfo) {
		var hitGameObject = hitInfo.collider.gameObject;
		var controller = hitGameObject.GetComponent<TDCBaseController>();
		var point = hitInfo.point;
		point.y = 0f;
		switch (hitGameObject.layer) {
		case (int) TDCEnum.ELayer.LayerPlane: {
			SetTargetPosition (point);
			SetEnemyController (null);
		} break;
		case (int) TDCEnum.ELayer.LayerEnviroment: {
			SetTargetPosition (point);
			SetEnemyController (controller);
		} break;
		case (int) TDCEnum.ELayer.LayerCreature: {
			
		} break;
		}
	}

	public override void OnSelectedItem (int itemIndex)
	{
		base.OnSelectedItem (itemIndex);
		var item = m_CreatureData.Inventory[itemIndex];
		item.ExcuteItem();
		if (item.GetData().Amount == 0)
		{
			m_Inventory.RemoveItem(itemIndex);
			m_CreatureData.Inventory[itemIndex] = null;
		}
	}

	public override int AddItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount)
	{
		var itemIndex = base.AddItem(gameType, itemType, amount);
		if (itemIndex != -1)
		{
			var item = GetInventory()[itemIndex];
			m_Inventory.AddItem(itemIndex, item);
		}

		return itemIndex;
	}

	#endregion

	#region FSM

	internal override bool CanMove() {
		return true;
	}
	
	internal override bool MoveToTarget()
	{
		if (GetEnemyController() == null)
		{
			var distance = (TransformPosition - GetTargetPosition()).sqrMagnitude;
			return distance < 0.5f * 0.5f; 
		}
		else
		{
			var distance = (TransformPosition - GetEnemyPosition()).sqrMagnitude;
			var range = GetEnemyController().GetColliderRadius() + m_CreatureData.AttackRange;
			return distance < range * range; 
		}
	}

	internal override bool HaveEnemy() {
		base.HaveEnemy();
		var enemy = this.GetEnemyController ();
		return enemy != null && enemy.GetActive();
	}

    #endregion

    #region Getter & Setter

	public override TDCBaseData GetData ()
	{
		return m_PlayerData;
	}

	public override void SetData(TDCBaseData value) {
		base.SetData (value);
		m_PlayerData = m_CreatureData as TDCPlayerData;
		UIInventory.Instance.SetPlayer (this);
	}

    #endregion

}
