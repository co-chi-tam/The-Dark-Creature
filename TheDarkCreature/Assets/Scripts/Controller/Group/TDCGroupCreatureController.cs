using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCGroupCreatureController : TDCBaseGroupController {

    #region Properties
    
	protected List<Vector3> m_ListMemberPosition;
	protected int m_PositionIndex = -1;

    #endregion

    #region Implementation Monobehaviour

	public override void Init()
	{
		base.Init();
		m_ListMemberPosition = new List<Vector3>();
		m_PositionIndex = -1;
		CreatePositionMember();
	}

	protected override void Start()
    {
        base.Start();

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
    }

	protected override void FixedUpdate() {
		base.FixedUpdate ();
        m_FSMManager.UpdateState();

		m_Entity.Update(Time.fixedDeltaTime);
    }

    #endregion

    #region Main method

	protected override void CreatePositionMember() {
		base.CreatePositionMember ();
		var maxMember = 50;
		for (int i = 0; i < maxMember; i++) {
			var randomPosition = UnityEngine.Random.insideUnitCircle * GetGroupRadius();
			var memPosition = new Vector3(randomPosition.x, 0f, randomPosition.y) + TransformPosition;
			m_ListMemberPosition.Add(memPosition);
		}
	}

	public override void ResetObject()
	{
		base.ResetObject();
		m_Entity.ResetObject();
		m_PositionIndex = -1;
		SetCurrentMember(0);
	}

	public override void ReturnMember(TDCEntity member) {
		base.ReturnMember(member);
		member.SetActive(false);
		member.SetGroupEntity (null);
		m_GameManager.SetObjectPool(member);
		var memberCount = GetCurrentMember();
		memberCount -= 1;
		SetCurrentMember(memberCount);
	}

	public override TDCEntity SpawnMember ()
	{
		TDCEntity member = null;
		if (m_GameManager.GetObjectPool (m_Entity.GetMemberType(), ref member))
		{
			var memberPos = this.TransformPosition;
			switch (m_Entity.GetGroupSpawnType())
			{
				case TDCEnum.EGroupSpawnType.Random:
					m_PositionIndex = (m_PositionIndex + 1) % m_ListMemberPosition.Count;
					memberPos = m_ListMemberPosition[m_PositionIndex];
					break;
			}
			member.GetController().ResetObject();
			member.GetController().TransformPosition = memberPos;
			member.SetStartPosition(memberPos);
			member.SetGroupEntity(this.GetEntity());
			member.SetActive(true);
			var memberCount = GetCurrentMember();
			memberCount += 1;
			SetCurrentMember(memberCount);
		}
		return member;
	}

	public override void SpawnAllMember ()
	{
		base.SpawnAllMember ();
		for (int i = 0; i < GetMaxMember (); i++) {
			SpawnMember ();
		}
	}

	#endregion

	#region FSM

	internal override bool IsFullGroup() {
		return GetCurrentMember() == GetMaxMember();
    }

	internal override bool CountdownWaitingTime()
    {
		return base.CountdownWaitingTime();
    }

    #endregion

	#region Getter & Setter

	public override string GetStateName()
	{
		return m_FSMManager.StateCurrentName;
	}

	#endregion

}
