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

	public UIManager ()
	{
		m_Instance = this;
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

	#region Main methods

	public void Init(TDCEntity owner) {
		m_Owner = owner;
		UpdateUIHealthBar(m_Owner.GetHealth(), m_Owner.GetHealth());
		m_Owner.OnHealthChange += UpdateUIHealthBar;
	}

	public void EnableLoadingScreen(bool value) {
		m_LoadingImage.gameObject.SetActive(value);
	}

	private void UpdateUIHealthBar(int source, int newValue) {
		m_HealthPointText.text = newValue > 0f ? newValue.ToString() : "0";
	}



	#endregion

}
