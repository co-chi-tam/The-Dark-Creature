using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonWinterState : FSMBaseState {

	private TDCPlaneController m_PlaneController;

	public FSMSeasonWinterState(TDCBaseController controller) : base (controller)
	{
		m_PlaneController = controller as TDCPlaneController;
	}

	public override void StartState() {
		m_PlaneController.SetTextureBySeason(TDCEnum.EGameSeason.Winter);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

