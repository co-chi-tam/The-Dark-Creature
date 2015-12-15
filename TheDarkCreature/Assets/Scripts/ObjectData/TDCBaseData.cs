
public class TDCBaseData : TDCPropertyReflection
{
    private int m_ID;
	private string m_Name;
	private string m_Description;
	private string m_Icon;
	private TDCEnum.EGameType m_GameType;

    public int ID { 
		get { return m_ID; } 
		set { m_ID = value; } 
	}

    public string Name { 
		get { return m_Name; } 
		set { m_Name = value; } 
	}

    public TDCEnum.EGameType GameType { 
		get { return m_GameType; } 
		set { m_GameType = value; } 
	}

    public string  Description { 
		get { return m_Description; } 
		set { m_Description = value; } 
	}
	


	public string Icon {
		get { return m_Icon; }
		set { m_Icon = value; }
	}

    public TDCBaseData()
    {
        this.m_ID           = 0;
        this.m_Name         = string.Empty;
        this.m_GameType     = TDCEnum.EGameType.None;
        this.m_Description  = string.Empty;
		this.m_Icon 		= string.Empty;
    }

	public static TDCBaseData Clone (TDCBaseData instance) {
		var tmp = new TDCBaseData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		return tmp;
	}
}
