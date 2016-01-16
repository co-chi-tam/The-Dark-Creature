using UnityEngine;
using System.Collections;
using FSM;

public class FSMHatchingState : FSMBaseState {

	public FSMHatchingState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetWaitingTime(m_Controller.GetDuration());
		m_Controller.SetAnimation(EAnimation.Action1);
		m_Controller.SetActive (true);
		m_Controller.CallBackEvent("OnIdle");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}

