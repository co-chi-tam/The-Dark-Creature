
public class TDCWeaponData : TDCItemData {

	private int m_MinDamage;
	private int m_MaxDamage;
	private int m_Duration;
	private float m_AttackRange;
	private int m_DecreaseDuration;

	public int MinDamage {
		get { return m_MinDamage; }
		set { m_MinDamage = value; }
	}
	
	public int MaxDamage {
		get { return m_MaxDamage; }
		set { m_MaxDamage = value; }
	}

	public int Duration {
		get { return m_Duration; }
		set { m_Duration = value; }
	}

	public float AttackRange {
		get { return m_AttackRange; }
		set { m_AttackRange = value; }
	}

	public int DecreaseDuration {
		get { return m_DecreaseDuration; }
		set { m_DecreaseDuration = value; }
	}

	public TDCWeaponData () : base ()
	{
		m_MinDamage = 0;
		m_MaxDamage = 0;
		m_Duration = 0;
		m_AttackRange = 0f;
		m_DecreaseDuration = 0;
	}

	public static TDCWeaponData Clone (TDCWeaponData instance) {
		var tmp = new TDCWeaponData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.MinDamage = instance.MinDamage;
		tmp.MaxDamage = instance.MaxDamage;
		tmp.Duration = instance.Duration;
		tmp.AttackRange = instance.AttackRange;
		tmp.ItemType = instance.ItemType;
		tmp.Icon = instance.Icon;
		tmp.DecreaseDuration = instance.DecreaseDuration;
		return tmp;
	}
}
