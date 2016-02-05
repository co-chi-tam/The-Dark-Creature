using UnityEngine;
using System;
using System.Collections;
using ObjectPool;

public class TDCSkillSlot {

	#region Properties

	private TDCEntity m_Owner;
	private TDCEnum.EGameType m_SkillType;
	private TDCSkillData m_SkillData;
	private TDCGameManager m_GameManager;
	private TDCEntity m_SkillEntity;

	private float m_TimeDelay;

	#endregion

	#region Contructor

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

	#endregion

	#region Main methods

	public void ActiveSkill() {
		if (CanActiveSkill())
		{
			m_TimeDelay = m_SkillData.TimeDelay;
			if (m_GameManager.GetObjectPool(m_SkillType, ref m_SkillEntity))
			{
				m_SkillEntity.SetSlot(this);
				m_SkillEntity.SetOwnerEntity(m_Owner);
				m_SkillEntity.SetEnemyEntity(m_Owner.GetEnemyEntity());
				m_SkillEntity.SetTargetPosition(m_Owner.GetEnemyPosition());
				m_SkillEntity.GetController().StartSkill();
				m_SkillEntity.SetActive(true);
				PayCost();
			}
		}
	}

	private bool CanActiveSkill() {
		return DidEndTimeDelay() && CanPayCost() && IsInRange();
	}

	private bool DidEndTimeDelay() {
		return m_TimeDelay <= 0f;
	}

	private bool IsInRange() {
		if (m_SkillData.SkillType == TDCEnum.ESkillType.Passive)
			return true;
		if (m_Owner == null || m_Owner.GetEnemyEntity() == null)
			return false;
		var enemy = m_Owner.GetEnemyEntity();
		var distance = (m_Owner.GetTransformPosition() - enemy.GetTransformPosition()).sqrMagnitude;
		var range = GetEffectRange() + enemy.GetColliderRadius();
		return distance <= range * range;
	}

	private bool CanPayCost() {
		if (!TDCUltilities.IsPlayer(m_Owner))
			return true;
		var healthCost = m_Owner.GetHealth() >= m_SkillData.CostHealthPoint;
		var heatCost = m_Owner.GetHeat() >= m_SkillData.CostHeatPoint;
		var sanityCost = m_Owner.GetSanity() >= m_SkillData.CostSanityPoint;
		var hungerCost = m_Owner.GetHunger() >= m_SkillData.CostHungerPoint;
		return healthCost && heatCost && sanityCost && hungerCost;
	}

	private void PayCost() {
		if (!TDCUltilities.IsPlayer(m_Owner))
			return;
//		m_Owner.SetProperty("HealthPoint", -m_SkillData.CostHealthPoint);
		m_Owner.SetProperty("HeatPoint", -m_SkillData.CostHeatPoint);
		m_Owner.SetProperty("SanityPoint", -m_SkillData.CostSanityPoint);
		m_Owner.SetProperty("HungerPoint", -m_SkillData.CostHungerPoint);
	}

	public void UpdateSkill(float dt) {
		if (m_TimeDelay >= 0f)
		{
			m_TimeDelay -= dt;
		}
	}

	private void OnDestroy() {
		for (int i = 0; i < m_SkillData.TriggerEnvent.Length; i++)
		{
			m_Owner.RemoveEventListener(m_SkillData.TriggerEnvent[i], ActiveSkill);
		}
	}

	#endregion

	#region Getter && Setter

	public void SetOwnerEntity(TDCEntity owner) {
		m_Owner = owner;
	}

	public float GetEffectRadius() {
		return m_SkillData.EffectRadius;
	}

	public float GetEffectRange() {
		return m_SkillData.EffectRange + m_Owner.GetAttackRange();
	}

	#endregion

}
