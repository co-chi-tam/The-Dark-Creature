using UnityEngine;
using System.Collections;
using FSM;

public class FSMFreezeEnemyState : FSMBaseState
{
    public FSMFreezeEnemyState(TDCBaseController controller) : base (controller)
    {

    }

    public override void StartState()
    {
		var target = m_Controller.GetEnemyController();
        target.SetCanMove(false);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}
