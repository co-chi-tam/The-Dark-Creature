using UnityEngine;
using FSM;

public class FSMGroupSpawnMinMemberState : FSMBaseState {

	private TDCBaseGroupController m_GroupController;

	public FSMGroupSpawnMinMemberState(TDCBaseController controller) : base (controller)
	{
		m_GroupController = controller as TDCBaseGroupController;
	}

	public override void StartState() {
		m_GroupController.SpawnMinMember();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}

