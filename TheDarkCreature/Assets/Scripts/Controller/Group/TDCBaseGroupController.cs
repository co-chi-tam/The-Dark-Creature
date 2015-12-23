using UnityEngine;
using ObjectPool;
using FSM;

public class TDCBaseGroupController : TDCBaseController
{
	#region Properties

	protected TDCObjectPool<TDCBaseController> m_MemberPool;
	protected TDCGroupData m_GroupData;
	protected TDCEnum.EGameType m_GroupMemberType;
	protected FSMManager m_FSMMamager;

	#endregion

    #region Implement Mono

	public override void Init ()
	{
		base.Init ();
		m_MemberPool = new TDCObjectPool<TDCBaseController> ();
	}

    public override void Start()
    {
		base.Start();

		m_FSMMamager = new FSMManager();

		var idleState = new FSMGroupIdleState(this);
		var waitingState = new FSMGroupWaitingState(this);
		var spawnMemberState = new FSMGroupSpawnMemberState(this);
		var spawnMinMemberState = new FSMGroupSpawnMinMemberState(this);
		var spawnMaxMemberState = new FSMGroupSpawnMaxMemberState(this);

		m_FSMMamager.RegisterState("IdleState", idleState);
		m_FSMMamager.RegisterState("WaitingState", waitingState);
		m_FSMMamager.RegisterState("SpawnMemberState", spawnMemberState);
		m_FSMMamager.RegisterState("SpawnMinMemberState", spawnMinMemberState);
		m_FSMMamager.RegisterState("SpawnMaxMemberState", spawnMaxMemberState);

		m_FSMMamager.RegisterCondition("IsActive", IsActive);
		m_FSMMamager.RegisterCondition("IsFullGroup", IsFullGroup);
		m_FSMMamager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
    }

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (TransformPosition, GetRadius());
	}

    #endregion

	#region Main method

	public virtual float GetTimeRespawnMember ()
	{
		return m_GroupData.TimeRespawnMember;
	}

	public virtual void CreatePoolMember(TDCEnum.EGameType type) {
		m_GroupMemberType = type;
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

	#endregion

}
