
using System.Collections.Generic;

public class TDCCreatureData : TDCBaseData {

	private TDCEnum.ECreatureType m_CreatureType;
	private string[] m_ModelPath;
    private TDCItemController[] m_Inventory;
	private string m_FSMPath;
	private TDCObjectProperty<float> m_WalkSpeed;
	private float m_RunSpeed;
	private float m_RotationSpeed;
	private int m_CurrentHP;
	private int m_MaxHP;
	private int m_MinDamage;
	private int m_MaxDamage;
	private float m_DetectRange;
	private float m_AttackRange;
	private List<TDCEnum.EGameType> m_TypeEnemies;
	private List<TDCEnum.EGameType> m_TypeFoods;

	public string[] ModelPath {
		get { return m_ModelPath; }
		set { m_ModelPath = value; }
	}

	public string FSMPath {
		get { return m_FSMPath; }
		set { m_FSMPath = value; }
	}

    public TDCItemController[] Inventory {
		get { return m_Inventory; }
		set { m_Inventory = value; }
	}

	public float WalkSpeed {
		get { return m_WalkSpeed.GetValue(); }
		set { m_WalkSpeed.SetValue (value); }
	}

	public float RunSpeed {
		get { return m_RunSpeed; }
		set { m_RunSpeed = value; }
	}

	public float RotationSpeed {
		get { return m_RotationSpeed; }
		set { m_RotationSpeed = value; }
	}

	public int MinDamage {
		get { return m_MinDamage; }
		set { m_MinDamage = value; }
	}

	public int MaxDamage {
		get { return m_MaxDamage; }
		set { m_MaxDamage = value; }
	}

	public int CurrentHP {
		get { return m_CurrentHP; }
		set { m_CurrentHP = value; }
	}

	public int MaxHP {
		get { return m_MaxHP; }
		set { m_MaxHP = value; }
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

    public TDCCreatureData() : base()
	{
		this.m_FSMPath 		= string.Empty;
		m_WalkSpeed = new TDCObjectProperty<float>("WalkSpeed");
		m_RunSpeed = 0;
		m_RotationSpeed = 5f;
		m_CurrentHP = 0;
		m_MaxHP = 0;
		m_MinDamage = 0;
		m_MaxDamage = 0;
		m_DetectRange = 0f;
		m_AttackRange = 0f;
        this.m_Inventory = new TDCItemController[20];
		m_TypeEnemies = new List<TDCEnum.EGameType> ();
		m_TypeFoods = new List<TDCEnum.EGameType> ();
		m_CreatureType = TDCEnum.ECreatureType.None;
		this.m_ModelPath 	= new string[] {};

		RegisterProperty (m_WalkSpeed);
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
		tmp.WalkSpeed = instance.WalkSpeed;
		tmp.RunSpeed = instance.RunSpeed;
		tmp.CurrentHP = instance.CurrentHP;
		tmp.MaxHP = instance.MaxHP;
		tmp.DetectRange = instance.DetectRange;
		tmp.AttackRange = instance.AttackRange;
		tmp.TypeEnemies = instance.TypeEnemies;
		tmp.TypeFoods = instance.TypeFoods;
		tmp.Inventory = instance.Inventory;
		tmp.Icon = instance.Icon;
		return tmp;
	}

}
