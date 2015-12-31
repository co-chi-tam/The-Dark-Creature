using UnityEngine;
using FSM;

public class FSMStartSkillState : FSMBaseState {

	private TDCSkillController m_SkillController;
	private TDCSkillData m_SkillData;

	public FSMStartSkillState(TDCBaseController controller) : base (controller)
	{
		m_SkillController = controller as TDCSkillController;
		m_SkillData = controller.GetData() as TDCSkillData;
	}

	public override void StartState() {
		m_SkillController.SetTargetPosition(m_SkillData.Owner.GetTargetPosition());
		m_SkillController.SetEnemyController(m_SkillData.Owner.GetEnemyController());
	}

	public override void UpdateState() {

	}

	public override void ExitState()
	{

	}
}