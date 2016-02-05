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
	public Image HealthPointImage;
	public Image HeatPointImage;
	public Image SanityPointImage;
	public Image HungerPointImage;
	// Action button
	public Button PickupButton;

	// Attack button
	public Button NormalAttackButton;
	public Button Skill1Button;
	public Button Skill2Button;
	public Button Skill3Button;

	[SerializeField]
	private TDCBaseController m_Owner;

	private float m_TimeRefreshScreen = 0f;

	#endregion

	#region Implementation MonoBeahvour

	void Awake() {
		m_Instance = this;
	}

	void Start() {
		HealthPointImage = SetupImage(HealthPointImage);
		HeatPointImage = SetupImage(HeatPointImage);
		HungerPointImage = SetupImage(HungerPointImage);
		SanityPointImage = SetupImage(SanityPointImage);
	}

	void LateUpdate() {
		if (m_TimeRefreshScreen < 0f)
		{
			RefreshScreen();
		}
		else
		{
			m_TimeRefreshScreen -= Time.deltaTime;
			if (BloodScreenImage.IsActive())
			{
				var color = BloodScreenImage.color;
				color.a = m_TimeRefreshScreen * 0.5f / 1f;
				BloodScreenImage.color = color;
			}
		}
	}

	#endregion

	#region Main methods

	public void Init(TDCBaseController owner) {
		m_Owner = owner;
		UpdateUIHealthBar(m_Owner.GetHealth(), m_Owner.GetHealth(), m_Owner.GetMaxHealth());
		UpdateUIHeatBar(m_Owner.GetHeat(), m_Owner.GetHeat(), m_Owner.GetMaxHeat());
		UpdateUISanityBar(m_Owner.GetSanity(), m_Owner.GetSanity(), m_Owner.GetMaxSanity());
		UpdateUIHungerBar(m_Owner.GetHunger(), m_Owner.GetHunger(), m_Owner.GetMaxHunger());

		m_Owner.GetEntity().OnHealthChange += UpdateUIHealthBar;
		m_Owner.GetEntity().OnHeatChange += UpdateUIHeatBar;
		m_Owner.GetEntity().OnSanityChange += UpdateUISanityBar;
		m_Owner.GetEntity().OnHungerChange += UpdateUIHungerBar;
		m_Owner.GetEntity().AddEventListener("OnApplyDamage", UpdateUIApplyDamage);
		m_Owner.GetEntity().AddEventListener("OnDeath", RemoveAllUIListener);

		PickupButton.onClick.RemoveAllListeners();
		NormalAttackButton.onClick.RemoveAllListeners();
		Skill1Button.onClick.RemoveAllListeners();
		Skill2Button.onClick.RemoveAllListeners();
		Skill3Button.onClick.RemoveAllListeners();

		PickupButton.onClick.AddListener(() => { PressActionButton(0); });
		NormalAttackButton.onClick.AddListener(() => { PressActionButton(1); });

		Skill1Button.onClick.AddListener(() => { PressAttackButton(1); });
		Skill2Button.onClick.AddListener(() => { PressAttackButton(2); });
		Skill3Button.onClick.AddListener(() => { PressAttackButton(3); });
	}

	public void EnableLoadingScreen(bool value) {
		LoadingImage.gameObject.SetActive(value);
	}

	private void PressActionButton(int index) {
		if (m_Owner == null)
			return;
		m_Owner.ActiveAction(index);
	}

	private void PressAttackButton(int index) {
		if (m_Owner == null)
			return;
		m_Owner.Activekill(index);
	}

	private void UpdateUIHealthBar(int source, int newValue, int max) {
		HealthPointImage.fillAmount = (float)newValue / max;
		if (newValue < source)
		{
			UpdateUIApplyDamage();
		}
	}

	private void UpdateUIHeatBar(int source, int newValue, int max) {
		HeatPointImage.fillAmount = (float)newValue / max;
	}

	private void UpdateUISanityBar(int source, int newValue, int max) {
		SanityPointImage.fillAmount = (float)newValue / max;
	}

	private void UpdateUIHungerBar(int source, int newValue, int max) {
		HungerPointImage.fillAmount = (float)newValue / max;
	}

	private void UpdateUIApplyDamage() {
		BloodScreenImage.gameObject.SetActive(true);
		var color = BloodScreenImage.color;
		color.a = 0.5f;
		BloodScreenImage.color = color;
		m_TimeRefreshScreen = 1f;
	}

	private void RefreshScreen() {
		BloodScreenImage.gameObject.SetActive(false);
		var color = BloodScreenImage.color;
		color.a = 0.5f;
		BloodScreenImage.color = color;
	}

	private void RemoveAllUIListener() {
		m_Owner.GetEntity().OnHealthChange -= UpdateUIHealthBar;
		m_Owner.GetEntity().OnHeatChange -= UpdateUIHeatBar;
		m_Owner.GetEntity().OnSanityChange -= UpdateUISanityBar;
		m_Owner.GetEntity().OnHungerChange -= UpdateUIHungerBar;
	}

	private Image SetupImage(Image value) {
		value.type = Image.Type.Filled;
		value.fillAmount = 1f;
		value.fillMethod = Image.FillMethod.Vertical;
		value.fillOrigin = 0;
		return value;
	}

	#endregion

}
