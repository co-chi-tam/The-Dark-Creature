
public class TDCResourceData : TDCCreatureData {

	public TDCResourceData () : base ()
	{

	}
	
	public static TDCResourceData Clone (TDCResourceData instance) {
		var tmp = new TDCResourceData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.Icon = instance.Icon;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.MaxHP = instance.MaxHP;
		return tmp;
	}
}
