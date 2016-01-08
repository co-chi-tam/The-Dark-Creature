using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCCreatureController : TDCBaseController {

	#region Property

	protected Animator m_AnimatorController;
	protected LayerMask m_ColliderLayerMask;
	protected TDCSkillSlot m_SkillSlot;

	#endregion
	
	#region Implement Monobehaviour

	public override void Init ()
	{
		base.Init ();
		m_AnimatorController = this.GetComponent<Animator> ();

		m_ColliderLayerMask = 1 << 8 | 1 << 10 | 1 << 11 | 1 << 31;

		var idleState   	= new FSMIdleState(this);
		var moveState   	= new FSMMoveState(this);
		var findRandomState = new FSMFindRandomState(this);
		var avoidState  	= new FSMAvoidState(this);
		var chaseState  	= new FSMChaseState(this);
		var attackState  	= new FSMAttackState(this);
		var dieState    	= new FSMDieState(this);
		var flyState 		= new FSMFlyState(this);
		var landingState 	= new FSMLandingState(this);

		m_FSMManager.RegisterState("IdleState", idleState);
		m_FSMManager.RegisterState("MoveState", moveState);
		m_FSMManager.RegisterState("FindRandomState", findRandomState);
		m_FSMManager.RegisterState("AvoidState", avoidState);
		m_FSMManager.RegisterState("ChaseState", chaseState);
		m_FSMManager.RegisterState("AttackState", attackState);
		m_FSMManager.RegisterState("DieState", dieState);
		m_FSMManager.RegisterState("FlyState", flyState);
		m_FSMManager.RegisterState("LandingState", landingState);

		m_FSMManager.RegisterCondition("IsActive", GetActive);
		m_FSMManager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMManager.RegisterCondition("MoveToEnemy", MoveToEnemy);
		m_FSMManager.RegisterCondition("FoundEnemy", FoundEnemy);
		m_FSMManager.RegisterCondition("IsDeath", IsDeath);
		m_FSMManager.RegisterCondition("IsToFarGroup", IsToFarGroup);
		m_FSMManager.RegisterCondition("FoundFood", FoundFood);
		m_FSMManager.RegisterCondition("IsEnemyDeath", IsEnemyDeath);
		m_FSMManager.RegisterCondition("IsLandingFinish", IsLandingFinish);
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		m_Entity.Update(Time.fixedDeltaTime);
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
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (TransformPosition, GetDetectEnemyRange());
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (TransformPosition, m_Entity.GetAttackRange());
		Gizmos.color = Color.white;
		Gizmos.DrawLine(TransformPosition, GetTargetPosition());
	}

	#endregion
	
	#region Main Method

	public override void ActiveSkill(int index) {
		base.ActiveSkill(index);
	}

	public override void ApplyDamage (int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		m_Entity.ApplyDamage(damage, attacker);
	}

	public virtual void OnSelectedItem(int itemIndex) {
		
	}

	public virtual int AddItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount) {
		return 0;
	}

	protected int FindEmptySlot() {
		var inventory = m_Entity.GetInventory();
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] == null)
				return i;
		}
		return -1;
	}

	protected int FindItemSlot(TDCEnum.EGameType gameType) {
		var inventory = m_Entity.GetInventory();
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
	
	public override void LookAtRotation(Vector3 rotation)
	{
		base.LookAtRotation(rotation);
		rotation.y = 0f;
		var mPos = m_Transform.position;
		mPos.y = 0f;
		var direction = mPos - rotation;
		Quaternion rot = Quaternion.LookRotation(direction);
		m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, rot, Time.deltaTime * m_Entity.GetRotationSpeed());
	}
	
	public override void MovePosition(Vector3 position) {
		position.y = 0f;
		var direction = position - m_Transform.position;
		m_Transform.position = m_Transform.position + direction.normalized * GetMoveSpeed() * Time.deltaTime;
		LookAtRotation(position);
	}

	public override void ResetObject ()
	{
		base.ResetObject();
		SetHealth (GetMaxHealth());
		SetEnemyEntity (null);
	}
	
	#endregion

	#region FSM

	internal bool IsLandingFinish() {
		return TransformPosition.y < 0.001f;
	}

	internal override bool IsEnemyDeath() {
		base.IsEnemyDeath();
		var enemyCtrl = GetEnemyEntity();
		if (enemyCtrl == null)
			return true;
		var isEnemyDeath = enemyCtrl != null && enemyCtrl.GetActive() == false;
		if (isEnemyDeath)
		{
			SetEnemyEntity(null);
		}
		return isEnemyDeath;
	}

	internal virtual bool IsToFarGroup() {
		return false;
	}

	internal override bool IsDeath() {
		return GetHealth() <= 0f || GetActive() == false;
	}

	internal override bool MoveToTarget()
	{
		base.MoveToTarget();
		var mPosition = TransformPosition;
		var target = GetTargetPosition();
		mPosition.y = 0f;
		target.y = 0f;
		return (mPosition - target).sqrMagnitude < 0.5f * 0.5f;  
	}

	internal override bool MoveToEnemy()
	{
		base.MoveToEnemy();
		if (GetEnemyEntity() != null)
		{
			var distance = (TransformPosition - GetEnemyPosition()).sqrMagnitude;
			var range = GetEnemyEntity().GetColliderRadius() + m_Entity.GetAttackRange();
			return distance < range * range; 
		}
		return true;
	}

	internal override bool CountdownWaitingTime() {
		return base.CountdownWaitingTime();     
	}

	internal virtual bool FoundEnemy() {
		var mPos = TransformPosition;
		var colliders = Physics.OverlapSphere(mPos, GetDetectEnemyRange(), m_ColliderLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var target = m_GameManager.GetEntityByName (colliders[i].name);
			if (target == null || target.GetActive () == false || target == this.GetEntity()) {
				continue;
			} else {
				if (GetTypeEnemies().IndexOf (target.GetGameType()) != -1) {
					SetEnemyEntity(target);
					return true;
				}
			}
		}
		return false;
	}

	internal virtual bool FoundFood() {
		var enemy = GetEnemyEntity();
		if (enemy != null)
		{
			return true;
		}
		var mPos = TransformPosition;
		var colliders = Physics.OverlapSphere(mPos, GetDetectEnemyRange(), m_ColliderLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var food = m_GameManager.GetEntityByName (colliders[i].name);
			if (food == null || food.GetActive () == false || food == this.GetEntity()) {
				continue;
			} else {
				if (GetTypeFoods().IndexOf (food.GetGameType()) != -1) {
					if (GetEnemyEntity() == null)
					{
						SetEnemyEntity(food);
					}
					return true;
				}
			}
		}
		return false;
	}

	#endregion
	
	#region Getter & Setter

	public override void SetEnemyEntity(TDCEntity entity)
	{
		m_Entity.SetEnemyEntity(entity);
	}

	public override float GetMoveSpeed()
	{
		return m_Entity.GetMoveSpeed();
	}

	public override int GetDamage()
	{
		base.GetDamage();
		return m_Entity.GetDamage();
	}

	public override void SetHeat(int value)
	{
		base.SetHeat(value);
		m_Entity.SetHeat(value);
	}

	public override float GetColliderRadius() {
		return base.GetColliderRadius();
	}

	public override void SetAnimation(EAnimation anim) {
		base.SetAnimation (anim);
		if (m_AnimatorController.runtimeAnimatorController != null) {
			m_AnimatorController.SetInteger ("AnimParam", (int)anim);
		}
	}
	
	public override float GetDetectEnemyRange() {
		return m_Entity.GetDetectEnemyRange();
	}

	public virtual List<TDCEnum.EGameType> GetTypeEnemies() {
		return m_Entity.GetTypeEnemies();
	}

	public virtual List<TDCEnum.EGameType> GetTypeFoods() {
		return m_Entity.GetTypeFoods();
	}

	public override int GetHealth ()
	{
		return m_Entity.GetHealth();
	}

	public override void SetHealth (int value)
	{
		base.SetHealth(value);
		m_Entity.SetHealth(value);
	}

	public override int GetMaxHealth() {
		return m_Entity.GetMaxHealth();
	}

	public override TDCItemController[] GetInventory()
	{
		return m_Entity.GetInventory();
	}
	
	#endregion

}
