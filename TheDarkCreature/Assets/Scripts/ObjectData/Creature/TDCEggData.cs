using UnityEngine;
using System.Collections;

public class TDCEggData : TDCGObjectData
{
	public TDCEnum.EGameType m_MemberType;

	public TDCEnum.EGameType MemberType
	{
		get { return m_MemberType; }
		set { m_MemberType = value; }
	}

	public static TDCEggData Clone (TDCEggData instance) {
		var tmp = new TDCEggData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.CreatureType = instance.CreatureType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.Icon = instance.Icon;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.MaxHP = instance.MaxHP;
		tmp.CurrentHeatPoint = instance.CurrentHeatPoint;
		tmp.MaxHeatPoint = instance.MaxHeatPoint;
		tmp.Duration = instance.Duration;
		tmp.DetectRange = instance.DetectRange;
		tmp.MemberType = instance.MemberType;
		tmp.Inventory = instance.Inventory;
		return tmp;
	}

}

