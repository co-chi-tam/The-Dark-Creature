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
	protected Rigidbody m_Rigidbody;
	protected Vector3 m_TargetPosition;
	protected TDCBaseController m_EnemyController;
	protected bool m_CanMove = true;
	protected int m_DamageTake = 0;

	protected TDCCreatureData m_CreatureData;
	private TDCGameManager m_GameManager;

	#endregion
	
	#region Implement Monobehaviour

	public override void Init ()
	{
		base.Init ();
		m_Rigidbody	= this.GetComponent<Rigidbody>();
		m_AnimatorController = this.GetComponent<Animator> ();
		m_GameManager = TDCGameManager.GetInstance();
		m_TargetPosition = m_Transform.position;
	}
	
	public override void Start()
	{
		base.Start ();

		m_FSMMamager    = new FSMManager();

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

		m_FSMMamager.RegisterState("IdleState", idleState);
		m_FSMMamager.RegisterState("MoveState", moveState);
		m_FSMMamager.RegisterState("FindRandomState", findRandomState);
		m_FSMMamager.RegisterState("AvoidState", avoidState);
		m_FSMMamager.RegisterState("ChaseState", chaseState);
		m_FSMMamager.RegisterState("AttackState", attackState);
		m_FSMMamager.RegisterState("WaitingState", waiting);
		m_FSMMamager.RegisterState("WaitingOneSecondState", waitingOne);
		m_FSMMamager.RegisterState("WaitingOne2ThreeSecondState", waitingOne2Three);
		m_FSMMamager.RegisterState("DieState", dieState);

		m_FSMMamager.RegisterCondition("IsActive", GetActive);
		m_FSMMamager.RegisterCondition("CanMove", CanMove);
		m_FSMMamager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
		m_FSMMamager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMMamager.RegisterCondition("MoveToEnemy", MoveToEnemy);
		m_FSMMamager.RegisterCondition("FoundEnemy", FoundEnemy);
		m_FSMMamager.RegisterCondition("IsEnemyDie", IsEnemyDie);
		m_FSMMamager.RegisterCondition("IsDie", IsDie);
		m_FSMMamager.RegisterCondition("IsToFarGroup", IsToFarGroup);
		m_FSMMamager.RegisterCondition("FoundFood", FoundFood);
	}

	public override void Update ()
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

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (TransformPosition, m_CreatureData.AttackRange);
		Gizmos.color = Color.white;
		Gizmos.DrawLine(TransformPosition, GetTargetPosition());
	}

	#endregion
	
	#region Main Method

	public override void ApplyDamage (int damage, TDCBaseController attacker)
	{
		base.ApplyDamage(damage, attacker);
		if (attacker.GetActive())
		{
			m_DamageTake += damage;
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

	public override void WalkPosition(Vector3 position) {
		base.WalkPosition(position);
		MovePosition(position, m_CreatureData.WalkSpeed);
	}
	
	public override void RunPosition(Vector3 position)
	{
		base.RunPosition(position);
		MovePosition(position, m_CreatureData.RunSpeed);
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

	internal virtual bool HaveEnemy() {
		return false;
	}

	internal virtual bool HaveAttacker() {
		return false;
	}

	internal virtual bool IsToFarGroup() {
		return false;
	}

	internal virtual bool IsEnemyDie() {
		return false;
	}

	internal virtual bool IsDie() {
		return false;
	}

	internal virtual bool CanMove() {
		return m_CanMove;
	}

	internal virtual bool MoveToTarget()
	{
		return false; 
	}

	internal virtual bool MoveToEnemy()
	{
		return false; 
	}

	internal virtual bool CountdownWaitingTime() {
		return false;   
	}

	internal virtual bool FoundEnemy() {
		return false;
	}

	internal virtual bool FoundFood() {
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
		return m_EnemyController.TransformPosition;
	}
	
	public override void SetEnemyController(TDCBaseController controller)
	{
		base.SetEnemyController (controller);
		m_EnemyController = controller;
	}
	
	public override TDCBaseController GetEnemyController()
	{
		base.GetEnemyController();
		return m_EnemyController;
	}

	public override void SetAnimation(EAnimation anim) {
		base.SetAnimation (anim);
		if (m_AnimatorController.runtimeAnimatorController != null) {
			m_AnimatorController.SetInteger ("AnimParam", (int)anim);
		}
	}
	
	public override Vector3 GetTargetPosition() {
		return m_TargetPosition;
	}
	
	public override void SetTargetPosition(Vector3 pos) {
		base.SetTargetPosition (pos);
		m_TargetPosition = pos;
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
