using UnityEngine;
using FSM;

public class FSMLandingHomeState : FSMBaseState {

	public FSMLandingHomeState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
		m_Controller.CallBackEvent("OnMove");
	}

	public override void UpdateState() {
		var targetPos = m_Controller.GetTargetPosition();
		m_Controller.LandingPosition(targetPos);
	}

	public override void ExitState()
	{

	}
}