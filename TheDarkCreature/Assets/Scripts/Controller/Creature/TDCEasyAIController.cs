using UnityEngine;
using System;
using System.Collections;
using FSM;

public class TDCEasyAIController : TDCCreatureController
{
    #region Properties
    
	private LayerMask m_ColliderLayerMask;

    #endregion

    #region Implementation Mono

    public override void Start()
    {
		base.Start ();

		m_FSMManager.RegisterCondition("HaveEnemy", HaveEnemy);

		m_ColliderLayerMask = 1 << 8 | 1 << 10 | 1 << 31;
		m_FSMManager.LoadFSM(m_CreatureData.FSMPath);
    }
	
	public override void FixedUpdate () {
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
		StateName = m_FSMManager.StateCurrentName;
	}

    #endregion

    #region FSM

	internal override bool HaveEnemy() {
		var enemies = GetTypeEnemies();
		var enemyCtrl = GetEnemyController();
		return enemyCtrl != null && enemies.IndexOf (enemyCtrl.GetGameType()) == -1;
	}

	internal override bool IsToFarGroup() {
		var distance = 0f;
		var range = 0f;
		var isFar = false;
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
		isFar = distance > range * range;
		if (isFar)
		{
			SetEnemyController(null);
		}
		return isFar;
	}

	internal override bool IsEnemyDie() {
		var enemy = m_EnemyController.GetHealth() <= 0 || m_EnemyController.GetActive() == false;
		if (enemy)
		{
			SetEnemyController(null);
		}
		return enemy;
	}

	internal override bool IsDeath() {
		return base.IsDeath();
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
		var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_ColliderLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var target = m_GameManager.GetControllerByName (colliders[i].name);
			if (target == null || target.GetActive () == false || target == this) {
				continue;
			} else {
				if (GetTypeEnemies().IndexOf (target.GetGameType()) != -1) {
					SetEnemyController(target);
					return true;
				}
			}
        }
        return false;
    }

	internal override bool FoundFood() {
		if (GetEnemyController() != null)
		{
			return true;
		}
		var colliders = Physics.OverlapSphere(TransformPosition, GetDetectEnemyRange(), m_ColliderLayerMask);
		for (int i = 0; i < colliders.Length; i++) {
			var food = m_GameManager.GetControllerByName (colliders[i].name);
			if (food == null || food.GetActive () == false || food == this) {
				continue;
			} else {
				if (GetTypeFoods().IndexOf (food.GetGameType()) != -1) {
					if (GetEnemyController() == null)
					{
						SetEnemyController(food);
					}
					return true;
				}
			}
		}
		return false;
	}

	#endregion
   
}
