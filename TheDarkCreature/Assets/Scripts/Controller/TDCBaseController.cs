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
    Sleep   = 7,
	// TODO
	Death     = 10
}

public class TDCBaseController : TDCMonoBehaviour, IContext
{
	#region Property

	protected TDCEntity m_Entity;
	protected FSMManager m_FSMManager;
	protected Transform m_Transform;
	protected Animator m_AnimatorController;
	protected CapsuleCollider m_Collider;
	protected TDCGameManager m_GameManager;
	protected float m_WaitingTime = 3f;

	public virtual Vector3 TransformPosition {
		get { return m_Transform.position; }
		set { m_Transform.position = value; }
	}

	public virtual Quaternion TransformRotation {
		get { return m_Transform.rotation; }
		set { m_Transform.rotation = value; }
	}

	protected Dictionary<string, object> m_ObjectCurrentvalue;

	#endregion

	#region Implement Monobehaviour

	public virtual void Init() {
		m_Transform	= this.transform;
		SetStartPosition (m_Transform.position);
		SetTargetPosition (m_Transform.position);

		m_AnimatorController = this.GetComponent<Animator> ();
		m_Collider 			 = this.GetComponent<CapsuleCollider> ();
		m_ObjectCurrentvalue = new Dictionary<string, object>();

		m_GameManager 	= TDCGameManager.GetInstance();
		m_FSMManager 	= new FSMManager();

		var waiting 		= new FSMWaitingState (this);
		var waitingOne 		= new FSMWaitingOneSecondState (this);
		var waitingOne2Three = new FSMWaitingOne2ThreeSecondState (this);
		var waitingThree2Five = new FSMWaitingThree2FiveSecondState (this);
		var sleepState 		= new FSMSleepState(this);
		var dayModeState	= new FSMDayModeState(this);
		var nightModeState	= new FSMNightModeState(this);

		m_FSMManager.RegisterState("WaitingState", waiting);
		m_FSMManager.RegisterState("WaitingOneSecondState", waitingOne);
		m_FSMManager.RegisterState("WaitingOne2ThreeSecondState", waitingOne2Three);
		m_FSMManager.RegisterState("WaitingThree2FiveSecondState", waitingThree2Five);
		m_FSMManager.RegisterState("DayModeState", dayModeState);
		m_FSMManager.RegisterState("NightModeState", nightModeState);
		m_FSMManager.RegisterState("SleepState", sleepState);

		m_FSMManager.RegisterCondition("IsActive", IsActive);
		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
		m_FSMManager.RegisterCondition("IsDayTime", TDCDateTime.IsDayTime);
		m_FSMManager.RegisterCondition("IsMidDayTime", TDCDateTime.IsMidDayTime);
		m_FSMManager.RegisterCondition("IsNightTime", TDCDateTime.IsNightTime);
		m_FSMManager.RegisterCondition("IsMidNightTime", TDCDateTime.IsMidNightTime);
		m_FSMManager.RegisterCondition("IsSpring", IsSpring);
		m_FSMManager.RegisterCondition("IsSummer", IsSummer);
		m_FSMManager.RegisterCondition("IsAutumn", IsAutumn);
		m_FSMManager.RegisterCondition("IsWinter", IsWinter);
		m_FSMManager.RegisterCondition("IsRainy", IsRainy);
		m_FSMManager.RegisterCondition("IsOverHeat", IsOverHeat);
		m_FSMManager.RegisterCondition("IsWindy", IsWindy);
		m_FSMManager.RegisterCondition("IsSnowy", IsSnowy);
	}

	protected virtual void OnEnable() {
		if (m_AnimatorController != null)
		{
			m_AnimatorController.enabled = true;
		}
		if (m_Collider != null)
		{
			m_Collider.enabled = true;
		}
	}

	protected virtual void OnDisable() {
		if (m_AnimatorController != null)
		{
			m_AnimatorController.enabled = false;
		}
		if (m_Collider != null)
		{
			m_Collider.enabled = false;
		}
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
		m_Entity.VisibleObject(true);
	}

