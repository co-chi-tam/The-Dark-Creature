using UnityEngine;
using ObjectPool;

public class TDCBaseGroupController : TDCBaseController
{
	#region Properties

	protected TDCObjectPool<TDCBaseController> m_MemberPool;
	protected TDCGroupData m_GroupData;
	protected TDCEnum.EGameType m_GroupMemberType;

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
    }

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (TransformPosition, GetRadius());
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (TransformPosition, GetRadius() * 2f);
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

	#region getter & Setter

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
