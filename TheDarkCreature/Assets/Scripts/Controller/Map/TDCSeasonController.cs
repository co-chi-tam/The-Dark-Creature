using UnityEngine;
using System.Collections;

public class TDCSeasonController : TDCBaseController
{
	#region Properties

	#endregion

	#region Implementation Mono

	public override void Init ()
	{
		base.Init ();

		var seasonIdleState = new FSMSeasonIdleState(this);
		var seasonSpringState = new FSMSeasonSpringState(this);
		var seasonSummerState = new FSMSeasonSummerState(this);
		var seasonAutumnState = new FSMSeasonAutumnState(this);
		var seasonWinterState = new FSMSeasonWinterState(this);

		var weatherIdleState  = new FSMWeatherIdleState(this);
		var weatherRainyState = new FSMWeatherRainyState(this);
		var weatherOverHeatState = new FSMWeatherOverHeatState(this);
		var weatherWindyState = new FSMWeatherWindyState(this);
		var weatherSnowyState = new FSMWeatherSnowyState(this);

		m_FSMManager.RegisterState("SeasonIdleState", seasonIdleState);
		m_FSMManager.RegisterState("SeasonSpringState", seasonSpringState);
		m_FSMManager.RegisterState("SeasonSummerState", seasonSummerState);
		m_FSMManager.RegisterState("SeasonAutumnState", seasonAutumnState);
		m_FSMManager.RegisterState("SeasonWinterState", seasonWinterState);

		m_FSMManager.RegisterState("WeatherIdleState", weatherIdleState);
		m_FSMManager.RegisterState("WeatherRainyState", weatherRainyState);
		m_FSMManager.RegisterState("WeatherOverHeatState", weatherOverHeatState);
		m_FSMManager.RegisterState("WeatherWindyState", weatherWindyState);
		m_FSMManager.RegisterState("WeatherSnowyState", weatherSnowyState);

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

	#region Main methods

	public override void ActiveSkill(int index)
	{
		base.ActiveSkill(index);
		m_Entity.ActiveSkill(index);
	}

	public override void IsActiveSeasonSpring() {
		base.IsActiveSeasonSpring();
		var planeController = m_GameManager.GetPlane().GetController() as TDCPlaneController;
		planeController.SetTextureBySeason(TDCEnum.EGameSeason.Spring);
	}

	public override void IsActiveSeasonSummer() {
		base.IsActiveSeasonSummer();
		var planeController = m_GameManager.GetPlane().GetController() as TDCPlaneController;
		planeController.SetTextureBySeason(TDCEnum.EGameSeason.Summer);
	}

	public override void IsActiveSeasonAutumn() {
		base.IsActiveSeasonAutumn();
		var planeController = m_GameManager.GetPlane().GetController() as TDCPlaneController;
		planeController.SetTextureBySeason(TDCEnum.EGameSeason.Autumn);
	}

	public override void IsActiveSeasonWinter() {
		base.IsActiveSeasonWinter();
		var planeController = m_GameManager.GetPlane().GetController() as TDCPlaneController;
		planeController.SetTextureBySeason(TDCEnum.EGameSeason.Winter);
	}

	#endregion

	#region Getter && Setter

	public override float GetDuration()
	{
		return m_Entity.GetDuration();
	}

	#endregion

}
