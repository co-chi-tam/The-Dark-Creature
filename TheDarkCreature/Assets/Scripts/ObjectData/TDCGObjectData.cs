
public class TDCGObjectData : TDCItemData{

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
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.GameType = instance.GameType;
		tmp.EffectName = instance.EffectName;
		tmp.EffectValue = instance.EffectValue;
		tmp.ItemType = instance.ItemType;
		tmp.Icon = instance.Icon;
		tmp.Duration = instance.Duration;
		return tmp;
	}
}
