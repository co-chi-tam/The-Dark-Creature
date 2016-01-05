
public class TDCGObjectData : TDCCreatureData{

	private float m_Duration;

	public float Duration {
		get { return m_Duration; }
		set { m_Duration = value; } 
	}

	public TDCGObjectData () : base()
	{
		
	}

	public static TDCGObjectData Clone (TDCGObjectData instance) {
		var tmp = new TDCGObjectData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.Icon = instance.Icon;
		tmp.Duration = instance.Duration;
		tmp.CreatureType = instance.CreatureType;
		tmp.Inventory = instance.Inventory;
		return tmp;
	}
}
