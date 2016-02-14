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

	private TDCEntity m_OwnerEnity;
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
		var inventory = m_OwnerEnity.GetInventory();
		QuickSort.SimpleSort(inventory, (x, y) => {
			var lh = (int) x.GetData().GameType;
			var rh = (int) y.GetData().GameType;
			return lh > rh ? 1 : lh < rh ? -1 : 0;
		});
		ReloadSlots();
	}

	public void SetPlayer(TDCCreatureController owner) {
		m_OwnerController = owner;
		m_OwnerEnity = owner.GetEntity();
		LoadAllSlots();	
		this.GetComponentInChildren<CUIGridLayout>().enabled = false;
		this.GetComponentInChildren<ContentSizeFitter>().enabled = false;
	}

	public bool AddItem(int index, UIItemController item) {
		m_ItemSlots[index].OnSelectedSlot = m_OwnerController.OnSelectedItem;
		m_ItemSlots[index].LoadSlot (index, item);
		m_OwnerEnity.GetInventory()[index].GetData().Owner = m_OwnerController;
		m_OwnerEnity.GetInventory()[index].SetIndex(index);
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
		if (m_OwnerEnity == null)
			return;
		m_ItemSlots = null;
		m_ItemSlots = new UISlot [20];
		var inventory = m_OwnerEnity.GetInventory();
		var panel = this.transform.GetChild(0);
		for (int i = 0; i < inventory.Length; i++) {
			var child = panel.GetChild (i).GetComponent<UISlot>();
			if (inventory[i] != null) {
				child.OnSelectedSlot = m_OwnerController.OnSelectedItem;
				child.LoadSlot (i, inventory[i]);
				inventory[i].GetData().Owner = m_OwnerController;
				inventory[i].SetIndex(i);
			}
			m_ItemSlots[i] = child;
		}
	}

	public void ReloadSlots() {
		var inventory = m_OwnerEnity.GetInventory();
		for (int i = 0; i < inventory.Length; i++) {
			var child = m_ItemSlots[i];
			if (inventory[i] != null)
			{
				child.OnSelectedSlot = m_OwnerController.OnSelectedItem;
				child.LoadSlot(i, inventory[i]);
				inventory[i].SetIndex (i);
				inventory[i].GetData().Owner = m_OwnerController;
			}
			else
			{
				child.EmptySlot();
			}
		}
	}

	#endregion
}
