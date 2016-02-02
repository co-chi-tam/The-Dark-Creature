using UnityEngine;
using System.Collections;

public class TDCSeasonData : TDCBaseData
{

	private float[] m_MoistureSpring;
	private float[] m_MoistureSummer;
	private float[] m_MoistureAutumn;
	private float[] m_MoistureWinter;
	private float[] m_TemperatureSpring;
	private float[] m_TemperatureSummer;
	private float[] m_TemperatureAutumn;
	private float[] m_TemperatureWinter;
	private float m_Duration;

	public float[] MoistureSpring {
		get { return m_MoistureSpring; }
		set { m_MoistureSpring = value; } 
	}

	public float[] MoistureSummer {
		get { return m_MoistureSummer; }
		set { m_MoistureSummer = value; } 
	}

	public float[] MoistureAutumn {
		get { return m_MoistureAutumn; }
		set { m_MoistureAutumn = value; } 
	}

	public float[] MoistureWinter {
		get { return m_MoistureWinter; }
		set { m_MoistureWinter = value; } 
	}

	public float[] TemperatureSpring {
		get { return m_TemperatureSpring; }
		set { m_TemperatureSpring = value; } 
	}

	public float[] TemperatureSummer {
		get { return m_TemperatureSummer; }
		set { m_TemperatureSummer = value; } 
	}

	public float[] TemperatureAutumn {
		get { return m_TemperatureAutumn; }
		set { m_TemperatureAutumn = value; } 
	}

	public float[] TemperatureWinter {
		get { return m_TemperatureWinter; }
		set { m_TemperatureWinter = value; } 
	}

	public float Duration {
		get { return m_Duration; }
		set { m_Duration = value; } 
	}

	public TDCSeasonData():base()
	{
		m_Duration = float.MaxValue;
	}

	public static TDCSeasonData Clone(TDCSeasonData instance) {
		var tmp = new TDCSeasonData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPath = instance.ModelPath;
		tmp.MoistureSpring = instance.MoistureSpring;
		tmp.MoistureSummer = instance.MoistureSummer;
		tmp.MoistureAutumn = instance.MoistureAutumn;
		tmp.MoistureWinter = instance.MoistureWinter;
		tmp.TemperatureSpring = instance.TemperatureSpring;
		tmp.TemperatureSummer = instance.TemperatureSummer;
		tmp.TemperatureAutumn = instance.TemperatureAutumn;
		tmp.TemperatureWinter = instance.TemperatureWinter;
		tmp.Duration = instance.Duration;
		return tmp;
	}

}



