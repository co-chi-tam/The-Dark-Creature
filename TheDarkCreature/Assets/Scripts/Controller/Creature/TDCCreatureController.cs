using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCCreatureController : TDCBaseController {

	#region Property
	[SerializeField]
	private int m_HealthPoint = 0;
	[SerializeField]
	private int m_HeatPoint = 0;
	[SerializeField]
	private int m_HungerPoint = 0;
	[SerializeField]
	private int m_SanityPoint = 0;

	protected Animator m_AnimatorController;
	protected bool m_CanMove = true;
	protected int m_DamageTake = 0;
	protected LayerMask m_ColliderLayerMask;

	protected TDCCreatureData m_CreatureData;

	protected TDCSkillSlot m_SkillSlot;

	#endregion
	
	#region Implement Monobehaviour

	public override void Init ()
	{
		base.Init ();
		m_AnimatorController = this.GetComponent<Animator> ();
		m_TargetPosition = m_Transform.position;

		m_ColliderLayerMask = 1 << 8 | 1 << 10 | 1 << 31;
	}
	
	protected override void Start()
	{
		base.Start ();

		var idleState   	= new FSMIdleState(this);
		var moveState   	= new FSMMoveState(this);
		var findRandomState = new FSMFindRandomState(this);
		var avoidState  	= new FSMAvoidState(this);
		var chaseState  	= new FSMChaseState(this);
		var attackState  	= new FSMAttackState(this);
		var dieState    	= new FSMDieState(this);

		m_FSMManager.RegisterState("IdleState", idleState);
		m_FSMManager.RegisterState("MoveState", moveState);
		m_FSMManager.RegisterState("FindRandomState", findRandomState);
		m_FSMManager.RegisterState("AvoidState", avoidState);
		m_FSMManager.RegisterState("ChaseState", chaseState);
		m_FSMManager.RegisterState("AttackState", attackState);
		m_FSMManager.RegisterState("DieState", dieState);

		m_FSMManager.RegisterCondition("IsActive", GetActive);
		m_FSMManager.RegisterCondition("CanMove", CanMove);
		m_FSMManager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMManager.RegisterCondition("MoveToEnemy", MoveToEnemy);
		m_FSMManager.RegisterCondition("FoundEnemy", FoundEnemy);
		m_FSMManager.RegisterCondition("IsDeath", IsDeath);
		m_FSMManager.RegisterCondition("IsToFarGroup", IsToFarGroup);
		m_FSMManager.RegisterCondition("FoundFood", FoundFood);
		m_FSMManager.RegisterCondition("HaveEnemy", HaveEnemy);

	}

	protected override void Update ()
	{
		base.Update ();
		m_HealthPoint = GetHealth();
		if (m_CreatureData != null) {
			m_HealthPoint -= m_DamageTake;
			m_DamageTake = 0;
			SetHealth(m_HealthPoint);
		}
//		if (m_HungerPoint != 0)
//		{
//			m_CreatureData.CurrentHungerPoint += m_HungerPoint;
//			m_HungerPoint = 0;
//		}
//		if (m_SanityPoint != 0)
//		{
//			m_CreatureData.CurrentSanityPoint += m_SanityPoint;
//			m_SanityPoint = 0;
//		}
//		if (m_HeatPoint != 0)
//		{
//			m_CreatureData.CurrentHeatPoint += m_HeatPoint;
//			m_HeatPoint = 0;
//		}
	}

	public override void OnBecameVisible()
	{
		base.OnBecameVisible();
		m_AnimatorController.enabled = true;
		m_Collider.enabled = true;
	}

	public override void OnBecameInvisible()
	{
		base.OnBecameInvisible();
		m_AnimatorController.enabled = false;
		m_Collider.enabled = false;
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (TransformPosition, m_CreatureData.AttackRange);
		Gizmos.color = Color.white;
		Gizmos.DrawLine(TransformPosition, GetTargetPosition());
	}

	#endregion
	
	#region Main Method

	public override void ActiveSkill(int index) {
		base.ActiveSkill(index);
	}

	public override void ApplyDamage (int damage, TDCBaseController attacker)
	{
		base.ApplyDamage(damage, attacker);
		if (attacker.GetActive())
		{
			m_DamageTake += damage;
		}
		if (GetEnemyController() == null)
		{
			SetEnemyController(attacker);
		}
	}

	public virtual void OnSelectedItem(int itemIndex) {
		
	}

	public virtual int AddItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount) {
		return 0;
	}

	protected int FindEmptySlot() {
		var inventory = m_CreatureData.Inventory;
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] == null)
				return i;
		}
		return -1;
	}

	protected int FindItemSlot(TDCEnum.EGameType gameType) {
		var inventory = m_CreatureData.Inventory;
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] == null)
				continue;
			var data = inventory[i].GetData();
			if (data != null)
			{
				if (data.GameType == gameType)
					return i;
			}
		}
		return -1;
	}

	public override void MovePosition(Vector3 position) {
		base.MovePosition(position);
		MovePosition(position, m_CreatureData.MoveSpeed);
	}
	
	public override void LookAtRotation(Vector3 rotation)
	{
		base.LookAtRotation(rotation);
		//Look at and dampen the rotation
		rotation.y = 0f;
		var direction = m_Transform.position - rotation;
		Quaternion rot = Quaternion.LookRotation(direction);
		m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, rot, 
		                                        Time.deltaTime * m_CreatureData.RotationSpeed);
	}
	
	protected void MovePosition(Vector3 position, float speed) {
		position.y = 0f;
		var direction = position - m_Transform.position;
		//m_Rigidbody.MovePosition(m_Transform.position + direction.normalized * speed * Time.deltaTime);
		m_Transform.position = m_Transform.position + direction.normalized * speed * Time.deltaTime;

		LookAtRotation(position);
	}

	public override void ResetObject ()
	{
		base.ResetObject();
		SetHealth (GetMaxHealth());
		SetEnemyController (null);
	}
	
	#endregion

	#region FSM

	internal override bool HaveEnemy() {
		base.HaveEnemy();
		var enemyCtrl = GetEnemyController();
		return enemyCtrl != null && enemyCtrl.GetActive();
	}

	internal virtual bool IsToFarGroup() {
		return false;
	}

	internal virtual bool IsDeath() {
		return GetHealth() <= 0f || GetActive() == false;
	}

	internal virtual bool CanMove() {
		return m_CanMove;
	}

	internal override bool MoveToTarget()
	{
		base.MoveToTarget();
		return (TransformPosition - m_TargetPosition).sqrMagnitude < 0.5f * 0.5f;  
	}

	internal override bool MoveToEnemy()
	{
		base.MoveToEnemy();
		if (GetEnemyController() != null)
		{
			var distance = (TransformPosition - GetEnemyPosition()).sqrMagnitude;
			var range = GetEnemyController().GetColliderRadius() + m_CreatureData.AttackRange;
			return distance < range * range; 
		}
		return true;
	}

	internal override bool CountdownWaitingTime() {
		return base.CountdownWaitingTime();     
	}

	internal virtual bool FoundEnemy() {
		var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_ColliderLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var target = m_GameManager.GetControllerByName (colliders[i].name);
			if (target == null || target.GetActive () == false || target == this) {
				continue;
			} else {
				if (GetTypeEnemies().IndexOf (target.GetGameType()) != -1) {
					SetEnemyController(target);
					return true;
				}
			}
		}
		return false;
	}

	internal virtual bool FoundFood() {
		if (GetEnemyController() != null)
		{
			return true;
		}
		var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_ColliderLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var food = m_GameManager.GetControllerByName (colliders[i].name);
			if (food == null || food.GetActive () == false || food == this) {
				continue;
			} else {
				if (GetTypeFoods().IndexOf (food.GetGameType()) != -1) {
					if (GetEnemyController() == null)
					{
						SetEnemyController(food);
					}
					return true;
				}
			}
		}
		return false;
	}

	#endregion
	
	#region Getter & Setter

	public override int GetMinDamage()
	{
		base.GetMinDamage();
		return m_CreatureData.MinDamage;
	}

	public override int GetMaxDamage()
	{
		base.GetMaxDamage();
		return m_CreatureData.MaxDamage;
	}

	public override void SetHeat(int value)
	{
		base.SetHeat(value);
		m_HeatPoint += value;
	}

	public override float GetColliderRadius() {
		return base.GetColliderRadius();
	}
	
	public override void SetData(TDCBaseData data) {
		base.SetData (data);
		m_CreatureData = data as TDCCreatureData;
	}

	public override TDCBaseData GetData() {
		return m_CreatureData as TDCBaseData;
	}
	
	public override void SetCanMove(bool value) {
		m_CanMove = value;
	}
	
	public override bool GetCanMove() {
		return m_CanMove;
	}

	public override void SetAnimation(EAnimation anim) {
		base.SetAnimation (anim);
		if (m_AnimatorController.runtimeAnimatorController != null) {
			m_AnimatorController.SetInteger ("AnimParam", (int)anim);
		}
	}
	
	public override float GetDetectEnemyRange() {
		return m_CreatureData.DetectRange;
	}

	public virtual List<TDCEnum.EGameType> GetTypeEnemies() {
		return m_CreatureData.TypeEnemies;
	}

	public virtual List<TDCEnum.EGameType> GetTypeFoods() {
		return m_CreatureData.TypeFoods;
	}

	public override int GetHealth ()
	{
		return m_CreatureData.CurrentHP;
	}

	public override void SetHealth (int value)
	{
		base.SetHealth(value);
		m_CreatureData.CurrentHP = value;
		var percentHP = m_CreatureData.CurrentHP / m_CreatureData.MaxHP * 100;
		CallBackEvent("OnHealthPoint" + percentHP);
	}

	public override int GetMaxHealth() {
		return m_CreatureData.MaxHP;
	}

	public override TDCItemController[] GetInventory()
	{
		return m_CreatureData.Inventory;
	}
	
	#endregion

}
