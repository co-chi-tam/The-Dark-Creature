using UnityEngine;
using FSM;

public class FSMFlyState : FSMBaseState {

	public FSMFlyState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
		m_Controller.CallBackEvent("OnMove");
	}

	public override void UpdateState() {
		var targetPos = m_Controller.GetTargetPosition();
		m_Controller.FlyPosition(targetPos);
	}

	public override void ExitState()
	{

	}
}
