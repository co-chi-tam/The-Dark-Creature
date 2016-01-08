﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using FSM;

public class TDCGroupCreatureController : TDCBaseGroupController {

    #region Properties
    
	private List<Vector3> m_ListMemberPosition;
	private int m_PositionIndex = -1;

    #endregion

    #region Implementation Mono

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
		StateName = m_FSMManager.StateCurrentName;

		m_Entity.Update(Time.fixedDeltaTime);
    }

    #endregion

    #region Main method

	protected override void CreatePositionMember() {
		base.CreatePositionMember ();
		for (int i = 0; i < GetMaxMember(); i++) {
			var randomPosition = UnityEngine.Random.insideUnitCircle * GetGroupRadius();
			var memPosition = new Vector3(randomPosition.x, 0f, randomPosition.y);
			m_ListMemberPosition.Add(memPosition);
		}
	}

	public override void ResetObject()
	{
		base.ResetObject();
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
			m_PositionIndex = (m_PositionIndex + 1) % m_ListMemberPosition.Count;
			member.GetController().TransformPosition = m_ListMemberPosition[m_PositionIndex];
			member.SetGroupEntity(this.GetEntity());
			member.GetController().ResetObject();
			member.SetActive(true);
			var memberCount = GetCurrentMember();
			memberCount += 1;
			SetCurrentMember(memberCount);
		}
		return member;
	}

	public override void SpawnMinMember ()
	{
		base.SpawnMinMember ();
		for (int i = 0; i < GetMinMember (); i++) {
			SpawnMember ();
		}
	}

	public override void SpawnMaxMember ()
	{
		base.SpawnMaxMember ();
		for (int i = 0; i < GetMaxMember (); i++) {
			SpawnMember ();
		}
	}

	#endregion

	#region FSM

	internal override bool IsFullMinGroup() {
		return GetCurrentMember() == GetMinMember();
    }

	internal override bool IsFullMaxGroup() {
		return GetCurrentMember() == GetMaxMember();
	}

	internal override bool CountdownWaitingTime()
    {
		return base.CountdownWaitingTime();
    }

    #endregion

}
