using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

public class TDCInventory : MonoBehaviour {

	#region Singleton
	public static object m_objectSingleton = new object();
	public static TDCInventory m_Instance;
	
	public static TDCInventory Instance {
		get {
			lock (m_objectSingleton)
			{
				if (m_Instance == null) {
					GameObject go = new GameObject ("Inventory");
					m_Instance = go.AddComponent <TDCInventory>();
				}
				return m_Instance;
			}
		}
	}
	
	public static TDCInventory GetInstance() {
		return m_Instance;
	}
	
	public TDCInventory ()
	{
		m_Instance = this;
	}
	#endregion

	#region Properties

	[SerializeField]
	private TDCCreatureController m_OwnerController;
	private TDCCreatureData m_OwnerData;
	private TDCSlot[] m_ItemSlots;

	public delegate void SelectedSlot(TDCSlot item);

	#endregion

	#region Main method

	public void SetPlayer(TDCCreatureController owner) {
		m_OwnerController = owner;
		m_OwnerData = m_OwnerController.GetData () as TDCCreatureData;
		LoadAllSlot();	
	}

	public bool AddItem(TDCItemData itemData) {
		for (int i = 0; i < m_OwnerData.Inventory.Length; i++) {
			var itemType = itemData.ItemType;
			switch (itemType) {
			case TDCEnum.EItemType.Food: 
			case TDCEnum.EItemType.Resource: 
			{
				if (m_OwnerData.Inventory[i] != null) {
					if (m_OwnerData.Inventory[i].GameType == itemData.GameType) {
						m_OwnerData.Inventory[i].Amount++;
						return true;
					}
				} else if (m_OwnerData.Inventory[i] == null) {
					m_OwnerData.Inventory[i] = itemData;
					m_ItemSlots[i].LoadSlot (itemData);
					if (m_ItemSlots[i].DelegateSelectedSlot == null) {
						m_ItemSlots[i].DelegateSelectedSlot += m_OwnerController.OnSelectedItem;
					}
					return true;
				}
				break;
			}
			case TDCEnum.EItemType.Weapon: {
				if (m_OwnerData.Inventory[i] == null) {
					m_OwnerData.Inventory[i] = itemData;
					m_ItemSlots[i].LoadSlot (itemData);
					if (m_ItemSlots[i].DelegateSelectedSlot == null) {
						m_ItemSlots[i].DelegateSelectedSlot += m_OwnerController.OnSelectedItem;
					}
					return true;
				}
				break;
			} 
			}
		}
		return false;
	}

	public bool RemoveItem(TDCItemData itemData) {
		for (int i = 0; i < m_OwnerData.Inventory.Length; i++) {
			if (m_OwnerData.Inventory[i] != null) {
				if (m_OwnerData.Inventory[i].GameType == itemData.GameType) {
					if (m_ItemSlots[i].DelegateSelectedSlot != null) {
						m_ItemSlots[i].DelegateSelectedSlot -= m_OwnerController.OnSelectedItem;
					}
					m_ItemSlots[i].EmptySlot();
					m_OwnerData.Inventory[i] = null;
					return true;
				}
			}
		}
		return false;
	}

	public void LoadAllSlot() {
		if (m_OwnerData == null)
			return;
		m_ItemSlots = new TDCSlot [this.transform.childCount];
		for (int i = 0; i < m_OwnerData.Inventory.Length; i++) {
			var child = this.transform.GetChild (i).GetComponent<TDCSlot>();
			if (m_OwnerData.Inventory[i] != null) {
				if (child.DelegateSelectedSlot == null) {
					child.DelegateSelectedSlot += m_OwnerController.OnSelectedItem;
				}
				child.LoadSlot (m_OwnerData.Inventory[i]);
			}
			m_ItemSlots[i] = child;
		}
	}

	#endregion
}
