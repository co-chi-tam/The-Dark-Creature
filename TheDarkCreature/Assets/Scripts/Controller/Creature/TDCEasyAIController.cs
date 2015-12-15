using UnityEngine;
using System.Collections;
using FSM;

public class TDCEasyAIController : TDCCreatureController
{
    #region Properties
    
    private FSMManager m_FSMMamager;
	private LayerMask m_EnemyLayerMask;
	private LayerMask m_FoodLayerMask;

	private TDCCreatureData m_CreatureData;

    #endregion

    #region Implementation Mono
    public override void Start()
    {
		base.Start ();

        m_FSMMamager    = new FSMManager();

        var idleState   	= new FSMIdleState(this);
        var moveState   	= new FSMMoveState(this);
        var findRandomState = new FSMFindRandomState(this);
		var avoidState  	= new FSMAvoidState(this);
		var huntingState  	= new FSMHuntingState(this);
		var attackState  	= new FSMAttackState(this);
        var dieState    	= new FSMDieState(this);
		var waitingState 	= new FSMWaitingState (this);

        m_FSMMamager.RegisterState("IdleState", idleState);
        m_FSMMamager.RegisterState("MoveState", moveState);
		m_FSMMamager.RegisterState("FindRandomState", findRandomState);
		m_FSMMamager.RegisterState("AvoidState", avoidState);
		m_FSMMamager.RegisterState("HuntingState", huntingState);
		m_FSMMamager.RegisterState("AttackState", attackState);
		m_FSMMamager.RegisterState("WaitingState", waitingState);
		m_FSMMamager.RegisterState("DieState", dieState);

        m_FSMMamager.RegisterCondition("IsActive", GetActive);
        m_FSMMamager.RegisterCondition("CanMove", CanMove);
		m_FSMMamager.RegisterCondition("CountdownWaitingTime", CountdownWaitingTime);
		m_FSMMamager.RegisterCondition("MoveToTarget", MoveToTarget);
		m_FSMMamager.RegisterCondition("MoveToEnemy", MoveToEnemy);
        m_FSMMamager.RegisterCondition("FoundEnemy", FoundEnemy);
		m_FSMMamager.RegisterCondition("IsEnemyDie", IsEnemyDie);
		m_FSMMamager.RegisterCondition("IsDie", IsDie);
		m_FSMMamager.RegisterCondition("IsToFarGroup", IsToFarGroup);
		m_FSMMamager.RegisterCondition("FoundFood", FoundFood);

        m_EnemyLayerMask = 1 << 8;
		m_FoodLayerMask = 1 << 8 | 1 << 10;
		m_FSMMamager.LoadFSM(m_CreatureData.FSMPath);
    }
	
	public virtual void FixedUpdate () {
		base.FixedUpdate ();
		m_FSMMamager.UpdateState();
		StateName = m_FSMMamager.StateCurrentName;
	}

    #endregion

    #region Main method

	private bool IsToFarGroup() {
		return (TransformPosition - m_GroupController.TransformPosition).magnitude 
					> (m_GroupController.GetRadius () * 2f);
	}

	private bool IsEnemyDie() {
		return m_EnemyController.GetHealth () <= 0;
	}

	private bool IsDie() {
		return m_CreatureData.CurrentHP <= 0f;
	}

    private bool CanMove() {
        return m_CanMove;
    }

    private bool MoveToTarget()
	{
		return (TransformPosition - m_TargetPosition).sqrMagnitude < 0.5f; 
	}

//	private bool MoveToObjectTarget()
//	{
//		return (TransformPosition - GetTargetController().TransformPosition).sqrMagnitude < GetTargetController().GetColliderRadius(); 
//	}
	
	private bool MoveToEnemy()
	{
		return (TransformPosition - GetEnemyPosition()).sqrMagnitude < 
			GetEnemyController().GetColliderRadius() + m_CreatureData.AttackRange; 
	}

    private bool CountdownWaitingTime() {
        m_WaitingTimeInterval -= Time.deltaTime;
		return m_WaitingTimeInterval <= 0;   
    }

    private bool FoundEnemy() {
        var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_EnemyLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var target = colliders[i].GetComponent<TDCBaseController>();
			if (target == null || target.GetActive () == false) {
				continue;
			} else {
				if (GetTypeEnemies().IndexOf (target.GetCreatureType()) != -1) {
					SetEnemyController (target);
					return true;
				}
			}
        }
        return false;
    }

	private bool FoundFood() {
		var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_FoodLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var food = colliders[i].GetComponent<TDCBaseController>();
			if (food == null || food.GetActive () == false) {
				continue;
			} else {
				if (GetTypeFoods().IndexOf (food.GetCreatureType()) != -1) {
					SetEnemyController (food);
					return true;
				}
			}
		}
		return false;
	}

	#endregion

	#region Getter & Setter

	public override void SetData (TDCBaseData data)
	{
		base.SetData (data);
		m_CreatureData = data as TDCCreatureData;
	}

	public override TDCBaseData GetData ()
	{
		return m_CreatureData;
	}

	#endregion
   
}
