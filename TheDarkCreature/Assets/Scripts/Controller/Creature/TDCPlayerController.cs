using UnityEngine;
using UnityEngine.EventSystems;
using FSM;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCPlayerController : TDCCreatureController
{
    #region Property

	private FSMManager m_FSMMamager;
	private TDCPlayerData m_PlayerData;
	private TDCInventory m_Inventory;

    #endregion

    #region Implementation Mono
	public override void Init ()
	{
		base.Init ();
	}

    public override void Start()
	{
		base.Start ();

        m_FSMMamager    = new FSMManager();
		m_Inventory 	= TDCInventory.GetInstance ();

		var idleState   = new FSMIdleState(this);
		var moveState   = new FSMMoveState(this);
		var attackState = new FSMAttackState (this);
		var waitingState = new FSMWaitingState (this);
		
		m_FSMMamager.RegisterState("IdleState", idleState);
		m_FSMMamager.RegisterState("MoveState", moveState);
		m_FSMMamager.RegisterState("AttackState", attackState);
		m_FSMMamager.RegisterState("WaitingState", waitingState);
		
		m_FSMMamager.RegisterCondition("CanMove", CanMove);
		m_FSMMamager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMMamager.RegisterCondition("HaveEnemy", HaveEnemy);

        m_FSMMamager.LoadFSM(m_PlayerData.FSMPath);

	}
	
	void LateUpdate () {
		m_FSMMamager.UpdateState();
#if UNITY_EDITOR
		StateName = m_FSMMamager.StateCurrentName;
		if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 100f)) {
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
				break;
			case TouchPhase.Moved:
				break;
			case TouchPhase.Ended:
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo, 100f)) {
                    if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
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

    public override void OnSelectedItem (TDCItemEntity item)
	{
		base.OnSelectedItem (item);
        var itemType = item.GetData().ItemType;
        item.GetData().Amount--;
        item.ExcuteItem();
	}

    private bool CanMove() {
		return true;
	}
	
	private bool MoveToTarget()
	{
		var distance = (TransformPosition - GetTargetPosition ()).sqrMagnitude;
		return distance < 0.5f; 
	}

	private bool HaveEnemy() {
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
		m_PlayerData = value as TDCPlayerData;
		TDCInventory.Instance.SetPlayer (this);
	}

    #endregion

}
