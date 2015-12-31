using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Effect;

public class TDCFlameBodyController : TDCSkillController {

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
		return GetActive();
	}

	internal override bool IsRepeatSkill()
	{
		return m_SkillData.RepeatSkill;
	}

	#endregion

	#region Effect

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	internal override void AddValueEffect(Dictionary<string, object> pars) {
		base.AddValueEffect(pars);
	}

	internal virtual void SubtractValueEffect(Dictionary<string, object> pars) {
		base.SubtractValueEffect(pars);
	}

	internal virtual void ApplyDamageEffect(Dictionary<string, object> pars) {
		base.ApplyDamageEffect(pars);
	}

	#endregion

}

