using UnityEngine;
using ObjectPool;
using FSM;

public class TDCBaseGroupController : TDCBaseController
{
	#region Properties

	protected TDCObjectPool<TDCBaseController> m_MemberPool;
	protected TDCGroupData m_GroupData;

	#endregion

    #region Implement Mono

	public override void Init ()
	{
		base.Init ();
		m_MemberPool = new TDCObjectPool<TDCBaseController> ();
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
		return m_GroupData.TimeRespawnMember;
	}

	public virtual void CreatePoolMember() {
		
	}

	public virtual void ReturnMember(TDCBaseController member) {
		
	}

	public virtual TDCBaseController SpawnMember() {
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
		return m_IsActive;
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

	public override void SetData (TDCBaseData data)
	{
		base.SetData (data);
		m_GroupData = data as TDCGroupData;
	}

	public override TDCBaseData GetData ()
	{
		return m_GroupData as TDCBaseData;
	}

	public virtual int GetMinMember() {
		return m_GroupData.MinMember;
	}

	public virtual int GetMaxMember() {
		return m_GroupData.MaxMember;
	}

	public virtual float GetRadius() {
		return m_GroupData.Radius;
	}

	public virtual TDCEnum.EGameType GetGroupMemberType() {
		return m_GroupData.MemberType;
	}

	#endregion

}
