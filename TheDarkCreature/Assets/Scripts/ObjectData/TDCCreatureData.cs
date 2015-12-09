
using System.Collections.Generic;

public class TDCCreatureData : TDCBaseData {

	private TDCObjectProperty<float> m_WalkSpeed;
	private float m_RunSpeed;
	private float m_RotationSpeed;
	private int m_CurrentHP;
	private int m_MaxHP;
	private int m_MinDamage;
	private int m_MaxDamage;
	private float m_DetectRange;
	private List<TDCEnum.EGameType> m_TypeEnemies;
	private List<TDCEnum.EGameType> m_TypeFoods;

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

	public List<TDCEnum.EGameType> TypeEnemies {
		get { return m_TypeEnemies; }
		set { m_TypeEnemies = value; }
	}

	public List<TDCEnum.EGameType> TypeFoods {
		get { return m_TypeFoods; }
		set { m_TypeFoods = value; }
	}

    public TDCCreatureData() : base()
    {
		m_WalkSpeed = new TDCObjectProperty<float>("WalkSpeed");
		m_RunSpeed = 0;
		m_RotationSpeed = 5f;
		m_CurrentHP = 0;
		m_MaxHP = 0;
		m_MinDamage = 0;
		m_MaxDamage = 0;
		m_DetectRange = 0f;
		m_TypeEnemies = new List<TDCEnum.EGameType> ();
		m_TypeFoods = new List<TDCEnum.EGameType> ();

		RegisterProperty (m_WalkSpeed);
    }

	public static TDCCreatureData Clone (TDCCreatureData instance) {
		var tmp = new TDCCreatureData ();
		tmp.ID = instance.ID;
		tmp.Name = instance.Name;
		tmp.GameType = instance.GameType;
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
		tmp.TypeEnemies = instance.TypeEnemies;
		tmp.TypeFoods = instance.TypeFoods;
		tmp.CreatureType = instance.CreatureType;
		tmp.Inventory = instance.Inventory;
		tmp.Icon = instance.Icon;
		return tmp;
	}

}
