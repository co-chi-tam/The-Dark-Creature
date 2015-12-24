
public class TDCFoodData : TDCItemData {

	private TDCObjectProperty<int> m_FreshPoint;

	public TDCObjectProperty<int> FreshPoint
	{
		get { return m_FreshPoint; }
		set { m_FreshPoint = value; }
	}

	public TDCFoodData () : base ()
	{
		m_FreshPoint = new TDCObjectProperty<int>("FreshPoint");

		RegisterProperty(m_FreshPoint);
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
		tmp.FreshPoint = instance.FreshPoint;
		return tmp;
	}
}
