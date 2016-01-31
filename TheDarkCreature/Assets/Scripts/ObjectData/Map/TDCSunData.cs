using UnityEngine;
using System.Collections;

public class TDCSunData : TDCCreatureData
{
	
	public static TDCSunData Clone(TDCSunData instance) {
		var tmp = new TDCSunData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPath = instance.ModelPath;
		tmp.ActiveSkill = instance.ActiveSkill;
		return tmp;
	}

}

