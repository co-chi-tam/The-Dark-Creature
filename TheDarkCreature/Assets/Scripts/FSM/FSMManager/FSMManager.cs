using System;
using System.Collections.Generic;
using System.Linq;

namespace FSM
{
    public enum EState
    {
        StartState = 0,
        UpdateState = 1,
        EndState = 2
    }

    public class FSMManager
    {
		public string StateCurrentName;
        private Dictionary<string, Func<bool>> m_Conditions;
        private Dictionary<string, IState> m_States;
        private FSMReader m_FSMLoader;
        private FSMStateData m_Map;
        private FSMStateData m_AnyState;
        private FSMStateData m_LastState;
        private EState m_CurrentState;

        public FSMManager()
        {
            m_Conditions = new Dictionary<string, Func<bool>>();
            m_States = new Dictionary<string, IState>();
            m_FSMLoader = new FSMReader();
        }

        public void LoadFSM(string path) {
            m_FSMLoader.LoadJSON(path);
            m_Map = m_FSMLoader.FSMCurrentState;
            m_AnyState = m_FSMLoader.FSMAnyState;
            m_Conditions["IsRoot"] = IsRoot;
            m_Conditions["IsAnyState"] = IsAnyState;
        }

        private bool IsRoot()
        {
            return true;
        }

        private bool IsAnyState() {
            return true;
        }

        public void UpdateState()
        {
            for (int i = 0; i < m_AnyState.ListStates.Count; i++)
            {
                var stateNext = m_AnyState.ListStates[i];
                if (stateNext.StateName.Equals(m_Map.StateName))
                {
                    break;
                }
                if (m_Conditions[stateNext.ConditionName]() == stateNext.ConditionValue)
                {
                    m_Map = m_AnyState.ListStates[i];
                    m_CurrentState = EState.StartState;
                    break;
                }
            }

            var stateNow = m_States[m_Map.StateName];
			StateCurrentName = m_Map.StateName;
            switch (m_CurrentState)
            {
	            case EState.StartState:
	                stateNow.StartState();
	                m_CurrentState = EState.UpdateState;
	                break;
	            case EState.UpdateState:
	                stateNow.UpdateState();
	                for (int i = 0; i < m_Map.ListStates.Count; i++)
	                {
						var stateNext = m_Map.ListStates[i];
						if (m_Conditions[stateNext.ConditionName]() == stateNext.ConditionValue)
	                    {
	                        m_CurrentState = EState.EndState;
	                        m_LastState = m_Map.ListStates[i];
	                        break;
	                    }
	                }
	                break;
	            case EState.EndState:
	                stateNow.ExitState();
	                m_Map = m_LastState;
	                m_CurrentState = EState.StartState;
	                break;
            }
        }

        public void RegisterState(string name, IState state)
        {
            if (!m_States.ContainsKey(name))
            {
                m_States[name] = state;
            }
        }

        public void RegisterCondition(string name, Func<bool> condition)
        {
            if (!m_Conditions.ContainsKey(name))
            {
                m_Conditions[name] = condition;
            }
        }

		public void SetState(string name) {
			m_Map = m_FSMLoader.FSMMaps [name];
			m_CurrentState = EState.StartState;
		}

		public void SetState(int index) {
			m_Map = m_FSMLoader.FSMMaps.ElementAt (index).Value;
			m_CurrentState = EState.StartState;
		}
    }
}
