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
		var toEnemy = pars["ToTarget"];
		if (toEnemy.Equals("Enemy"))
		{
			var damage = int.Parse(pars["Damage"].ToString());
			var enemy = GetEnemyEntity();
			if (enemy != null)
			{
				enemy.ApplyDamage(damage + GetDamage(), m_Entity.GetOwnerEntity());
			}
		}
	}

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	#endregion

}