using UnityEngine;
using FSM;

public class TDCGroupCreatureController : TDCBaseGroupController {

    #region Properties
    private FSMManager m_FSMMamager;
    #endregion

    #region Implementation Mono
    public override void Start()
    {
        base.Start();

        m_FSMMamager = new FSMManager();

        var idleState = new FSMGroupIdleState(this);
        var waitingState = new FSMGroupWaitingState(this);
        var spawnMemberState = new FSMGroupSpawnMemberState(this);

        m_FSMMamager.RegisterState("IdleState", idleState);
        m_FSMMamager.RegisterState("WaitingState", waitingState);
        m_FSMMamager.RegisterState("SpawnMemberState", spawnMemberState);

        m_FSMMamager.RegisterCondition("IsActive", IsActive);
        m_FSMMamager.RegisterCondition("IsFullGroup", IsFullGroup);
        m_FSMMamager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);

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
			if (i < GetMinMember()) 
			{
				var go = m_MemberPool.Get();
				go.SetActive (true);
			}
		}
	}

	public override void ReturnMember(TDCBaseController member) {
		member.gameObject.SetActive (false);
		m_MemberPool.Set (member);
	}

	public override TDCBaseController SpawnMember ()
	{
		var member = m_MemberPool.Get ();
		member.ResetObject ();
		member.SetActive (true);
		return member;
	}

    private bool IsActive()
    {
        return m_IsActive;
    }

    private bool IsFullGroup() {
		return m_MemberPool.Count() == GetMinMember();
    }

    private bool CountdownWaitingTime()
    {
        m_WaitingTimeInterval -= Time.deltaTime;
        return m_WaitingTimeInterval <= 0;
    }

    #endregion

}
