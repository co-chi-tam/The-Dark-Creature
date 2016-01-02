using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

public enum EAnimation:int {
	Idle    = 0,
	Walk    = 1,
	Run     = 2,
	Action1 = 3,
	Action2 = 4,
	Attack1 = 5,
	Attack2 = 6,
    Hiden   = 7,
	// TODO
	Death     = 10
}

public class TDCBaseController : TDCMonoBehaviour
{
	#region Property
	[SerializeField]
	protected string StateName = "";

	protected FSMManager m_FSMManager;
	protected Transform m_Transform;
	protected Vector3 m_StartPosition;
	protected bool m_IsActive = false;
	protected float m_WaitingTimeInterval = 3f;
	protected TDCBaseGroupController m_GroupController;
	protected CapsuleCollider m_Collider;
	protected TDCBaseData m_BaseData;
	protected Vector3 m_TargetPosition;
	protected Dictionary<string, Action> m_TriggerEvents;
	protected TDCBaseController m_EnemyController;
	protected TDCGameManager m_GameManager;

    public Vector3 TransformPosition {
		get { return m_Transform.position; }
		set { m_Transform.position = value; }
	}

	public Quaternion TransformRotation {
		get { return m_Transform.rotation; }
		set { m_Transform.rotation = value; }
	}

	#endregion

	#region Event 

	public event Action OnIdleEvent;
	public event Action OnFindRandomEvent;
	public event Action OnMoveEvent;
	public event Action OnApplyDamageEvent;
	public event Action OnHealthPoint50Event;
	public event Action OnHealthPoint25Event;
	public event Action OnAttackEvent;
	public event Action OnAvoidEvent;
	public event Action OnAliveEvent;
	public event Action OnDeathEvent;

	#endregion

	#region Implement Monobehaviour

	public virtual void Init() {
		m_Transform	= this.transform;
		m_StartPosition = m_Transform.position;
		m_TargetPosition = m_StartPosition;
		m_TriggerEvents = new Dictionary<string, Action>();
		m_Collider = this.GetComponent<CapsuleCollider> ();
		m_GameManager = TDCGameManager.GetInstance();
		m_FSMManager = new FSMManager();
		SetActive (true);
		LoadEventCallBack();
	}

	protected virtual void OnEnable() {
		
	}

	protected virtual void OnDisable() {
	
	}

	protected virtual void Start()
	{
		var waiting 		= new FSMWaitingState (this);
		var waitingOne 		= new FSMWaitingOneSecondState (this);
		var waitingOne2Three = new FSMWaitingOne2ThreeSecondState (this);

		m_FSMManager.RegisterState("WaitingState", waiting);
		m_FSMManager.RegisterState("WaitingOneSecondState", waitingOne);
		m_FSMManager.RegisterState("WaitingOne2ThreeSecondState", waitingOne2Three);

		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
    }

	protected virtual void Update() {
		if (GetActive())
		{
			CallBackEvent("OnAlive");
		}
	}

	protected virtual void LateUpdate() {
		
	}

	protected virtual void FixedUpdate() {
		if (!HaveEnemy())
		{
			SetEnemyController(null);
		}
	}

	public virtual void OnBecameVisible() {
		
	}

	public virtual void OnBecameInvisible() {
			
	}

	protected virtual void OnDestroy() {
		
	}

	protected virtual void OnApplicationQuit() {
		
	}

	protected virtual void OnApplicationFocus(bool focus) {
		
	}

