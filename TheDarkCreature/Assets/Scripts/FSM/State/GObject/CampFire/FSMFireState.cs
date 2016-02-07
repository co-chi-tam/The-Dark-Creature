using UnityEngine;
using System.Collections;
using FSM;

public class FSMFireState : FSMBaseState {

	private TDCCampfireController m_CampfireController;
	private float m_OffsetTime;

	public FSMFireState(TDCBaseController controller) : base (controller)
	{
		m_CampfireController = controller as TDCCampfireController;
	}
	
	public override void StartState() {
		m_OffsetTime = Random.Range(1f, 99f);
	}
	
	public override void UpdateState() {
		var time = Time.time + m_OffsetTime;
		m_CampfireController.UpdateFirePower (Time.deltaTime);
		m_CampfireController.SetIntensity(Mathf.PerlinNoise (time, time));
		m_CampfireController.Activekill(0);
	}
	
	public override void ExitState()
	{
		
	}
}
