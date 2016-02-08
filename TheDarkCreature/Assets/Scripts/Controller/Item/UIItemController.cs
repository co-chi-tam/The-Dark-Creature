using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Effect;

public class UIItemController {

	#region Properties

	private TDCGameManager m_GameManager;
	private UIInventory m_Inventory;
	private EffectManager m_EffectManager;
	private UIItemData m_Data;
	private int m_Index;

	#endregion

	#region Main methods

	public UIItemController()
	{
		m_Data = null;
		m_EffectManager = null;
	}

	public UIItemController(UIItemData data)
	{
		m_Data = data;
		m_GameManager = TDCGameManager.GetInstance();
		m_Inventory = UIInventory.GetInstance();

		m_EffectManager = new EffectManager();
		m_EffectManager.LoadEffect(m_Data.EffectPath);
		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);

		m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);
		m_EffectManager.RegisterExcuteMethod("CreateObjectEffect", CreateObjectEffect);
		m_EffectManager.RegisterExcuteMethod("DropItSelfEffect", DropItSelfEffect);
		m_EffectManager.RegisterExcuteMethod("AddValueEffect", AddValueEffect);
		m_EffectManager.RegisterExcuteMethod("SubtractValueEffect", SubtractValueEffect);
	}

	private void UsedItem() {
		var type = m_Data.ItemType;
		switch (type)
		{
			case TDCEnum.EItemType.Food:
			case TDCEnum.EItemType.GObject:
				m_Data.Amount--;
				break;
			case TDCEnum.EItemType.Weapon:{
				var weaponData = m_Data as UIWeaponData;
				weaponData.Duration -= weaponData.DecreaseDuration;
				if (weaponData.Duration <= 0f)
				{
					m_Data.Amount--;
				}
				break;
			} 
			case TDCEnum.EItemType.Item: {
				// TODO
				break;
			}
		}
		if (m_Data.Amount == 0)
		{
			m_Inventory.RemoveItem(m_Index);
			m_Data.Owner.GetInventory()[m_Index] = null;
		}
	}

	public bool DecreaseAmountItem(int amount) {
		if (m_Data.Amount >= amount)
		{
			m_Data.Amount -= amount;
			if (m_Data.Amount == 0)
			{
				m_Inventory.RemoveItem(m_Index);
				m_Data.Owner.GetInventory()[m_Index] = null;
			}
			return true;
		}
		Debug.LogError("[UIItemController] Something wrong");
		return false;
	}

	#endregion

	#region Effect

	public virtual void ExcuteItem() {
		
		m_EffectManager.ExcuteEffect();
	}

	internal virtual bool CanActiveEffect(Dictionary<string, object> pars)
	{
		var active = m_Data.Amount > 0 && m_Data.Owner != null && m_Data.Owner.GetStateName() != "FlyState";;
		if (active)
		{
			UsedItem();
		}
		return active;
	}

	internal virtual void PrintDebug(Dictionary<string, object> pars)
	{
//#if UNITY_EDITOR
//		foreach (var item in pars) {
//			Debug.Log(string.Format ("[{0}] - {1}", item.Key, item.Value));
//		}
//#endif
	}

	internal virtual void CreateObjectEffect(Dictionary<string, object> pars)
	{
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
		TDCEntity obj = null;
		var gameType = (TDCEnum.EGameType)int.Parse (pars["GameType"].ToString());
		var amount = int.Parse (pars["Amount"].ToString());
		for (int i = 0; i < amount; i++)
		{
			if (m_GameManager.GetObjectPool(gameType, ref obj))
			{
				var distance = m_Data.Owner.GetColliderRadius() + obj.GetColliderRadius();
				obj.GetController().TransformPosition = 
					m_Data.Owner.transform.V3Forward(-distance);
				obj.SetActive(true);
			}
		}
	}

	internal virtual void DropItSelfEffect(Dictionary<string, object> pars) {
		TDCEntity obj = null;
		var gameType = m_Data.GameType;
		var amount = int.Parse (pars["Amount"].ToString());
		for (int i = 0; i < amount; i++)
		{
			if (m_GameManager.GetObjectPool(gameType, ref obj))
			{
				obj.GetController().TransformPosition = 
					m_Data.Owner.transform.V3Forward(-m_Data.Owner.GetColliderRadius());
				obj.SetActive(true);
			}
		}
	}

	internal virtual void AddValueEffect(Dictionary<string, object> pars)
	{
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
		var nameValue = pars["NameValue"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		m_Data.Owner.GetEntity().SetProperty(nameValue, toValue);
	}

	internal virtual void SubtractValueEffect(Dictionary<string, object> pars)
	{
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
		var nameValue = pars["NameValue"].ToString();
		var toValue = float.Parse (pars["ToValue"].ToString());
		m_Data.Owner.GetEntity().SetProperty(nameValue, -toValue);
	}

	#endregion

	#region Getter & Setter

	public void SetIndex(int value) {
		m_Index = value;
	}

	public UIItemData GetData() {
		return m_Data;
	}

	public TDCEnum.EGameType GetGameType() {
		if (GetAmount() == 0)
			return TDCEnum.EGameType.None;
		return m_Data.GameType;
	}

	public int GetAmount() {
		return m_Data.Amount;
	}

	#endregion

}
