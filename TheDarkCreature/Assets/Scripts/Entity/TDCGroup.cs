﻿using UnityEngine;
using System.Collections;

public class TDCGroup : TDCEntity
{

	#region Properties

	private TDCBaseGroupController m_Controller;
	private TDCGroupData m_Data;
	private int m_DamageTake = 0;
	protected Vector3 m_StartPosition;

	#endregion

	#region Contructor

	public TDCGroup(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCBaseGroupController;
		m_Data = data as TDCGroupData;
	}

	#endregion

	#region Main methods

	public override void ReturnMember(TDCEntity member)
	{
		base.ReturnMember(member);
		m_Controller.ReturnMember (member);
	}

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
		if (m_DamageTake != 0) {
			health -= m_DamageTake;
			m_DamageTake = 0;
			SetHealth(health);
		}
	}

	public override void ResetObject()
	{
		base.ResetObject();
		SetHealth (GetMaxHealth());
		SetHeat(GetHeat() / 3);
		SetEnemyEntity (null);

		m_DamageTake = 0;
		m_HealthPoint.Value = 0;
		m_HeatPoint.Value = 0;
	}

	#endregion

	#region Getter && Setter

	public override void SetStartPosition(Vector3 pos)
	{
		m_StartPosition = pos;
	}

	public override Vector3 GetStartPosition()
	{
		return m_StartPosition;
	}

	public override int GetHealth()
	{
		return m_Data.CurrentHP;
	}

	public override void SetHealth(int value)
	{
		base.SetHealth(value);
		m_Data.CurrentHP = value;
		var percentHP = m_Data.CurrentHP / m_Data.MaxHP * 100;
		CallBackEvent("OnHealthPoint" + percentHP);
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCBaseGroupController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCGroupData;
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

	public override int GetCurrentMember()
	{
		return m_Data.CurrentMember;
	}

	public override void SetCurrentMember(int value)
	{
		m_Data.CurrentMember = value;
	}

	public override int GetMaxMember() {
		return m_Data.MaxMember;
	}

	public override float GetGroupRadius() {
		return m_Data.Radius;
	}

	public override float GetTimeSpawnMember()
	{
		return m_Data.TimeSpawnMember;
	}

	public override TDCEnum.EGameType GetMemberType()
	{
		return m_Data.MemberType;
	}

	public override TDCEnum.EGameType GetGameType()
	{
		return m_Data.GameType;
	}

	public override TDCEnum.EGroupType GetGroupType()
	{
		return m_Data.GroupType;
	}

	public override TDCEnum.EGroupSpawnType GetGroupSpawnType()
	{
		return m_Data.GroupSpawnType;
	}

	public override float GetColliderRadius()
	{
		return m_Controller.GetColliderRadius();
	}

	public override void SetTransformPosition(Vector3 pos) {
		m_Controller.TransformPosition = pos;
	}

	public override Vector3 GetTransformPosition() {
		return m_Controller.TransformPosition;
	}

	public override UIItemController[] GetInventory()
	{
		return m_Data.Inventory;
	}

	public override bool GetIsShine()
	{
		return m_Data.IsShine;
	}

	#endregion

}

