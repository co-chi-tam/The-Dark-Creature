using UnityEngine;
using FSM;

public class FSMDestroySkillState : FSMBaseState {

	public FSMDestroySkillState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ResetObject();
		m_Controller.SetActive(false);
		Debug.LogError("FSMDestroySkillState");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}