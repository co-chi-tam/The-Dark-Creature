
using System.Collections.Generic;

public class TDCCreatureData : TDCBaseData {

	private TDCEnum.ECreatureType m_CreatureType;
    private TDCItemController[] m_Inventory;
	private float m_RotationSpeed;
	private TDCObjectProperty<float> m_MoveSpeed;
	private TDCObjectProperty<int> m_CurrentHP;
	private TDCObjectProperty<int> m_MaxHP;
	private TDCObjectProperty<int> m_MinDamage;
	private TDCObjectProperty<int> m_MaxDamage;
	private TDCObjectProperty<int> m_CurrentHeatPoint;
	private TDCObjectProperty<int> m_MaxHeatPoint;

	private float m_DetectRange;
	private float m_AttackRange;
	private List<TDCEnum.EGameType> m_TypeEnemies;
	private List<TDCEnum.EGameType> m_TypeFoods;

    public TDCItemController[] Inventory {
		get { return m_Inventory; }
		set { m_Inventory = value; }
	}

	public float MoveSpeed {
		get { return m_MoveSpeed.GetValue(); }
		set { m_MoveSpeed.SetValue (value); }
	}

	public float RotationSpeed {
		get { return m_RotationSpeed; }
		set { m_RotationSpeed = value; }
	}

	public int MinDamage {
		get { return m_MinDamage.GetValue(); }
		set { m_MinDamage.SetValue (value); }
	}

	public int MaxDamage {
		get { return m_MaxDamage.GetValue(); }
		set { m_MaxDamage.SetValue(value); }
	}

	public int CurrentHP {
		get { return m_CurrentHP.GetValue(); }
		set { m_CurrentHP.SetValue (value); }
	}

	public int MaxHP {
		get { return m_MaxHP.GetValue(); }
		set { m_MaxHP.SetValue (value); }
	}

	public float DetectRange {
		get { return m_DetectRange; }
		set { m_DetectRange = value; }
	}

	public float AttackRange {
		get { return m_AttackRange; }
		set { m_AttackRange = value; }
	}

	public List<TDCEnum.EGameType> TypeEnemies {
		get { return m_TypeEnemies; }
		set { m_TypeEnemies = value; }
	}

	public List<TDCEnum.EGameType> TypeFoods {
		get { return m_TypeFoods; }
		set { m_TypeFoods = value; }
	}

	public TDCEnum.ECreatureType CreatureType {
		get { return m_CreatureType; }
		set { m_CreatureType = value; }
	}

	public int CurrentHeatPoint {
		get { return m_CurrentHeatPoint.Value; }
		set { m_CurrentHeatPoint.Value = value; }
	}

	public int MaxHeatPoint {
		get { return m_MaxHeatPoint.Value; }
		set { m_MaxHeatPoint.Value = value; }
	}



    public TDCCreatureData() : base()
	{
		m_MoveSpeed = new TDCObjectProperty<float>("MoveSpeed");
		m_RotationSpeed = 5f;
		m_CurrentHP = new TDCObjectProperty<int>("CurrentHP");
		m_MaxHP = new TDCObjectProperty<int>("MaxHP");
		m_MinDamage = new TDCObjectProperty<int> ("MinDamage");
		m_MaxDamage = new TDCObjectProperty<int>("MaxDamage");
		m_CurrentHeatPoint = new TDCObjectProperty<int>("CurrentHeatPoint");
		m_MaxHeatPoint = new TDCObjectProperty<int>("MaxHeatPoint");

		m_DetectRange = 0f;
		m_AttackRange = 0f;
        this.m_Inventory = new TDCItemController[20];
		m_TypeEnemies = new List<TDCEnum.EGameType> ();
		m_TypeFoods = new List<TDCEnum.EGameType> ();
		m_CreatureType = TDCEnum.ECreatureType.None;

		RegisterProperty(m_CurrentHeatPoint);
		RegisterProperty(m_MaxHeatPoint);
		RegisterProperty(m_CurrentHP);
		RegisterProperty(m_MaxHP);
		RegisterProperty(m_MinDamage);
		RegisterProperty(m_MaxDamage);
    }

	public static TDCCreatureData Clone (TDCCreatureData instance) {
		var tmp = new TDCCreatureData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
		tmp.CreatureType = instance.CreatureType;
		tmp.Description = instance.Description;
		tmp.ModelPath = instance.ModelPath;
		tmp.FSMPath = instance.FSMPath;
		tmp.MinDamage = instance.MinDamage;
		tmp.MaxDamage = instance.MaxDamage;
		tmp.MoveSpeed = instance.MoveSpeed;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.MaxHP = instance.MaxHP;
		tmp.CurrentHeatPoint = instance.CurrentHeatPoint;
		tmp.MaxHeatPoint = instance.MaxHeatPoint;
		tmp.DetectRange = instance.DetectRange;
		tmp.AttackRange = instance.AttackRange;
		tmp.TypeEnemies = instance.TypeEnemies;
		tmp.TypeFoods = instance.TypeFoods;
		tmp.Inventory = instance.Inventory;
		tmp.Icon = instance.Icon;
		return tmp;
	}

}
