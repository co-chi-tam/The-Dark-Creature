using UnityEngine;
using System.Collections;
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

	internal override bool CanActiveEffect(object[] pars)
	{
		return GetActive();
	}

	internal override void ExcuteStatusEffect(object[] pars)
	{
		base.ExcuteStatusEffect(pars);
		var nameParam = pars[0].ToString();
		var valueParam = int.Parse(pars[1].ToString());
		for (int i = 0; i < m_ControllersInRadius.Length; i++)
		{
			m_ControllersInRadius[i].SetHeat (valueParam);
		}
	}

	#endregion

}

