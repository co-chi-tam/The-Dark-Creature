using UnityEngine;
using FSM;

public class FSMEndSkillState : FSMBaseState {

	public FSMEndSkillState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ResetObject();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}