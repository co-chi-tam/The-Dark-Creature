using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Effect;

public class UIItemController {

	#region Properties

	private TDCGameManager m_GameManager;
	private EffectManager m_EffectManager;
	private UIItemData m_Data;

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

		m_EffectManager = new EffectManager();
		m_EffectManager.LoadEffect(m_Data.EffectPath);
		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);

		m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);
		m_EffectManager.RegisterExcuteMethod("ExcuteEffect", ExcuteEffect);
		m_EffectManager.RegisterExcuteMethod("AddValueEffect", AddValueEffect);
		m_EffectManager.RegisterExcuteMethod("SubtractValueEffect", SubtractValueEffect);
	}

	private void UsedItem() {
		var type = m_Data.ItemType;
		switch (type)
		{
			case TDCEnum.EItemType.Food:
				m_Data.Amount--;
				break;
			case TDCEnum.EItemType.Weapon:{
				var weaponData = m_Data as UIWeaponData;
				weaponData.Duration -= weaponData.DecreaseDuration;
				if (weaponData.Duration < 0f)
				{
					m_Data.Amount--;
				}
				break;
			} 
			case TDCEnum.EItemType.Item: {
				// TODO
				break;
			}
			case TDCEnum.EItemType.GObject:{
				var owner = m_Data.Owner;
				TDCEntity obj = null;
				if (m_GameManager.GetObjectPool(m_Data.GameType, ref obj))
				{
					obj.GetController().TransformPosition = owner.transform.V3Forward(-owner.GetColliderRadius());
					obj.SetActive(true);
				}
				m_Data.Amount--;
				break;
			}
		}
	}

	#endregion

	#region Effect

	public virtual void ExcuteItem() {
		
		m_EffectManager.ExcuteEffect();
	}

	protected virtual bool CanActiveEffect(Dictionary<string, object> pars)
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

	protected virtual void ExcuteEffect(Dictionary<string, object> pars)
	{
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
	}

	protected virtual void AddValueEffect(Dictionary<string, object> pars)
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

	protected virtual void SubtractValueEffect(Dictionary<string, object> pars)
	{
//#if UNITY_EDITOR
//		foreach (var item in pars)
//		{
//			Debug.Log(string.Format("[{0} : {1}]", item.Key, item.Value));
//		}
//#endif
		var nameValue = pars["NameValue"].ToString();
		var value = pars["ToValue"];
		var toValue = float.Parse (pars["ToValue"].ToString());
		m_Data.Owner.GetEntity().SetProperty(nameValue, -toValue);
	}

	#endregion

	#region Getter & Setter

	public UIItemData GetData() {
		return m_Data;
	}

	#endregion

}
