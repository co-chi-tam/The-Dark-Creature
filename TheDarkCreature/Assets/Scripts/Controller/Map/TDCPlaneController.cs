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

	#region Getter && Setter

	public void SetTextureBySeason(TDCEnum.EGameSeason season) {
		if (m_Renderer != null)
		{
			m_Renderer.material.mainTexture = m_SeasonTextures[(int)season];
		}
	}


	#endregion

}

