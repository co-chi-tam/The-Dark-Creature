
public class TDCItemData : TDCBaseData {
	
	private string m_EffectPath;
	private TDCEnum.EItemType m_ItemType;
    private TDCBaseController m_Owner;
	private TDCObjectProperty<int> m_Amount;

	public TDCEnum.EItemType ItemType {
		get { return m_ItemType; }
		set { m_ItemType = value; }
	}

    public TDCBaseController Owner {
		get { return m_Owner; }
		set { m_Owner = value; }
	}
	
	public int Amount {
		get { return m_Amount.Value; }
		set { m_Amount.SetValue (value < 0 ? 0 : value); }
	}
	
	public string EffectPath {
        get { return m_EffectPath; }
		set { m_EffectPath = value; }
	}

	public TDCItemData () : base ()
	{	
		this.m_ItemType		= TDCEnum.EItemType.None;
		this.m_Amount 		= new TDCObjectProperty<int> ("Amount");
		
		RegisterProperty (m_Amount);
	}

	public static TDCItemData Clone (TDCItemData instance) {
		var tmp = new TDCItemData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.GameType = instance.GameType;
		tmp.Amount = instance.Amount;
		tmp.Icon = instance.Icon;
		tmp.ItemType = instance.ItemType;
		tmp.Owner = instance.Owner;
		tmp.EffectPath = instance.EffectPath;
		return tmp;
	}

	public static TDCItemData operator + (TDCItemData lh, TDCItemData rh) {
		var tmp = new TDCItemData ();
		tmp.ID = lh.ID;
		tmp.Name = lh.Name;
		tmp.GameType = lh.GameType;
		tmp.Description = lh.Description;
		tmp.Amount = lh.Amount + rh.Amount;
		tmp.Icon = lh.Icon;
		tmp.ItemType = lh.ItemType;
		tmp.Owner = lh.Owner;
		tmp.EffectPath = lh.EffectPath;
		return tmp;
	}
}
