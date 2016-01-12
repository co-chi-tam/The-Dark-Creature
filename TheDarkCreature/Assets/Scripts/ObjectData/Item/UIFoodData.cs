
public class UIFoodData : UIItemData {

	private TDCObjectProperty<int> m_FreshPoint;

	public TDCObjectProperty<int> FreshPoint
	{
		get { return m_FreshPoint; }
		set { m_FreshPoint = value; }
	}

	public UIFoodData () : base ()
	{
		m_FreshPoint = new TDCObjectProperty<int>("FreshPoint");

		RegisterProperty(m_FreshPoint);
	}

	public static UIFoodData Clone (UIFoodData instance) {
		var tmp = new UIFoodData ();
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
