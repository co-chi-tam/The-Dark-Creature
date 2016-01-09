using UnityEngine;
using FSM;

public class FSMGroupIdleState : FSMBaseState {
	
	public FSMGroupIdleState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Idle);
	}
	
	public override void UpdateState() {
		
	}
	
	public override void ExitState()
	{
		
	}
}

