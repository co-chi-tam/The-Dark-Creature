using UnityEngine;
using System.Collections;
using FSM;
using System;

public class FSMMoveState : FSMBaseState
{
	public FSMMoveState(TDCBaseController controller) : base (controller)
    {

	}

	public override void StartState()
    {
        m_Controller.SetAnimation(EAnimation.Walk);
    }

	public override void UpdateState()
    {
        var targetPos = m_Controller.GetTargetPosition();
        m_Controller.WalkPosition(targetPos);
        m_Controller.MoveRotation(targetPos);
    }

	public override void ExitState()
    {
       
    }
}