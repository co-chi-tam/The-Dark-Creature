
public class TDCItemData : TDCBaseData {

	public TDCItemData () : base ()
	{

	}

	public static TDCItemData Clone (TDCItemData instance) {
		var tmp = new TDCItemData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.GameType = instance.GameType;
		tmp.Amount = instance.Amount;
		tmp.Icon = instance.Icon;
		tmp.ItemType = instance.ItemType;
		return tmp;
	}

	public static TDCItemData operator + (TDCItemData lh, TDCItemData rh) {
		var tmp = new TDCItemData ();
		tmp.ID = lh.ID;
		tmp.Name = lh.Name;
		tmp.GameType = lh.GameType;
		tmp.Description = lh.Description;
		tmp.ModelPath = lh.ModelPath;
		tmp.FSMPath = lh.FSMPath;
		tmp.Amount = lh.Amount + rh.Amount;
		tmp.Icon = lh.Icon;
		tmp.ItemType = lh.ItemType;
		return tmp;
	}
}
