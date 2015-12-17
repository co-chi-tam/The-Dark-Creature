using UnityEngine;
using UnityEngine.UI;
using Effect;

public class TDCSlot : MonoBehaviour {

	#region Properties
	private TDCInventory.SelectedSlot m_SelectedSlot;
	public TDCInventory.SelectedSlot OnSelectedSlot {
		get { return m_SelectedSlot; }
		set { m_SelectedSlot = value; }
	}

	[SerializeField]
	private Image m_IconImage = null;
	[SerializeField]
	private Text m_AmountItem = null;
	[SerializeField]
	private Button m_ItemButton = null;

	private TDCItemData m_ItemData;
	private Sprite m_NoImage;

	#endregion 

	#region Main method

	void Start() {
		
	}

	public TDCItemData GetData() {
		return m_ItemData;
	}

	public void LoadSlot(TDCItemData data) {
		m_ItemData = data;
		m_NoImage = m_IconImage.sprite;
		m_IconImage.sprite = TDCUltilities.LoadImage (m_ItemData.Icon);
		m_ItemData.GetChangeValue<int> ("Amount", ChangeAmount);
		m_ItemButton.onClick.AddListener (() => {
			OnPointerClick();
		});
		m_AmountItem.text = m_ItemData.Amount.ToString ();
	}

	private void ChangeAmount(int source, int target) {
		m_AmountItem.text = target.ToString ();
	}

	public void EmptySlot() {
		m_ItemData = null;
		m_IconImage.sprite = m_NoImage;
		m_ItemButton.onClick.RemoveAllListeners ();
		m_AmountItem.text = string.Empty;
	}

	private void OnPointerClick() {
		if (m_SelectedSlot != null) {
			m_SelectedSlot (this.m_ItemData);
		}
	}

	#endregion
}
