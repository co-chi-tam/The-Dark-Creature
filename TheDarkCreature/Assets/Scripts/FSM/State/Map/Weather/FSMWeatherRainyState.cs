using UnityEngine;
using System.Collections;
using FSM;

public class FSMWeatherRainyState : FSMBaseState {

	public FSMWeatherRainyState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.Activekill((int)TDCEnum.EGameType.WeatherRainySkill);
		Debug.Log("FSMWeatherRainyState");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}
