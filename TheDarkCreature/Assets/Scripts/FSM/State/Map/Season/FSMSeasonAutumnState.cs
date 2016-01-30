using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonAutumnState : FSMBaseState {

	public FSMSeasonAutumnState(TDCBaseController controller) : base (controller)
	{
		
	}

	public override void StartState() {
		m_Controller.IsActiveSeasonAutumn();
		m_Controller.SetWaitingTime(m_Controller.GetDuration());
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

