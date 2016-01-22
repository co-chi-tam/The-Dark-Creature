using System;
using System.Collections;

public class TDCSkillSlotComponent
{
	private TDCEntity m_Owner;
	private TDCSkillSlot m_NormalSkill;
	private TDCSkillSlot[] m_PassiveSkills;

	public TDCSkillSlotComponent(TDCEntity owner)
	{
		m_Owner = owner;
	}

	public void CreateNormalSkillSlot(TDCEnum.EGameType activeSkill) {
		m_NormalSkill = new TDCSkillSlot(activeSkill, m_Owner);
	}

	public void CreatePassiveSkillSlot(params TDCEnum.EGameType[] passiveSkills) {
		m_PassiveSkills = new TDCSkillSlot[passiveSkills.Length];
		for (int i = 0; i < passiveSkills.Length; i++)
		{
			m_PassiveSkills[i] = new TDCSkillSlot(passiveSkills[i], m_Owner);
		}
	}

	public void ActiveSkill(int index) {
		if (index == 0)
		{
			m_NormalSkill.ActiveSkill();
		}
	}

	public void UpdateComponent(float dt) {
		if (m_Owner.GetActive() == false || m_NormalSkill == null)
			return;
		m_NormalSkill.UpdateSkill(dt);
		if (m_PassiveSkills == null)
			return;
		for (int i = 0; i < m_PassiveSkills.Length; i++)
		{
			m_PassiveSkills[i].UpdateSkill(dt);
		}
	}

}

