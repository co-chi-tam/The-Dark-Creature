using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonSpringState : FSMBaseState {

	private TDCPlaneController m_PlaneController;

	public FSMSeasonSpringState(TDCBaseController controller) : base (controller)
	{
		m_PlaneController = controller as TDCPlaneController;
	}

	public override void StartState() {
		m_PlaneController.SetTextureBySeason(TDCEnum.EGameSeason.Spring);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

