using UnityEngine;
using System.Collections;

public class TDCGroupData : TDCCreatureData {

	private float m_Radius;
	private TDCEnum.EGameType m_MemberType;
	private int m_CurrentMember;
	private int m_MinMember;
	private int m_MaxMember;
	private TDCEnum.EGroupType m_GroupType;
	private float m_TimeRespawnMember;

	public float Radius {
		get { return m_Radius; }
		set { m_Radius = value; }
	}

	public TDCEnum.EGameType MemberType
	{
		get { return m_MemberType; }
		set { m_MemberType = value; }
	}

	public int CurrentMember {
		get { return m_CurrentMember; }
		set { m_CurrentMember = value; }
	}

	public int MinMember {
		get { return m_MinMember; }
		set { m_MinMember = value; }
	}

	public int MaxMember {
		get { return m_MaxMember; }
		set { m_MaxMember = value; }
	}

	public TDCEnum.EGroupType GroupType {
		get { return m_GroupType; }
		set { m_GroupType = value; }
	}

	public float TimeRespawnMember {
		get { return m_TimeRespawnMember; }
		set { m_TimeRespawnMember = value; }
	}

	public TDCGroupData () : base ()
	{
		this.m_Radius = 0f;
		this.m_CurrentMember = 0;
		this.m_MinMember = 0;
		this.m_MaxMember = 0;
		this.m_GroupType = TDCEnum.EGroupType.None;
		this.m_TimeRespawnMember = 0f;
	}

	public static TDCGroupData Clone (TDCGroupData instance) {
		var tmp = new TDCGroupData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.GameType = instance.GameType;
		tmp.Radius = instance.Radius;
		tmp.MemberType = instance.MemberType;
		tmp.CurrentMember = instance.CurrentMember;
		tmp.MinMember = instance.MinMember;
		tmp.MaxMember = instance.MaxMember;
		tmp.GroupType = instance.GroupType;
		tmp.Icon = instance.Icon;
		tmp.TimeRespawnMember = instance.TimeRespawnMember;
		tmp.FSMPath = instance.FSMPath;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.MaxHP = instance.MaxHP;
		return tmp;
	}
}
