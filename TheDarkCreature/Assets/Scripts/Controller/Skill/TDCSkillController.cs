using UnityEngine;
using System.Collections;
using FSM;
using Effect;

public class TDCSkillController : TDCBaseController {

	#region Properties

	protected float m_TimeDelay;
	protected float m_TimeEffect;
	protected int m_LayerEffect;
	protected TDCSkillData m_SkillData;
	protected TDCGameManager m_GameManager;
	protected EffectManager m_EffectManager;
	protected TDCBaseController[] m_ControllersInRadius;

	#endregion

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();
		m_GameManager = TDCGameManager.GetInstance();
	}

	public override void Start()
	{
		base.Start();

		m_LayerEffect = 1 << 8 | 1 << 10 | 1 << 31;

		m_FSMMamager = new FSMManager();
		m_EffectManager = new EffectManager();

		LoadFSM();
		LoadEffect();

		m_FSMMamager.LoadFSM(m_SkillData.FSMPath);
		m_EffectManager.LoadEffect(m_SkillData.EffectPath);

		m_TimeEffect = m_SkillData.TimeEffect;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		m_FSMMamager.UpdateState();
		StateName = m_FSMMamager.StateCurrentName;
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(TransformPosition, m_SkillData.EffectRadius);
	}

	#endregion

	#region Main methods

	private void LoadFSM() {
		var startState = new FSMStartSkillState(this);
		var updateState = new FSMUpdateSkillState(this);
		var excuteEffectState = new FSMExcuteEffectSkillState(this);
		var endState = new FSMEndSkillState(this);
		var destroyState = new FSMDestroySkillState(this);

		m_FSMMamager.RegisterState("StartSkillState", startState);
		m_FSMMamager.RegisterState("UpdateSkillState", updateState);
		m_FSMMamager.RegisterState("ExcuteEffectSkillState", excuteEffectState);
		m_FSMMamager.RegisterState("EndSkillState", endState);
		m_FSMMamager.RegisterState("DestroySkillState", destroyState);

		m_FSMMamager.RegisterCondition("CanActiveSkill", CanActiveSkill);
		m_FSMMamager.RegisterCondition("HaveEndTimeDelay", HaveEndTimeDelay);
		m_FSMMamager.RegisterCondition("HaveEndTimeEffect", HaveEndTimeEffect);
		m_FSMMamager.RegisterCondition("HaveCreatureAroundOwner", HaveCreatureAroundOwner);
		m_FSMMamager.RegisterCondition("IsRepeatSkill", IsRepeatSkill);
	}

	private void LoadEffect() {
		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);
		m_EffectManager.RegisterCondition("CanPayHealthPoint", CanPayHealthPoint);
		m_EffectManager.RegisterCondition("CanPayHeatPoint", CanPayHeatPoint);
		m_EffectManager.RegisterCondition("CanPaySanityPoint", CanPaySanityPoint);

		m_EffectManager.RegisterExcuteMethod("ExcuteStatusEffect", ExcuteStatusEffect);
		m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);
	}

	public virtual void ExcuteEffect() {
		m_EffectManager.ExcuteEffect();
	}

	public override void ResetObject()
	{
		base.ResetObject();
		m_TimeDelay = m_SkillData.TimeDelay;
		m_TimeEffect = m_SkillData.TimeEffect;
	}

	#endregion

	#region Getter & Setter

	public virtual void SetOwner(TDCBaseController owner) {
		m_SkillData.Owner = owner;
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

	#endregion

	#region FSM

	internal virtual bool CanActiveSkill() {
		return GetActive();
	}

	internal virtual bool IsRepeatSkill() {
		return false;
	}

	internal virtual bool HaveEndTimeDelay()
	{
		m_TimeDelay -= Time.fixedDeltaTime;
		return m_TimeDelay <= 0f;
	}

	internal virtual bool HaveEndTimeEffect()
	{
		m_TimeEffect -= Time.fixedDeltaTime;
		return m_TimeEffect <= 0f;
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

	internal virtual void PrintDebug(object[] pars) {
		Debug.LogError(string.Format("[{0} : {1}]", pars[0], pars[1]));
	}

	internal virtual void ExcuteStatusEffect(object[] pars) {
		
	}

	internal virtual bool CanActiveEffect(object[] pars) {
		return GetActive();
	}

	internal virtual bool CanPayHealthPoint(object[] pars)
	{
		return false;
	}

	internal virtual bool CanPayHeatPoint(object[] pars)
	{
		return false;
	}

	internal virtual bool CanPaySanityPoint(object[] pars)
	{
		return false;
	}

	#endregion

}
