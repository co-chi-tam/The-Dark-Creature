using UnityEngine;
using System.Collections;
using FSM;

public class FSMWeatherWindyState : FSMBaseState {

	public FSMWeatherWindyState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState() {
		m_Controller.ActiveSkill((int)TDCEnum.EGameType.WeatherWindySkill);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}
