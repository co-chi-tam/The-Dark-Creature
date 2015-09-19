﻿using UnityEngine;
using System.Collections;

public class FSMWaitingState : FSMBaseState
{
	public FSMWaitingState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Action1);
		var random = Random.Range (1f, 3f);
		m_Controller.SetWaitingTimeInterval (random);
	}
	
	public override void UpdateState()
	{
		
	}
	
	public override void ExitState()
	{
		
	}
}