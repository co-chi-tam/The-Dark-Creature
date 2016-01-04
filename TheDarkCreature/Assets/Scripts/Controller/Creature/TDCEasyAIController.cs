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

		m_FSMManager.RegisterCondition("HaveEnemy", HaveEnemy);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	protected override void Start()
    {
		base.Start ();

		m_SkillSlot = new TDCSkillSlot(TDCEnum.EGameType.NormalMeleeSkill, this.GetEntity());
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
		var enemyCtrl = GetEnemyEntity();
		return base.HaveEnemy() && enemies.IndexOf (enemyCtrl.GetGameType()) == -1;
	}

	internal override bool IsToFarGroup() {
		var distance = 0f;
		var range = 0f;
		var isFar = false;
		if (GetGroupEntity() == null)
		{
			distance = (TransformPosition - GetStartPosition()).sqrMagnitude;
			range = 50f;
		}
		else
		{
			distance = (TransformPosition - GetGroupEntity().GetController().TransformPosition).sqrMagnitude;
			range = GetGroupEntity().GetRadius();
		}
		return distance > range * range;
	}

	#endregion
   
}
