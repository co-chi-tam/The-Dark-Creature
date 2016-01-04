using UnityEngine;
using System.Collections;

public class TDCGroup : TDCEntity
{
	private TDCBaseGroupController m_Controller;
	private TDCGroupData m_Data;

	public TDCGroup(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCBaseGroupController;
		m_Data = data as TDCGroupData;
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

	public override int GetMinMember() {
		return m_Data.MinMember;
	}

	public override int GetMaxMember() {
		return m_Data.MaxMember;
	}

	public override float GetRadius() {
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

	public override void ReturnMember(TDCEntity member)
	{
		base.ReturnMember(member);
		m_Controller.ReturnMember (member);
	}

}

