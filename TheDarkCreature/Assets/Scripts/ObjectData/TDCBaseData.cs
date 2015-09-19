
public class TDCBaseData : TDCPropertyReflection
{
    private int m_ID;
    private string m_Name;
	private TDCEnum.EGameType m_GameType;
	private TDCEnum.ECreatureType m_CreaturType;
	private TDCEnum.EItemType m_ItemType;
    private string m_Description;
	private string[] m_ModelPath;
	private string m_FSMPath;
	private string m_Icon;
	private TDCBaseData[] m_Inventory;
	private TDCObjectProperty<int> m_Amount;

    public int ID { get { return m_ID; } set { m_ID = value; } }
    public string  Name { get { return m_Name; } set { m_Name = value; } }
    public TDCEnum.EGameType GameType { get { return m_GameType; } set { m_GameType = value; } }
    public string  Description { get { return m_Description; } set { m_Description = value; } }

	public TDCEnum.EItemType ItemType {
		get { return m_ItemType; }
		set { m_ItemType = value; }
	}

	public int Amount {
		get { return m_Amount.Value; }
		set { m_Amount.SetValue (value < 0 ? 0 : value); }
	}

	public TDCBaseData[] Inventory {
		get { return m_Inventory; }
		set { m_Inventory = value; }
	}

	public string Icon {
		get { return m_Icon; }
		set { m_Icon = value; }
	}

	public string[] ModelPath {
		get { return m_ModelPath; }
		set { m_ModelPath = value; }
	}
	
	public string FSMPath {
		get { return m_FSMPath; }
		set { m_FSMPath = value; }
	}
	
	public TDCEnum.ECreatureType CreatureType {
		get { return m_CreaturType; }
		set { m_CreaturType = value; }
	}

    public TDCBaseData()
    {
        this.m_ID           = 0;
        this.m_Name         = string.Empty;
        this.m_GameType     = TDCEnum.EGameType.None;
		this.m_ItemType		= TDCEnum.EItemType.None;
		m_CreaturType 		= TDCEnum.ECreatureType.None;
        this.m_Description  = string.Empty;
		this.m_ModelPath 	= new string[] {};
		this.m_FSMPath = string.Empty;
		this.m_Icon = string.Empty;
		this.m_Inventory = new TDCBaseData[20];
		this.m_Amount = new TDCObjectProperty<int> ("Amount");

		RegisterProperty (m_Amount);
    }

	public static TDCBaseData Clone (TDCBaseData instance) {
		var tmp = new TDCBaseData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPath = instance.ModelPath;
		tmp.Icon = instance.Icon;
		return tmp;
	}
}
