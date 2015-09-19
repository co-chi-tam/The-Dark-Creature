using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TDCCreatureController : TDCBaseController {

	#region Property
	protected Animator m_AnimatorController;
	protected Rigidbody m_Rigidbody;
	protected Vector3 m_TargetPosition;
	protected TDCBaseController m_EnemyController;
//	protected TDCBaseController m_TargetController;
	protected bool m_CanMove = true;

	protected TDCCreatureData m_CreatureData;

	public string m_HealthPoint;
	#endregion
	
	#region Implement Mono

	public override void Init ()
	{
		base.Init ();
		m_Rigidbody	= this.GetComponent<Rigidbody>();
		m_AnimatorController = this.GetComponent<Animator> ();
		m_TargetPosition = m_Transform.position;
	}
	
	public override void Start()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();

		if (m_CreatureData != null) {
			m_CreatureData.CurrentHP -= m_DamageTake;
			m_DamageTake = 0;
			m_HealthPoint = m_CreatureData.CurrentHP.ToString();
		}
	}

	#endregion
	
	#region Main Method

	public virtual void OnSelectedItem(TDCSlot slot) {
		
	}
	
	public override void WalkPosition(Vector3 position) {
		MovePosition(position, m_CreatureData.WalkSpeed);
	}
	
	public override void RunPosition(Vector3 position)
	{
		MovePosition(position, m_CreatureData.RunSpeed);
	}
	
	public override void MoveRotation(Vector3 rotation)
	{
		//Vector3 inverseVect = -m_Transform.InverseTransformPoint(rotation);
		//float rotationAngle = Mathf.Atan2(inverseVect.x, inverseVect.z) * Mathf.Rad2Deg;
		//Vector3 rotationVelocity = (Vector3.up * rotationAngle) * m_RotationSpeed * Time.deltaTime;
		//Vector3 deltavel = (rotationVelocity - m_Rigidbody.angularVelocity);
		//m_Rigidbody.AddTorque(deltavel, ForceMode.Impulse);
		
		//Look at and dampen the rotation
		var direction = m_Transform.position - rotation;
		Quaternion rot = Quaternion.LookRotation(direction);
		m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, rot, 
		                                        Time.deltaTime * m_CreatureData.RotationSpeed);
	}
	
	protected void MovePosition(Vector3 position, float speed) {
		var direction = position - m_Transform.position;
		//m_Rigidbody.MovePosition(m_Transform.position + direction.normalized * speed * Time.deltaTime);
		m_Transform.position = m_Transform.position + direction.normalized * speed * Time.deltaTime;
	}

	public override void ResetObject ()
	{
		SetHealth (GetMaxHealth());
	}
	
	#endregion
	
	#region Getter & Setter

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

//	public override void SetTargetController(TDCBaseController controller)
//	{
//		m_TargetController = controller;
//	}
//	
//	public override TDCBaseController GetTargetController()
//	{
//		return m_TargetController;
//	}
	
	public override void SetAnimation(EAnimation anim) {
		base.SetAnimation (anim);
		m_AnimatorController.SetInteger("AnimParam", (int) anim);
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
	
	#endregion

}
