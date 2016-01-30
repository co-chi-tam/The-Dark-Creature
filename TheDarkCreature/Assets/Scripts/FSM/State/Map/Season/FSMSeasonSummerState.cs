using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonSummerState : FSMBaseState {

	public FSMSeasonSummerState(TDCBaseController controller) : base (controller)
	{
		
	}

	public override void StartState() {
		m_Controller.IsActiveSeasonSummer();
		m_Controller.SetWaitingTime(m_Controller.GetDuration());
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

