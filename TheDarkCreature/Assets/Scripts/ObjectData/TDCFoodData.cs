
public class TDCFoodData : TDCItemData {

	private int m_RecoverPoint;

	public int RecoverPoint {
		get { return m_RecoverPoint; }
		set { m_RecoverPoint = value; }
	}

	public TDCFoodData () : base ()
	{
		m_RecoverPoint = 0;
	}

	public static TDCFoodData Clone (TDCFoodData instance) {
		var tmp = new TDCFoodData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.GameType = instance.GameType;
		tmp.RecoverPoint = instance.RecoverPoint;
		tmp.ItemType = instance.ItemType;
		tmp.Icon = instance.Icon;
		return tmp;
	}
}
