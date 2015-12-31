using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Effect;

public class TDCItemController {

	#region Properties

	private TDCGameManager m_GameManager;
	private EffectManager m_EffectManager;
	private TDCItemData m_Data;

	#endregion

	#region Main methods

	public TDCItemController()
	{
		m_Data = null;
		m_EffectManager = null;
	}

	public TDCItemController(TDCItemData data)
	{
		m_Data = data;
		m_GameManager = TDCGameManager.GetInstance();

		m_EffectManager = new EffectManager();
		m_EffectManager.LoadEffect(m_Data.EffectPath);
		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);
		m_EffectManager.RegisterExcuteMethod("PrintDebug", PrintDebug);
		m_EffectManager.RegisterExcuteMethod("ExcuteEffect", ExcuteEffect);
	}

	internal virtual void PrintDebug(Dictionary<string, object> pars)
	{
#if UNITY_EDITOR
		foreach (var item in pars) {
			Debug.LogError(string.Format ("[{0}] - {1}", item.Key, item.Value));
		}
#endif
	}

	public virtual void ExcuteItem() {
		m_EffectManager.ExcuteEffect();
	}

	protected virtual bool CanActiveEffect(Dictionary<string, object> pars)
	{
		return m_Data.Amount > 0 && m_Data.Owner != null;
	}

	protected virtual void ExcuteEffect(Dictionary<string, object> pars)
	{
#if UNITY_EDITOR
		foreach (var item in pars)
		{
			Debug.LogError(string.Format("[{0} : {1}]", item.Key, item.Value));
		}
#endif

		var type = m_Data.ItemType;
		switch (type)
		{
			case TDCEnum.EItemType.Food:
				m_Data.Amount--;
				break;
			case TDCEnum.EItemType.Weapon: 
				{
					var weaponData = m_Data as TDCWeaponData;
					weaponData.Duration -= weaponData.DecreaseDuration;
					if (weaponData.Duration < 0f)
					{
						m_Data.Amount--;
					}
				} break;
			case TDCEnum.EItemType.GObject:
			case TDCEnum.EItemType.Item:
				{
					var owner = m_Data.Owner;
					m_GameManager.CreateCreature(m_Data.GameType, owner.TransformPosition, Quaternion.identity);
					m_Data.Amount--;
				}
				break;
		}
	}

	#endregion

	#region Getter & Setter

	public TDCItemData GetData() {
		return m_Data;
	}

	#endregion

}
