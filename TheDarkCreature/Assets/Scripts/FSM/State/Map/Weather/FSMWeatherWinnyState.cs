﻿using UnityEngine;
using System.Collections;
using FSM;

public class FSMWeatherWindyState : FSMBaseState {

	public FSMWeatherWindyState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.Activekill((int)TDCEnum.EGameType.WeatherWindySkill);
		Debug.Log("FSMWeatherWindyState");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}
