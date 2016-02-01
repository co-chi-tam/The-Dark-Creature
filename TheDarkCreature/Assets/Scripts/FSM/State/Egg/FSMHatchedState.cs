using UnityEngine;
using System.Collections;
using FSM;

public class FSMHatchedState : FSMBaseState {

	public FSMHatchedState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ReturnObject();
		m_Controller.SetActive (false);
		m_Controller.CallBackEvent("OnDeath");
		m_Controller.SpawnMember();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{
		m_Controller.ResetObject();
	}
}