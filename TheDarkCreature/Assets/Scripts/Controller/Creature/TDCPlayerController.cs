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
	private TDCGameManager m_GameManager;

	private Dictionary<string, Action<object>> m_EffectEvents;

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
		
		m_FSMMamager.RegisterState("IdleState", idleState);
		m_FSMMamager.RegisterState("MoveState", moveState);
		
		m_FSMMamager.RegisterCondition("CanMove", CanMove);
		m_FSMMamager.RegisterCondition("MoveToTarget", MoveToTarget);

        m_FSMMamager.LoadFSM(m_PlayerData.FSMPath);

		m_EffectEvents = new Dictionary<string, Action<object>> ();
		m_EffectEvents["Default"] = UsedDefaultItem;
		m_EffectEvents["IncreaseHealthPoint"] = IncreaseHealthPoint;
		m_EffectEvents["DecreaseHealthPoint"] = DecreaseHealthPoint;
		m_EffectEvents["CreateObject"] = CreateObject;

		m_GameManager = TDCGameManager.GetInstance ();
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

	private void UsedDefaultItem(object value) {
		// TODO
	}

	private void IncreaseHealthPoint(object value) {
		Debug.LogError (value);
	}

	private void DecreaseHealthPoint(object value) {
		
	}

	private void CreateObject(object value) {
		var gameType = (TDCEnum.EGameType)(int.Parse (value.ToString ()));
		m_GameManager.CreateCreature (gameType, this.transform.position, Quaternion.identity);
	}

	private void PlayerAction(RaycastHit hitInfo) {
		var hitGameObject = hitInfo.collider.gameObject;
		var controller = hitGameObject.GetComponent<TDCBaseController>();
//		Debug.LogError (hitGameObject.layer);
		switch (hitGameObject.layer) {
		case (int) TDCEnum.ELayer.LayerPlane: {
			SetTargetPosition (hitInfo.point);
		} break;
		case (int) TDCEnum.ELayer.LayerEnviroment: {

		} break;
		case (int) TDCEnum.ELayer.LayerCreature: {
			
		} break;
		}
	}

	public override void OnSelectedItem (TDCItemData item)
	{
		base.OnSelectedItem (item);
		var itemType = item.ItemType;
		switch (itemType) {
		case TDCEnum.EItemType.Food: 
		case TDCEnum.EItemType.Item: 
		{
			var itemData = item as TDCFoodData;
			itemData.Amount --;
			if (itemData.Amount == 0) {
				m_Inventory.RemoveItem (itemData);
			}
			break;
		}
		case TDCEnum.EItemType.Weapon:{
			var weapon = item as TDCWeaponData;
			weapon.Duration -= weapon.DecreaseDuration;
			if (weapon.Duration == 0) {
				weapon.Amount --;
				if (weapon.Amount == 0) {
					m_Inventory.RemoveItem (weapon);
				}
			}
			break;
		}
		case TDCEnum.EItemType.GObject: {
			var gObject = item;
//			gObject.Amount --;
//			if (gObject.Amount == 0) {
				m_Inventory.RemoveItem (gObject);
//			}
		} break;
		}
		m_EffectEvents [item.EffectName] (item.EffectValue);
	}

    private bool CanMove() {
		return true;
	}
	
	private bool MoveToTarget()
	{
		return (TransformPosition - GetTargetPosition()).sqrMagnitude < 0.5f; 
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
