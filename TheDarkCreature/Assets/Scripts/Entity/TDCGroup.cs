using UnityEngine;
using System.Collections;

public class TDCGroup : TDCEntity
{

	#region Properties

	private TDCBaseGroupController m_Controller;
	private TDCGroupData m_Data;
	private int m_HealthPoint = 0;
	private int m_DamageTake = 0;

	#endregion

	#region Contructor

	public TDCGroup(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCBaseGroupController;
		m_Data = data as TDCGroupData;
	}

	#endregion

	#region Main methods

	public override void ReturnMember(TDCEntity member)
	{
		base.ReturnMember(member);
		m_Controller.ReturnMember (member);
	}

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		if (attacker.GetActive())
		{
			m_DamageTake += damage;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);

		m_HealthPoint = GetHealth();
		if (m_Data != null) {
			m_HealthPoint -= m_DamageTake;
			m_DamageTake = 0;
			SetHealth(m_HealthPoint);
		}
	}

	#endregion

	#region Getter && Setter

	public override int GetHealth()
	{
		return m_Data.CurrentHP;
	}

	public override void SetHealth(int value)
	{
		m_Data.CurrentHP = value;
		var percentHP = m_Data.CurrentHP / m_Data.MaxHP * 100;
		CallBackEvent("OnHealthPoint" + percentHP);
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCBaseGroupController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCGroupData;
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

	public override int GetCurrentMember()
	{
		return m_Data.CurrentMember;
	}

	public override void SetCurrentMember(int value)
	{
		m_Data.CurrentMember = value;
	}

	public override int GetMinMember() {
		return m_Data.MinMember;
	}

	public override int GetMaxMember() {
		return m_Data.MaxMember;
	}

	public override float GetGroupRadius() {
		return m_Data.Radius;
	}

	public override TDCEnum.EGameType GetGroupMemberType() {
		return m_Data.MemberType;
	}

	public override float GetTimeRespawnMember()
	{
		return m_Data.TimeRespawnMember;
	}

	public override TDCEnum.EGameType GetMemberType()
	{
		return m_Data.MemberType;
	}

	public override TDCEnum.EGameType GetGameType()
	{
		return m_Data.GameType;
	}

	public override TDCEnum.EGroupType GetGroupType()
	{
		return m_Data.GroupType;
	}

	public override float GetColliderRadius()
	{
		return m_Controller.GetColliderRadius();
	}

	#endregion

}

