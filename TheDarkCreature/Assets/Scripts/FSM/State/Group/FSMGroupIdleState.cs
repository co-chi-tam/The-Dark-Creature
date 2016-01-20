using UnityEngine;
using FSM;

public class FSMGroupIdleState : FSMBaseState {

	private TDCBaseGroupController m_GroupController;

	public FSMGroupIdleState(TDCBaseController controller) : base (controller)
	{
		m_GroupController = controller as TDCBaseGroupController;
	}
	
	public override void StartState() {
		m_GroupController.SetAnimation(EAnimation.Idle);
	}
	
	public override void UpdateState() {
		
	}
	
	public override void ExitState()
	{
		
	}
}

