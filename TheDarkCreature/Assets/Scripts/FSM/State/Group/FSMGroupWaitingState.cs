using UnityEngine;
using FSM;

public class FSMGroupWaitingState : FSMBaseState {

	private TDCBaseGroupController m_GroupController;

	public FSMGroupWaitingState(TDCBaseController controller) : base (controller)
	{
		m_GroupController = controller as TDCBaseGroupController;
	}
	
	public override void StartState() {
		m_Controller.SetWaitingTime (m_GroupController.GetTimeSpawnMember());
	}
	
	public override void UpdateState() {
		
	}
	
	public override void ExitState()
	{
		
	}
}