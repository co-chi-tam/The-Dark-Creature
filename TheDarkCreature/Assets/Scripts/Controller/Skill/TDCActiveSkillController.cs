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
		return base.CanActiveSkill() && GetEnemyEntity() != null;
	}

	#endregion

	#region Effect

	internal override void ApplyDamageEffect(Dictionary<string, object> pars) {
		base.ApplyDamageEffect(pars);
	}

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	#endregion

}