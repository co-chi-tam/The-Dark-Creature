using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TDCPlaneController : TDCBaseController
{
	#region Properties

	private Texture2D[] m_SeasonTextures;
	private Renderer m_Renderer;

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

		m_Renderer = this.GetComponent<Renderer>();

		m_SeasonTextures = new Texture2D[4];
		m_SeasonTextures[(int)TDCEnum.EGameSeason.Spring] = TDCUltilities.LoadImage<Texture2D>(m_Entity.GetSeasonSpringTexture(), "Images/Map");
		m_SeasonTextures[(int)TDCEnum.EGameSeason.Summer] = TDCUltilities.LoadImage<Texture2D>(m_Entity.GetSeasonSummerTexture(), "Images/Map");
		m_SeasonTextures[(int)TDCEnum.EGameSeason.Autumn] = TDCUltilities.LoadImage<Texture2D>(m_Entity.GetSeasonAutumnTexture(), "Images/Map");
		m_SeasonTextures[(int)TDCEnum.EGameSeason.Winter] = TDCUltilities.LoadImage<Texture2D>(m_Entity.GetSeasonWinterTexture(), "Images/Map");

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

	public void SetTextureBySeason(TDCEnum.EGameSeason season) {
		if (m_Renderer != null)
		{
			m_Renderer.material.mainTexture = m_SeasonTextures[(int)season];
		}
	}


	#endregion

}

