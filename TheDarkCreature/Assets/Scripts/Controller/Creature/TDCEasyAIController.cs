using UnityEngine;
using System;
using System.Collections;
using FSM;

public class TDCEasyAIController : TDCCreatureController
{
    #region Properties

    #endregion

    #region Implementation Mono

	public override void Init()
	{
		base.Init();

		m_FSMManager.RegisterCondition("HaveEnemy", IsEnemyDeath);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	protected override void Start()
    {
		base.Start ();
    }
	
	protected override void FixedUpdate () {
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
	}

    #endregion

	#region Main methods

	public override void ActiveSkill(int index)
	{
		base.ActiveSkill(index);
		if (MoveToEnemy())
		{
			m_Entity.ActiveSkill(index);
		}
	}

   	#endregion

    #region FSM

	internal override bool IsToFarGroup() {
		var distance = 0f;
		var range = 0f;
		if (GetGroupEntity() == null)
		{
			distance = (TransformPosition - GetStartPosition()).sqrMagnitude;
			range = 50f;
		}
		else
		{
			distance = (TransformPosition - GetGroupEntity().GetController().TransformPosition).sqrMagnitude;
			range = GetGroupEntity().GetGroupRadius();
		}
		var result = distance > range * range;
		if (result)
		{
			SetEnemyEntity(null);
		}
		return result;
	}

	internal override bool IsToFarStartBattlePosition() {
		var distance = (TransformPosition - m_Entity.GetStartBattlePosition()).sqrMagnitude;
		var range = 50f;
		var result = distance > range * range;
		if (result)
		{
			SetEnemyEntity(null);
		}
		return result;
	}

	internal override bool IsToFarStartPosition() {
		var distance = (TransformPosition - m_Entity.GetStartPosition()).sqrMagnitude;
		var range = 50f;
		var result = distance > range * range;
		if (result)
		{
			SetEnemyEntity(null);
		}
		return result;
	}

	internal override bool IsToFarLeaderPosition()
	{
		var distance = (TransformPosition - m_Entity.GetLeaderPosition()).sqrMagnitude;
		var range = GetDetectRange();
		var result = distance > range * range;
		return result;
	}

	internal override bool HaveLeader()
	{
		return m_Entity.GetLeaderEntity() != null;
	}

	internal override bool HaveEnemyByLeader()
	{
		var leaderEnemy = m_Entity.GetLeaderEntity().GetEnemyEntity();
		var enemy = m_Entity.GetEnemyEntity();
		if (leaderEnemy == null)
		{
			if (enemy != null)
			{
				SetEnemyEntity(enemy);
				return true;
			}
		} 
		else if (GetEnemyEntity() == null && leaderEnemy != this.GetEntity())
		{
			SetEnemyEntity(leaderEnemy);
			return true;
		}
		return false;
	}

	internal override bool IsLeaderDeath()
	{
		var leader = m_Entity.GetLeaderEntity();
		var result = leader == null || leader.GetActive() == false;
		if (result)
		{
			SetLeaderEntity(null);
		}
		return result;
	}

	#endregion
   
}
