﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCDateTime : MonoBehaviour {

	#region Properties
	[SerializeField]
	private bool m_Active = false;
	[Space(10f)]
	[SerializeField]
	private Text m_DayText;
	[Space (10f)]
	[SerializeField]
	private Image m_SunImage;
	[SerializeField]
	private Image m_MoonImage;
	[SerializeField]
	private AnimationCurve m_DayNightCurve;
	[Space (10f)]
	[SerializeField]
	private float m_Speed 	= 10f;
	[SerializeField]
	private float m_Hour 	= 0f;
	[SerializeField]
	private float m_Day 	= 0f;

	[SerializeField]
	private TDCEnum.EGameSeason m_Season = TDCEnum.EGameSeason.Spring;

	private float m_Timer 		= 0f;
	private int m_SecondPerMinute = 60;
	private int m_MinutePerHour = 60;
	private int m_HourPerDay 	= 24;
	private int m_DayPerSeason 	= 30;
	private int m_SeasonCount	= 0;

	private int m_SecondPerHour 	= 0;

	private List<AlarmClockInfo> m_AlarmClocks;

	private float m_Tick = 0f;
	private int m_CurrentSeason = 0;

	public static float DeltaTime;
	public static float DeltaHour;
	public static float Hour;
	public static int DayFix;
	public static TDCEnum.EGameSeason Season;
	public static float MinMoisture = 0f;
	public static float MaxMoisture = 1f;
	public static float MinTemperature = 0f;
	public static float MaxTemperature = 1f;
	public static float Moisture;
	public static float Temperature;

	private static int m_Day1FullMoon 	= 14;
	private static int m_Day2FullMoon 	= 15;
	private static int m_Day3FullMoon 	= 16;

	private float m_OffsetTimeMoisture;
	private float m_OffsetTimeHeat;

	#endregion

	#region Internal Class

	private class AlarmClockInfo {
		
		public Action Callback;
		public bool Repeat;
		
		private int hour;
		private int day;
		private int dayPerSeason;
		private TDCEnum.EGameSeason season;
		private byte alarmFlag;
		
		private bool completed;
		
		public AlarmClockInfo ()
		{
			alarmFlag = 0;
			completed = false;
		}
		
		public void SetAlarmHour(int h) {
			this.hour = h;
			this.completed = false;
			alarmFlag |= 1 << 0;
		}
		
		public void SetAlarmDay(int d) {
			this.day = d;
			this.completed = false;
			alarmFlag |= 1 << 1;
		}
		
		public void SetAlarmSeason(TDCEnum.EGameSeason s) {
			this.season = s;
			this.completed = false;
			alarmFlag |= 1 << 2;
		}
		
		public bool AlarmComplete(int h, int d, TDCEnum.EGameSeason s) {
			bool complete = false;
			if (alarmFlag == (1 << 0)) {
				if (this.hour != h) {
					completed = false;
				}
				complete = this.hour == h && completed == false;
			} else if (alarmFlag == (1 << 1)) {
				if (this.day != d) {
					completed = false;
				}
				complete = this.day == d && completed == false;
			} else if (alarmFlag == (1 << 2)) {
				if (this.season != s) {
					completed = false;
				}
				complete = this.season == s && completed == false;
			} else if (alarmFlag == (1 << 1 | 1 << 2)) {
				if (this.day != (d % 30) || this.season != s) {
					completed = false;
				}
				complete = this.day == (d % 30) && this.season == s && completed == false;
			} else if (alarmFlag == (1 << 0 | 1 << 1 | 1 << 2)) {
				if (this.hour == h || this.day != d || this.season != s) {
					completed = false;
				}
				complete = this.hour == h && this.day == d && this.season == s && completed == false;
			}
			if (complete) {
				alarmFlag = Repeat == false ? (byte)0 : alarmFlag;
				completed = true;
				if (Callback != null) {
					Callback();
				}
			}
			return complete;
		}
	}

	#endregion

	#region Implementation MonoBehaviour

	void Awake () {
		m_Active 			= m_SunImage != null && m_MoonImage != null && m_DayText != null;
		m_SecondPerHour 	= m_SecondPerMinute * m_MinutePerHour;
		m_SeasonCount 		= Enum.GetNames (typeof(TDCEnum.EGameSeason)).Length;
		m_AlarmClocks 		= new List<AlarmClockInfo> ();
	}

	void Start() {
		SetHour (8);
		SetSeason(TDCEnum.EGameSeason.Spring); 
		SetupImage (m_SunImage, 0, 1f);
		SetupImage (m_MoonImage, 1, 0f);
		m_DayText.text = "Day " + m_Day.ToString();
		DeltaTime = m_Tick;

		m_OffsetTimeMoisture = UnityEngine.Random.Range(10, 9999);
		m_OffsetTimeHeat = 9999 - m_OffsetTimeMoisture;
	}
	
	void Update () {
		if (m_Active == false)
		{
			this.gameObject.SetActive(false);
			return;
		}
		m_Tick		= Time.deltaTime * m_Speed;
		m_Timer 	+= m_Tick;
		Hour 		= m_Hour = m_Timer / m_SecondPerHour;
		DeltaTime 	= m_Tick;

		var day = m_HourPerDay * m_Day + m_Hour;
		var nextDay = m_HourPerDay * (m_Day + 1);
		if (day > nextDay) {
			m_Day ++;
			m_Timer = 0f;
			m_Hour = 0f;
			m_DayText.text = "Day " + (m_Day + 1).ToString();
			DayFix = (int)m_Day % m_DayPerSeason;
		}

		m_Season	= (TDCEnum.EGameSeason)(((m_Day / m_DayPerSeason) + m_CurrentSeason) % m_SeasonCount);
		Season 		= m_Season;

		for (int i = 0; i < m_AlarmClocks.Count; i++) {
			if (m_AlarmClocks[i].AlarmComplete ((int) m_Hour, (int) m_Day, m_Season)) {
				if (m_AlarmClocks[i].Repeat == false) {
					m_AlarmClocks.RemoveAt (i);
					m_AlarmClocks.TrimExcess();
				}
			}
		}

		var timeMoisture = (Time.time + m_OffsetTimeMoisture) / m_Speed;
		var timeTemperature = (Time.time + m_OffsetTimeHeat) / m_Speed;
		Moisture = ((MaxMoisture - MinMoisture) * Mathf.PerlinNoise(timeMoisture , timeMoisture)) + MinMoisture;
		Temperature = ((MaxTemperature - MinTemperature) * Mathf.PerlinNoise(timeTemperature, timeTemperature)) + MinTemperature;
	}

	void LateUpdate() {
		DeltaTime	= m_Tick;
		var value 	= m_Hour / 24;
		m_SunImage.fillAmount = DeltaHour = m_DayNightCurve.Evaluate (value);
		m_MoonImage.fillAmount 	= 1f - m_DayNightCurve.Evaluate (value);
	}

	#endregion

	#region Main methods

	public static bool IsDayTime() {
		return Hour > 5f && Hour < 18f;
	}

	public static bool IsNightTime() {
		return (Hour > 18f && Hour < 24f) || (Hour > 0f && Hour < 5f);
	}

	public static bool IsMidNightTime() {
		return (Hour > 21f && Hour < 24f) || (Hour > 0f && Hour < 2f);
	}

	public static bool IsMidDayTime() {
		return (Hour > 10f && Hour < 14f);
	}

	public static bool IsSeason(TDCEnum.EGameSeason season) {
		return Season == season;
	}

	public static bool IsFullMoon() {
		return (DayFix == m_Day1FullMoon || DayFix == m_Day2FullMoon || DayFix == m_Day3FullMoon) && IsMidNightTime();
	}

	public static bool IsRainy() {
		return Moisture > 0.8f && Moisture < 1f;
	}

	public static bool IsOverHeat() {
		return Temperature > 0.8f && Temperature < 1f;
	}

	public static bool IsWindy() {
		return Moisture < 0.25f || Temperature < 0.25f ;
	}

	public static bool IsSnowy() {
		return Temperature < 0.25f || Moisture < 0.25f;
	}

	#endregion

	#region Getter & Setter

	private void SetupImage(Image image, int origin, float value) {
		image.type = Image.Type.Filled;
		image.fillMethod = Image.FillMethod.Horizontal;
		image.fillOrigin = origin;
		image.fillAmount = value;
	}

	public void SetHour(int hour) {
		m_Timer = m_SecondPerHour * hour;
	}

	public void SetDay(int day) {
		m_Day = day;
	}

	public void SetHourAlarmClock(int h, Action complete, bool repeat = false) {
		var alarm = new AlarmClockInfo ();
		alarm.SetAlarmHour (h);
		alarm.Callback = complete;
		alarm.Repeat = repeat;
		m_AlarmClocks.Add (alarm);
	}

	public void SetDayAlarmClock(int d, Action complete) {
		var alarm = new AlarmClockInfo ();
		alarm.SetAlarmDay (d);
		alarm.Callback = complete;
		alarm.Repeat = false;
		m_AlarmClocks.Add (alarm);
	}

	public void SetSeasonAlarmClock(TDCEnum.EGameSeason s, Action complete, bool repeat = false) {
		var alarm = new AlarmClockInfo ();
		alarm.SetAlarmSeason (s);
		alarm.Callback = complete;
		alarm.Repeat = repeat;
		m_AlarmClocks.Add (alarm);
	}

	public void SetDayPerSeasonAlarmClock(int d, TDCEnum.EGameSeason s, Action complete, bool repeat = false) {
		var alarm = new AlarmClockInfo ();
		alarm.SetAlarmDay (d);
		alarm.SetAlarmSeason (s);
		alarm.Callback = complete;
		alarm.Repeat = repeat;
		m_AlarmClocks.Add (alarm);
	}

	public int Getday() {
		return (int) m_Day;
	}

	public void SetSeason(TDCEnum.EGameSeason value) {
		m_CurrentSeason = (int)value;
	}

	#endregion

}



