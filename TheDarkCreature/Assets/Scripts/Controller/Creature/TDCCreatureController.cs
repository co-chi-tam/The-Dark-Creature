using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCCreatureController : TDCBaseController {

	#region Property
	[SerializeField]
	private float HealthPoint;

	protected FSMManager m_FSMMamager;
	protected Animator m_AnimatorController;
	protected Rigidbody m_Rigidbody;
	protected Vector3 m_TargetPosition;
	protected TDCBaseController m_EnemyController;
	protected TDCBaseController m_AttackerController;
	protected bool m_CanMove = true;

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
		base.ApplyDamage (damage, attacker);
		SetAttacker (attacker);
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
		MovePosition(position, m_CreatureData.WalkSpeed);
	}
	
	public override void RunPosition(Vector3 position)
	{
		MovePosition(position, m_CreatureData.RunSpeed);
	}
	
	public override void LookAtRotation(Vector3 rotation)
	{
		//Vector3 inverseVect = -m_Transform.InverseTransformPoint(rotation);
		//float rotationAngle = Mathf.Atan2(inverseVect.x, inverseVect.z) * Mathf.Rad2Deg;
		//Vector3 rotationVelocity = (Vector3.up * rotationAngle) * m_RotationSpeed * Time.deltaTime;
		//Vector3 deltavel = (rotationVelocity - m_Rigidbody.angularVelocity);
		//m_Rigidbody.AddTorque(deltavel, ForceMode.Impulse);
		
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
		SetAttacker (null);
		SetEnemyController (null);
	}
	
	#endregion

	#region FSM

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

	public override TDCBaseController GetAttacker()
	{
		return m_AttackerController;
	}

	public override void SetAttacker(TDCBaseController attacker)
	{
		base.SetAttacker(attacker);
		m_AttackerController = attacker;
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
		base.SetCanMove (value);
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
