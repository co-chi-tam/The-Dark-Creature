using UnityEngine;
using System.Collections;

public class TDCSeasonData : TDCBaseData
{

	private float m_Duration;

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
		tmp.Duration = instance.Duration;
		return tmp;
	}

}



