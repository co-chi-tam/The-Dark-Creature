using System;
using System.Collections;

public class TDCSkillSlotComponent
{
	private TDCEntity m_Owner;
	private TDCSkillSlot[] m_ActiveSkills;
	private TDCSkillSlot[] m_PassiveSkills;

	public TDCSkillSlotComponent(TDCEntity owner)
	{
		m_Owner = owner;
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

	public void ActiveSkill(int index) {
		m_ActiveSkills[index].ActiveSkill();
	}

	public void UpdateComponent(float dt) {
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

}

