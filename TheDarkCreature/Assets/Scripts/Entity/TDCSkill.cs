using UnityEngine;
using System.Collections;

public class TDCSkill : TDCCreature
{

	protected TDCSkillSlot m_Slot;
	protected TDCEntity m_Owner;

	private TDCSkillController m_Controller;
	private TDCSkillData m_Data;

	public TDCSkill(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCSkillController;
		m_Data = data as TDCSkillData;
	}

	public override void SetData(TDCBaseData data) {
		base.SetData (data);
		m_Data = data as TDCSkillData;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCSkillController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetActive(bool value)
	{
		base.SetActive(value);
		if (m_Controller != null && m_Controller.gameObject != null)
		{
			m_Controller.gameObject.SetActive(value);
		}
	}

	public override string GetFSMPath()
	{
		return m_Data.FSMPath;
	}

	public override string GetEffectPath()
	{
		return m_Data.EffectPath;
	}

	public override TDCEnum.EGameType GetGameType()
	{
		return m_Data.GameType;
	}

	public override void SetSlot(TDCSkillSlot slot)
	{
		m_Slot = slot;
	}

	public override TDCEnum.ESkillType GetSkillType() {
		return m_Data.SkillType;
	}

	public override float GetTimeDelay() {
		return m_Data.TimeDelay;
	}

	public override float GetTimeEffect() {
		return m_Data.TimeEffect;
	}

	public override float GetEffectPerTime() {
		return m_Data.EffectPerTime;
	}

	public override float GetEffectRadius() {
		return m_Data.EffectRadius;
	}

	public override bool GetRepeatSkill()
	{
		return m_Data.RepeatSkill;
	}

	public override bool GetAttachOwner()
	{
		return m_Data.AttachOwner;
	}

	public override bool GetAttachEnemy()
	{
		return m_Data.AttachEnemy;
	}

	public override TDCEntity GetOwnerEntity() {
		return m_Owner;
	}

	public override void SetOwnerEntity(TDCEntity owner) {
		m_Owner = owner;
	}

	public override int GetDamage()
	{
		return m_Owner != null ? m_Owner.GetDamage() : 0;
	}

}

