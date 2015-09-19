using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TDCPlayerData : TDCCreatureData {

	private TDCObjectProperty<int> m_Level;

	public int Level {
		get { return m_Level.Value; }
		set { m_Level.Value = value; }
	}


	public TDCPlayerData () : base ()
	{
		m_Level = new TDCObjectProperty<int> ("Level");

		RegisterProperty (m_Level);
	}

	public static TDCPlayerData Clone(TDCPlayerData instance) {
		var tmp = new TDCPlayerData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.WalkSpeed = instance.WalkSpeed;
		tmp.RunSpeed = instance.RunSpeed;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.MaxHP = instance.MaxHP;
		tmp.MinDamage = instance.MinDamage;
		tmp.MaxDamage = instance.MaxDamage;
		tmp.Level = instance.Level;
		tmp.Inventory = instance.Inventory;
		tmp.CreatureType = instance.CreatureType;
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
		tmp.Level = lh.Level + 1;
		tmp.Inventory = lh.Inventory;
		tmp.CreatureType = lh.CreatureType;
		tmp.Icon = lh.Icon;
		return tmp;
	}
}
