
public class TDCBaseData : TDCInfo
{
	private string m_Icon;
	private TDCEnum.EGameType m_GameType;
	private string m_FSMPath;
	private string[] m_ModelPath;

	public string Icon {
		get { return m_Icon; }
		set { m_Icon = value; }
	}

	public TDCEnum.EGameType GameType { 
		get { return m_GameType; } 
		set { m_GameType = value; } 
	}

	public string FSMPath {
		get { return m_FSMPath; }
		set { m_FSMPath = value; }
	}

	public string[] ModelPath {
		get { return m_ModelPath; }
		set { m_ModelPath = value; }
	}

	public TDCBaseData() : base()
    {
		this.m_GameType	= TDCEnum.EGameType.None;
		this.m_Icon		= string.Empty;
		this.m_FSMPath 	= string.Empty;
		this.m_ModelPath = null;
    }

	public static TDCBaseData Clone (TDCBaseData instance) {
		var tmp = new TDCBaseData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.Icon = instance.Icon;
		tmp.FSMPath = instance.FSMPath;
		tmp.ModelPath = instance.ModelPath;
		return tmp;
	}
}
