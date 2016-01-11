using UnityEngine;
using System.Collections;

public class TDCEnviroment : TDCEntity
{
	protected TDCEntity m_GroupEntity;

	private TDCEnviromentController m_Controller;
	private TDCEnviromentData m_Data;
	private int m_DamageTake = 0;
	private int m_HealthPoint = 0;

	public TDCEnviroment(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCEnviromentController;
		m_Data = data as TDCEnviromentData;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (GetActive())
		{
			CallBackEvent("OnAlive");
		}
		m_HealthPoint = GetHealth();
		if (m_DamageTake != 0) {
			m_HealthPoint -= m_DamageTake;
			m_DamageTake = 0;
			SetHealth(m_HealthPoint);
		}
	}

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);

		CallBackEvent("OnApplyDamage");

		if (attacker.GetActive())
		{
			m_DamageTake += damage;
		}
	}

	public override void SetGroupEntity(TDCEntity group)
	{
		m_GroupEntity = group;
	}

	public override TDCEntity GetGroupEntity()
	{
		return m_GroupEntity;
	}

	public override void SetHealth(int value)
	{
		base.SetHealth(value);
		m_Data.CurrentHP = value;
		var percentHP = m_Data.CurrentHP / m_Data.MaxHP * 100;
		CallBackEvent("OnHealthPoint" + percentHP);
	}

	public override int GetHealth ()
	{
		return m_Data.CurrentHP;
	}

	public override int GetMaxHealth()
	{
		return m_Data.MaxHP;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCEnviromentController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCEnviromentData;
	}

	public override void SetActive(bool value)
	{
		base.SetActive(value);
		if (m_Controller != null && m_Controller.gameObject != null)
		{
			m_Controller.gameObject.SetActive(value);
		}
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

	public override float GetColliderRadius()
	{
		return m_Controller.GetColliderRadius();
	}

}

