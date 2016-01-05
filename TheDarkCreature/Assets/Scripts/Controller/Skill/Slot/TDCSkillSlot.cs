using UnityEngine;
using System;
using System.Collections;
using ObjectPool;

public class TDCSkillSlot {

	private TDCEntity m_Owner;
	private TDCEnum.EGameType m_SkillType;
	private TDCSkillData m_SkillData;
	private TDCGameManager m_GameManager;
	private TDCEntity m_SkillEntity;

	private float m_TimeDelay;

	public TDCSkillSlot(TDCEnum.EGameType skillType, TDCEntity owner)
	{
		m_GameManager = TDCGameManager.GetInstance();
		m_SkillData = m_GameManager.GetSkillData(skillType);
		m_SkillType = skillType;
		m_Owner = owner;
		m_TimeDelay = 0f;
		m_Owner.AddEventListener(m_SkillData.TriggerEnvent, ActiveSkill);
	}

	public void ActiveSkill() {
		if (DidEndTimeDelay())
		{
			m_TimeDelay = m_SkillData.TimeDelay;
			m_SkillEntity = m_GameManager.GetObjectPool(m_SkillType);
			if (m_SkillEntity != null)
			{
				m_SkillEntity.SetSlot(this);
				m_SkillEntity.SetActive(true);
				m_SkillEntity.SetOwnerEntity(m_Owner);
				m_SkillEntity.SetTargetPosition(m_Owner.GetEnemyPosition());
				m_SkillEntity.SetEnemyEntity(m_Owner.GetEnemyEntity());
				(m_SkillEntity.GetController() as TDCSkillController).StartSkill(m_Owner.GetController().TransformPosition, m_Owner.GetController().TransformRotation);
			}
		}
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

	public void SetOwnerEntity(TDCEntity owner) {
		m_Owner = owner;
	}
}
