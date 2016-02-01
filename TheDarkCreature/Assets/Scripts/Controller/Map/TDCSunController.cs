using UnityEngine;
using System.Collections;

public class TDCSunController : TDCBaseController
{

	#region Properties

	[SerializeField]
	private Gradient m_SunColorGradient;
	private Light m_SunLight;
	private float m_SunLightIntensity = 0f;

	#endregion

	#region Implementation Mono

	public override void Init ()
	{
		base.Init ();

		var color1 = new Color(255f / 255f, 255f / 255f, 234f / 255f, 255f / 255f);
		var color2 = new Color(189f / 255f, 223f / 255f, 255f / 255f, 255f / 255f);
		m_SunColorGradient = new Gradient();
		m_SunColorGradient.colorKeys = new GradientColorKey[] { new GradientColorKey (color2, 0f), new GradientColorKey (color1, 1f) };

		m_SunLight = this.GetComponent<Light>();
		m_SunLightIntensity = m_SunLight != null ? m_SunLight.intensity : 0f;

		var sunIdleState = new FSMSunIdleState(this);
		var sunUpdateState = new FSMSunUpdateState(this);

		m_FSMManager.RegisterState ("SunIdleState", sunIdleState);
		m_FSMManager.RegisterState ("SunUpdateState", sunUpdateState);

		m_FSMManager.LoadFSM (m_Entity.GetFSMPath());
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void FixedUpdate() {
		base.FixedUpdate ();
		m_Entity.Update(Time.deltaTime);
		m_FSMManager.UpdateState();
	}

	#endregion

	#region Main methods

	public void UpdateLight() {
		var value = TDCDateTime.DeltaHour;
		var lightOffset = m_Entity.GetIntensityOffset();
		var calcLight = (value * (m_SunLightIntensity - lightOffset)) + lightOffset;
		m_SunLight.intensity = calcLight;
		m_SunLight.color = m_SunColorGradient.Evaluate (calcLight / m_SunLightIntensity);
	}

	#endregion

	#region Getter && Setter

	#endregion

}

