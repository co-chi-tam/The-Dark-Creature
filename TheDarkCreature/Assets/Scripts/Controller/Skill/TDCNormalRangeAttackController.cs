using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Effect;

public class TDCNormalRangeAttackController : TDCSkillController {

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();
	}

	public override void Start()
	{
		base.Start();
	}

	#endregion

	#region FSM

	internal override bool CanActiveSkill()
	{
		base.CanActiveSkill();
		var active = m_SkillData.Owner.GetEnemyController() != null;
		return active;
	}

	internal override bool IsRepeatSkill()
	{
		return m_SkillData.RepeatSkill;
	}

	#endregion

	#region Effect

	internal override void AddValueEffect(Dictionary<string, object> pars) {
		base.AddValueEffect(pars);
	}

	internal override void SubtractValueEffect(Dictionary<string, object> pars) {
		base.SubtractValueEffect(pars);
	}

	internal override void ApplyDamageEffect(Dictionary<string, object> pars) {
		base.ApplyDamageEffect(pars);
		var paramValue = int.Parse(pars["Damage"].ToString());
		GetEnemyController().ApplyDamage (paramValue, m_SkillData.Owner);
	}

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	#endregion

}