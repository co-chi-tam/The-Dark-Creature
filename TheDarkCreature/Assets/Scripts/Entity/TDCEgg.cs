using UnityEngine;
using System.Collections;

public class TDCEgg : TDCEntity
{
	#region Properties

	private TDCEntity m_GroupEntity;
	private TDCEggController m_Controller;
	private TDCEggData m_Data;
	private int m_DamageTake = 0;

	#endregion

	#region Contructor

	public TDCEgg(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCEggController;
		m_Data = data as TDCEggData;
	}

	#endregion

	#region Main methods

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		if (attacker.GetActive())
		{
			m_DamageTake += damage;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (GetActive())
		{
			CallBackEvent("OnAlive");
		}
		var health = GetHealth();
		if (m_HealthPoint.Value != 0)
		{
			health += m_HealthPoint.Value;
			m_HealthPoint.Value = 0;
			SetHealth(health);
		}
		if (m_DamageTake != 0) {
			health -= m_DamageTake;
			m_DamageTake = 0;
			SetHealth(health);
		}
		if (m_HeatPoint.Value != 0)
		{
			var heat = GetHeat() + m_HeatPoint.Value;
			m_HeatPoint.Value = 0;
			SetHeat(heat);
		}
	}

	public override void ResetObject()
	{
		base.ResetObject();
		SetHealth (GetMaxHealth());
		SetHeat(GetMaxHeat() / 3);
		SetLeaderEntity(null);
		SetGroupEntity(null);
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

	public override float GetDetectRange()
	{
		return m_Data.DetectRange;
	}

	public override UIItemController[] GetInventory()
	{
		return m_Data.Inventory;
	}

	public override void SetTransformPosition(Vector3 pos) {
		m_Controller.TransformPosition = pos;
	}

	public override Vector3 GetTransformPosition() {
		return m_Controller.TransformPosition;
	}

	public override TDCEntity GetGroupEntity()
	{
		return m_GroupEntity;
	}

	public override void SetGroupEntity(TDCEntity group)
	{
		m_GroupEntity = group;
	}

	#endregion
}

