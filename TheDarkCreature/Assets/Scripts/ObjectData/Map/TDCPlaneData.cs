using UnityEngine;
using System.Collections;

public class TDCPlaneData : TDCBaseData
{

	private string m_SpringTexture;
	private string m_SummerTexture;
	private string m_AutumnTexture;
	private string m_WinterTexture;

	public string SpringTexture
	{
		get { return m_SpringTexture; }
		set { m_SpringTexture = value; }
	}

	public string SummerTexture
	{
		get { return m_SummerTexture; }
		set { m_SummerTexture = value; }
	}

	public string AutumnTexture
	{
		get { return m_AutumnTexture; }
		set { m_AutumnTexture = value; }
	}

	public string WinterTexture
	{
		get { return m_WinterTexture; }
		set { m_WinterTexture = value; }
	}

	public TDCPlaneData():base()
	{
		m_SpringTexture = string.Empty;
		m_SummerTexture = string.Empty;
		m_AutumnTexture = string.Empty;
		m_WinterTexture = string.Empty;
	}

	public static TDCPlaneData Clone(TDCPlaneData instance) {
		var tmp = new TDCPlaneData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPath = instance.ModelPath;
		tmp.SpringTexture = instance.SpringTexture;
		tmp.SummerTexture = instance.SummerTexture;
		tmp.AutumnTexture = instance.AutumnTexture;
		tmp.WinterTexture = instance.WinterTexture;
		return tmp;
	}

}

