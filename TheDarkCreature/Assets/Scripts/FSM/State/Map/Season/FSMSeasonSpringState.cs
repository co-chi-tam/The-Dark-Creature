using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonSpringState : FSMBaseState {

	public FSMSeasonSpringState(TDCBaseController controller) : base (controller)
	{
		
	}

	public override void StartState() {
		m_Controller.IsActiveSeasonSpring();
		m_Controller.SetWaitingTime(m_Controller.GetDuration());
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

