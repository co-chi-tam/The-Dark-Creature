using System;

public class TDCObjectPoolData
{
	private TDCEnum.EGameType m_GameType;
	private int m_Amount;

	public TDCEnum.EGameType GameType {
		get { return m_GameType; }
		set { m_GameType = value; }
	}

	public int Amount {
		get { return m_Amount; }
		set { m_Amount = value; }
	}

	public TDCObjectPoolData()
	{
		m_GameType = TDCEnum.EGameType.None;
		m_Amount = 0;
	}

}

