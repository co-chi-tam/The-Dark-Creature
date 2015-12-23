using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TDCPlayerData : TDCCreatureData {

	private TDCObjectProperty<int> m_CurrentHeatPoint;
	private TDCObjectProperty<int> m_MaxHeatPoint;
	private TDCObjectProperty<int> m_CurrentHungerPoint;
	private TDCObjectProperty<int> m_MaxHungerPoint;
	private TDCObjectProperty<int> m_CurrentSanityPoint;
	private TDCObjectProperty<int> m_MaxSanityPoint;
	private TDCObjectProperty<int> m_Level;

	public int Level {
		get { return m_Level.Value; }
		set { m_Level.Value = value; }
	}

	public int CurrentHeatPoint {
		get { return m_CurrentHeatPoint.Value; }
		set { m_CurrentHeatPoint.Value = value; }
	}

	public int MaxHeatPoint {
		get { return m_MaxHeatPoint.Value; }
		set { m_MaxHeatPoint.Value = value; }
	}

	public int CurrentHungerPoint {
		get { return m_CurrentHungerPoint.Value; }
		set { m_CurrentHungerPoint.Value = value; }
	}

	public int MaxHungerPoint {
		get { return m_MaxHungerPoint.Value; }
		set { m_MaxHungerPoint.Value = value; }
	}

	public int CurrentSanityPoint {
		get { return m_CurrentSanityPoint.Value; }
		set { m_CurrentSanityPoint.Value = value; }
	}

	public int MaxSanityPoint {
		get { return m_MaxSanityPoint.Value; }
		set { m_MaxSanityPoint.Value = value; }
	}

	public TDCPlayerData () : base ()
	{
		m_Level = new TDCObjectProperty<int> ("Level");
		m_CurrentHeatPoint = new TDCObjectProperty<int>("CurrentHeatPoint");
		m_MaxHeatPoint = new TDCObjectProperty<int>("MaxHeatPoint");
		m_CurrentHungerPoint = new TDCObjectProperty<int>("CurrentHungerPoint");
		m_MaxHungerPoint = new TDCObjectProperty<int>("MaxHungerPoint");
		m_CurrentSanityPoint = new TDCObjectProperty<int>("CurrentSanityPoint");
		m_MaxSanityPoint = new TDCObjectProperty<int>("MaxSanityPoint");
		RegisterProperty (m_Level);
		RegisterProperty(m_CurrentHeatPoint);
		RegisterProperty(m_MaxHeatPoint);
		RegisterProperty(m_CurrentHungerPoint);
		RegisterProperty(m_MaxHungerPoint);
		RegisterProperty(m_CurrentSanityPoint);
		RegisterProperty(m_MaxSanityPoint);
	}

	public static TDCPlayerData Clone(TDCPlayerData instance) {
		var tmp = new TDCPlayerData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.AttackRange = instance.AttackRange;
		tmp.DetectRange = instance.DetectRange;
		tmp.WalkSpeed = instance.WalkSpeed;
		tmp.RunSpeed = instance.RunSpeed;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.MaxHP = instance.MaxHP;
		tmp.MinDamage = instance.MinDamage;
		tmp.MaxDamage = instance.MaxDamage;
		tmp.CurrentHeatPoint = instance.CurrentHeatPoint;
		tmp.MaxHeatPoint = instance.MaxHeatPoint;
		tmp.CurrentHungerPoint = instance.CurrentHungerPoint;
		tmp.MaxHungerPoint = instance.MaxHungerPoint;
		tmp.CurrentSanityPoint = instance.CurrentSanityPoint;
		tmp.MaxSanityPoint = instance.MaxSanityPoint;
		tmp.Level = instance.Level;
		tmp.Inventory = instance.Inventory;
		return tmp;
	}

	public static TDCPlayerData operator + (TDCPlayerData lh, TDCPlayerData rh) {
		var tmp = new TDCPlayerData ();
		tmp.ID = lh.ID;
		tmp.Name = lh.Name;
		tmp.GameType = lh.GameType;
		tmp.Description = lh.Description;
		tmp.ModelPath = lh.ModelPath;
		tmp.WalkSpeed = (lh.WalkSpeed + rh.WalkSpeed) / 2;
		tmp.RunSpeed = (lh.RunSpeed + rh.RunSpeed) / 2;
		tmp.CurrentHP = (lh.CurrentHP + rh.CurrentHP) / 2;
		tmp.MaxHP = (lh.MaxHP + rh.MaxHP) / 2;
		tmp.MinDamage = (lh.MinDamage + rh.MinDamage) / 2;
		tmp.MaxDamage = (lh.MaxDamage + rh.MaxDamage) / 2;
		tmp.CurrentHeatPoint = (lh.CurrentHeatPoint + rh.CurrentHeatPoint) / 2;
		tmp.MaxHeatPoint = (lh.MaxHeatPoint + rh.MaxHeatPoint) / 2;
		tmp.CurrentHungerPoint = (lh.CurrentHungerPoint + rh.CurrentHungerPoint) / 2;
		tmp.MaxHungerPoint = (lh.MaxHungerPoint + rh.MaxHungerPoint) / 2;
		tmp.CurrentSanityPoint = (lh.CurrentSanityPoint + rh.CurrentSanityPoint) / 2;
		tmp.MaxSanityPoint = (lh.MaxSanityPoint + rh.MaxSanityPoint) / 2;
		tmp.Level = lh.Level + 1;
		tmp.Inventory = lh.Inventory;
		tmp.Icon = lh.Icon;
		return tmp;
	}
}
