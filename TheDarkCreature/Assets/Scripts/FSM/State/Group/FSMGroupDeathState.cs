﻿using UnityEngine;
using FSM;

public class FSMGroupDeathState : FSMBaseState {

	public FSMGroupDeathState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ResetObject();
		m_Controller.SetActive(false);
		m_Controller.CallBackEvent("OnDealth");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}

