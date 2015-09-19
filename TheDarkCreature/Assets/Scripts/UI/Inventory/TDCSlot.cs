﻿using UnityEngine;
using UnityEngine.UI;

public class TDCSlot : MonoBehaviour {

	#region Properties
	private TDCInventory.SelectedSlot m_SelectedSlot;
	public TDCInventory.SelectedSlot DelegateSelectedSlot {
		get { return m_SelectedSlot; }
		set { m_SelectedSlot = value; }
	}

	[SerializeField]
	private Image m_IconImage;
	[SerializeField]
	private Text m_AmountItem;
	[SerializeField]
	private Button m_ItemButton;

	private TDCBaseData m_ItemData;

	private Sprite m_NoImage;

	#endregion 

	#region Main method

	public TDCBaseData GetData() {
		return m_ItemData;
	}

	public void LoadSlot(TDCBaseData data) {
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
//		m_IconImage.sprite = TDCUltilities.LoadImage ("NoIcon");
		m_IconImage.sprite = m_NoImage;
		m_ItemButton.onClick.RemoveAllListeners ();
		m_AmountItem.text = string.Empty;
	}

	private void OnPointerClick() {
		if (m_SelectedSlot != null) {
			m_SelectedSlot (this);
		}
	}

	#endregion
}
