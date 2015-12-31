using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCCreatureController : TDCBaseController {

	#region Property
	[SerializeField]
	private int HealthPoint = 0;

	private int m_HeatPoint = 0;
	private int m_HungerPoint = 0;
	private int m_SanityPoint = 0;

	protected Animator m_AnimatorController;
	protected bool m_CanMove = true;
	protected int m_DamageTake = 0;
	protected LayerMask m_ColliderLayerMask;

	protected TDCCreatureData m_CreatureData;
	protected TDCGameManager m_GameManager;

	protected TDCSkillSlot m_SkillSlot;

	#endregion
	
	#region Implement Monobehaviour

	public override void Init ()
	{
		base.Init ();
		m_AnimatorController = this.GetComponent<Animator> ();
		m_GameManager = TDCGameManager.GetInstance();
		m_TargetPosition = m_Transform.position;
	}
	
	protected override void Start()
	{
		base.Start ();

		m_FSMManager    = new FSMManager();

		var idleState   	= new FSMIdleState(this);
		var moveState   	= new FSMMoveState(this);
		var findRandomState = new FSMFindRandomState(this);
		var avoidState  	= new FSMAvoidState(this);
		var chaseState  	= new FSMChaseState(this);
		var attackState  	= new FSMAttackState(this);
		var dieState    	= new FSMDieState(this);
		var waiting 		= new FSMWaitingState (this);
		var waitingOne 		= new FSMWaitingOneSecondState (this);
		var waitingOne2Three = new FSMWaitingOne2ThreeSecondState (this);

		m_FSMManager.RegisterState("IdleState", idleState);
		m_FSMManager.RegisterState("MoveState", moveState);
		m_FSMManager.RegisterState("FindRandomState", findRandomState);
		m_FSMManager.RegisterState("AvoidState", avoidState);
		m_FSMManager.RegisterState("ChaseState", chaseState);
		m_FSMManager.RegisterState("AttackState", attackState);
		m_FSMManager.RegisterState("WaitingState", waiting);
		m_FSMManager.RegisterState("WaitingOneSecondState", waitingOne);
		m_FSMManager.RegisterState("WaitingOne2ThreeSecondState", waitingOne2Three);
		m_FSMManager.RegisterState("DieState", dieState);

		m_FSMManager.RegisterCondition("IsActive", GetActive);
		m_FSMManager.RegisterCondition("CanMove", CanMove);
		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
		m_FSMManager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMManager.RegisterCondition("MoveToEnemy", MoveToEnemy);
		m_FSMManager.RegisterCondition("FoundEnemy", FoundEnemy);
		m_FSMManager.RegisterCondition("IsEnemyDie", IsEnemyDie);
		m_FSMManager.RegisterCondition("IsDeath", IsDeath);
		m_FSMManager.RegisterCondition("IsToFarGroup", IsToFarGroup);
		m_FSMManager.RegisterCondition("FoundFood", FoundFood);
		m_FSMManager.RegisterCondition("HaveEnemy", HaveEnemy);

		m_ColliderLayerMask = 1 << 8 | 1 << 10 | 1 << 31;
	}

	protected override void Update ()
	{
		base.Update ();
		if (m_CreatureData != null) {
			m_CreatureData.CurrentHP -= m_DamageTake;
			m_DamageTake = 0;
			// UI
			HealthPoint = m_CreatureData.CurrentHP;
		}
		if (m_HungerPoint != 0)
		{
			m_CreatureData.CurrentHungerPoint += m_HungerPoint;
			m_HungerPoint = 0;
		}
		if (m_SanityPoint != 0)
		{
			m_CreatureData.CurrentSanityPoint += m_SanityPoint;
			m_SanityPoint = 0;
		}
		if (m_HeatPoint != 0)
		{
			m_CreatureData.CurrentHeatPoint += m_HeatPoint;
			m_HeatPoint = 0;
		}
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

	public override void ActiveSkill() {
		base.ActiveSkill();
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
		var inventory = m_CreatureData.Inventory;
		var indexItemInInventory = FindItemSlot(gameType);
		var emptySlot = FindEmptySlot();
		if (emptySlot == -1)
			return -1;
		switch (itemType)
		{
			case TDCEnum.EItemType.Food:
			case TDCEnum.EItemType.Item:
				{
					if (indexItemInInventory != -1)
					{
						inventory[indexItemInInventory].GetData().Amount++;
						return indexItemInInventory;
					}
					else
					{
						inventory[emptySlot] = m_GameManager.CreateItem(gameType, itemType, this, amount);
					}
				}
				break;
			case TDCEnum.EItemType.GObject:
			case TDCEnum.EItemType.Weapon:
				{
					inventory[emptySlot] = m_GameManager.CreateItem(gameType, itemType, this, amount);
				}
				break;
		}
		return emptySlot;
	}

	private int FindEmptySlot() {
		var inventory = m_CreatureData.Inventory;
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] == null)
				return i;
		}
		return -1;
	}

	private int FindItemSlot(TDCEnum.EGameType gameType) {
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
		SetHealth (GetMaxHealth());
		SetEnemyController (null);
	}
	
	#endregion

	#region FSM

	internal override bool HaveEnemy() {
		return false;
	}

	internal virtual bool IsToFarGroup() {
		return false;
	}

	internal virtual bool IsEnemyDie() {
		if (m_EnemyController != null)
		{
			var enemy = m_EnemyController.GetHealth() <= 0 || m_EnemyController.GetActive() == false;
			if (enemy)
			{
				SetEnemyController(null);
			}
			return enemy;
		}
		return true;
	}

	internal virtual bool IsDeath() {
		return m_CreatureData.CurrentHP <= 0f || GetActive() == false;
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

	internal virtual bool CountdownWaitingTime() {
		m_WaitingTimeInterval -= Time.deltaTime;
		return m_WaitingTimeInterval <= 0;     
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
	
	public override Vector3 GetEnemyPosition() {
		return base.GetEnemyPosition();
	}
	
	public override void SetEnemyController(TDCBaseController controller)
	{
		base.SetEnemyController (controller);
	}
	
	public override TDCBaseController GetEnemyController()
	{
		return base.GetEnemyController();
	}

	public override void SetAnimation(EAnimation anim) {
		base.SetAnimation (anim);
		if (m_AnimatorController.runtimeAnimatorController != null) {
			m_AnimatorController.SetInteger ("AnimParam", (int)anim);
		}
	}
	
	public override Vector3 GetTargetPosition() {
		return base.GetTargetPosition();
	}
	
	public override void SetTargetPosition(Vector3 pos) {
		base.SetTargetPosition (pos);
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
		m_CreatureData.CurrentHP = value;
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