	protected virtual void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere (TransformPosition, GetColliderRadius());
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (TransformPosition, GetDetectEnemyRange());
	}

	#endregion

	#region Main Method

	public virtual void ActiveSkill(int index) {
		
	}

	public virtual void MovePosition(Vector3 position) {

	}
	
	public virtual void LookAtRotation(Vector3 rotation) {

	}

	public virtual void ApplyDamage(int damage, TDCBaseController attacker) {
		CallBackEvent("OnApplyDamage");
	}

	public virtual void ResetObject() {
	
	}

	public virtual void CallBackEvent(string name) {
		if (m_TriggerEvents.ContainsKey(name))
		{
			var evnt = m_TriggerEvents[name];
			if (evnt != null)
			{
				evnt.Invoke();
			}
		}
	}

	public virtual bool AddEventListener(string name, Action evnt) {
		if (m_TriggerEvents.ContainsKey(name))
		{
			m_TriggerEvents[name] += evnt;
			return true;
		}
		return false;
	}

	public virtual bool RemoveEventListener(string name, Action evnt) {
		if (m_TriggerEvents.ContainsKey(name))
		{
			m_TriggerEvents[name] -= evnt;
			return true;
		}
		return false;
	}

	protected virtual void LoadEventCallBack() {
		m_TriggerEvents.Add("OnIdle", OnIdleEvent);
		m_TriggerEvents.Add("OnFindRandom", OnFindRandomEvent);
		m_TriggerEvents.Add("OnMove", OnMoveEvent);
		m_TriggerEvents.Add("OnApplyDamage", OnApplyDamageEvent);
		m_TriggerEvents.Add("OnHealthPoint50", OnHealthPoint50Event);
		m_TriggerEvents.Add("OnHealthPoint25", OnHealthPoint25Event);
		m_TriggerEvents.Add("OnAttack", OnAttackEvent);
		m_TriggerEvents.Add("OnAvoid", OnAvoidEvent);
		m_TriggerEvents.Add("OnAlive", OnAliveEvent);
		m_TriggerEvents.Add("OnDeath", OnDeathEvent);
	}

	public virtual void ReturnObject() {
		
	}

	#endregion
		
	#region FSM

	internal virtual bool CountdownWaitingTime() {
		m_WaitingTimeInterval -= Time.deltaTime;
		return m_WaitingTimeInterval <= 0;     
	}

	internal virtual bool HaveEnemy()
	{
		return false;  
	}

	internal virtual bool MoveToTarget()
	{
		return true;  
	}

	internal virtual bool MoveToEnemy()
	{
		return true;
	}

	#endregion

    #region Getter & Setter

	public virtual int GetMinDamage() {
		return 0;
	}

	public virtual int GetMaxDamage() {
		return 0;
	}

	public virtual void SetTriggerEvent(string name, Action evnt) {
		m_TriggerEvents.Add(name, evnt);
	}

	public virtual Action GetTriggerEvent(string name) {
		return m_TriggerEvents[name];
	}

	public virtual float GetColliderRadius() {
		if (m_Collider == null)
			return 0;
		return m_Collider.radius;
	}

	public virtual void SetData(TDCBaseData data) {
		m_BaseData = data;
	}

	public virtual void SetActive(bool value) {
		m_IsActive = value;
        gameObject.SetActive(value);
    }

    public virtual bool GetActive() {
		return m_IsActive;
    }

	public virtual void SetStartPosition(Vector3 pos) {
		m_StartPosition = pos;
		this.transform.position = pos;
	}

	public virtual Vector3 GetStartPosition() {
		return m_StartPosition;
	}

	public virtual TDCEnum.EGameType GetGameType() {
		return m_BaseData.GameType;
	}
	
	public virtual void SetWaitingTimeInterval(float time) {
		m_WaitingTimeInterval = time;
	}
	
	public virtual TDCBaseData GetData() {
		return m_BaseData;
	}
	
	public virtual void SetCanMove(bool value) {

	}
	
	public virtual bool GetCanMove() {
		return false;
	}
	
	public virtual Vector3 GetEnemyPosition() {
		return m_EnemyController.TransformPosition;
	}
	
	public virtual void SetEnemyController(TDCBaseController controller)
	{
		m_EnemyController = controller;
	}
	
	public virtual TDCBaseController GetEnemyController()
	{
		return m_EnemyController;
	}
	
	public virtual void SetAnimation(EAnimation anim) {

	}
	
	public virtual Vector3 GetTargetPosition() {
		return m_TargetPosition;
	}
	
	public virtual void SetTargetPosition(Vector3 pos) {
		m_TargetPosition = pos;
	}
	
	public virtual float GetDetectEnemyRange() {
		return 0f;
	}

	public virtual void SetGroupController(TDCBaseGroupController group) {
		m_GroupController = group;
	}

	public virtual TDCBaseGroupController GetGroupController() {
		return m_GroupController;
	}

	public virtual int GetHealth() {
		return 0;
	}

	public virtual void SetHealth(int value) {

	}

	public virtual int GetMaxHealth() {
		return 0;
	}

	public virtual void SetHeat(int value) {

	}

	public virtual void SetHunger(int value) {

	}

	public virtual void SetSanity(int value) {

	}

	public virtual TDCItemController[] GetInventory() {
		return null;
	}

	#endregion
}