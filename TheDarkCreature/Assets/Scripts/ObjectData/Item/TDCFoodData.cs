
public class TDCFoodData : TDCItemData {

	public TDCFoodData () : base ()
	{

	}

	public static TDCFoodData Clone (TDCFoodData instance) {
		var tmp = new TDCFoodData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.GameType = instance.GameType;
		tmp.EffectPath = instance.EffectPath;
		tmp.ItemType = instance.ItemType;
		tmp.Icon = instance.Icon;
		return tmp;
	}
}
