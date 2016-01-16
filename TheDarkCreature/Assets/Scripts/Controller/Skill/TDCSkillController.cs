using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Effect;

public class TDCSkillController : TDCBaseController {

	#region Properties

	protected float m_TimeDelay = 0f;
	protected float m_TimeEffect = 0f;
	protected float m_EffectPerTime = 0f;
	protected float m_EffectRadius = 0f;
	protected float m_IsFinishSkill = 1f;

	protected LayerMask m_ColliderLayerMask;
	protected EffectManager m_EffectManager;
	protected Transform m_AttachTransform;
	protected TDCBaseController[] m_ControllersInRadius;

	#endregion

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();

		m_EffectManager = new EffectManager();

		m_ColliderLayerMask = 1 << 8 | 1 << 10 | 1 << 11 | 1 << 31;

		LoadFSM();
		LoadEffect();

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
		m_EffectManager.LoadEffect(m_Entity.GetEffectPath());

		m_EffectPerTime = m_Entity.GetEffectPerTime();
		m_TimeDelay = m_Entity.GetTimeDelay();
		m_TimeEffect = m_Entity.GetTimeEffect();
		m_EffectRadius = m_Entity.GetEffectRadius();

		m_AttachTransform = this.transform;
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void FixedUpdate()
	{
		// base.FixedUpdate();
		m_FSMManager.UpdateState();
		if (m_TimeEffect > 0f)
		{
			m_TimeEffect -= Time.fixedDeltaTime;
		}
		TransformPosition = m_AttachTransform.position;
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(TransformPosition, m_Entity.GetEffectRadius());
	}

	#endregion

	#region Main methods

	private void LoadFSM() {
		var startState 		= new FSMStartSkillState(this);
		var updateState 	= new FSMUpdateSkillState(this);
		var excuteEffectState = new FSMExcuteEffectSkillState(this);
		var moveState 		= new FSMMoveState(this);
		var chaseState 		= new FSMChaseState(this);
		var endState 		= new FSMEndSkillState(this);
		var destroyState 	= new FSMDestroySkillState(this);

		m_FSMManager.RegisterState("StartSkillState", startState);
		m_FSMManager.RegisterState("UpdateSkillState", updateState);
		m_FSMManager.RegisterState("ExcuteEffectSkillState", excuteEffectState);
		m_FSMManager.RegisterState("MoveState", moveState);
		m_FSMManager.RegisterState("ChaseState", chaseState);
		m_FSMManager.RegisterState("EndSkillState", endState);
		m_FSMManager.RegisterState("DestroySkillState", destroyState);

		m_FSMManager.RegisterCondition("CanActiveSkill", CanActiveSkill);
		m_FSMManager.RegisterCondition("HaveCreatureAroundOwner", HaveCreatureAroundOwner);
		m_FSMManager.RegisterCondition("IsRepeatSkill", IsRepeatSkill);
		m_FSMManager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMManager.RegisterCondition("MoveToEnemy", MoveToEnemy);
		m_FSMManager.RegisterCondition("IsFinishSkill", IsFinishSkill);
		m_FSMManager.RegisterCondition("IsEnemyDeath", IsEnemyDeath);
		m_FSMManager.RegisterCondition("HaveEndEffectPerTime", HaveEndEffectPerTime);
		m_FSMManager.RegisterCondition("HaveEndTimeEffect", HaveEndTimeEffect);
	}

	private void LoadEffect() {
		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);
		m_EffectManager.RegisterCondition("CanPayHealthPoint", CanPayHealthPoint);
		m_EffectManager.RegisterCondition("CanPayHeatPoint", CanPayHeatPoint);
		m_EffectManager.RegisterCondition("CanPaySanityPoint", CanPaySanityPoint);

