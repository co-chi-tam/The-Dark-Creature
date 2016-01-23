using UnityEngine;
using System.Collections;
using FSM;

public class FSMFireState : FSMBaseState {

	private TDCCampFireController m_CampFireController;
	private float m_OffsetTime;

	public FSMFireState(TDCBaseController controller) : base (controller)
	{
		m_CampFireController = controller as TDCCampFireController;
	}
	
	public override void StartState() {
		m_OffsetTime = Random.Range(1f, 99f);
	}
	
	public override void UpdateState() {
		var time = Time.time + m_OffsetTime;
		m_CampFireController.UpdateFirePower (Time.deltaTime);
		m_CampFireController.SetIntensity(Mathf.PerlinNoise (time, time));
		m_CampFireController.ActiveSkill(0);
	}
	
	public override void ExitState()
	{
		
	}
}
