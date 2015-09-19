using UnityEngine;
using System.Collections;
using FSM;

public class FSMIdleState : FSMBaseState {

    public FSMIdleState(TDCBaseController controller) : base (controller)
    {

	}

	public override void StartState() {
        m_Controller.SetAnimation(EAnimation.Idle);
		m_Controller.SetActive (true);
    }

	public override void UpdateState() {

	}

	public override void ExitState()
    {

    }
}
