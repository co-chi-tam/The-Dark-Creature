using UnityEngine;
using System.Collections;
using FSM;

public class FSMFireState : FSMBaseState {

	private TDCCampFireController m_CampFireController;

	public FSMFireState(TDCBaseController controller) : base (controller)
	{
		m_CampFireController = controller as TDCCampFireController;
	}
	
	public override void StartState() {
		m_CampFireController.Init ();
	}
	
	public override void UpdateState() {
		m_CampFireController.UpdateFirePower (Time.deltaTime);
		m_CampFireController.SetIntensityPerlinNoise(Mathf.PerlinNoise (Time.time, Time.time));
	}
	
	public override void ExitState()
	{
		
	}
}
