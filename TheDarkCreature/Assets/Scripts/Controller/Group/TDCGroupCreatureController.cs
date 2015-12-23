using UnityEngine;
using FSM;

public class TDCGroupCreatureController : TDCBaseGroupController {

    #region Properties
    
    #endregion

    #region Implementation Mono
    public override void Start()
    {
        base.Start();

        m_FSMMamager.LoadFSM(m_GroupData.FSMPath);
    }

	public override void Update() {
		base.Update ();
        m_FSMMamager.UpdateState();
        StateName = m_FSMMamager.StateCurrentName;
    }

    #endregion

    #region Main method

	public override void CreatePoolMember(TDCEnum.EGameType type) {
		base.CreatePoolMember (type);
		for (int i = 0; i < GetMaxMember(); i++) {
			var position = new Vector3 (Random.Range (TransformPosition.x - GetRadius(), 
			                                          TransformPosition.x + GetRadius()),
			                              TransformPosition.y,
			                            Random.Range (TransformPosition.z - GetRadius(), 
			             				 TransformPosition.z + GetRadius()));
			var controller = TDCGameManager.Instance.CreateCreature (type, 
			                                                         position, 
			                                                         Quaternion.identity, 
			                                                         this.gameObject);
			controller.name = string.Format ("{0} {1}", type.ToString(), i);
			controller.SetGroupController (this);
			controller.gameObject.SetActive (false);
			m_MemberPool.Create (controller);
		}
	}

	public override void ReturnMember(TDCBaseController member) {
		member.gameObject.SetActive (false);
		m_MemberPool.Set (member);
	}

	public override TDCBaseController SpawnMember ()
	{
		if (m_MemberPool.CountUnuse () == 0)
			return null;
		var member = m_MemberPool.Get ();
		member.ResetObject ();
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
        return m_IsActive;
    }

	internal override bool IsFullGroup() {
		return m_MemberPool.Count() == GetMinMember();
    }

	internal override bool CountdownWaitingTime()
    {
        m_WaitingTimeInterval -= Time.deltaTime;
        return m_WaitingTimeInterval <= 0;
    }

    #endregion

}
