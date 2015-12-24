using UnityEngine;
using System.Collections;
using Effect;

public class TDCSkillController {

	protected EffectManager m_EffectManager;
	protected TDCSkillData m_SkillData;
	protected TDCBaseController m_OwnerController;
	protected int m_EnemyLayerMask;
	protected TDCBaseController[] m_ControllerInsideRadius;
	protected float m_TimeDelay;
	protected float m_TimeEffect;

	public TDCSkillController()
	{
		m_SkillData = null;
		m_OwnerController = null;
		m_EffectManager = null;
		m_EnemyLayerMask = 0;
		m_ControllerInsideRadius = null;
		m_TimeDelay = 0;
	}

	public TDCSkillController(TDCSkillData skillData, TDCBaseController ownerCtrl)
	{
		m_SkillData = skillData;
		m_OwnerController = ownerCtrl;
		m_EnemyLayerMask = 1 << 8 | 1 << 10 | 1 << 31;

		m_EffectManager = new EffectManager();
		m_EffectManager.LoadEffect(m_SkillData.EffectPath);

		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);
		m_EffectManager.RegisterCondition("CanPayHealthPoint", CanPayHealthPoint);
		m_EffectManager.RegisterCondition("CanPayHeatPoint", CanPayHeatPoint);
		m_EffectManager.RegisterCondition("CanPaySanityPoint", CanPaySanityPoint);
		m_EffectManager.RegisterCondition("HaveCreatureAroundOwner", HaveCreatureAroundOwner);
		m_EffectManager.RegisterCondition("HaveEndTimeDelay", HaveEndTimeDelay);

		m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);
		m_EffectManager.RegisterExcuteMethod("SpawnParticle", SpawnParticle);
		m_EffectManager.RegisterExcuteMethod("ExcuteEffect", ExcuteEffect);

		m_TimeDelay = skillData.TimeDelay;
	}

	public void UpdateSkill(float dt) {
		m_TimeDelay -= dt;
	}

	public virtual void ExcuteSkill() {
		m_EffectManager.ExcuteEffect();
	}

	internal virtual bool HaveEndTimeDelay(object[] pas)
	{
		var endTime = m_TimeDelay < 0f;
		if (endTime)
		{
			m_TimeDelay = m_SkillData.TimeDelay;
		}
		return endTime;
	}

	internal virtual bool HaveEndTimeEffect(object[] pas)
	{
		var endTime = m_TimeEffect < 0f;
		if (endTime)
		{
			m_TimeEffect = m_SkillData.TimeEffect;
		}
		return endTime;
	}

	internal virtual bool HaveCreatureAroundOwner(object[] pas)
	{
		var colliders = Physics.OverlapSphere(m_OwnerController.TransformPosition, 
												m_SkillData.EffectRadius, m_EnemyLayerMask);
		m_ControllerInsideRadius = new TDCBaseController[colliders.Length];
		for (int i = 0; i < colliders.Length; i++)
		{
			m_ControllerInsideRadius[i] = colliders[i].GetComponent<TDCBaseController>();				
		}
		return m_ControllerInsideRadius.Length > 0;
	}

	internal virtual bool CanPayHealthPoint(object[] pas)
	{
		return false;
	}

	internal virtual bool CanPayHeatPoint(object[] pas)
	{
		return false;
	}

	internal virtual bool CanPaySanityPoint(object[] pas)
	{
		return false;
	}

	internal virtual bool CanActiveEffect(object[] pas)
	{
		return true;
	}

	internal virtual void PrintDebug(object[] pas)
	{
#if UNITY_EDITOR
		for (int i = 0; i < pas.Length; i+=2)
		{
			var name = pas[i];
			var value = pas[i + 1];
			Debug.LogError(string.Format ("[{0}] - {1}", name, value));
		}
#endif
	}

	internal virtual void ExcuteEffect(object[] pas)
	{
		
	}

	internal virtual void SpawnParticle(object[] pas)
	{

	}

}
