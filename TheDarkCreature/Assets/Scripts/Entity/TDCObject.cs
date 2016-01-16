using UnityEngine;
using System.Collections;

public class TDCObject : TDCEntity
{
	#region Properties
	protected TDCObjectController m_Controller;
	protected TDCGObjectData m_Data;
	#endregion

	#region Contructor

	public TDCObject(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCObjectController;
		m_Data = data as TDCGObjectData;
	}

	#endregion

	#region Main methods

	public override void ResetObject()
	{
		base.ResetObject();
		SetHealth (GetMaxHealth());
		SetHeat(GetHeat() / 3);
		SetEnemyEntity (null);

		m_HealthPoint.Value = 0;
		m_HeatPoint.Value = 0;
	}

	#endregion

	#region Getter && Setter

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCGObjectData;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCObjectController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetActive(bool value)
	{
		base.SetActive(value);
		if (m_Controller != null && m_Controller.gameObject != null)
		{
			m_Controller.gameObject.SetActive(value);
		}
	}

	public override float GetDuration()
	{
		return m_Data.Duration;
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

	public override UIItemController[] GetItemInventory()
	{
		return m_Data.Inventory;
	}

	#endregion
}


