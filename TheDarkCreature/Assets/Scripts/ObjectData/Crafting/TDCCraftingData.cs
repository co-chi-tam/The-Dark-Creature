using UnityEngine;
using System.Collections;

public class TDCCraftingData : TDCBaseData
{

	private int m_Amount;
	private TDCEnum.ECraftingTab m_CraftingTab;
	private TDCElement[] m_Elements;

	public int Amount
	{
		get { return m_Amount; }
		set { m_Amount = value;}
	}

	public TDCEnum.ECraftingTab CraftingTab
	{
		get { return m_CraftingTab; }
		set { m_CraftingTab = value; }
	}

	public TDCElement[] Elements
	{
		get { return m_Elements; }
		set { m_Elements = value; }
	}

	public TDCCraftingData() : base()
	{
		m_CraftingTab = TDCEnum.ECraftingTab.None;
		m_Elements = null;
	}

	public static TDCCraftingData Clone (TDCCraftingData instance) {
		var tmp = new TDCCraftingData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.GameType = instance.GameType;
		tmp.Amount = instance.Amount;
		tmp.CraftingTab = instance.CraftingTab;
		tmp.Elements = instance.Elements;
		return tmp;
	}

}

