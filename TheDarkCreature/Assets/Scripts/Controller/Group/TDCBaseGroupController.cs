using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCBaseGroupController : TDCBaseController
{
	#region Properties

	#endregion

    #region Implement Monobehaviour

	public override void Init ()
	{
		base.Init ();

		var idleState 			= new FSMGroupIdleState(this);
		var waitingState 		= new FSMGroupWaitingState(this);
		var spawnMemberState 	= new FSMGroupSpawnMemberState(this);
		var spawnAllMemberState = new FSMGroupSpawnAllMemberState(this);
		var deathState 			= new FSMGroupDeathState(this);

		m_FSMManager.RegisterState("GroupIdleState", idleState);
		m_FSMManager.RegisterState("GroupWaitingState", waitingState);
		m_FSMManager.RegisterState("GroupSpawnMemberState", spawnMemberState);
		m_FSMManager.RegisterState("GroupSpawnAllMemberState", spawnAllMemberState);
		m_FSMManager.RegisterState("GroupDeathState", deathState);

		m_FSMManager.RegisterCondition("IsDeath", IsDeath);
		m_FSMManager.RegisterCondition("IsFullGroup", IsFullGroup);
		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
	}

	protected override void Start()
    {
		base.Start();
    }

	protected override void OnDrawGizmos() {
		base.OnDrawGizmos();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (TransformPosition, GetGroupRadius());
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

    #endregion

	#region Main method

	public override void VisibleObject(bool value)
	{
		base.VisibleObject(value);
		m_AnimatorController.enabled = value;
		m_Collider.enabled = value;
	}

	protected virtual void CreatePositionMember() {

	}

	public virtual float GetTimeSpawnMember ()
	{
		return m_Entity.GetTimeSpawnMember();
	}

	public virtual void ReturnMember(TDCEntity member) {
		
	}

	public override TDCEntity SpawnMember() {
		return null;
	}

	public virtual void SpawnAllMember() {
		
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

	internal virtual bool IsFullGroup() {
		return false;
	}

	internal virtual bool IsFullEgg() {
		return false;
	}

	internal override bool CountdownWaitingTime()
	{
		return base.CountdownWaitingTime();
	}

	internal virtual bool CanSpawnEggANDCountdownWaitingTime()
	{
		return false;
	}

	#endregion

	#region Getter & Setter

	public virtual void SetCurrentMember(int value) {
		if (m_Entity.GetActive() == false)
			return;
		m_Entity.SetCurrentMember(value);
#if UNITY_EDITOR
		if (m_Entity.GetCurrentMember() < 0) {
			Debug.LogError ("[GroupController] SetCurrentMember Something wrong " + this.name + " with value" + value);
		}
#endif
	}

	public virtual int GetCurrentMember() {
		return m_Entity.GetCurrentMember();
	}

	public virtual int GetMaxMember() {
		return m_Entity.GetMaxMember();
	}

	public virtual float GetGroupRadius() {
		return m_Entity.GetGroupRadius();
	}

	public virtual TDCEnum.EGameType GetMemberType() {
		return m_Entity.GetMemberType();
	}

	public override UIItemController[] GetInventory()
	{
		return m_Entity.GetInventory();
	}

	#endregion

}
