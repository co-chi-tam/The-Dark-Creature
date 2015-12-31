using UnityEngine;
using System;
using System.Collections;
using FSM;

public class TDCEasyAIController : TDCCreatureController
{
    #region Properties

    #endregion

    #region Implementation Mono

	protected override void Start()
    {
		base.Start ();

		m_FSMManager.RegisterCondition("HaveEnemy", HaveEnemy);
	
		m_FSMManager.LoadFSM(m_CreatureData.FSMPath);
    }
	
	protected override void FixedUpdate () {
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
		return base.IsEnemyDie();
	}

	internal override bool IsDeath() {
		return base.IsDeath();
	}

	internal override bool CanMove() {
        return m_CanMove;
    }

	internal override bool MoveToTarget()
	{
		return base.MoveToTarget(); 
	}

	internal override bool MoveToEnemy()
	{
		return base.MoveToEnemy();
	}

	internal override bool CountdownWaitingTime() {
		return base.CountdownWaitingTime();
    }

	internal override bool FoundEnemy() {
		return base.FoundEnemy();
    }

	internal override bool FoundFood() {
		return base.FoundFood();
	}

	#endregion
   
}
