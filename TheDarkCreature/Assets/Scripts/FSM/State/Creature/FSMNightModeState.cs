﻿using UnityEngine;
using System.Collections;
using FSM;

public class FSMNightModeState : FSMBaseState {

	public FSMNightModeState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Idle);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}
