using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCEntity : TDCPropertyReflection {

	#region Properties

	protected bool m_IsActive;

	protected TDCObjectProperty<float> m_OffsetSpeed;
	protected TDCObjectProperty<int> m_HealthPoint;
	protected TDCObjectProperty<int> m_HeatPoint;
	protected TDCObjectProperty<int> m_HungerPoint;
	protected TDCObjectProperty<int> m_SanityPoint;

	protected Dictionary<string, Action> m_TriggerEvents;

	#endregion

	#region Event 

	public event Action OnIdleEvent;
	public event Action OnFindRandomEvent;
	public event Action OnMoveEvent;
	public event Action OnApplyDamageEvent;
	public event Action OnAttackEvent;
	public event Action OnAvoidEvent;
	public event Action OnOverHeatdEvent;
	public event Action OnAliveEvent;
	public event Action OnDeathEvent;

	public event Action<int, int> OnHealthChange;
	public event Action<int, int> OnHeatChange;
	public event Action<int, int> OnSanityChange;
	public event Action<int, int> OnHungerChange;

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
		m_TriggerEvents.Add("OnAttack", OnAttackEvent);
		m_TriggerEvents.Add("OnAvoid", OnAvoidEvent);
		m_TriggerEvents.Add("OnOverHeat", OnOverHeatdEvent);
		m_TriggerEvents.Add("OnAlive", OnAliveEvent);
		m_TriggerEvents.Add("OnDeath", OnDeathEvent);
	}

	#endregion

	#region Contructor

	public TDCEntity(TDCBaseController ctrl, TDCBaseData data)
	{
		m_TriggerEvents = new Dictionary<string, Action>();
		LoadEventCallBack();

		m_OffsetSpeed = new TDCObjectProperty<float>("OffsetSpeed", 1f);
		m_HealthPoint = new TDCObjectProperty<int>("HealthPoint");
		m_HeatPoint = new TDCObjectProperty<int>("HeatPoint");
		m_SanityPoint = new TDCObjectProperty<int>("SanityPoint");
		m_HungerPoint = new TDCObjectProperty<int>("HungerPoint");

		RegisterProperty(m_OffsetSpeed);
		RegisterProperty(m_HealthPoint);
		RegisterProperty(m_HeatPoint);
		RegisterProperty(m_SanityPoint);
		RegisterProperty(m_HungerPoint);
	}

	#endregion

	#region Main methods

	public virtual void Update(float dt) {

	}

	public virtual void ApplyDamage(int damage, TDCEntity attacker) {

	}

	public virtual void ActiveSkill(int index) {

	}

	public virtual void ResetObject(){
		
	}

	#endregion

	#region Common

	public virtual void SetStartBattlePosition(Vector3 value) {
	
	}

	public virtual Vector3 GetStartBattlePosition() {
		return Vector3.zero;
	}

	public virtual void SetTriggerEvent(string name, Action evnt) {
		m_TriggerEvents.Add(name, evnt);
	}

	public virtual Action GetTriggerEvent(string name) {
		return m_TriggerEvents[name];
	}

	public virtual string GetFSMPath() {
		return string.Empty;
	}

	public virtual void SetData(TDCBaseData data) {

	}
	public virtual void SetController(TDCBaseController controller) {

	}

	public virtual TDCBaseController GetController() {
		return null;
	}

	public virtual void SetActive(bool value) {
		m_IsActive = value;
	}

	public virtual bool GetActive() {
		return m_IsActive;
	}

	public virtual TDCEnum.EGameType GetGameType()
	{
		return TDCEnum.EGameType.None;
	}

	public virtual Vector3 GetStartPosition()
	{
		return Vector3.zero;
	}

	public virtual void SetStartPosition(Vector3 pos) {
		
	}

	#endregion

	#region Creature

	public virtual TDCEnum.ECreatureType GetCreatureType()
	{
		return TDCEnum.ECreatureType.None;
	}

	public virtual Vector3 GetEnemyPosition()
	{
		return Vector3.zero;
	}

	public virtual void SetEnemyEntity(TDCEntity entity)
	{
		
	}

	public virtual TDCEntity GetEnemyEntity()
	{
		return null;
	}

	public virtual void SetGroupEntity(TDCEntity group) {
		
	}

	public virtual TDCEntity GetGroupEntity() {
		return null;
	}

	public virtual Vector3 GetGroupPosition()
	{
		return Vector3.zero;
	}

	public virtual Vector3 GetTargetPosition() {
		return Vector3.zero;
	}

	public virtual void SetTargetPosition(Vector3 pos) {
		
	}

	public virtual float GetMoveSpeed()
	{
		return 0f;
	}

	public virtual float GetRotationSpeed()
	{
		return 0f;
	}

	public virtual int GetDamage()
	{
		return 0;
	}
		
	public virtual float GetColliderRadius() {
		return 0f;
	}

	public virtual void SetAnimation(EAnimation anim) {
		
	}

	public virtual float GetDetectEnemyRange() {
		return 0f;
	}

	public virtual List<TDCEnum.EGameType> GetTypeEnemies() {
		return null;
	}

	public virtual List<TDCEnum.EGameType> GetTypeFoods() {
		return null;
	}

	public virtual int GetHealth ()
	{
		return 0;
	}

	public virtual void SetHealth (int value)
	{
		if (OnHealthChange != null)
		{
			OnHealthChange(GetHealth(), value);
		}
	}

	public virtual int GetMaxHealth() {
		return 0;
	}

	public virtual int GetHeat() {
		return 0;
	}

	public virtual int GetMaxHeat() {
		return 0;
	}

	public virtual void SetHeat(int value)
	{
		if (OnHeatChange != null)
		{
			OnHeatChange(GetHeat(), value);
		}
	}

	public virtual int GetSanity() {
		return 0;
	}

	public virtual int GetMaxSanity() {
		return 0;
	}

	public virtual void SetSanity(int value)
	{
		if (OnSanityChange != null)
		{
			OnSanityChange(GetSanity(), value);
		}
	}

	public virtual int GetHunger() {
		return 0;
	}

	public virtual int GetMaxHunger() {
		return 0;
	}

	public virtual void SetHunger(int value)
	{
		if (OnHungerChange != null)
		{
			OnHungerChange(GetSanity(), value);
		}
	}

	public virtual int AddItemInventory(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount)
	{
		return 0;
	}

	public virtual UIItemController[] GetItemInventory()
	{
		return null;
	}

	public virtual float GetAttackRange()
	{
		return 0f;
	}

	#endregion

	#region Group

	public virtual TDCEnum.EGroupType GetGroupType()
	{
		return TDCEnum.EGroupType.None;
	}

	public virtual TDCEnum.EGroupSpawnType GetGroupSpawnType()
	{
		return TDCEnum.EGroupSpawnType.None;
	}

	public virtual int GetCurrentMember() {
		return 0;
	}

	public virtual void SetCurrentMember(int value) {
		
	}

	public virtual int GetMaxMember() {
		return 0;
	}

	public virtual float GetGroupRadius() {
		return 0;
	}

	public virtual TDCEnum.EGameType GetGroupMemberType() {
		return TDCEnum.EGameType.None;
	}

	public virtual float GetTimeRespawnMember() {
		return 0f;
	}

	public virtual TDCEnum.EGameType GetMemberType() {
		return TDCEnum.EGameType.None;
	}

	public virtual void ReturnMember(TDCEntity member) {
		
	}

	#endregion

	#region Skill

	public virtual TDCEnum.ESkillType GetSkillType()
	{
		return TDCEnum.ESkillType.None;
	}

	public virtual void SetSlot(TDCSkillSlot slot) {
		
	}

	public virtual string GetEffectPath() {
		return string.Empty;
	}

	public virtual float GetTimeDelay() {
		return 0f;
	}

	public virtual float GetTimeEffect() {
		return 0f;
	}
	
	public virtual float GetEffectPerTime() {
		return 0f;
	}

	public virtual float GetEffectRadius() {
		return 0f;
	}

	public virtual bool GetAttachOwner() {
		return false;
	}

	public virtual bool GetAttachEnemy() {
		return false;
	}

	public virtual bool GetRepeatSkill() {
		return false;
	}

	public virtual TDCEntity GetOwnerEntity() {
		return null;
	}

	public virtual void SetOwnerEntity(TDCEntity owner) {
		
	}

	#endregion

	#region Object

	public virtual float GetDuration() {
		return 0f;
	}

	#endregion

	#region Item

	public virtual bool GetItemActive()
	{
		return false;
	}

	public virtual void SetItemActive(bool value)
	{
		
	}

	public virtual TDCEnum.EItemType GetItemType()
	{
		return TDCEnum.EItemType.None;
	}

	#endregion

	#region Pet

	public virtual void SetLeader(TDCEntity leader) {

	}

	public virtual TDCEntity GetLeader() {
		return null;
	}

	#endregion
}
