using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCBaseGroupController : TDCBaseController
{
	#region Properties

	#endregion

    #region Implement Mono

	public override void Init ()
	{
		base.Init ();
	}

	protected override void Start()
    {
		base.Start();

		var idleState = new FSMGroupIdleState(this);
		var waitingState = new FSMGroupWaitingState(this);
		var spawnMemberState = new FSMGroupSpawnMemberState(this);
		var spawnAllMemberState = new FSMGroupSpawnAllMemberState(this);
		var deathState = new FSMGroupDeathState(this);

		m_FSMManager.RegisterState("GroupIdleState", idleState);
		m_FSMManager.RegisterState("GroupWaitingState", waitingState);
		m_FSMManager.RegisterState("GroupSpawnMemberState", spawnMemberState);
		m_FSMManager.RegisterState("GroupSpawnAllMemberState", spawnAllMemberState);
		m_FSMManager.RegisterState("GroupDeathState", deathState);

		m_FSMManager.RegisterCondition("IsDeath", IsDeath);
		m_FSMManager.RegisterCondition("IsFullGroup", IsFullGroup);
		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
    }

	protected override void OnDrawGizmos() {
		base.OnDrawGizmos();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (TransformPosition, GetGroupRadius());
	}

    #endregion

	#region Main method

	public override Dictionary<string, object> GetObjectCurrentValue()
	{
		var curValue = base.GetObjectCurrentValue();
		curValue["State"] = m_FSMManager.StateCurrentName;
		return curValue;
	}

	protected virtual void CreatePositionMember() {

	}

	public virtual float GetTimeRespawnMember ()
	{
		return m_Entity.GetTimeRespawnMember();
	}

	public virtual void ReturnMember(TDCEntity member) {
		
	}

	public override TDCEntity SpawnMember() {
		return null;
	}

	public virtual void SpawnAllMember() {
		
	}

	#endregion

	#region FSM

	internal virtual bool IsFullGroup() {
		return false;
	}

	internal override bool CountdownWaitingTime()
	{
		return base.CountdownWaitingTime();
	}

	#endregion

	#region Getter & Setter

	public virtual void SetCurrentMember(int value) {
		m_Entity.SetCurrentMember(value);
#if UNITY_EDITOR
		if (m_Entity.GetCurrentMember() < 0) {
			Debug.LogError ("[GroupController] Something wrong");
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

	public virtual TDCEnum.EGameType GetGroupMemberType() {
		return m_Entity.GetGroupMemberType();
	}

	#endregion

}
