using UnityEngine;
using System.Collections;

public class TDCEgg : TDCEntity
{
	#region Properties
	protected TDCEggController m_Controller;
	protected TDCEggData m_Data;
	#endregion

	#region Contructor

	public TDCEgg(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCEggController;
		m_Data = data as TDCEggData;
	}

	#endregion

	#region Main methods
	private float m_Time = 0f;

	public override void Update(float dt)
	{
		base.Update(dt);
		var health = GetHealth();
		if (m_HealthPoint.Value != 0)
		{
			health += m_HealthPoint.Value;
			m_HealthPoint.Value = 0;
			SetHealth(health);
		}
		if (m_HeatPoint.Value != 0)
		{
			var heat = GetHeat() + m_HeatPoint.Value;
			m_HeatPoint.Value = 0;
			SetHeat(heat);
		}

		if (Time.time - m_Time > 1f)
		{
			m_HeatPoint.Value += -1;
			m_Time = Time.time;
		}
	}

	public override void ResetObject()
	{
		base.ResetObject();
		SetHealth (GetMaxHealth());
		SetHeat(GetHeat() / 2);
		SetEnemyEntity (null);

		m_HealthPoint.Value = 0;
		m_HeatPoint.Value = 0;
	}

	#endregion

	#region Getter && Setter

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCEggData;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCEggController;
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
		m_Time = Time.time;
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

	public override int GetHealth()
	{
		return m_Data.CurrentHP;
	}

	public override void SetHealth(int value)
	{
		m_Data.CurrentHP = value;
	}

	public override int GetMaxHealth()
	{
		return m_Data.MaxHP;
	}

	public override void SetHeat(int value)
	{
		m_Data.CurrentHeatPoint = value;
	}

	public override int GetHeat()
	{
		return m_Data.CurrentHeatPoint;
	}

	public override int GetMaxHeat()
	{
		return m_Data.MaxHeatPoint;
	}

	public override TDCEnum.EGameType GetMemberType()
	{
		return m_Data.MemberType;
	}

	public override UIItemController[] GetItemInventory()
	{
		return m_Data.Inventory;
	}

	#endregion
}

