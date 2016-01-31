using UnityEngine;
using System.Collections;
using FSM;

public class FSMExcuteEffectUpdateSkillState : FSMBaseState {

	private TDCSkillController m_SkillController;

	public FSMExcuteEffectUpdateSkillState(TDCBaseController controller) : base (controller)
	{
		m_SkillController = controller as TDCSkillController;
	}

	public override void StartState() {
		m_SkillController.ExcuteEffect();
	}

	public override void UpdateState() {
		m_SkillController.ExcuteEffect();
	}

	public override void ExitState()
	{

	}
}
