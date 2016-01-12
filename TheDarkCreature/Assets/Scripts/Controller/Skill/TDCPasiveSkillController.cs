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

	internal override void AddValueEffect(Dictionary<string, object> pars)
	{
		base.AddValueEffect(pars);
		if (pars["ToTarget"].Equals("InRange"))
		{
			var mPos = TransformPosition;
			var valueName = pars["ValueName"].ToString();
			var value = pars["ToValue"];
			mPos.y = 0f;
			var colliders = Physics.OverlapSphere(mPos, GetEffectRadius(), m_ColliderLayerMask);
			for (int i = 0; i < colliders.Length; i++)
			{
				var target = m_GameManager.GetEntityByName(colliders[i].name);
				if (target == null || target.GetActive() == false || target == this.GetEntity())
				{
					continue;
				}
				else
				{
					target.SetProperty(valueName, value);
				}
			}
		}
	}

	internal override void SetValueEffect(Dictionary<string, object> pars)
	{
		base.SetValueEffect(pars);
		var toTarget = pars["ToTarget"];
		var valueName = pars["ValueName"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		if (toTarget.Equals("Enemy"))
		{
			var enemy = GetEnemyEntity();
			if (enemy != null)
			{
				enemy.SetProperty(valueName, toValue);
			}
		} else if (toTarget.Equals("Owner")) {
			var owner = GetOwnerEntity();
			if (owner != null)
			{
				owner.SetProperty(valueName, toValue);
			}
		}
	}

	internal override void SubtractValueEffect(Dictionary<string, object> pars)
	{
		base.SubtractValueEffect(pars);
		var toTarget = pars["ToTarget"];
		var valueName = pars["ValueName"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		if (toTarget.Equals("Enemy"))
		{
			var enemy = GetEnemyEntity();
			if (enemy != null)
			{
				enemy.SetProperty(valueName, -toValue);
			}
		} else if (toTarget.Equals("Owner")) {
			var owner = GetOwnerEntity();
			if (owner != null)
			{
				owner.SetProperty(valueName, -toValue);
			}
		}
	}

	#endregion

}

