using UnityEngine;
using System.Collections;
using FSM;

public class FSMSeasonSummerState : FSMBaseState {

	private TDCPlaneController m_PlaneController;

	public FSMSeasonSummerState(TDCBaseController controller) : base (controller)
	{
		m_PlaneController = controller as TDCPlaneController;
	}

	public override void StartState() {
		m_PlaneController.SetTextureBySeason(TDCEnum.EGameSeason.Summer);
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}

}

