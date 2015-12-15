using UnityEngine;
using System.Collections;
using FSM;

public enum EAnimation:int {
	Idle    = 0,
	Walk    = 1,
	Run     = 2,
	Action1 = 3,
	Action2 = 4,
	Attack1 = 5,
	Attack2 = 6,
    Hiden   = 7,
	// TODO
	Die     = 10
}

public class TDCBaseController : TDCMonoBehaviour
{
	#region Property
	[SerializeField]
	protected string StateName = "";

	protected Transform m_Transform;
	protected Vector3 m_StartPosition;
    protected bool m_IsActive = true;
	protected float m_WaitingTimeInterval = 3f;
	protected int m_DamageTake = 0;
	protected TDCEnum.EGameType m_CreatureType;
	protected TDCBaseGroupController m_GroupController;
	protected SphereCollider m_Collider;
	protected TDCBaseData m_BaseData;

    public Vector3 TransformPosition {
		get { return m_Transform.position; }
	}

	public Quaternion TransformRotation {
		get { return m_Transform.rotation; }
	}

	public TDCEnum.EGameType CreatureType {
		get { return m_CreatureType; }
		set { m_CreatureType = value; }
	}

	#endregion

	#region Implement Mono

	public virtual void Init() {
		m_Transform	= this.transform;
		m_StartPosition = m_Transform.position;
		SetActive (true);
	}

	public virtual void Start()
	{
		m_Collider = this.GetComponent<SphereCollider> ();
    }

	public virtual void Update() {

	}

	public virtual void FixedUpdate() {
		
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (TransformPosition, GetDetectEnemyRange());
	}

	#endregion

	#region Main Method

	public virtual void WalkPosition(Vector3 position) {

	}
	
	public virtual void RunPosition(Vector3 position) {

	}
	
	public virtual void MoveRotation(Vector3 rotation) {

	}

	public virtual void ApplyDamage(int damage, TDCBaseController attacker) {
		m_DamageTake += damage;
	}

	public virtual void ResetObject() {
	
	}

    #endregion

    #region Getter & Setter

	public virtual float GetColliderRadius() {
		if (m_Collider == null)
			return 0;
		return m_Collider.radius;
	}

	public virtual void SetData(TDCBaseData data) {
		m_BaseData = data;
	}
	
	public virtual void SetIsActive(bool value) {
		m_IsActive = value;
	}

	public virtual void SetActive(bool value) {
		m_IsActive = value;
        gameObject.SetActive(value);
    }

    public virtual bool GetActive() {
		return m_IsActive;
    }

	public virtual void SetStartPosition(Vector3 pos) {
		m_StartPosition = pos;
		this.transform.position = pos;
	}

	public virtual Vector3 GetStartPosition() {
		return m_StartPosition;
	}

	public virtual TDCEnum.EGameType GetCreatureType() {
		return m_CreatureType;
	}

	public virtual void SetCreatureType(TDCEnum.EGameType type) {
		m_CreatureType = type;
	}
	
	public virtual void SetWaitingTimeInterval(float time) {
		m_WaitingTimeInterval = time;
	}
	
	public virtual TDCBaseData GetData() {
		return m_BaseData;
	}
	
	public virtual void SetCanMove(bool value) {

	}
	
	public virtual bool GetCanMove() {
		return false;
	}
	
	public virtual Vector3 GetEnemyPosition() {
		return TransformPosition;
	}
	
	public virtual void SetEnemyController(TDCBaseController controller)
	{

	}
	
	public virtual TDCBaseController GetEnemyController()
	{
		return null;
	}
	
	public virtual void SetAnimation(EAnimation anim) {

	}
	
	public virtual Vector3 GetTargetPosition() {
		return TransformPosition;
	}
	
	public virtual void SetTargetPosition(Vector3 pos) {

	}
	
	public virtual float GetDetectEnemyRange() {
		return 0f;
	}

	public virtual void SetGroupController(TDCBaseGroupController group) {
		m_GroupController = group;
	}

	public virtual TDCBaseGroupController GetGroupController() {
		return m_GroupController;
	}

	public virtual int GetHealth() {
		return 0;
	}

	public virtual void SetHealth(int value) {

	}

	public virtual int GetMaxHealth() {
		return 0;
	}

	public virtual void SetAttacker(TDCBaseController attacker) {

	}
	
	public virtual TDCBaseController GetAttacker() {
		return null;
	}

	#endregion
}
