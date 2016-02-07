using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCCreature : TDCEntity
{
	#region Properties

	protected int m_DamageTake = 0;
	protected TDCEntity m_EnemyEntity;
	protected Vector3 m_TargetPosition;
	protected Vector3 m_StartPosition;
	protected Vector3 m_StartBattlePosition;

	private TDCEntity m_LeaderEntity;
	private TDCEntity m_GroupEntity;
	private TDCCreatureController m_Controller;
	private TDCCreatureData m_Data;

	#endregion

	#region Contructor

	public TDCCreature(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCCreatureController;
		m_Data = data as TDCCreatureData;

		m_SkillSlotComponent = new TDCSkillSlotComponent(this);
		m_SkillSlotComponent.CreateActiveSkillSlot(m_Data.ActiveSkill);
	}

	#endregion

	#region Main methods

	public override void Update(float dt)
	{
		base.Update(dt);
		if (GetActive())
		{
			CallBackEvent("OnAlive");
		}
		if (GetHeat() >= GetMaxHeat() - 10)
		{
			CallBackEvent("OnOverHeat");
		}
		if (GetHeat() <= 10)
		{
			CallBackEvent("OnUnderHeat");
		}

		m_SkillSlotComponent.UpdateComponent(dt);

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

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);

		CallBackEvent("OnApplyDamage");

		if (attacker.GetActive())
		{
			m_DamageTake += damage;
		}

		if (GetEnemyEntity() == null)
		{
			SetEnemyEntity(attacker);
		}
	}

	public override void AddSkillChain(int index)
	{
		base.AddSkillChain(index);
		if (m_EnemyEntity != null)
		{
			m_SkillSlotComponent.AddSkillChain(index);
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

		m_DamageTake = 0;
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
		m_Controller = controller as TDCCreatureController;
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

	public override Vector3 GetStartPosition()
	{
		return m_StartPosition;
	}

	public override void SetStartPosition(Vector3 pos) {
		m_StartPosition = pos;
	}

	public override Vector3 GetEnemyPosition()
	{
		if (m_EnemyEntity == null)
			return base.GetEnemyPosition();
		return m_EnemyEntity.GetController().TransformPosition;
	}

	public override void SetEnemyEntity(TDCEntity enemy)
	{
		if (m_EnemyEntity != enemy)
		{
			SetStartBattlePosition(m_Controller.TransformPosition);
		}
		m_EnemyEntity = enemy;
	}

	public override TDCEntity GetEnemyEntity()
	{
		return m_EnemyEntity;
	}

	public override void SetStartBattlePosition(Vector3 value)
	{
		m_StartBattlePosition = value;
	}

	public override Vector3 GetStartBattlePosition()
	{
		return m_StartBattlePosition;
	}

	public override void SetTransformPosition(Vector3 pos) {
		m_Controller.TransformPosition = pos;
	}

	public override Vector3 GetTransformPosition() {
		return m_Controller.TransformPosition;
	}

	public override void SetGroupEntity(TDCEntity group) {
		m_GroupEntity = group;
	}

	public override TDCEntity GetGroupEntity() {
		return m_GroupEntity;
	}

	public override Vector3 GetGroupPosition()
	{
		return m_GroupEntity.GetController().TransformPosition;
	}

	public override Vector3 GetTargetPosition() {
		return m_TargetPosition;
	}

	public override void SetTargetPosition(Vector3 pos) {
		m_TargetPosition = pos;
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
		m_Data = data as TDCCreatureData;
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
		m_Data.CurrentHeatPoint = value > m_Data.MaxHeatPoint ? m_Data.MaxHeatPoint : value;
		base.SetHeat(m_Data.CurrentHeatPoint);
	}

	public override int GetHeat()
	{
		return m_Data.CurrentHeatPoint;
	}

	public override int GetMaxHeat()
	{
		return m_Data.MaxHeatPoint;
	}

	public override void SetLeaderEntity(TDCEntity leader)
	{
		m_LeaderEntity = leader;
	}

	public override TDCEntity GetLeaderEntity()
	{
		return m_LeaderEntity;
	}

	public override Vector3 GetLeaderPosition()
	{
		return m_LeaderEntity.GetController().TransformPosition;
	}

	public override UIItemController[] GetInventory()
	{
		return m_Data.Inventory;
	}

	public override float GetAttackRange()
	{
		var enemy = GetEnemyEntity();
		if (enemy != null)
		{
			var creatureType = enemy.GetCreatureType();
			if (creatureType == TDCEnum.ECreatureType.Enviroment ||
			    creatureType == TDCEnum.ECreatureType.GObject ||
			    creatureType == TDCEnum.ECreatureType.Item)
			{ 
				return GetColliderRadius();
			}
//			else
//			{
//				return GetColliderRadius() + m_SkillSlotComponent.GetEffectRange();
//			}
		}
		return m_Data.AttackRange + GetColliderRadius();
	}

	public override bool GetIsShine()
	{
		return m_Data.IsShine;
	}

	#endregion
}

