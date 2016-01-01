﻿using UnityEngine;
using System.Collections;
using FSM;

public class FSMFireState : FSMBaseState {

	private TDCCampFireController m_CampFireController;

	public FSMFireState(TDCBaseController controller) : base (controller)
	{
		m_CampFireController = controller as TDCCampFireController;
	}
	
	public override void StartState() {
		
	}
	
	public override void UpdateState() {
		m_CampFireController.UpdateFirePower (Time.deltaTime);
		m_CampFireController.SetIntensity(Mathf.PerlinNoise (Time.time, Time.time));
	}
	
	public override void ExitState()
	{
		
	}
}
