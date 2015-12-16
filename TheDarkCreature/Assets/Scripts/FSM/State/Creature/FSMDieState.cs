using UnityEngine;
using System.Collections;
using FSM;

public class FSMDieState : FSMBaseState
{
    public FSMDieState(TDCBaseController controller) : base (controller)
    {

    }

    public override void StartState()
    {
        m_Controller.SetAnimation(EAnimation.Die);
		m_Controller.SetActive (false);
		m_Controller.GetGroupController ().ReturnMember (m_Controller);

//		var attacker = m_Controller.GetAttacker ();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
		m_Controller.SetAttacker (null);
    }
}
