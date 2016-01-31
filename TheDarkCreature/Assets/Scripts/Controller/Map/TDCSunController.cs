using UnityEngine;
using System.Collections;

public class TDCSunController : TDCBaseController
{

	#region Properties

	private Light m_SunLight;
	private float m_SunLightIntensity = 0f;

	#endregion

	#region Implementation Mono

	public override void Init ()
	{
		base.Init ();

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
		m_SunLight.intensity = Mathf.Clamp ((value * (m_SunLightIntensity - lightOffset)) + lightOffset, 0f, m_SunLightIntensity);
	}

	#endregion

	#region Getter && Setter

	#endregion

}

