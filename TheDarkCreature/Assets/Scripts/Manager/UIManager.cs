using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	#region Singleton
	public static object m_objectSingleton = new object();
	public static UIManager m_Instance;

	public static UIManager Instance {
		get {
			lock (m_objectSingleton)
			{
				if (m_Instance == null) {
					GameObject go = new GameObject ("UIManager");
					m_Instance = go.AddComponent <UIManager>();
				}
				return m_Instance;
			}
		}
	}

	public static UIManager GetInstance() {
		return m_Instance;
	}

	#endregion

	#region Properties

	public Image LoadingImage;
	public Image BloodScreenImage;
	public Text HealthPointText;
	public Text SanityPointText;
	public Text HungerPointText;
	public Text HeatPointText;

	[SerializeField]
	private TDCEntity m_Owner;

	private float m_TimeRefreshScreen = 0f;

	#endregion

	#region Implementation MonoBeahvour

	void Awake() {
		m_Instance = this;
	}

	void LateUpdate() {
		if (m_TimeRefreshScreen < 0f)
		{
			RefreshScreen();
		}
		else
		{
			m_TimeRefreshScreen -= Time.deltaTime;
		}
	}

	#endregion

	#region Main methods

	public void Init(TDCEntity owner) {
		m_Owner = owner;
		UpdateUIHealthBar(m_Owner.GetHealth(), m_Owner.GetHealth());
		UpdateUIHeatBar(m_Owner.GetHeat(), m_Owner.GetHeat());
		UpdateUISanityBar(m_Owner.GetSanity(), m_Owner.GetSanity());
		UpdateUIHungerBar(m_Owner.GetHunger(), m_Owner.GetHunger());

		m_Owner.OnHealthChange += UpdateUIHealthBar;
		m_Owner.OnHeatChange += UpdateUIHeatBar;
		m_Owner.OnSanityChange += UpdateUISanityBar;
		m_Owner.OnHungerChange += UpdateUIHungerBar;
		m_Owner.AddEventListener("OnApplyDamage", UpdateUIApplyDamage);
		m_Owner.AddEventListener("OnDeath", RemoveAllUIListener);
	}

	public void EnableLoadingScreen(bool value) {
		LoadingImage.gameObject.SetActive(value);
	}

	private void UpdateUIHealthBar(int source, int newValue) {
		HealthPointText.text = newValue > 0 ? newValue.ToString() : "0";
		if (newValue < source)
		{
			UpdateUIApplyDamage();
		}
	}

	private void UpdateUIHeatBar(int source, int newValue) {
		HeatPointText.text = newValue > 0 ? newValue.ToString() : "0";
	}

	private void UpdateUISanityBar(int source, int newValue) {
		SanityPointText.text = newValue > 0 ? newValue.ToString() : "0";
	}

	private void UpdateUIHungerBar(int source, int newValue) {
		HungerPointText.text = newValue > 0 ? newValue.ToString() : "0";
	}

	private void UpdateUIApplyDamage() {
		BloodScreenImage.gameObject.SetActive(true);
		m_TimeRefreshScreen = 0.25f;
	}

	private void RefreshScreen() {
		BloodScreenImage.gameObject.SetActive(false);
	}

	private void RemoveAllUIListener() {
		m_Owner.OnHealthChange -= UpdateUIHealthBar;
		m_Owner.OnHeatChange -= UpdateUIHeatBar;
		m_Owner.OnSanityChange -= UpdateUISanityBar;
		m_Owner.OnHungerChange -= UpdateUIHungerBar;
	}

	#endregion

}
