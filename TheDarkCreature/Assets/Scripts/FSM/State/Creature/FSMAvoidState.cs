using UnityEngine;
using FSM;

public class FSMAvoidState : FSMBaseState
{
    public FSMAvoidState(TDCBaseController controller) : base (controller)
    {

    }

    public override void StartState()
    {
        m_Controller.SetAnimation(EAnimation.Run);
        var direction = m_Controller.TransformPosition - m_Controller.GetEnemyPosition();
        var newTarget = m_Controller.TransformPosition + direction.normalized 
                            * (m_Controller.GetDetectEnemyRange() + 10f);
        m_Controller.SetTargetPosition(newTarget);
    }

    public override void UpdateState()
    {
        var target = m_Controller.GetTargetPosition();
        m_Controller.RunPosition(target);
    }

    public override void ExitState()
    {

    }
}