	public virtual void OnBecameInvisible() {
		m_Entity.VisibleObject(false);
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

	public virtual Dictionary<string, object> GetObjectCurrentValue() {
		m_ObjectCurrentvalue["Active"] = GetActive();
		m_ObjectCurrentvalue["State Name"] = m_FSMManager != null ? m_FSMManager.StateCurrentName : "None";
		m_ObjectCurrentvalue["HealthPoint"] = m_Entity.GetHealth();
		m_ObjectCurrentvalue["HeatPoint"] = m_Entity.GetHeat();
		m_ObjectCurrentvalue["SanityPoint"] = m_Entity.GetSanity();
		m_ObjectCurrentvalue["HungerPoint"] = m_Entity.GetHunger();
		m_ObjectCurrentvalue["Enemy Name"] = GetEnemyEntity() != null ? GetEnemyEntity().GetController().name : "None";
		m_ObjectCurrentvalue["Group Name"] = m_Entity.GetGroupEntity() != null ? m_Entity.GetGroupEntity().GetController().name : "None";
		return m_ObjectCurrentvalue;
	}

	public virtual void Activekill(int index) {

	}

	public virtual void ActiveAction(int index) {

	}

	public virtual void MovePosition(Vector3 position) {
		CallBackEvent("OnMove");
		if (m_Entity.GetVisibleObject() == false)
		{
			VisibleObject(true);
		}
	}

	public virtual void FlyPosition(Vector3 position) {
		CallBackEvent("OnMove");
		if (m_Entity.GetVisibleObject() == false)
		{
			VisibleObject(true);
		}
	}

	public virtual void LandingPosition(Vector3 position) {

	}
	
	public virtual void LookAtRotation(Vector3 rotation) {

	}

	public virtual void ApplyDamage(int damage, TDCEntity attacker) {
		
	}

	public virtual void ResetObject() {
	
	}

	public virtual void VisibleObject(bool value) {
		m_Entity.VisibleObject(value);
		var childCount = m_Transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			m_Transform.GetChild(i).gameObject.SetActive(value);
		}
	}

	public virtual int AddItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount) {
		return 0;
	}

	public virtual void DropItem() {

	}

	public virtual void ReturnObject() {
		
	}

	public virtual void CallBackEvent(string name) {
		m_Entity.CallBackEvent(name);
	}

	public virtual TDCEntity SpawnMember() {
		return null;
	}

	public virtual void IsActiveSeasonSpring() {
		
	}

	public virtual void IsActiveSeasonSummer() {
		
	}

	public virtual void IsActiveSeasonAutumn() {
		
	}

	public virtual void IsActiveSeasonWinter() {
		
	}

	public virtual void StartSkill() {
		
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
		return m_WaitingTime <= 0f;     
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

	internal virtual bool IsSpring() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Spring);
	}

	internal virtual bool IsSummer() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Summer);
	}

	internal virtual bool IsAutumn() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Autumn);
	}

	internal virtual bool IsWinter() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Winter);
	}

	internal virtual bool IsRainy() {
		return TDCDateTime.IsRainy();
	}

	internal virtual bool IsOverHeat() {
		return TDCDateTime.IsOverHeat();
	}

	internal virtual bool IsWindy() {
		return TDCDateTime.IsWindy();
	}

	internal virtual bool IsSnowy() {
		return TDCDateTime.IsSnowy();
	}

	#endregion

    #region Getter & Setter

	public virtual string GetStateName() {
		return string.Empty;
	}

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
		this.gameObject.SetActive(value);
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
	
	public virtual float GetDetectRange() {
		return m_Entity.GetDetectRange();
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

	public virtual int GetHeat() {
		return 0;
	}

	public virtual int GetMaxHeat() {
		return 0;
	}

	public virtual void SetHunger(int value) {

	}

	public virtual int GetHunger() {
		return 0;
	}

	public virtual int GetMaxHunger() {
		return 0;
	}

	public virtual void SetSanity(int value) {

	}

	public virtual int GetSanity() {
		return 0;
	}

	public virtual int GetMaxSanity() {
		return 0;
	}

	public virtual UIItemController[] GetInventory() {
		return null;
	}

	public virtual float GetMoveSpeed() {
		return 0f;
	}

	public virtual float GetDuration() {
		return 0f;
	}

	public virtual void SetLeaderEntity(TDCEntity leader) {
		
	}

	public virtual TDCEntity GetLeaderEntity() {
		return null;
	}

	public virtual Vector3 GetLeaderPosition() {
		return Vector3.zero;
	}

	#endregion

}