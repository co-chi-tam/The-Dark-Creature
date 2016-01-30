using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonWinterState : FSMBaseState {

	public FSMSeasonWinterState(TDCBaseController controller) : base (controller)
	{
		
	}

	public override void StartState() {
		m_Controller.IsActiveSeasonWinter();
		m_Controller.SetWaitingTime(m_Controller.GetDuration());
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

