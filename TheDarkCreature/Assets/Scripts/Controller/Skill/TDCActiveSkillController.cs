using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Effect;

public class TDCActiveSkillController : TDCSkillController {

	#region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();
	}

	protected override void Start()
	{
		base.Start();
	}

	#endregion

	#region FSM

	internal override bool CanActiveSkill()
	{
		base.CanActiveSkill();
		return GetEnemyController() != null;
	}

	internal override bool IsRepeatSkill()
	{
		return m_SkillData.RepeatSkill;
	}

	#endregion

	#region Effect

	internal override void ApplyDamageEffect(Dictionary<string, object> pars) {
		base.ApplyDamageEffect(pars);
		var paramValue = int.Parse(pars["Damage"].ToString());
		var enemy = GetEnemyController();
		if (enemy != null) {
			enemy.ApplyDamage (paramValue + GetDamage(), m_Owner);
		}
	}

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	#endregion

}