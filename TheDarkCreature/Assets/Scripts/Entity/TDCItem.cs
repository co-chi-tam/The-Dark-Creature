using UnityEngine;
using System.Collections;

public class TDCItem : TDCEntity
{
	#region Properties

	private TDCItemController m_Controller;
	private TDCItemData m_Data;
	private bool m_ItemActive;
	private TDCEntity m_Enemy;

	#endregion

	#region Contructor

	public TDCItem(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCItemController;
		m_Data = data as TDCItemData;
		m_ItemActive = true;
	}

	#endregion

	#region Main methods

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		m_ItemActive = false;
		m_Enemy = attacker;
	}

	public override void ResetObject()
	{
		base.ResetObject();
		SetItemActive(true);
	}

	#endregion

	#region Getter && Setter

	public override void SetItemActive(bool value)
	{
		base.SetItemActive(value);
		m_ItemActive = value;
	}

	public override bool GetItemActive() {
		return m_ItemActive;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCItemController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCItemData;
	}

	public override void SetActive(bool value)
	{
		base.SetActive(value);
		if (m_Controller != null && m_Controller.gameObject != null)
		{
			m_Controller.gameObject.SetActive(value);
		}
		m_ItemActive = value;
	}

	public override TDCEntity GetEnemyEntity()
	{
		return m_Enemy;
	}

	public override string GetFSMPath()
	{
		return m_Data.FSMPath;
	}

	public override TDCEnum.EGameType GetGameType()
	{
		return m_Data.GameType;
	}

	public override TDCEnum.ECreatureType GetCreatureType()
	{
		return m_Data.CreatureType;
	}

	public override TDCEnum.EItemType GetItemType()
	{
		return m_Data.ItemType;
	}

	public override float GetColliderRadius()
	{
		return m_Controller.GetColliderRadius();
	}

	#endregion
}

