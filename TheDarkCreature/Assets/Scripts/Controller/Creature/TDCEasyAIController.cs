using UnityEngine;
using System;
using System.Collections;
using FSM;

public class TDCEasyAIController : TDCCreatureController
{
    #region Properties
    
	private LayerMask m_EnemyLayerMask;
	private LayerMask m_FoodLayerMask;

    #endregion

    #region Implementation Mono

    public override void Start()
    {
		base.Start ();

		m_FSMMamager.RegisterCondition("HaveEnemy", HaveEnemy);

		m_EnemyLayerMask = 1 << 8;
		m_FoodLayerMask = 1 << 8 | 1 << 10;
		m_FSMMamager.LoadFSM(m_CreatureData.FSMPath);
    }
	
	public override void FixedUpdate () {
		base.FixedUpdate ();
		m_FSMMamager.UpdateState();
		StateName = m_FSMMamager.StateCurrentName;
	}

    #endregion

    #region FSM

	internal override bool HaveEnemy() {
		var enemies = GetTypeEnemies();
		var enemyCtrl = GetEnemyController();
		return GetEnemyController() != null && enemies.IndexOf (enemyCtrl.GetGameType()) == -1;
	}

	internal override bool IsToFarGroup() {
		var distance = 0f;
		var range = 0f;
		if (m_GroupController == null)
		{
			distance = (TransformPosition - m_StartPosition).sqrMagnitude;
			range = 50f;
		}
		else
		{
			distance = (TransformPosition - m_GroupController.TransformPosition).sqrMagnitude;
			range = m_GroupController.GetRadius();
		}
		return distance > range * range;
	}

	internal override bool IsEnemyDie() {
		return m_EnemyController.GetHealth () <= 0;
	}

	internal override bool IsDie() {
		return m_CreatureData.CurrentHP <= 0f;
	}

	internal override bool CanMove() {
        return m_CanMove;
    }

	internal override bool MoveToTarget()
	{
		return (TransformPosition - m_TargetPosition).sqrMagnitude < 0.5f; 
	}

	internal override bool MoveToEnemy()
	{
		var distance = (TransformPosition - GetEnemyPosition()).sqrMagnitude;
		var range = GetEnemyController().GetColliderRadius() + m_CreatureData.AttackRange;
		return distance < range * range; 
	}

	internal override bool CountdownWaitingTime() {
        m_WaitingTimeInterval -= Time.deltaTime;
		return m_WaitingTimeInterval <= 0;   
    }

	internal override bool FoundEnemy() {
        var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_EnemyLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var target = colliders[i].GetComponent<TDCBaseController>();
			if (target == null || target.GetActive () == false) {
				continue;
			} else {
				if (GetTypeEnemies().IndexOf (target.GetGameType()) != -1) {
					SetEnemyController (target);
					return true;
				}
			}
        }
        return false;
    }

	internal override bool FoundFood() {
		var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_FoodLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var food = colliders[i].GetComponent<TDCBaseController>();
			if (food == null || food.GetActive () == false) {
				continue;
			} else {
				if (GetTypeFoods().IndexOf (food.GetGameType()) != -1) {
					SetEnemyController (food);
					return true;
				}
			}
		}
		return false;
	}

	#endregion
   
}
