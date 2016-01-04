using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TDCCreature : TDCEntity
{
	#region Properties

	private int m_HealthPoint = 0;
	private int m_HeatPoint = 0;
	private int m_HungerPoint = 0;
	private int m_SanityPoint = 0;
	private int m_DamageTake = 0;
	private bool m_CanMove = true;
	private float m_TimeReset = 0f;
	private TDCCreatureController m_Controller;
	private TDCCreatureData m_Data;

	protected TDCEntity m_GroupEntity;
	protected TDCEntity m_EnemyEntity;
	protected TDCObjectProperty<float> m_OffsetSpeed;

	protected Vector3 m_TargetPosition;
	protected Vector3 m_StartPosition;

	#endregion

	#region Contructor

	public TDCCreature(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCCreatureController;
		m_Data = data as TDCCreatureData;

		m_OffsetSpeed = new TDCObjectProperty<float>("OffsetSpeed", 1f);

		RegisterProperty(m_OffsetSpeed);
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

		if ((Time.time - m_TimeReset) > 1f)
		{
			m_OffsetSpeed.SetValue (1f);
			m_TimeReset = Time.time;
		}
		m_HealthPoint = GetHealth();
		if (m_Data != null) {
			m_HealthPoint -= m_DamageTake;
			m_DamageTake = 0;
			SetHealth(m_HealthPoint);
		}
//		if (m_HungerPoint != 0)
//		{
//			m_Data.CurrentHungerPoint += m_HungerPoint;
//			m_HungerPoint = 0;
//		}
//		if (m_SanityPoint != 0)
//		{
//			m_Data.CurrentSanityPoint += m_SanityPoint;
//			m_SanityPoint = 0;
//		}
//		if (m_HeatPoint != 0)
//		{
//			m_Data.CurrentHeatPoint += m_HeatPoint;
//			m_HeatPoint = 0;
//		}
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

	public override Vector3 GetStartPosition()
	{
		return m_StartPosition;
	}

	public override void SetStartPosition(Vector3 pos) {
		m_StartPosition = pos;
	}

	public override Vector3 GetEnemyPosition()
	{
		return m_EnemyEntity.GetController().TransformPosition;
	}

	public override void SetEnemyEntity(TDCEntity enemy)
	{
		m_EnemyEntity = enemy;
	}

	public override TDCEntity GetEnemyEntity()
	{
		return m_EnemyEntity;
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

	public override void SetHeat(int value)
	{
		base.SetHeat(value);
		m_HeatPoint += value;
	}

	public override float GetColliderRadius() {
		return m_Controller.GetColliderRadius();
	}

	public override void SetData(TDCBaseData data) {
		base.SetData (data);
		m_Data = data as TDCCreatureData;
	}

	public override void SetCanMove(bool value) {
		m_CanMove = value;
	}

	public override bool GetCanMove() {
		return m_CanMove;
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

	public override int GetHealth ()
	{
		return m_Data.CurrentHP;
	}

	public override void SetHealth (int value)
	{
		m_Data.CurrentHP = value;
		var percentHP = m_Data.CurrentHP / m_Data.MaxHP * 100;
		CallBackEvent("OnHealthPoint" + percentHP);
	}

	public override int GetMaxHealth() {
		return m_Data.MaxHP;
	}

	public override TDCItemController[] GetInventory()
	{
		return m_Data.Inventory;
	}

	public override float GetAttackRange()
	{
		return m_Data.AttackRange;
	}

	#endregion
}

