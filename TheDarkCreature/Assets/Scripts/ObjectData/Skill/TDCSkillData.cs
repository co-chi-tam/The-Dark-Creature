using System;

public class TDCSkillData : TDCBaseData
{
	private int m_SkillLevel;
	private TDCEnum.ESkillType m_SkillType;
	private int m_CostHealthPoint;
	private int m_CostHeatPoint;
	private int m_CostSanityPoint;
	private int m_CostHungerPoint;
	private float m_TimeDelay;
	private float m_TimeEffect;
	private float m_EffectPerTime;
	private float m_EffectRadius;
	private string m_EffectPath;
	private bool m_RepeatSkill;
	private bool m_AttachOwner;
	private bool m_AttachEnemy;
	private string m_TriggerEvent;

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

	public int CostHungerPoint
	{
		get { return m_CostHungerPoint; }
		set { m_CostHungerPoint = value; }
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

	public float EffectPerTime
	{
		get { return m_EffectPerTime; }
		set { m_EffectPerTime = value; }
	}

	public float EffectRadius
	{
		get { return m_EffectRadius; }
		set { m_EffectRadius = value; }
	}

	public string EffectPath
	{
		get { return m_EffectPath; }
		set { m_EffectPath = value; }
	}

	public bool RepeatSkill
	{
		get { return m_RepeatSkill; }
		set { m_RepeatSkill = value; }
	}

	public bool AttachOwner
	{
		get { return m_AttachOwner; }
		set { m_AttachOwner = value; }
	}

	public bool AttachEnemy
	{
		get { return m_AttachEnemy; }
		set { m_AttachEnemy = value; }
	}

	public string TriggerEnvent
	{
		get { return m_TriggerEvent; }
		set { m_TriggerEvent = value; }
	}

	public TDCSkillData(): base()
	{
		this.m_SkillLevel = 0;
		this.m_SkillType = TDCEnum.ESkillType.None;
		this.m_CostHealthPoint = 0;
		this.m_CostHeatPoint = 0;
		this.m_CostSanityPoint = 0;
		this.m_CostHungerPoint = 0;
		this.m_TimeDelay = 0f;
		this.m_TimeEffect = 0f;
		this.m_EffectPerTime = 0f;
		this.m_EffectRadius = 0f;
		this.m_EffectPath = string.Empty;
		this.m_RepeatSkill = false;
		this.m_AttachOwner = false;
		this.m_AttachEnemy = false;
		this.m_TriggerEvent = string.Empty;
	}

	public static TDCSkillData Parse(TDCSkillData instance) {
		var tmp = new TDCSkillData();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.GameType = instance.GameType;
		tmp.SkillLevel = instance.SkillLevel;
		tmp.SkillType = instance.SkillType;
		tmp.CostHealthPoint = instance.CostHealthPoint;
		tmp.CostHeatPoint = instance.CostHeatPoint;
		tmp.CostSanityPoint = instance.CostHeatPoint;
		tmp.CostHungerPoint = instance.CostHungerPoint;
		tmp.TimeDelay = instance.TimeDelay;
		tmp.TimeEffect = instance.TimeEffect;
		tmp.EffectPerTime = instance.EffectPerTime;
		tmp.EffectRadius = instance.EffectRadius;
		tmp.FSMPath = instance.FSMPath;
		tmp.EffectPath = instance.EffectPath;
		tmp.ModelPath = instance.ModelPath;
		tmp.RepeatSkill = instance.RepeatSkill;
		tmp.AttachOwner = instance.AttachOwner;
		tmp.AttachEnemy = instance.AttachEnemy;
		tmp.TriggerEnvent = instance.TriggerEnvent;
		return tmp;
	}
}

