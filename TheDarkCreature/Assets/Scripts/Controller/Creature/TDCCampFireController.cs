using UnityEngine;
using System.Collections;
using FSM;

public class TDCCampFireController : TDCObjectController {

	#region Property
	private ParticleSystem m_ParticleSystem;
	private Light m_Light;
	
	private float m_CurrentIntensity;
	private float m_CurrentStartSize;
	private bool m_IsFireActive = true;
	private float m_FirePower;

	#endregion
	
	#region Implementation Mono

	public override void Init ()
	{
		base.Init ();
	}

	protected override void Start()
	{
		base.Start();

		m_FirePower = m_Entity.GetDuration();
		m_IsFireActive = true;

		var fireState = new FSMFireState(this);
		var disableState = new FSMDisableState(this);
		
		m_FSMManager.RegisterState("FireState", fireState);
		m_FSMManager.RegisterState("DisableState", disableState);
		
		m_FSMManager.RegisterCondition("IsFireActive", IsFireActive);

		m_FSMManager.LoadFSM (m_Entity.GetFSMPath());

		var fireParticle = this.transform.FindChild ("FireParticle");
		var pointLight = this.transform.FindChild ("PointLight");
		
		m_ParticleSystem = fireParticle.GetComponent<ParticleSystem>();
		m_Light = pointLight.GetComponent<Light> ();
		
		m_CurrentIntensity = m_Light.intensity;
		m_CurrentStartSize = m_ParticleSystem.startSize;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
		StateName = m_FSMManager.StateCurrentName;
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

	public override void ReturnObject(TDCEntity obj)
	{
		base.ReturnObject(obj);
		m_GameManager.SetObjectPool(obj);
	}

	public override void ResetObject()
	{
		base.ResetObject();
		m_FirePower = m_Entity.GetDuration();
		m_Light.intensity = m_CurrentIntensity;
		m_ParticleSystem.startSize = m_CurrentStartSize;
	}

	#endregion

	#region Getter & Setter

	public void SetIntensity(float value) {
		m_Light.intensity = value * (m_CurrentIntensity - 1) + 1;
	}

	public void SetParticleSize(float value) {
		m_ParticleSystem.startSize = value / m_Entity.GetDuration() * (m_CurrentStartSize - 5) + 5;  
	}

	#endregion
}
