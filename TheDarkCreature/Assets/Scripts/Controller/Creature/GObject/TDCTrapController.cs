using UnityEngine;
using System.Collections;
using FSM;

public class TDCTrapController : TDCCreatureController
{
    #region Property

	private FSMManager m_FSMMamager;
    private LayerMask m_EnemyLayerMask;
    private TDCGameManager m_GameManager;

    #endregion

    #region Implementation Mono
    public override void Start()
    {
        base.Start();

        m_FSMMamager = new FSMManager();

        var idleState = new FSMIdleState(this);
        var freezeEnemyState = new FSMFreezeEnemyState(this);
        var dieState = new FSMDieState(this);

        m_FSMMamager.RegisterState("IdleState", idleState);
        m_FSMMamager.RegisterState("FreezeEnemyState", freezeEnemyState);
        m_FSMMamager.RegisterState("DieState", dieState);

        m_FSMMamager.RegisterCondition("FoundEnemy", FoundEnemy);
        m_FSMMamager.RegisterCondition("IsActive", GetActive);
        m_FSMMamager.LoadFSM(GetData().FSMPath);

        m_EnemyLayerMask = 1 << 8;

        m_GameManager = TDCGameManager.GetInstance();
    }

	public override void Update() {
		base.Update ();
        m_FSMMamager.UpdateState();
        StateName = m_FSMMamager.StateCurrentName;
    }

    #endregion

    #region Main method

    public override void SetActive(bool value)
    {
        base.SetActive(value);
        if (!value) {
//            m_GameManager.ReturnTrap(this);
        }
    }

    private bool FoundEnemy()
    {
        var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_EnemyLayerMask);
        if (colliders.Length != 0)
        {
			SetEnemyController(colliders[0].GetComponent<TDCBaseController>());
            return true;
        }
        return false;
    }

    #endregion
}
