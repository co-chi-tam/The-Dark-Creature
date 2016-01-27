using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonAutumnState : FSMBaseState {

	private TDCPlaneController m_PlaneController;

	public FSMSeasonAutumnState(TDCBaseController controller) : base (controller)
	{
		m_PlaneController = controller as TDCPlaneController;
	}

	public override void StartState() {
		m_PlaneController.SetTextureBySeason(TDCEnum.EGameSeason.Autumn);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

