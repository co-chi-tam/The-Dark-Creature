
public class TDCEnviromentData : TDCCreatureData {

	public TDCEnviromentData () : base ()
	{

	}
	
	public static TDCEnviromentData Clone (TDCEnviromentData instance) {
		var tmp = new TDCEnviromentData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.Icon = instance.Icon;
		tmp.CreatureType = instance.CreatureType;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.CurrentHeatPoint = instance.CurrentHeatPoint;
		tmp.MaxHeatPoint = instance.MaxHeatPoint;
		tmp.MaxHP = instance.MaxHP;
		tmp.Inventory = instance.Inventory;
		tmp.IsShine = instance.IsShine;
		return tmp;
	}
}
