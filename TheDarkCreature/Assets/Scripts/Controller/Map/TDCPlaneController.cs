using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TDCPlaneController : TDCBaseController
{
	#region Implementation Mono

	public override void Init ()
	{
		base.Init ();

		var seasonIdleState = new FSMSeasonIdleState(this);
		var seasonSpringState = new FSMSeasonSpringState(this);
		var seasonSummerState = new FSMSeasonSummerState(this);
		var seasonAutumnState = new FSMSeasonAutumnState(this);
		var seasonWinterState = new FSMSeasonWinterState(this);

		m_FSMManager.RegisterState("SeasonIdleState", seasonIdleState);
		m_FSMManager.RegisterState("SeasonSpringState", seasonSpringState);
		m_FSMManager.RegisterState("SeasonSummerState", seasonSummerState);
		m_FSMManager.RegisterState("SeasonAutumnState", seasonAutumnState);
		m_FSMManager.RegisterState("SeasonWinterState", seasonWinterState);

		m_FSMManager.RegisterCondition("IsSpring", IsSpring);
		m_FSMManager.RegisterCondition("IsSummer", IsSummer);
		m_FSMManager.RegisterCondition("IsAutumn", IsAutumn);
		m_FSMManager.RegisterCondition("IsWinter", IsWinter);

		m_FSMManager.LoadFSM (m_Entity.GetFSMPath());
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void FixedUpdate() {
		base.FixedUpdate ();
		m_Entity.Update(Time.fixedDeltaTime);
		m_FSMManager.UpdateState();
	}

	#endregion

	#region FSM

	internal virtual bool IsSpring() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Spring);
	}

	internal virtual bool IsSummer() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Summer);
	}

	internal virtual bool IsAutumn() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Autumn);
	}

	internal virtual bool IsWinter() {
		return TDCDateTime.IsSeason (TDCEnum.EGameSeason.Winter);
	}

	#endregion

	#region Getter && Setter


	#endregion

}

