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
		return GetEnemyEntity() != null;
	}

	internal override bool IsRepeatSkill()
	{
		return m_Entity.GetRepeatSkill();
	}

	#endregion

	#region Effect

	internal override void ApplyDamageEffect(Dictionary<string, object> pars) {
		base.ApplyDamageEffect(pars);
		var paramValue = int.Parse(pars["Damage"].ToString());
		var enemy = GetEnemyEntity();
		if (enemy != null) {
			enemy.ApplyDamage (paramValue + GetDamage(), m_Entity.GetOwnerEntity());
		}
	}

	internal override void SetValueEffect(Dictionary<string, object> pars)
	{
		base.SetValueEffect(pars);
//		var toTarget = pars["ToTarget"];
		var valueName = pars["ValueName"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		var enemy = GetEnemyEntity();
		if (enemy != null) {
			enemy.SetProperty(valueName, toValue);
		}
	}

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	#endregion

}