using UnityEngine;
using FSM;

public class FSMFlyFollowState : FSMBaseState {

	public FSMFlyFollowState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
		var radius = m_Controller.GetColliderRadius();
		var leadRadius = m_Controller.GetLeaderEntity().GetColliderRadius();
		var randomPos = TDCUltilities.RandomAroundCircle(radius + leadRadius);
		var leadPos = m_Controller.GetLeaderPosition();
		leadPos.x += randomPos.x;
		leadPos.y = 0f;
		leadPos.z += randomPos.y;
		m_Controller.SetTargetPosition(leadPos);
	}

	public override void UpdateState() {
		var targetPos = m_Controller.GetTargetPosition();
		m_Controller.FlyPosition(targetPos);
		m_Controller.CallBackEvent("OnMove");
	}

	public override void ExitState()
	{

	}
}

