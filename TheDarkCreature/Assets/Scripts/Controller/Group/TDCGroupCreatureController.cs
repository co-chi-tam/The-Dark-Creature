using UnityEngine;
using System;
using FSM;

public class TDCGroupCreatureController : TDCBaseGroupController {

    #region Properties
    
	private TDCGameManager m_GameManager;

    #endregion

    #region Implementation Mono

	public override void Init()
	{
		base.Init();

		m_GameManager = TDCGameManager.GetInstance();
	}

	protected override void Start()
    {
        base.Start();

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
    }

	protected override void FixedUpdate() {
		base.FixedUpdate ();
        m_FSMManager.UpdateState();
        StateName = m_FSMManager.StateCurrentName;
    }

    #endregion

    #region Main method

	public override void CreatePoolMember() {
		base.CreatePoolMember ();
		for (int i = 0; i < GetMaxMember(); i++) {
			var randomPosition = UnityEngine.Random.insideUnitCircle * GetRadius();
			var memPosition = new Vector3(randomPosition.x, 0f, randomPosition.y);
			var entity = m_GameManager.CreateCreature ( m_Entity.GetMemberType(), 
															memPosition, 
															Quaternion.identity, 
															this.gameObject);
			entity.SetGroupEntity (this.GetEntity());
			entity.SetActive(false);
			m_MemberPool.Create (entity);
		}
	}

	public override void ReturnMember(TDCEntity member) {
		base.ReturnMember(member);
		member.SetActive(false);
		m_MemberPool.Set (member);
	}

	public override TDCEntity SpawnMember ()
	{
		if (m_MemberPool.CountUnuse () == 0)
			return null;
		var member = m_MemberPool.Get ();
		member.GetController().ResetObject ();
		member.SetActive (true);
		return member;
	}

	public override void SpawnMinMember ()
	{
		base.SpawnMinMember ();
		if (m_MemberPool.CountUnuse () == 0)
			return;
		for (int i = 0; i < GetMinMember (); i++) {
			SpawnMember ();
		}
	}

	public override void SpawnMaxMember ()
	{
		base.SpawnMaxMember ();
		if (m_MemberPool.CountUnuse () == 0)
			return;
		for (int i = 0; i < GetMaxMember (); i++) {
			SpawnMember ();
		}
	}

	#endregion

	#region FSM

	internal override bool IsActive()
    {
		return GetActive();
    }

	internal override bool IsFullGroup() {
		return m_MemberPool.Count() == GetMinMember();
    }

	internal override bool CountdownWaitingTime()
    {
		return base.CountdownWaitingTime();
    }

    #endregion

}
