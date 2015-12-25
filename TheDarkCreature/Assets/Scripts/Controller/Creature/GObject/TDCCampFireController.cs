using UnityEngine;
using System.Collections;
using FSM;

public class TDCCampFireController : TDCBaseController {

	#region Property
	private ParticleSystem m_ParticleSystem;
	private Light m_Light;
	
	private FSMManager m_FSMMamager;
	private TDCGObjectData m_GObjectData;
	private float m_CurrentIntensity;
	private float m_CurrentStartSize;
	private bool m_IsFireActive = true;
	private float m_FirePower;

	#endregion
	
	#region Implementation Mono

	public override void Start()
	{
		base.Start();

		m_IsFireActive = true;

		m_FSMMamager = new FSMManager();

		var fireState = new FSMFireState(this);
		var disableState = new FSMDisableState(this);
		
		m_FSMMamager.RegisterState("FireState", fireState);
		m_FSMMamager.RegisterState("DisableState", disableState);
		
		m_FSMMamager.RegisterCondition("IsFireActive", IsFireActive);

		m_GObjectData = GetData () as TDCGObjectData;
		m_FSMMamager.LoadFSM (m_GObjectData.FSMPath);

		var fireParticle = this.transform.FindChild ("FireParticle");
		var pointLight = this.transform.FindChild ("PointLight");
		
		m_ParticleSystem = fireParticle.GetComponent<ParticleSystem>();
		m_Light = pointLight.GetComponent<Light> ();
		
		m_CurrentIntensity = m_Light.intensity;
		m_CurrentStartSize = m_ParticleSystem.startSize;
	}

	public override void Init ()
	{
		base.Init ();
		m_GObjectData = GetData () as TDCGObjectData;
		m_FirePower = m_GObjectData.Duration;
		m_IsFireActive = true;
	}
	
	public override void FixedUpdate() {
		base.FixedUpdate ();
		m_FSMMamager.UpdateState();
		StateName = m_FSMMamager.StateCurrentName;
	}
	
	#endregion
	
	#region Main method
	
	private bool IsFireActive() {
		return m_IsFireActive;
	}
	
	public void UpdateFirePower(float value) {
		m_FirePower -= value;
		SetParticleSize (m_FirePower);
		m_IsFireActive = m_FirePower > 0f;
	}

	public void AddPower(float value) {
		m_FirePower += Mathf.Abs(value);
	}

	public void DestroyGameObject() {
		DestroyImmediate(this.gameObject);
	}

	#endregion

	#region Getter & Setter

	public void SetIntensity(float value) {
		m_Light.intensity = value * (m_CurrentIntensity - 1) + 1;
	}

	public void SetParticleSize(float value) {
		m_ParticleSystem.startSize = value / m_GObjectData.Duration * (m_CurrentStartSize - 5) + 5;  
	}

	#endregion
}
