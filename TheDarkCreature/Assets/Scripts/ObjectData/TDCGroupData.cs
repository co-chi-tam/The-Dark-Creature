﻿using UnityEngine;
using System.Collections;

public class TDCGroupData : TDCBaseData {

	private float m_Radius;
	private int m_MinMember;
	private int m_MaxMember;
	private TDCEnum.EGroupType m_GroupType;
	private float m_TimeRespawnMember;

	public float Radius {
		get { return m_Radius; }
		set { m_Radius = value; }
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
		m_Radius = 0f;
		m_MinMember = 0;
		m_MaxMember = 0;
		m_GroupType = TDCEnum.EGroupType.None;
		m_TimeRespawnMember = 0f;
	}

	public static TDCGroupData Clone (TDCGroupData instance) {
		var tmp = new TDCGroupData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.GameType = instance.GameType;
		tmp.Radius = instance.Radius;
		tmp.MinMember = instance.MinMember;
		tmp.MaxMember = instance.MaxMember;
		tmp.GroupType = instance.GroupType;
		tmp.Icon = instance.Icon;
		tmp.TimeRespawnMember = instance.TimeRespawnMember;
		return tmp;
	}
}
