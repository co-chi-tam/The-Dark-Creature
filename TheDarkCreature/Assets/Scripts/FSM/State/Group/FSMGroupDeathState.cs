using UnityEngine;
using FSM;

public class FSMGroupDeathState : FSMBaseState {

	public FSMGroupDeathState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.DropItem();
		m_Controller.SetActive(false);
		m_Controller.CallBackEvent("OnDealth");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{
		m_Controller.ResetObject();
	}
}

