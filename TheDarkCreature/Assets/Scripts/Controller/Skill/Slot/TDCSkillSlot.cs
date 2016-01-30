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
		for (int i = 0; i < m_SkillData.TriggerEnvent.Length; i++)
		{
			m_Owner.AddEventListener(m_SkillData.TriggerEnvent[i], ActiveSkill);
		}
	}

	public void ActiveSkill() {
		if (DidEndTimeDelay() && CanPayCost())
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
				(m_SkillEntity.GetController() as TDCSkillController).StartSkill(m_Owner.GetController().TransformPosition, m_Owner.GetController().TransformRotation, m_Owner);
				PayCost();
			}
		}
	}

	public bool DidEndTimeDelay() {
		return m_TimeDelay <= 0f;
	}

	public bool CanPayCost() {
		if (m_Owner.GetCreatureType() != TDCEnum.ECreatureType.FlyPlayer &&
			m_Owner.GetCreatureType() != TDCEnum.ECreatureType.GroundPlayer)
			return true;
		var healthCost = m_Owner.GetHealth() >= m_SkillData.CostHealthPoint;
		var heatCost = m_Owner.GetHeat() >= m_SkillData.CostHeatPoint;
		var sanityCost = m_Owner.GetSanity() >= m_SkillData.CostSanityPoint;
		var hungerCost = m_Owner.GetHunger() >= m_SkillData.CostHungerPoint;
		return healthCost && heatCost && sanityCost && hungerCost;
	}

	public void PayCost() {
		if (m_Owner.GetCreatureType() != TDCEnum.ECreatureType.FlyPlayer &&
		    m_Owner.GetCreatureType() != TDCEnum.ECreatureType.GroundPlayer)
			return;
		m_Owner.SetProperty("HealthPoint", -m_SkillData.CostHealthPoint);
		m_Owner.SetProperty("HeatPoint", -m_SkillData.CostHeatPoint);
		m_Owner.SetProperty("SanityPoint", -m_SkillData.CostSanityPoint);
		m_Owner.SetProperty("HungerPoint", -m_SkillData.CostHungerPoint);
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

	public void OnDestroy() {
		for (int i = 0; i < m_SkillData.TriggerEnvent.Length; i++)
		{
			m_Owner.RemoveEventListener(m_SkillData.TriggerEnvent[i], ActiveSkill);
		}
	}

}
