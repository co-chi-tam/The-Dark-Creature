using UnityEngine;
using System;
using System.Collections;

public class TDCDateTime : MonoBehaviour {

	[SerializeField]
	private bool m_Active = false;
	[SerializeField]
	private float m_Speed 	= 10f;
	[SerializeField]
	private float m_Second 	= 0f;
	[SerializeField]
	private float m_Minute 	= 0f;
	[SerializeField]
	private float m_Hour 	= 0f;
	[SerializeField]
	private float m_Day 	= 0f;
	[SerializeField]
	private TDCEnum.EGameSeason m_Season = TDCEnum.EGameSeason.Spring;
	[SerializeField]
	private float m_Year 	= 0f;

	private float m_Timer 		= 0f;
	private int m_TickPerSecond = 60;
	private int m_SecondPerMinute = 60;
	private int m_MinutePerHour = 60;
	private int m_HourPerDay 	= 24;
	private int m_DayPerSeason 	= 30;
	private int m_DayPerYear 	= 120;
	private int m_SeasonCount	= 0;

	void Start () {
		m_Active = false;
		m_SeasonCount = Enum.GetNames (typeof(TDCEnum.EGameSeason)).Length;
	}
	
	void Update () {
		if (m_Active == false)
			return;
		m_Timer 	+= Time.deltaTime * m_Speed;
		m_Second 	= m_Timer % m_SecondPerMinute;
		m_Minute 	= m_Timer / m_SecondPerMinute % m_MinutePerHour;
		m_Hour 		= m_Timer / (m_SecondPerMinute * m_MinutePerHour) % m_HourPerDay;
		m_Day 		= m_Timer / (m_SecondPerMinute * m_MinutePerHour * m_HourPerDay) % m_DayPerSeason;
		m_Season	= (TDCEnum.EGameSeason)(m_Timer / (m_SecondPerMinute * m_MinutePerHour * m_HourPerDay * m_DayPerSeason) % m_SeasonCount);
		m_Year		= m_Timer / (m_SecondPerMinute * m_MinutePerHour * m_HourPerDay * m_DayPerSeason * m_DayPerYear);
	}
}



