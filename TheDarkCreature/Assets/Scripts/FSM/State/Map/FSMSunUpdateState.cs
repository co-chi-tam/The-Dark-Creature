using UnityEngine;
using System.Collections;
using FSM;

public class FSMSunUpdateState : FSMBaseState {

	private TDCSunController m_SunController;

	public FSMSunUpdateState(TDCBaseController controller) : base (controller)
	{
		m_SunController = controller as TDCSunController;
	}

	public override void StartState() {

	}

	public override void UpdateState() {
		m_SunController.UpdateLight();
	}

	public override void ExitState()
	{

	}
}

