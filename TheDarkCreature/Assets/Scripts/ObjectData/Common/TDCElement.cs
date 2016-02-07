using UnityEngine;
using System.Collections;

public class TDCElement : TDCInfo
{

	private int m_ID;
	private TDCEnum.EGameType m_GameType;
	private int m_Amount;

	public int ID
	{
		get { return m_ID; }
		set { m_ID = value; }
	}

	public TDCEnum.EGameType GameType
	{
		get { return m_GameType; }
		set { m_GameType = value; }
	}

	public int Amount
	{
		get { return m_Amount; }
		set { m_Amount = value; }
	}

	public TDCElement()
	{
		m_ID = 0;
		m_GameType = TDCEnum.EGameType.None;
		m_Amount = 0;
	}

}

