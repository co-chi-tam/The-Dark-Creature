using UnityEngine;
using System.Collections;
using FSM;

public class FSMWeatherOverHeatState : FSMBaseState {

	public FSMWeatherOverHeatState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.Activekill((int)TDCEnum.EGameType.WeatherOverHeatSkill);
		Debug.Log("FSMWeatherOverHeatState");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}
