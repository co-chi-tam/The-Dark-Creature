
public class TDCBaseData : TDCPropertyReflection
{
    private int m_ID;
	private string m_Name;
	private string m_Description;
	private TDCEnum.EGameType m_GameType;
	private string m_Icon;
	private string[] m_ModelPath;
	private string m_EffectName;
	private object m_EffectValue;

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
	
	public string[] ModelPath {
		get { return m_ModelPath; }
		set { m_ModelPath = value; }
	}

	public string Icon {
		get { return m_Icon; }
		set { m_Icon = value; }
	}

	public string EffectName {
		get { return m_EffectName; }
		set { m_EffectName = value; }
	}
	
	public object EffectValue {
		get { return m_EffectValue; }
		set { m_EffectValue = value; }
	}

    public TDCBaseData()
    {
        this.m_ID           = 0;
        this.m_Name         = string.Empty;
        this.m_GameType     = TDCEnum.EGameType.None;
        this.m_Description  = string.Empty;
		this.m_Icon 		= string.Empty;
		this.m_ModelPath 	= new string[] {};
		this.m_EffectName	= "Default";
		this.m_EffectValue	= null;	
    }

	public static TDCBaseData Clone (TDCBaseData instance) {
		var tmp = new TDCBaseData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.ModelPath = instance.ModelPath;
		tmp.EffectName = instance.EffectName;
		tmp.EffectValue = instance.EffectValue;
		return tmp;
	}
}
