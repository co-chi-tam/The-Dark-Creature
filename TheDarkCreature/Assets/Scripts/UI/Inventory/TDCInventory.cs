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

	public delegate void SelectedSlot(int itemIndex);

	#endregion

	#region Main method

	public void SetPlayer(TDCCreatureController owner) {
		m_OwnerController = owner;
		m_OwnerData = m_OwnerController.GetData () as TDCCreatureData;
		LoadAllSlot();	
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

	public void LoadAllSlot() {
		if (m_OwnerData == null)
			return;
		m_ItemSlots = null;
		m_ItemSlots = new TDCSlot [this.transform.childCount];
		for (int i = 0; i < m_OwnerData.Inventory.Length; i++) {
			var child = this.transform.GetChild (i).GetComponent<TDCSlot>();
			if (m_OwnerData.Inventory[i] != null) {
				child.OnSelectedSlot = m_OwnerController.OnSelectedItem;
				child.LoadSlot (i, m_OwnerData.Inventory[i]);
                m_OwnerData.Inventory[i].GetData().Owner = m_OwnerController;
			}
			m_ItemSlots[i] = child;
		}
	}

	#endregion
}
