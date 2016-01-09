using UnityEngine;
using FSM;

public class FSMGroupSpawnAllMemberState : FSMBaseState {

	private TDCBaseGroupController m_GroupController;

	public FSMGroupSpawnAllMemberState(TDCBaseController controller) : base (controller)
	{
		m_GroupController = controller as TDCBaseGroupController;
	}

	public override void StartState() {
		m_GroupController.SpawnAllMember();
		m_GroupController.SetAnimation(EAnimation.Action1);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}

