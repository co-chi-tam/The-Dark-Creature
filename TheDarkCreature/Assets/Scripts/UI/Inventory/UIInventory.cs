using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine.UI;
using System.Linq;
using System;

public class UIInventory : MonoBehaviour {

	#region Singleton
	public static object m_objectSingleton = new object();
	public static UIInventory m_Instance;
	
	public static UIInventory Instance {
		get {
			lock (m_objectSingleton)
			{
				if (m_Instance == null) {
					GameObject go = new GameObject ("Inventory");
					m_Instance = go.AddComponent <UIInventory>();
				}
				return m_Instance;
			}
		}
	}
	
	public static UIInventory GetInstance() {
		return m_Instance;
	}

	#endregion

	#region Properties

	[SerializeField]
	private Button m_SortButton;

	[SerializeField]
	private TDCCreatureController m_OwnerController;

	private TDCCreatureData m_OwnerData;
	private UISlot[] m_ItemSlots;

	public delegate void SelectedSlot(int itemIndex);

	#endregion

	#region Implementation Monobehaviour

	void Awake() {
		m_Instance = this;
	}

	void Start() {
		m_SortButton.onClick.AddListener(() =>
			{
				SortInventory();
			});
	}

	#endregion

	#region Main method

	private void SortInventory() {
		var inventory = m_OwnerData.Inventory;
		QuickSort.SimpleSort(inventory, (x, y) => {
			var lh = (int) x.GetData().GameType;
			var rh = (int) y.GetData().GameType;
			return lh > rh ? 1 : lh < rh ? -1 : 0;
		});
		ReloadSlots();
	}

	public void SetPlayer(TDCCreatureController owner) {
		m_OwnerController = owner;
		m_OwnerData = m_OwnerController.GetData () as TDCCreatureData;
		LoadAllSlots();	
	}

	public bool AddItem(int index, TDCItemController item) {
		m_ItemSlots[index].OnSelectedSlot = m_OwnerController.OnSelectedItem;
		m_ItemSlots[index].LoadSlot (index, item);
		m_OwnerData.Inventory[index].GetData().Owner = m_OwnerController;
		return true;
	}

	public bool RemoveItem(int index) {
		if (m_ItemSlots[index] != null)
		{
			m_ItemSlots[index].EmptySlot();
			return true;
		}
		return false;
	}

	public void LoadAllSlots() {
		if (m_OwnerData == null)
			return;
		m_ItemSlots = null;
		m_ItemSlots = new UISlot [this.transform.childCount];
		for (int i = 0; i < m_OwnerData.Inventory.Length; i++) {
			var child = this.transform.GetChild (i).GetComponent<UISlot>();
			if (m_OwnerData.Inventory[i] != null) {
				child.OnSelectedSlot = m_OwnerController.OnSelectedItem;
				child.LoadSlot (i, m_OwnerData.Inventory[i]);
                m_OwnerData.Inventory[i].GetData().Owner = m_OwnerController;
			}
			m_ItemSlots[i] = child;
		}
	}

	public void ReloadSlots() {
		for (int i = 0; i < m_OwnerData.Inventory.Length; i++) {
			var child = m_ItemSlots[i];
			if (m_OwnerData.Inventory[i] != null)
			{
				child.OnSelectedSlot = m_OwnerController.OnSelectedItem;
				child.LoadSlot(i, m_OwnerData.Inventory[i]);
				m_OwnerData.Inventory[i].GetData().Owner = m_OwnerController;
			}
			else
			{
				child.EmptySlot();
			}
		}
	}

	#endregion
}
