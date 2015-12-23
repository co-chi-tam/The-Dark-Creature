using System;
using UnityEngine;

public class MapObjectData
{
	private int m_ID;
	private TDCEnum.EGameType m_GameType;
	private Vector3 m_Position;
	private Quaternion m_Rotation;

	public int ID
	{
		get
		{
			return m_ID;
		}
		set
		{
			m_ID = value;
		}
	}

	public TDCEnum.EGameType GameType
	{
		get
		{
			return m_GameType;
		}
		set
		{
			m_GameType = value;
		}
	}

	public Vector3 Position
	{
		get
		{
			return m_Position;
		}
		set
		{
			m_Position = value;
		}
	}

	public Quaternion Rotation
	{
		get
		{
			return m_Rotation;
		}
		set
		{
			m_Rotation = value;
		}
	}

	public MapObjectData()
	{
		m_ID = 0;
		m_GameType = TDCEnum.EGameType.None;
		m_Position = Vector3.zero;
		m_Rotation = Quaternion.identity;
	}
}

