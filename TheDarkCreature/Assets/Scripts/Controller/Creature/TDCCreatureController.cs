using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCCreatureController : TDCBaseController {

	#region Property

	protected LayerMask m_ColliderLayerMask;

	#endregion
	
	#region Implement Monobehaviour

	public override void Init ()
	{
		base.Init ();

		m_ColliderLayerMask = 1 << 8 | 1 << 10 | 1 << 11 | 1 << 31;

		var idleState   	= new FSMIdleState(this);
		var moveState   	= new FSMMoveState(this);
		var findRandomState = new FSMFindRandomState(this);
		var avoidState  	= new FSMAvoidState(this);
		var flyAvoidState 	= new FSMFlyAvoidState(this);
		var chaseState  	= new FSMChaseState(this);
		var attackState  	= new FSMAttackState(this);
		var dieState    	= new FSMDieState(this);
		var flyState 		= new FSMFlyState(this);
		var landingState 	= new FSMLandingState(this);
		var wildModeState 	= new FSMWildModeState(this);
		var petModeState	= new FSMPetModeState(this);
		var followState 	= new FSMFollowState(this);
		var flyFollowState	= new FSMFlyFollowState(this);

		m_FSMManager.RegisterState("IdleState", idleState);
		m_FSMManager.RegisterState("MoveState", moveState);
		m_FSMManager.RegisterState("FindRandomState", findRandomState);
		m_FSMManager.RegisterState("AvoidState", avoidState);
		m_FSMManager.RegisterState("ChaseState", chaseState);
		m_FSMManager.RegisterState("AttackState", attackState);
		m_FSMManager.RegisterState("DieState", dieState);
		m_FSMManager.RegisterState("FlyAvoidState", flyAvoidState);
		m_FSMManager.RegisterState("FlyState", flyState);
		m_FSMManager.RegisterState("LandingState", landingState);
		m_FSMManager.RegisterState("WildModeState", wildModeState);
		m_FSMManager.RegisterState("PetModeState", petModeState);
		m_FSMManager.RegisterState("FollowState", followState);
		m_FSMManager.RegisterState("FlyFollowState", flyFollowState);

		m_FSMManager.RegisterCondition("IsActive", GetActive);
		m_FSMManager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMManager.RegisterCondition("MoveToEnemy", MoveToEnemy);
		m_FSMManager.RegisterCondition("FoundEnemy", FoundEnemy);
		m_FSMManager.RegisterCondition("IsDeath", IsDeath);
		m_FSMManager.RegisterCondition("IsToFarGroup", IsToFarGroup);
		m_FSMManager.RegisterCondition("IsToFarStartBattlePosition", IsToFarStartBattlePosition);
		m_FSMManager.RegisterCondition("IsToFarStartPosition", IsToFarStartPosition);
		m_FSMManager.RegisterCondition("IsToFarLeaderPosition", IsToFarLeaderPosition);
		m_FSMManager.RegisterCondition("FoundFood", FoundFood);
		m_FSMManager.RegisterCondition("IsEnemyDeath", IsEnemyDeath);
		m_FSMManager.RegisterCondition("IsLandingFinish", IsLandingFinish);
		m_FSMManager.RegisterCondition("HaveLeader", HaveLeader);
		m_FSMManager.RegisterCondition("HaveEnemyByLeader", HaveEnemyByLeader);
		m_FSMManager.RegisterCondition("IsLeaderDeath", IsLeaderDeath);
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
		Gizmos.DrawWireSphere (TransformPosition, GetDetectRange());
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (TransformPosition, m_Entity.GetAttackRange());
		Gizmos.color = Color.white;
		Gizmos.DrawLine(TransformPosition, GetTargetPosition());
	}

	#endregion
	
	#region Main Method

	public override Dictionary<string, object> GetObjectCurrentValue()
	{
		var curValue = base.GetObjectCurrentValue();
		curValue["State"] = m_FSMManager.StateCurrentName;
		curValue["Leader Name"] = m_Entity.GetLeaderEntity() == null ? "None" : m_Entity.GetLeaderEntity().GetController().name;
		return curValue;
	}

	public override void ActiveSkill(int index) {
		base.ActiveSkill(index);
	}

	public override void ApplyDamage (int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		m_Entity.ApplyDamage(damage, attacker);
		if (GetEnemyEntity() == null)
		{
			SetEnemyEntity(attacker);
		}
	}

	public virtual void OnSelectedItem(int itemIndex) {
		
	}

	public override int AddItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, int amount) {
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
		m_Entity.ResetObject();
	}

	public override void DropItem()
	{
		base.DropItem();
		var inventory = GetInventory();
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] == null)
				continue;
			var itemType = inventory[i].GetData().GameType;
			var amount = inventory[i].GetData().Amount;
			for (int x = 0; x < amount; x++) {
				TDCEntity item = null;
				if (m_GameManager.GetObjectPool(itemType, ref item))
				{
					var pos = UnityEngine.Random.insideUnitCircle * GetColliderRadius();
					var mPos = this.TransformPosition;
					mPos.y = 0f;
					item.GetController().TransformPosition = mPos + new Vector3(pos.x, 0f, pos.y);
					item.SetActive(true);
				}
			}
		}
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

	internal virtual bool IsToFarStartBattlePosition() {
		return false;
	}

	internal virtual bool IsToFarStartPosition() {
		return false;
	}

	internal virtual bool IsToFarLeaderPosition() {
		return false;
	}

	internal override bool IsDeath() {
		return GetHealth() <= 0 || GetActive() == false;
	}

	internal override bool MoveToTarget()
	{
		base.MoveToTarget();
		var mPosition = TransformPosition;
		var target = GetTargetPosition();
		mPosition.y = 0f;
//		target.y = 0f;
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
		var leader = GetLeaderEntity();
		if (leader != null)
		{
			return false;
		}
		var mPos = TransformPosition;
		var colliders = Physics.OverlapSphere(mPos, GetDetectRange(), m_ColliderLayerMask);
		if (colliders.Length == 0)
			return false;
		for (int i = 0; i < colliders.Length; i++) {
			var target = m_GameManager.GetEntityByName (colliders[i].name);
			if (target == null || target.GetActive () == false || target == this.GetEntity()) {
				continue;
			} else {
				if (GetTypeEnemies().IndexOf (target.GetGameType()) != -1) {
					if (GetEnemyEntity() == null)
					{
						SetEnemyEntity(target);
					}
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
		var colliders = Physics.OverlapSphere(mPos, GetDetectRange(), m_ColliderLayerMask);
		if (colliders.Length == 0)
			return false;
		for (int i = 0; i < colliders.Length; i++) {
			var food = m_GameManager.GetEntityByName (colliders[i].name);
			if (food == null || food.GetActive () == false || food == this.GetEntity()) {
				continue;
			} else {
				if (GetTypeFoods().IndexOf (food.GetGameType()) != -1 || (TDCUltilities.IsPlayer(food) && food.GetSanity() < 1)) {
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

	internal virtual bool HaveLeader() {
		return false;
	}

	internal virtual bool HaveEnemyByLeader() {
		return false;
	}

	internal virtual bool IsLeaderDeath() {
		return false;
	}

	#endregion
	
	#region Getter & Setter

	public override string GetStateName()
	{
		return m_FSMManager.StateCurrentName;
	}

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
	
	public override float GetDetectRange() {
		return m_Entity.GetDetectRange();
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

	public override UIItemController[] GetInventory()
	{
		return m_Entity.GetInventory();
	}

	public override TDCEntity GetLeaderEntity()
	{
		return m_Entity.GetLeaderEntity();
	}

	public override void SetLeaderEntity(TDCEntity leader)
	{
		base.SetLeaderEntity(leader);
		m_Entity.SetLeaderEntity(leader);
	}

	public override Vector3 GetLeaderPosition() {
		return m_Entity.GetLeaderPosition();
	}

	#endregion

}
