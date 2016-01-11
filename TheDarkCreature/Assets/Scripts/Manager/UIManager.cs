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

	[SerializeField]
	private Image m_LoadingImage;
	[SerializeField]
	private Text m_HealthPointText;
	[SerializeField]
	private Text m_SanityPointText;
	[SerializeField]
	private Text m_HungerPointText;
	[SerializeField]
	private Text m_HeatPointText;

	private TDCEntity m_Owner;

	#endregion

	#region Implementation MonoBeahvour

	void Awake() {
		m_Instance = this;
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

		m_Owner.OnDeathEvent += RemoveAllUIListener;
	}

	public void EnableLoadingScreen(bool value) {
		m_LoadingImage.gameObject.SetActive(value);
	}

	private void UpdateUIHealthBar(int source, int newValue) {
		m_HealthPointText.text = newValue > 0 ? newValue.ToString() : "0";
	}

	private void UpdateUIHeatBar(int source, int newValue) {
		m_HeatPointText.text = newValue > 0 ? newValue.ToString() : "0";
	}

	private void UpdateUISanityBar(int source, int newValue) {
		m_SanityPointText.text = newValue > 0 ? newValue.ToString() : "0";
	}

	private void UpdateUIHungerBar(int source, int newValue) {
		m_HungerPointText.text = newValue > 0 ? newValue.ToString() : "0";
	}

	private void RemoveAllUIListener() {
		m_Owner.OnHealthChange -= UpdateUIHealthBar;
		m_Owner.OnHeatChange -= UpdateUIHeatBar;
		m_Owner.OnSanityChange -= UpdateUISanityBar;
		m_Owner.OnHungerChange -= UpdateUIHungerBar;
	}

	#endregion

}
