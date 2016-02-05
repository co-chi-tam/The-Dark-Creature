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

	#region Main methods

	protected override void LoadEffect()
	{
		base.LoadEffect();
	}

	#endregion

	#region FSM

	internal override bool CanActiveSkill()
	{
		return GetActive();
	}

	#endregion

	#region Effect

	internal override bool HaveCreatureIsShineAround(Dictionary<string, object> pars)
	{
		return base.HaveCreatureIsShineAround(pars);
	}

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	internal override void AddValueEffect(Dictionary<string, object> pars)
	{
		base.AddValueEffect(pars);
	}

	internal override void SetValueEffect(Dictionary<string, object> pars)
	{
		base.SetValueEffect(pars);
	}

	internal override void SubtractValueEffect(Dictionary<string, object> pars)
	{
		base.SubtractValueEffect(pars);
	}

	#endregion

}

