﻿using UnityEngine;
using System.Collections;
using FSM;

public class FSMWeatherOverHeatState : FSMBaseState {

	public FSMWeatherOverHeatState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ActiveSkill((int)TDCEnum.EGameType.WeatherOverHeatSkill);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}
