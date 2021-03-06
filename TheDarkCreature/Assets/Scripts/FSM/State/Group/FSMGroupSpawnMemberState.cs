﻿using UnityEngine;
using FSM;

public class FSMGroupSpawnMemberState : FSMBaseState {

	private TDCBaseGroupController m_GroupController;

	public FSMGroupSpawnMemberState(TDCBaseController controller) : base (controller)
	{
		m_GroupController = controller as TDCBaseGroupController;
	}

	public override void StartState() {
		m_GroupController.SetAnimation(EAnimation.Action1);
		m_GroupController.SpawnMember();
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}

