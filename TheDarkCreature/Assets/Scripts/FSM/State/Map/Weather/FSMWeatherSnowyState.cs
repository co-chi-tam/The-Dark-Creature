using UnityEngine;
using System.Collections;
using FSM;

public class FSMWeatherSnowyState : FSMBaseState {

	public FSMWeatherSnowyState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ActiveSkill((int)TDCEnum.EGameType.WeatherSnowySkill);
		Debug.Log("FSMWeatherSnowyState");
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}
