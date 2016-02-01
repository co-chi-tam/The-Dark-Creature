using UnityEngine;
using System.Collections;
using FSM;

public class FSMWeatherRainyState : FSMBaseState {

	public FSMWeatherRainyState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ActiveSkill((int)TDCEnum.EGameType.WeatherRainySkill);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}
