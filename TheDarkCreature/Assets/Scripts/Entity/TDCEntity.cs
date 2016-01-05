using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCEntity : TDCPropertyReflection {

	#region Properties

	protected bool m_IsActive;

	protected Dictionary<string, Action> m_TriggerEvents;

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

	#endregion

	#region Contructor

	public TDCEntity(TDCBaseController ctrl, TDCBaseData data)
	{
		m_TriggerEvents = new Dictionary<string, Action>();

		LoadEventCallBack();
	}

	#endregion

	#region Main methods

	public virtual void Update(float dt) {

	}

	public virtual void ApplyDamage(int damage, TDCEntity attacker) {

	}

	#endregion

	#region Common

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

	public virtual void SetHeat(int value)
	{
		
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
		
	}

	public virtual int GetMaxHealth() {
		return 0;
	}

	public virtual TDCItemController[] GetInventory()
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

	public virtual int GetMinMember() {
		return 0;
	}

	public virtual int GetMaxMember() {
		return 0;
	}

	public virtual float GetRadius() {
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

	public virtual TDCEnum.EItemType GetItemType()
	{
		return TDCEnum.EItemType.None;
	}

	#endregion
}
