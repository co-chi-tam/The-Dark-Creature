
public class TDCBaseData : TDCInfo
{
   
	private string m_Icon;
	private TDCEnum.EGameType m_GameType;

	public string Icon {
		get { return m_Icon; }
		set { m_Icon = value; }
	}

	public TDCEnum.EGameType GameType { 
		get { return m_GameType; } 
		set { m_GameType = value; } 
	}

	public TDCBaseData() : base()
    {
		this.m_GameType     = TDCEnum.EGameType.None;
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
