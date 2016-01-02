using UnityEngine;
using System;
using System.Collections;
using ObjectPool;

public class TDCSkillSlot {

	private TDCBaseController m_Owner;
	private TDCEnum.EGameType m_SkillType;
	private TDCSkillData m_SkillData;
	private TDCGameManager m_GameManager;
	private TDCSkillController m_SkillController;

	private float m_TimeDelay;
	private float m_TimeEffect;

	public TDCSkillSlot(TDCEnum.EGameType skillType, TDCBaseController owner)
	{
		m_GameManager = TDCGameManager.GetInstance();
		m_SkillData = m_GameManager.GetSkillData(skillType);
		m_SkillType = skillType;
		m_Owner = owner;
		m_TimeDelay = 0f;
		m_TimeEffect = 0f;
		m_Owner.AddEventListener(m_SkillData.TriggerEnvent, ActiveSkill);
	}

	public void ActiveSkill() {
		if (DidEndTimeDelay())
		{
			m_TimeDelay = m_SkillData.TimeDelay;
			m_SkillController = m_GameManager.GetObjectPool(m_SkillType) as TDCSkillController;
			if (m_SkillController != null)
			{
				m_SkillController.SetSlot(this);
				m_SkillController.SetActive(true);
				m_SkillController.SetOwner(m_Owner);
				m_SkillController.SetTargetPosition(m_Owner.GetEnemyPosition());
				m_SkillController.SetEnemyController(m_Owner.GetEnemyController());
				m_SkillController.SetTimeDelay(m_SkillData.TimeDelay);
				m_SkillController.SetTimeEffect(m_SkillData.TimeEffect);
				m_SkillController.SetEffectPerTime(m_SkillData.EffectPerTime);
				m_SkillController.SetEffectRadius(m_SkillData.EffectRadius);
				m_SkillController.StartSkill(m_Owner.TransformPosition, m_Owner.TransformRotation, true);
			}
		}
	}

	public void DeactiveSkill(TDCSkillController member) {
		member.SetOwner(null);
		m_GameManager.SetObjectPool(member);
	}

	public bool DidEndTimeDelay() {
		return m_TimeDelay <= 0f;
	}

	public void UpdateSkill(float dt) {
		if (m_TimeDelay > 0f)
		{
			m_TimeDelay -= dt;
		}
	}

	public void SetOwner(TDCBaseController owner) {
		m_Owner = owner;
	}
}
