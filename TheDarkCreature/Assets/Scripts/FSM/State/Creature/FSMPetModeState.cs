﻿using UnityEngine;
using System.Collections;
using FSM;

public class FSMPetModeState : FSMBaseState {

	public FSMPetModeState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Idle);
		m_Controller.CallBackEvent("OnIdle");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}
