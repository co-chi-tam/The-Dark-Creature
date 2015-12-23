using UnityEngine;
using System.Collections;
using FSM;

public class FSMFindRandomState : FSMBaseState
{
	public FSMFindRandomState(TDCBaseController controller) : base (controller)
    {

	}

	public override void StartState()
    {
        m_Controller.SetAnimation(EAnimation.Idle);
		var groupRadius = m_Controller.GetGroupController ().GetRadius ();
		var randomX = Random.Range(m_Controller.GetStartPosition().x - groupRadius, m_Controller.GetStartPosition().x + groupRadius);
		var randomZ = Random.Range(m_Controller.GetStartPosition().z - groupRadius, m_Controller.GetStartPosition().z + groupRadius);

        m_Controller.SetTargetPosition (new Vector3(randomX, 0f, randomZ));
		m_Controller.SetWaitingTimeInterval (Random.Range (1f, 5f));
    }

	public override void UpdateState()
    {

	}

	public override void ExitState()
    {

    }
}