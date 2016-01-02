using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Effect;

public class TDCPasiveSkillController : TDCSkillController {

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
		return GetActive();
	}

	#endregion

	#region Effect

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	#endregion

}

