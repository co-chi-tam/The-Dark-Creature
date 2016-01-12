using System;
using System.Collections;

public class TDCItemData : TDCCreatureData
{
	private TDCEnum.EItemType m_ItemType;

	public TDCEnum.EItemType ItemType
	{
		get { return m_ItemType; }
		set { m_ItemType = value; }
	}

	public static TDCItemData Clone (TDCItemData instance) {
		var tmp = new TDCItemData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.CreatureType = instance.CreatureType;
		tmp.ItemType = instance.ItemType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPath = instance.ModelPath;
		return tmp;
	}
}

