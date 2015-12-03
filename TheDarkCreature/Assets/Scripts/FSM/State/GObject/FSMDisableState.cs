using UnityEngine;
using System.Collections;
using FSM;

public class FSMDisableState : FSMBaseState {

	private TDCCampFireController m_CampFireController;

	public FSMDisableState(TDCBaseController controller) : base (controller)
	{
		m_CampFireController = controller as TDCCampFireController;
	}
	
	public override void StartState() {
		m_CampFireController.SetActive (false);
	}
	
	public override void UpdateState() {
		
	}
	
	public override void ExitState()
	{
		
	}
}
