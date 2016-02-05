using System;
using System.Collections;
using System.Collections.Generic;

public class TDCSkillSlotComponent
{
	private TDCEntity m_Owner;
	private TDCSkillSlot[] m_ActiveSkills;
	private TDCSkillSlot[] m_PassiveSkills;

	private Queue<int> m_SkillList;

	public TDCSkillSlotComponent(TDCEntity owner)
	{
		m_Owner = owner;

		m_SkillList = new Queue<int>();
	}

	public void CreateActiveSkillSlot(params TDCEnum.EGameType[] activeSkill) {
		m_ActiveSkills = new TDCSkillSlot[activeSkill.Length];
		for (int i = 0; i < activeSkill.Length; i++)
		{
			m_ActiveSkills[i] = new TDCSkillSlot(activeSkill[i], m_Owner);
		}
	}

	public void CreatePassiveSkillSlot(params TDCEnum.EGameType[] passiveSkills) {
		m_PassiveSkills = new TDCSkillSlot[passiveSkills.Length];
		for (int i = 0; i < passiveSkills.Length; i++)
		{
			m_PassiveSkills[i] = new TDCSkillSlot(passiveSkills[i], m_Owner);
		}
	}

	public void AddSkillChain(int index) {
		if (index > m_ActiveSkills.Length - 1)
			return;
		if (!m_SkillList.Contains(index)) {
			m_SkillList.Enqueue (index);
		}
	}

	public void ActiveSkill() {
		if (m_SkillList.Count != 0)
		{
			var indexSkill = m_SkillList.Dequeue();
			m_ActiveSkills[indexSkill].ActiveSkill();
		}
	}

	public void UpdateComponent(float dt) {
		ActiveSkill();

		if (m_Owner.GetActive() == false || m_ActiveSkills == null)
			return;
		for (int i = 0; i < m_ActiveSkills.Length; i++)
		{
			m_ActiveSkills[i].UpdateSkill(dt);
		}

		if (m_PassiveSkills == null)
			return;
		for (int i = 0; i < m_PassiveSkills.Length; i++)
		{
			m_PassiveSkills[i].UpdateSkill(dt);
		}
	}

	public float GetEffectRange() {
		if (m_SkillList.Count == 0)
			return 1f;
		var index = m_SkillList.Peek();
		return m_ActiveSkills[index].GetEffectRange();
	}

}