		m_EffectManager.RegisterExcuteMethod("ApplyDamageEffect", ApplyDamageEffect);
		m_EffectManager.RegisterExcuteMethod("AddValueEffect", AddValueEffect);
		m_EffectManager.RegisterExcuteMethod("SubtractValueEffect", SubtractValueEffect);
		m_EffectManager.RegisterExcuteMethod("SetValueEffect", SetValueEffect);
		m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);
	}

	public virtual void StartSkill(Vector3 position, Quaternion rotation, TDCEntity owner) {
		m_AttachTransform = m_Entity.GetAttachOwner() ? 
							m_Entity.GetOwnerEntity().GetController().transform : 
							m_Entity.GetAttachEnemy() ? 
							GetEnemyEntity().GetController().transform : 
							this.transform;

		TransformPosition = position;
		TransformRotation = rotation;
		SetOwnerEntity(owner);
		SetActive(true);
	}

	public virtual void ExcuteEffect() {
		m_EffectManager.ExcuteEffect();
		m_IsFinishSkill = 1f;
	}

	public override void ResetObject()
	{
		base.ResetObject();
		m_Entity.ResetObject();
		m_IsFinishSkill = 1f;
		m_GameManager.SetObjectPool(this.GetEntity());
		m_EffectPerTime = m_Entity.GetEffectPerTime();
		m_TimeDelay = m_Entity.GetTimeDelay();
		m_TimeEffect = m_Entity.GetTimeEffect();
		m_EffectRadius = m_Entity.GetEffectRadius();
	}

	public override void MovePosition(Vector3 position)
	{
		base.MovePosition(position);
		var direction = position - m_Transform.position;
		m_Transform.position = m_Transform.position + direction.normalized * 50f * Time.deltaTime;
		LookAtRotation(position);
	}

	public override void LookAtRotation(Vector3 rotation)
	{
		base.LookAtRotation(rotation);
		//Look at and dampen the rotation
		rotation.y = 0f;
		var direction = m_Transform.position - rotation;
		Quaternion rot = Quaternion.LookRotation(direction);
		m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, rot, Time.deltaTime * 3f);
	}

	public override Dictionary<string, object> GetObjectCurrentValue()
	{
		var curValue = base.GetObjectCurrentValue();
		curValue["State"] = m_FSMManager.StateCurrentName;
		return curValue;
	}

	#endregion

	#region Getter & Setter

	public virtual float GetEffectRadius() {
		return m_Entity.GetEffectRadius();
	}

	public override TDCEntity GetEnemyEntity()
	{
		return m_Entity.GetEnemyEntity();
	}

	public override void SetEnemyEntity(TDCEntity entity)
	{
		m_Entity.SetEnemyEntity(entity);
	}

	public virtual TDCEnum.ESkillType GetSkillType() {
		return m_Entity.GetSkillType();
	}

	public virtual void SetEffectRadius(float radius) {
		m_EffectRadius = radius;
	}

	public virtual void SetTimeDelay(float timeDelay) {
		m_TimeDelay = timeDelay;
	}

	public virtual void SetTimeEffect(float timeEffect) {
		m_TimeEffect = timeEffect;
	}

	public virtual void SetEffectPerTime(float time) {
		m_EffectPerTime = time;
	}

	public virtual void SetOwnerEntity(TDCEntity owner) {
		m_Entity.SetOwnerEntity (owner);
	}

	public virtual TDCEntity GetOwnerEntity() {
		return m_Entity.GetOwnerEntity ();
	}

	public virtual void SetSlot(TDCSkillSlot slot) {
		m_Entity.SetSlot(slot);
	}

	public override int GetDamage()
	{
		return m_Entity.GetDamage();
	}

	#endregion

	#region FSM

	internal virtual bool HaveEndEffectPerTime() {
		m_EffectPerTime -= Time.deltaTime;
		if (m_EffectPerTime <= 0f)
		{
			m_EffectPerTime = m_Entity.GetEffectPerTime();
			return true;
		}
		return false;
	}

	internal virtual bool HaveEndTimeEffect() {
		m_TimeEffect -= Time.deltaTime;
		return m_TimeEffect <= 0f;
	}

	internal override bool IsEnemyDeath()
	{
		return GetEnemyEntity() != null && GetEnemyEntity().GetActive();
	}

	internal override bool MoveToEnemy()
	{
		base.MoveToEnemy();
		var enemy = GetEnemyEntity();
		if (enemy != null)
		{
			var distance = (TransformPosition - enemy.GetController().TransformPosition).sqrMagnitude;
			var range = enemy.GetColliderRadius();
			return distance < range * range; 
		}
		else
		{
			return MoveToTarget();
		}
	}

	internal override bool MoveToTarget()
	{
		return (TransformPosition - GetTargetPosition()).sqrMagnitude < 0.5f * 0.5f;  
	}

	internal virtual bool IsFinishSkill() {
		m_IsFinishSkill -= Time.deltaTime;
		return m_IsFinishSkill < 0f;
	}

	internal virtual bool CanActiveSkill() {
		return GetActive();
	}

	internal virtual bool IsRepeatSkill() {
		return m_Entity.GetRepeatSkill();
	}

	internal virtual bool HaveCreatureAroundOwner()
	{
		var colliders = Physics.OverlapSphere(TransformPosition, m_Entity.GetEffectRadius(), m_ColliderLayerMask);
		var haveCollider = colliders.Length > 0;
		if (haveCollider)
		{
			m_ControllersInRadius = new TDCBaseController[colliders.Length];
			for (int i = 0; i < colliders.Length; i++)
			{
				var gName = colliders[i].name;
				m_ControllersInRadius[i] = m_GameManager.GetControllerByName(gName);
			}
		}
		return haveCollider;
	}

	#endregion

	#region Effect

	internal virtual void PrintDebug(Dictionary<string, object> pars) {
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
	}

	internal virtual void AddValueEffect(Dictionary<string, object> pars) {
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
	}

	internal virtual void SubtractValueEffect(Dictionary<string, object> pars) {
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
	}

	internal virtual void ApplyDamageEffect(Dictionary<string, object> pars) {
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
	}

	internal virtual void SetValueEffect(Dictionary<string, object> pars) {
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
	}

	internal virtual bool CanActiveEffect(Dictionary<string, object> pars) {
		return GetActive();
	}

	internal virtual bool CanPayHealthPoint(Dictionary<string, object> pars)
	{
		return false;
	}

	internal virtual bool CanPayHeatPoint(Dictionary<string, object> pars)
	{
		return false;
	}

	internal virtual bool CanPaySanityPoint(Dictionary<string, object> pars)
	{
		return false;
	}

	#endregion

}
