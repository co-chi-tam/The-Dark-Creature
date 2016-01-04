using UnityEngine;
using ObjectPool;
using FSM;

public class TDCBaseGroupController : TDCBaseController
{
	#region Properties

	protected TDCObjectPool<TDCEntity> m_MemberPool;

	#endregion

    #region Implement Mono

	public override void Init ()
	{
		base.Init ();
		m_MemberPool = new TDCObjectPool<TDCEntity> ();
	}

	protected override void Start()
    {
		base.Start();

		m_FSMManager = new FSMManager();

		var idleState = new FSMGroupIdleState(this);
		var waitingState = new FSMGroupWaitingState(this);
		var spawnMemberState = new FSMGroupSpawnMemberState(this);
		var spawnMinMemberState = new FSMGroupSpawnMinMemberState(this);
		var spawnMaxMemberState = new FSMGroupSpawnMaxMemberState(this);

		m_FSMManager.RegisterState("IdleState", idleState);
		m_FSMManager.RegisterState("WaitingState", waitingState);
		m_FSMManager.RegisterState("SpawnMemberState", spawnMemberState);
		m_FSMManager.RegisterState("SpawnMinMemberState", spawnMinMemberState);
		m_FSMManager.RegisterState("SpawnMaxMemberState", spawnMaxMemberState);

		m_FSMManager.RegisterCondition("IsActive", IsActive);
		m_FSMManager.RegisterCondition("IsFullGroup", IsFullGroup);
		m_FSMManager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
    }

	protected override void OnDrawGizmos() {
		base.OnDrawGizmos();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (TransformPosition, GetRadius());
	}

    #endregion

	#region Main method

	public virtual float GetTimeRespawnMember ()
	{
		return m_Entity.GetTimeRespawnMember();
	}

	public virtual void CreatePoolMember() {
		
	}

	public virtual void ReturnMember(TDCEntity member) {
		
	}

	public virtual TDCEntity SpawnMember() {
		return null;
	}

	public virtual void SpawnMinMember() {
		
	}

	public virtual void SpawnMaxMember() {

	}

	#endregion

	#region FSM

	internal virtual bool IsActive()
	{
		return GetActive();
	}

	internal virtual bool IsFullGroup() {
		return false;
	}

	internal virtual bool CountdownWaitingTime()
	{
		return false;
	}

	#endregion

	#region Getter & Setter

	public virtual int GetMinMember() {
		return m_Entity.GetMinMember();
	}

	public virtual int GetMaxMember() {
		return m_Entity.GetMaxMember();
	}

	public virtual float GetRadius() {
		return m_Entity.GetRadius();
	}

	public virtual TDCEnum.EGameType GetGroupMemberType() {
		return m_Entity.GetGroupMemberType();
	}

	#endregion

}
