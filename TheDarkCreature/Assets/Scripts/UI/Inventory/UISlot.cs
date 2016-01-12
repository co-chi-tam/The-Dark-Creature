using UnityEngine;
using UnityEngine.UI;
using Effect;

public class UISlot : MonoBehaviour {

	#region Properties
	private UIInventory.SelectedSlot m_SelectedSlot;
	public UIInventory.SelectedSlot OnSelectedSlot {
		get { return m_SelectedSlot; }
		set { m_SelectedSlot = value; }
	}

	[SerializeField]
	private int m_SlotIndex = 0;
	public int SlotIndex
	{
		get { return m_SlotIndex; }
		set { m_SlotIndex = value; }
	}
	[SerializeField]
	private Image m_IconImage = null;
	[SerializeField]
	private Text m_AmountItem = null;
	[SerializeField]
	private Button m_ItemButton = null;

	private UIItemController m_ItemController;
	private Sprite m_NoImage;

	#endregion 

	#region Main method

	void Start() {
		m_ItemButton.onClick.AddListener (() => {
			OnPointerClick();
		});
		m_NoImage = TDCUltilities.LoadImage ("NoImage");
	}

	public void LoadSlot(UIItemController item) {
		m_ItemController = item;
		m_IconImage.sprite = TDCUltilities.LoadImage (m_ItemController.GetData().Icon);
		m_ItemController.GetData().GetChangeValue<int> ("Amount", ChangeAmount);
		m_AmountItem.text = m_ItemController.GetData().Amount.ToString ();
	}

    public void LoadSlot(int index, UIItemController item) {
		m_SlotIndex = index;
		LoadSlot(item);
	}

	private void ChangeAmount(int source, int target) {
		m_AmountItem.text = target.ToString ();
	}

	public void EmptySlot() {
		m_ItemController = null;
		m_IconImage.sprite = m_NoImage;
		m_AmountItem.text = string.Empty;
		m_SelectedSlot = null;
	}

	private void OnPointerClick() {
		if (m_SelectedSlot != null) {
			m_SelectedSlot (m_SlotIndex);
		}
	}

	#endregion
}
