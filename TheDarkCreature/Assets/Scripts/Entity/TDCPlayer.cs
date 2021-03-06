﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TDCPlayer : TDCCreature
{
	#region Properties

	private TDCPlayerController m_Controller;
	private TDCPlayerData m_Data;

	#endregion

	#region Contructor

	public TDCPlayer(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCPlayerController;
		m_Data = data as TDCPlayerData;

		m_SkillSlotComponent.CreatePassiveSkillSlot(m_Data.PassiveSkill);
	}

	#endregion

	#region Main methods

	public override void Update(float dt)
	{
		base.Update(dt);

		if (m_SanityPoint.Value != 0)
		{
			var sanity = GetSanity() + m_SanityPoint.Value;
			m_SanityPoint.Value = 0;
			SetSanity(sanity);
		}

		if (m_HungerPoint.Value != 0)
		{
			var hunger = GetHunger() + m_HungerPoint.Value;
			m_HungerPoint.Value = 0;
			SetHunger(hunger);
		}
	}

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		CallBackEvent("OnApplyDamage");

		if (attacker.GetActive())
		{
			m_DamageTake += damage;
		}
	}

	public override void AddSkillChain(int index)
	{
		base.AddSkillChain(index);
	}

	public override void ResetObject()
	{
		base.ResetObject();
		SetSanity(GetMaxSanity() / 3);
		SetHunger(GetMaxHunger() / 3);
		m_SanityPoint.Value = 0;
		m_HungerPoint.Value = 0;
	}

	#endregion

	#region Getter & Setter

	public override void SetActive(bool value)
	{
		base.SetActive(value);
		if (m_Controller != null && m_Controller.gameObject != null)
		{
			m_Controller.gameObject.SetActive(value);
		}
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCPlayerController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
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

	public override float GetMoveSpeed()
	{
		var offset = m_OffsetSpeed.Value;
		m_OffsetSpeed.Value = 1f;
		return m_Data.MoveSpeed * offset;
	}

	public override float GetRotationSpeed()
	{
		return m_Data.RotationSpeed;
	}

	public override int GetDamage()
	{
		base.GetDamage();
		return m_Data.Damage;
	}

	public override float GetColliderRadius() {
		return m_Controller.GetColliderRadius();
	}

	public override void SetData(TDCBaseData data) {
		base.SetData (data);
		m_Data = data as TDCPlayerData;
	}

	public override void SetAnimation(EAnimation anim) {
		m_Controller.SetAnimation (anim);
	}

	public override float GetDetectRange() {
		return m_Data.DetectRange;
	}

	public override List<TDCEnum.EGameType> GetTypeEnemies() {
		return m_Data.TypeEnemies;
	}

	public override List<TDCEnum.EGameType> GetTypeFoods() {
		return m_Data.TypeFoods;
	}

	// ======================= Status ================================
	public override int GetHealth ()
	{
		return m_Data.CurrentHP;
	}

	public override void SetHealth (int value)
	{
		base.SetHealth(value);
		m_Data.CurrentHP = value > m_Data.MaxHP ? m_Data.MaxHP : value;
	}

	public override int GetMaxHealth() {
		return m_Data.MaxHP;
	}

	public override void SetHeat(int value)
	{
		m_Data.CurrentHeatPoint = value > m_Data.MaxHeatPoint ? m_Data.MaxHeatPoint : value < 0 ? 0 : value;
		base.SetHeat(m_Data.CurrentHeatPoint);
	}

	public override int GetHeat()
	{
		return m_Data.CurrentHeatPoint;
	}

	public override void SetSanity(int value)
	{
		m_Data.CurrentSanityPoint = value > m_Data.MaxSanityPoint ? m_Data.MaxSanityPoint : value < 0 ? 0 : value;
		base.SetSanity(m_Data.CurrentSanityPoint);
	}

	public override int GetSanity()
	{
		return m_Data.CurrentSanityPoint;
	}

	public override void SetHunger(int value)
	{
		m_Data.CurrentHungerPoint = value > m_Data.MaxHungerPoint ? m_Data.MaxHungerPoint : value;
		base.SetHunger(m_Data.CurrentHungerPoint);
	}

	public override int GetHunger()
	{
		return m_Data.CurrentHungerPoint;
	}

	public override int GetMaxHunger()
	{
		return m_Data.MaxHungerPoint;
	}

	public override int GetMaxSanity()
	{
		return m_Data.MaxSanityPoint;
	}

	public override int AddItemInventory(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount)
	{
		return m_Controller.AddItem(gameType, itemType, amount);
	}

	public override UIItemController[] GetInventory()
	{
		return m_Data.Inventory;
	}

	public override float GetAttackRange()
	{
		return base.GetAttackRange();
	}

	public override void SetTransformPosition(Vector3 pos) {
		m_Controller.TransformPosition = pos;
	}

	public override Vector3 GetTransformPosition() {
		return m_Controller.TransformPosition;
	}

	public override bool GetIsShine()
	{
		return m_Data.IsShine;
	}

	#endregion

}

