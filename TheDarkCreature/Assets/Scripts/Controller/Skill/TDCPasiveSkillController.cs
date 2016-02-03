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
		m_EffectManager.RegisterCondition("HaveCreatureIsShineAround", HaveCreatureIsShineAround);
	}

	#endregion

	#region FSM

	internal override bool CanActiveSkill()
	{
		return GetActive();
	}

	#endregion

	#region Effect

	internal virtual bool HaveCreatureIsShineAround(Dictionary<string, object> pars)
	{
		var mPos = TransformPosition;
		mPos.y = 0f;
		var colliders = Physics.OverlapSphere(mPos, GetEffectRadius(), m_ColliderLayerMask);
		if (colliders.Length == 0)
			return false;
		for (int i = 0; i < colliders.Length; i++)
		{
			var target = m_GameManager.GetEntityByName(colliders[i].name);
			if (target == null || target.GetActive() == false || target == this.GetEntity())
			{
				continue;
			}
			else if (target.GetIsShine())
			{
				return true;
			}
		}
		return false;
	}

	internal override bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return GetActive();
	}

	internal override void AddValueEffect(Dictionary<string, object> pars)
	{
		base.AddValueEffect(pars);
		var toTarget = pars["ToTarget"].ToString();
		var valueName = pars["ValueName"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		if (toTarget.Equals("InRange"))
		{
			var mPos = TransformPosition;
			mPos.y = 0f;
			var colliders = Physics.OverlapSphere(mPos, GetEffectRadius(), m_ColliderLayerMask);
			if (colliders.Length == 0)
				return;
			for (int i = 0; i < colliders.Length; i++)
			{
				var target = m_GameManager.GetEntityByName(colliders[i].name);
				if (target == null || target.GetActive() == false || target == this.GetEntity())
				{
					continue;
				}
				else
				{
					var sourceValue = target.GetProperty<float>(valueName);
					target.SetProperty(valueName, sourceValue + toValue);
				}
			}
		}
	}

	internal override void SetValueEffect(Dictionary<string, object> pars)
	{
		base.SetValueEffect(pars);
		var toTarget = pars["ToTarget"].ToString();
		var valueName = pars["ValueName"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		TDCEntity entityTarget = null; 
		switch (toTarget)
		{
			case "Enemy":
				entityTarget = GetEnemyEntity();
				break;
			case "Owner":
				entityTarget = GetOwnerEntity();
				break;
			case "Sun":
				entityTarget = m_GameManager.GetSunEntity();
				break;
		}
		if (entityTarget != null)
		{
			entityTarget.SetProperty(valueName, toValue);
		}
	}

	internal override void SubtractValueEffect(Dictionary<string, object> pars)
	{
		base.SubtractValueEffect(pars);
		var toTarget = pars["ToTarget"].ToString();
		var valueName = pars["ValueName"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		var sourceValue = 0f;
		TDCEntity entityTarget = null; 
		switch (toTarget)
		{
			case "Enemy":
				entityTarget = GetEnemyEntity();
				break;
			case "Owner":
				entityTarget = GetOwnerEntity();
				break;
		}
		if (entityTarget != null)
		{
			sourceValue = entityTarget.GetProperty<float>(valueName);
			entityTarget.SetProperty(valueName, sourceValue - toValue);
		}
	}

	#endregion

}

