using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCCreature : TDCEntity
{
	#region Properties

	protected int m_DamageTake = 0;
	protected float m_TimeReset = 0f;
	protected TDCEntity m_EnemyEntity;
	protected Vector3 m_TargetPosition;
	protected Vector3 m_StartPosition;
	protected Vector3 m_StartBattlePosition;

	protected TDCSkillSlot m_NormalSkill;

	private TDCEntity m_GroupEntity;
	private TDCCreatureController m_Controller;
	private TDCCreatureData m_Data;

	#endregion

	#region Contructor

	public TDCCreature(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCCreatureController;
		m_Data = data as TDCCreatureData;

		m_NormalSkill = new TDCSkillSlot(m_Data.NormalSkill, this);
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
		if (GetHeat() >= GetMaxHeat())
		{
			CallBackEvent("OnOverHeat");
		}

		m_NormalSkill.UpdateSkill(Time.fixedDeltaTime);

		if ((Time.time - m_TimeReset) > 1f)
		{
			m_OffsetSpeed.SetValue (1f);
			m_TimeReset = Time.time;
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

	public override void ActiveSkill(int index)
	{
		base.ActiveSkill(index);
		if (index == 0)
		{
			m_NormalSkill.ActiveSkill();
		}
		else
		{
			// TODO
		}
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
		m_EnemyEntity = enemy;
		SetStartBattlePosition(m_Controller.TransformPosition);
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
		return m_Data.MoveSpeed * m_OffsetSpeed.Value;
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

	public override float GetDetectEnemyRange() {
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
		m_Data.CurrentHP = value > m_Data.MaxHP ? m_Data.MaxHP : value;
		var percentHP = m_Data.CurrentHP / m_Data.MaxHP * 100;
		CallBackEvent("OnHealthPoint" + percentHP);
		base.SetHealth(m_Data.CurrentHP);
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

	public override TDCItemController[] GetInventory()
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
				creatureType == TDCEnum.ECreatureType.GObject) { 
				return GetColliderRadius();
			}
		}
		return m_Data.AttackRange + GetColliderRadius();
	}

	#endregion
}

