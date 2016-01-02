using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Effect;

public class TDCSkillController : TDCBaseController {

	#region Properties

	protected TDCSkillSlot m_Slot;
	protected int m_LayerEffect;
	protected TDCSkillData m_SkillData;
	protected EffectManager m_EffectManager;
	protected TDCBaseController[] m_ControllersInRadius;
	protected float m_IsFinishSkill = 1f;
	protected TDCBaseController m_Owner;
	protected float m_TimeDelay = 0f;
	protected float m_TimeEffect = 0f;
	protected float m_EffectPerTime = 0f;
	protected float m_EffectRadius = 0f;

	#endregion

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();

		m_EffectManager = new EffectManager();

		m_LayerEffect = 1 << 8 | 1 << 10 | 1 << 31;
	}

	protected override void Start()
	{
		base.Start();

		LoadFSM();
		LoadEffect();

		m_FSMManager.LoadFSM(m_SkillData.FSMPath);
		m_EffectManager.LoadEffect(m_SkillData.EffectPath);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		m_FSMManager.UpdateState();
		StateName = m_FSMManager.StateCurrentName;
	}

	protected override void Update()
	{
		base.Update();
		if (m_TimeEffect > 0f)
		{
			m_TimeEffect -= Time.deltaTime;
		}
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(TransformPosition, m_SkillData.EffectRadius);
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
		m_FSMManager.RegisterCondition("HaveEnemy", HaveEnemy);
	}

	private void LoadEffect() {
		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);
		m_EffectManager.RegisterCondition("CanPayHealthPoint", CanPayHealthPoint);
		m_EffectManager.RegisterCondition("CanPayHeatPoint", CanPayHeatPoint);
		m_EffectManager.RegisterCondition("CanPaySanityPoint", CanPaySanityPoint);

		m_EffectManager.RegisterExcuteMethod("ApplyDamageEffect", ApplyDamageEffect);
		m_EffectManager.RegisterExcuteMethod("AddValueEffect", AddValueEffect);
		m_EffectManager.RegisterExcuteMethod("SubtractValueEffect", SubtractValueEffect);
		m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);
	}

	public virtual void StartSkill(Vector3 position, Quaternion rotation, bool active = true) {
		TransformPosition = position;
		TransformRotation = rotation;
		SetActive(active);
	}

	public virtual void ExcuteEffect() {
		m_EffectManager.ExcuteEffect();
		m_IsFinishSkill = 1f;
	}

	public override void ResetObject()
	{
		base.ResetObject();
		m_IsFinishSkill = 1f;
		m_Slot.DeactiveSkill(this);
		m_TimeDelay = m_SkillData.TimeDelay;
		m_TimeEffect = m_SkillData.TimeEffect;
		m_EffectPerTime = m_SkillData.EffectPerTime;
		m_EffectRadius = m_SkillData.EffectRadius;
	}

	public override void MovePosition(Vector3 position)
	{
		base.MovePosition(position);
		position.y = 0f;
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

	#endregion

	#region Getter & Setter

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

	public virtual void SetOwner(TDCBaseController owner) {
		m_Owner = owner;
	}

	public override void SetEnemyController(TDCBaseController controller)
	{
		base.SetEnemyController(controller);
		m_EnemyController = controller;
	}

	public override TDCBaseController GetEnemyController()
	{
		return m_EnemyController;
	}

	public override Vector3 GetEnemyPosition()
	{
		return m_EnemyController.TransformPosition;
	}

	public virtual void SetSlot(TDCSkillSlot slot) {
		m_Slot = slot;
	}

	public override void SetData(TDCBaseData data)
	{
		base.SetData(data);
		m_SkillData = data as TDCSkillData;
	}

	public override TDCBaseData GetData()
	{
		base.GetData();
		return m_SkillData;
	}

	public override int GetMinDamage()
	{
		return m_Owner.GetMinDamage();
	}

	public override int GetMaxDamage()
	{
		return m_Owner.GetMaxDamage();
	}

	#endregion

	#region FSM

	internal override bool MoveToEnemy()
	{
		base.MoveToEnemy();
		var enemy = m_Owner.GetEnemyController();
		if (enemy != null)
		{
			var distance = (TransformPosition - enemy.TransformPosition).sqrMagnitude;
			var range = enemy.GetColliderRadius();
			return distance < range * range; 
		}
		else
		{
			return MoveToTarget();
		}
		return true;
	}

	internal override bool MoveToTarget()
	{
		return (TransformPosition - m_TargetPosition).sqrMagnitude < 0.5f * 0.5f;  
	}

	internal virtual bool IsFinishSkill() {
		m_IsFinishSkill -= Time.deltaTime;
		return m_IsFinishSkill < 0f;
	}

	internal virtual bool CanActiveSkill() {
		return GetActive();
	}

	internal virtual bool IsRepeatSkill() {
		return false;
	}

	internal virtual bool HaveCreatureAroundOwner()
	{
		var collider = Physics.OverlapSphere(TransformPosition, m_SkillData.EffectRadius, m_LayerEffect);
		var haveCollider = collider.Length > 0;
		if (haveCollider)
		{
			m_ControllersInRadius = new TDCBaseController[collider.Length];
			for (int i = 0; i < collider.Length; i++)
			{
				var gName = collider[i].name;
				m_ControllersInRadius[i] = m_GameManager.GetControllerByName(gName);
			}
		}
		return haveCollider;
	}

	#endregion

	#region Effect

	internal virtual void PrintDebug(Dictionary<string, object> pars) {
#if UNITY_EDITOR
		foreach (var item in pars)
		{
			Debug.LogError(string.Format("[{0} : {1}]", item.Key, item.Value));
		}
#endif
	}

	internal virtual void AddValueEffect(Dictionary<string, object> pars) {
#if UNITY_EDITOR
		foreach (var item in pars)
		{
			Debug.LogError(string.Format("[{0} : {1}]", item.Key, item.Value));
		}
#endif
	}

	internal virtual void SubtractValueEffect(Dictionary<string, object> pars) {
#if UNITY_EDITOR
		foreach (var item in pars)
		{
			Debug.LogError(string.Format("[{0} : {1}]", item.Key, item.Value));
		}
#endif
	}

	internal virtual void ApplyDamageEffect(Dictionary<string, object> pars) {
#if UNITY_EDITOR
		foreach (var item in pars)
		{
			Debug.LogError(string.Format("[{0} : {1}]", item.Key, item.Value));
		}
#endif
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
