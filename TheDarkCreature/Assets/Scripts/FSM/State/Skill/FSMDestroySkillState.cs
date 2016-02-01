using UnityEngine;
using FSM;

public class FSMDestroySkillState : FSMBaseState {

	public FSMDestroySkillState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetActive(false);
		m_Controller.ReturnObject();
		m_Controller.ResetObject();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{
		
	}
}