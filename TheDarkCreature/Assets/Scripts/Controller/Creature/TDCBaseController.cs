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

	protected TDCEntity m_Entity;
	protected FSMManager m_FSMManager;
	protected Transform m_Transform;
	protected CapsuleCollider m_Collider;
	protected TDCGameManager m_GameManager;
	protected float m_WaitingTime = 3f;

    public Vector3 TransformPosition {
		get { return m_Transform.position; }
		set { m_Transform.position = value; }
	}

	public Quaternion TransformRotation {
		get { return m_Transform.rotation; }
		set { m_Transform.rotation = value; }
	}

	#endregion

	#region Implement Monobehaviour

	public virtual void Init() {
		m_Transform	= this.transform;
		SetStartPosition (m_Transform.position);
		SetTargetPosition (m_Transform.position);

		m_Collider 		= this.GetComponent<CapsuleCollider> ();
		m_GameManager 	= TDCGameManager.GetInstance();
		m_FSMManager 	= new FSMManager();

		var waiting 		= new FSMWaitingState (this);
		var waitingOne 		= new FSMWaitingOneSecondState (this);
		var waitingOne2Three = new FSMWaitingOne2ThreeSecondState (this);

		m_FSMManager.RegisterState("WaitingState", waiting);
		m_FSMManager.RegisterState("WaitingOneSecondState", waitingOne);
		m_FSMManager.RegisterState("WaitingOne2ThreeSecondState", waitingOne2Three);

		m_FSMManager.RegisterCondition("IsActive", IsActive);
		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
	}

	protected virtual void OnEnable() {
		
	}

	protected virtual void OnDisable() {
	
	}

	protected virtual void Start()
	{
		
    }

	protected virtual void FixedUpdate() {
		
	}

	protected virtual void Update() {
		
	}

	protected virtual void LateUpdate() {
		
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
	}

	#endregion

	#region Main Method

	public virtual void ActiveSkill(int index) {
		
	}

	public virtual void MovePosition(Vector3 position) {

	}

	public virtual void FlyPosition(Vector3 position) {

	}

	public virtual void LandingPosition(Vector3 position) {

	}
	
	public virtual void LookAtRotation(Vector3 rotation) {

	}

	public virtual void ApplyDamage(int damage, TDCEntity attacker) {
		
	}

	public virtual void ResetObject() {
	
	}

	public virtual void ReturnObject(TDCEntity obj) {
		m_GameManager.SetObjectPool(obj);
	}

	public virtual void CallBackEvent(string name) {
		m_Entity.CallBackEvent(name);
	}

	#endregion
		
	#region FSM

	internal virtual bool IsActive()
	{
		return GetActive();
	}

	internal virtual bool IsDeath() {
		return GetActive() == false;
	}

	internal virtual bool CountdownWaitingTime() {
		m_WaitingTime -= Time.deltaTime;
		return m_WaitingTime <= 0;     
	}

	internal virtual bool IsEnemyDeath()
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

	public virtual void SetEntity(TDCEntity entity) {
		m_Entity = entity;
	}

	public virtual TDCEntity GetEntity() {
		return m_Entity;
	}

	public virtual int GetDamage() {
		return 0;
	}
	public virtual float GetColliderRadius() {
		if (m_Collider == null)
			return 0;
		return m_Collider.radius;
	}

	public virtual void SetActive(bool value) {
		m_Entity.SetActive(value);
    }

    public virtual bool GetActive() {
		return m_Entity.GetActive();
    }

	public virtual void SetStartPosition(Vector3 pos) {
		m_Entity.SetStartPosition(pos);
		this.transform.position = pos;
	}

	public virtual Vector3 GetStartPosition() {
		return m_Entity.GetStartPosition();
	}

	public virtual TDCEnum.EGameType GetGameType() {
		return m_Entity.GetGameType();
	}
	
	public virtual void SetWaitingTime(float time) {
		m_WaitingTime = time;
	}
	
	public virtual Vector3 GetEnemyPosition() {
		return m_Entity.GetEnemyPosition();
	}
	
	public virtual void SetEnemyEntity(TDCEntity entity)
	{
		m_Entity.SetEnemyEntity(entity);
	}
	
	public virtual TDCEntity GetEnemyEntity()
	{
		return m_Entity.GetEnemyEntity();
	}
	
	public virtual void SetAnimation(EAnimation anim) {
		
	}
	
	public virtual Vector3 GetTargetPosition() {
		return m_Entity.GetTargetPosition();
	}
	
	public virtual void SetTargetPosition(Vector3 pos) {
		m_Entity.SetTargetPosition (pos);
	}
	
	public virtual float GetDetectEnemyRange() {
		return m_Entity.GetDetectEnemyRange();
	}

	public virtual void SetGroupEntity(TDCEntity group) {
		m_Entity.SetGroupEntity (group);
	}

	public virtual TDCEntity GetGroupEntity() {
		return m_Entity.GetGroupEntity();
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

	public virtual float GetMoveSpeed() {
		return 0f;
	}

	#endregion
}