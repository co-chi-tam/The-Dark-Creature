using UnityEngine;
using FSM;

public class FSMGroupSpawnMaxMemberState : FSMBaseState {

	private TDCBaseGroupController m_GroupController;

	public FSMGroupSpawnMaxMemberState(TDCBaseController controller) : base (controller)
	{
		m_GroupController = controller as TDCBaseGroupController;
	}

	public override void StartState() {
		m_GroupController.SpawnMaxMember();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}

