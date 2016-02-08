using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

	// Loading
	public Image LoadingImage;
	public Text LoadingText;

	// Status
	[Space(10f)]
	public Image BloodScreenImage;
	public Image HealthPointImage;
	public Image HeatPointImage;
	public Image SanityPointImage;
	public Image HungerPointImage;

	// Action button
	[Space(10f)]
	public Button PickupButton;

	// Attack button
	[Space(10f)]
	public Button NormalAttackButton;
	public Button Skill1Button;
	public Button Skill2Button;
	public Button Skill3Button;

	// Crafting sample
	[Space(10f)]
	public Button CraftingCampfireButton;
	public Button CraftingBigCampfireButton;

	// Owner
	[Space(10f)]
	[SerializeField]
	private TDCBaseController m_Owner;

	private TDCGameManager m_GameManager;
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
		m_GameManager = TDCGameManager.GetInstance();

		SetupCraftingButton();
		SetupStatusUI();
		SetupActionButtons();
		SetupSkillButtons();
	}

	private void SetupCraftingButton() {
		CraftingCampfireButton.onClick.RemoveAllListeners();
		CraftingBigCampfireButton.onClick.RemoveAllListeners();
		CraftingCampfireButton.onClick.AddListener(() => { 
			PressedCraftingButton(TDCEnum.EGameType.ItemCampfire); 
		});
		CraftingBigCampfireButton.onClick.AddListener(() => { 
			PressedCraftingButton(TDCEnum.EGameType.ItemBigCampfire); 
		});
	}

	private void SetupStatusUI() {
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
	}

	private void SetupActionButtons() {
		PickupButton.onClick.RemoveAllListeners();
		PickupButton.onClick.AddListener(() => { PressActionButton(0); });
	}

	private void SetupSkillButtons() {
		NormalAttackButton.onClick.RemoveAllListeners();
		Skill1Button.onClick.RemoveAllListeners();
		Skill2Button.onClick.RemoveAllListeners();
		Skill3Button.onClick.RemoveAllListeners();

		NormalAttackButton.onClick.AddListener(() => { PressActionButton(1); });
		Skill1Button.onClick.AddListener(() => { PressAttackButton(1); });
		Skill2Button.onClick.AddListener(() => { PressAttackButton(2); });
		Skill3Button.onClick.AddListener(() => { PressAttackButton(3); });
	}

	public void EnableLoadingScreen(bool value) {
		LoadingImage.gameObject.SetActive(value);
	}

	private void PressedCraftingButton(TDCEnum.EGameType gameType) {
		var craftingData = m_GameManager.GetCraftingData(gameType);
		var inventory = m_Owner.GetInventory();
		var canCrafting = CheckEnoughResources(craftingData, inventory);
		var payResources = false;
		if (canCrafting)
		{
			payResources = HandleResources (craftingData, inventory);
		}
		if (payResources)
		{
			for (int i = 0; i < craftingData.Amount; i++)
			{
				TDCEntity item = null;
				if (m_GameManager.GetObjectPool(craftingData.GameType, ref item))
				{
					var randomAroundPosition = UnityEngine.Random.insideUnitCircle * m_Owner.GetColliderRadius();
					var ownerPosition = m_Owner.TransformPosition;
					ownerPosition.x += randomAroundPosition.x;
					ownerPosition.y = 0f;
					ownerPosition.z += randomAroundPosition.y;
					item.SetTransformPosition(ownerPosition); 
					item.SetActive(true);
				}
			}
		}
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

	public void SetLoadingStep(int step) {
		var dot = step % 3;
		LoadingText.text = (dot == 0 ? "." : dot == 1 ? ".." : "...");
	}

	#endregion

	#region Ultilities

	private bool CheckEnoughResources(TDCCraftingData craftingData, UIItemController[] inventory) {
		var canCrafting = true;
		for (int j = 0; j < craftingData.Elements.Length; j++) 
		{
			var isEnoughResources = false;
			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] == null)
				{
					continue;
				}
				var type = inventory[i].GetGameType() == craftingData.Elements[j].GameType;
				var amount = inventory[i].GetAmount() >= craftingData.Elements[j].Amount;
				if (type && amount)
				{
					isEnoughResources = true;
				}
			}
			canCrafting &= isEnoughResources;
		}
		return canCrafting;
	}

	private bool HandleResources(TDCCraftingData craftingData, UIItemController[] inventory) {
		var canCrafting = true;
		for (int j = 0; j < craftingData.Elements.Length; j++) 
		{
			for (int i = 0; i < inventory.Length; i++)
			{
				if (inventory[i] == null)
				{
					continue;
				}
				var type = inventory[i].GetGameType() == craftingData.Elements[j].GameType;
				var amount = inventory[i].GetAmount() >= craftingData.Elements[j].Amount;
				if (type && amount)
				{
					canCrafting &= inventory[i].DecreaseAmountItem(craftingData.Elements[j].Amount);
				}
			}
		}
		return canCrafting;
	}

	#endregion

}
