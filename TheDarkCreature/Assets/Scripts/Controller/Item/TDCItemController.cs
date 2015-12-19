using UnityEngine;
using System.Collections;
using Effect;

public class TDCItemController {

	#region Properties

	private EffectManager m_EffectManager;
	private TDCItemData m_Data;

	#endregion

	#region Main methods

	public TDCItemController(TDCItemData data)
	{
		m_Data = data;
		Init();
	}

	public virtual void Init() {
		m_EffectManager = new EffectManager();
		m_EffectManager.LoadEffect(m_Data.EffectPath);
		m_EffectManager.RegisterCondition("CanActiveEffect", CanActiveEffect);
		m_EffectManager.RegisterExcuteMethod("ExcuteEffect", ExcuteEffect);
	}

	public virtual void Init(TDCItemData data) {
		m_Data = data;
		Init();
	}

	public virtual void ExcuteItem() {
		m_EffectManager.ExcuteEffect();
	}

	protected virtual bool CanActiveEffect(object[] pas)
	{
		return m_Data.Amount > 0 && m_Data.Owner != null;
	}

	protected virtual void ExcuteEffect(object[] pas)
	{
#if UNITY_EDITOR
		for (int i = 0; i < pas.Length; i+=2)
		{
			UnityEngine.Debug.LogError (pas[i] + " / "+ pas[i + 1]);
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
				} break;
		}

	}

	#endregion

	#region Getter & Setter

	public TDCItemData GetData() {
		return m_Data;
	}

	public void SetData(TDCItemData value) {
		m_Data = value;
		Init();
	}

	#endregion

}
