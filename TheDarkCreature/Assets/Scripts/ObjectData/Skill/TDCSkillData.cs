using System;

public class TDCSkillData : TDCInfo
{
	private int m_SkillLevel;
	private TDCEnum.ESkillType m_SkillType;
	private int m_CostHealthPoint;
	private int m_CostHeatPoint;
	private int m_CostSanityPoint;
	private float m_TimeDelay;
	private float m_TimeEffect;
	private float m_EffectRadius;
	private string m_EffectPath;

	public int SkillLevel
	{
		get { return m_SkillLevel; }
		set { m_SkillLevel = value; }
	}

	public TDCEnum.ESkillType SkillType
	{
		get { return m_SkillType; }
		set { m_SkillType = value; }
	}

	public int CostHealthPoint
	{
		get { return m_CostHealthPoint; }
		set { m_CostHealthPoint = value; }
	}

	public int CostHeatPoint
	{
		get { return m_CostHeatPoint; }
		set { m_CostHeatPoint = value; }
	}

	public int CostSanityPoint
	{
		get { return m_CostSanityPoint; }
		set { m_CostSanityPoint = value; }
	}

	public float TimeDelay
	{
		get { return m_TimeDelay; }
		set { m_TimeDelay = value; }
	}

	public float TimeEffect
	{
		get { return m_TimeEffect; }
		set { m_TimeEffect = value; }
	}

	public string EffectPath
	{
		get { return m_EffectPath; }
		set { m_EffectPath = value; }
	}

	public float EffectRadius
	{
		get { return m_EffectRadius; }
		set { m_EffectRadius = value; }
	}

	public TDCSkillData(): base()
	{
		ID = 0;
		Name = string.Empty;
		Description = string.Empty;
		SkillLevel = 0;
		SkillType = TDCEnum.ESkillType.None;
		CostHealthPoint = 0;
		CostHeatPoint = 0;
		CostSanityPoint = 0;
		TimeDelay = 0f;
		TimeEffect = 0f;
		EffectRadius = 0f;
		EffectPath = string.Empty;
	}

	public static TDCSkillData Parse(TDCSkillData instance) {
		var tmp = new TDCSkillData();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.SkillLevel = instance.SkillLevel;
		tmp.SkillType = instance.SkillType;
		tmp.CostHealthPoint = instance.CostHealthPoint;
		tmp.CostHeatPoint = instance.CostHeatPoint;
		tmp.CostSanityPoint = instance.CostHeatPoint;
		tmp.TimeDelay = instance.TimeDelay;
		tmp.TimeEffect = instance.TimeEffect;
		tmp.EffectRadius = instance.EffectRadius;
		tmp.EffectPath = instance.EffectPath;
		return tmp;
	}
}

