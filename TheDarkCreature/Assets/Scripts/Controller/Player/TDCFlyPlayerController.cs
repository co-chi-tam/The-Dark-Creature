using UnityEngine;
using System.Collections;

public class TDCFlyPlayerController : TDCPlayerController
{
	#region Properties

	[SerializeField]
	private AnimationCurve m_FlyCurve;

	private float m_TimeFlying = 0f;
	private float m_FlyHeight = 30f;

	#endregion

	#region Implementation Mono

	public override void Init()
	{
		base.Init();

		m_FlyCurve = new AnimationCurve();
		m_FlyCurve.AddKey(new Keyframe() { time = 0f, value = 0f });
		m_FlyCurve.AddKey(new Keyframe() { time = 1f, value = 1f });

		m_FSMManager.RegisterCondition("IsFarEnoughFly", IsFarEnoughFly);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	protected override void Start()
	{
		base.Start ();
	}

	#endregion

	#region Main methods

	public override void FlyPosition(Vector3 position)
	{
		m_TimeFlying = (m_TimeFlying + Time.deltaTime) > 1f ? 1f : (m_TimeFlying + Time.deltaTime);
		position.y = 0f;
		var mPos = m_Transform.position;
		mPos.y = 0f;
		var direction = position - mPos;
		var newPosition = mPos + direction.normalized * GetMoveSpeed() * Time.deltaTime;
		newPosition.y = m_FlyCurve.Evaluate(m_TimeFlying) * m_FlyHeight;
		m_Transform.position = newPosition;
		LookAtRotation(position);
	}

	public override void LandingPosition(Vector3 position)
	{
		var mPos = m_Transform.position;
		m_TimeFlying = (m_TimeFlying - Time.deltaTime) < 0f ? 0f : (m_TimeFlying - Time.deltaTime);
		mPos.y = m_FlyCurve.Evaluate(m_TimeFlying) * m_FlyHeight;
		m_Transform.position = mPos;
	}

	#endregion

	#region FSM

	internal virtual bool IsFarEnoughFly() {
		var targetPos = GetTargetPosition();
		var mPos = TransformPosition;
		var distance = GetDetectEnemyRange();
		return (targetPos - mPos).sqrMagnitude > distance * distance;
	}

	#endregion

}

