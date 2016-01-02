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

		m_SkillSlot = new TDCSkillSlot(TDCEnum.EGameType.NormalMeleeSkill, this);
    }
	
	protected override void FixedUpdate () {
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
		StateName = m_FSMManager.StateCurrentName;
	}

	protected override void Update()
	{
		base.Update();
		m_SkillSlot.UpdateSkill(Time.deltaTime);
	}

    #endregion

	#region Main methods

	public override void ActiveSkill(int index)
	{
		base.ActiveSkill(index);
		m_SkillSlot.ActiveSkill();
	}

	#endregion

    #region FSM

	internal override bool HaveEnemy() {
		var enemies = GetTypeEnemies();
		var enemyCtrl = GetEnemyController();
		return base.HaveEnemy() && enemies.IndexOf (enemyCtrl.GetGameType()) == -1;
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
		return distance > range * range;
	}

	#endregion
   
}
