using UnityEngine;
using System;
using System.Collections;
using ObjectPool;

public class TDCSkillSlot {

	private TDCBaseController m_Owner;
	private TDCEnum.EGameType m_SkillType;
	private TDCSkillData m_SkillData;
	private TDCGameManager m_GameManager;
	private TDCObjectPool<TDCSkillController> m_SkillPool;

	private float m_TimeDelay;
	private float m_TimeEffect;

	public TDCSkillSlot(TDCEnum.EGameType skillType, TDCBaseController owner)
	{
		m_SkillPool = new TDCObjectPool<TDCSkillController>();
		m_GameManager = TDCGameManager.GetInstance();
		m_SkillData = m_GameManager.GetSkillData(skillType);
		m_SkillData.Owner = owner;
		m_Owner = owner;
		m_TimeDelay = 0f;
		m_TimeEffect = 0f;

		CreateSkillPool();
	}

	private void CreateSkillPool() {
		for (int i = 0; i < 5; i++)
		{
			var skill = m_GameManager.CreateSkill(m_SkillData, m_SkillData.GameType, Vector3.zero, Quaternion.identity);
			skill.Init();
			skill.SetSlot(this);
			skill.SetActive(false);
			m_SkillPool.Create(skill);
		}
	}

	public void ActiveSkill() {
		TDCSkillController skill = null;
		if (DidEndTimeDelay())
		{
			m_TimeDelay = m_SkillData.TimeDelay;
			if (m_SkillPool.Get(ref skill))
			{
				skill.SetActive(true);
				skill.SetOwner(m_Owner);
				skill.StartSkill(m_Owner.TransformPosition, m_Owner.TransformRotation, true);
			}
		}
	}

	public void DeactiveSkill(TDCSkillController member) {
		member.SetActive(false);
		m_SkillPool.Set(member);
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
		m_SkillData.Owner = owner;
	}
}
