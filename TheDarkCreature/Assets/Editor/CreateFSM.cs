using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UIState {
    public string StateName = string.Empty;
    public Rect StateRect = new Rect(100, 100, 150, 100);
    public bool IsRoot = false;
    public CreateFSM Window;

//    private float m_RectWidth = 100;
    private Rect m_NameStateLabel           = new Rect(5, 30, 40, 30);
    private Rect m_NameStateText            = new Rect(45, 30, 100, 20);
    private Rect m_IsRootCheckBox           = new Rect(45, 50, 100, 20);
    private Rect m_CreateConditionButton    = new Rect(0, 80, 150, 20);
    private Rect m_CloseButton              = new Rect(130, 0, 20, 20);
            
    public void DrawState(int id) {
        if (GUI.Button (m_CloseButton, "X"))
        {
            Window.DeleteState(this);
        }
        GUI.Label(m_NameStateLabel, "Name: ");
        StateName   = GUI.TextField(m_NameStateText, StateName);
        IsRoot      = GUI.Toggle(m_IsRootCheckBox, IsRoot, "Is Root: ");

        if (GUI.Button(m_CreateConditionButton, "+")) {
            StateRect.height += 20;
        }












        GUI.DragWindow();
    }
}

public class CreateFSM : EditorWindow
{
    [MenuItem("FSM/WindowEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateFSM));
    }

    private List<UIState> m_UIStates = new List<UIState>();

    void OnGUI() {
        if (GUI.Button(new Rect(0, 0, 100, 20), "Create State"))
        {
            UIState newState = new UIState();
            if (m_UIStates.Count == 0) { newState.IsRoot = true; }
            newState.StateName = string.Format ("State {0}", m_UIStates.Count);
            newState.Window = this;
            m_UIStates.Add(newState);
        }

        BeginWindows();
        for (int i = 0; i < m_UIStates.Count; i++)
        {
            m_UIStates[i].StateRect = GUI.Window(i, m_UIStates[i].StateRect, 
                                                    m_UIStates[i].DrawState, 
                                                    m_UIStates[i].StateName);
        }
        EndWindows();
    }

    public void DeleteState(UIState state) {
        m_UIStates.Remove(state);
    } 

    /*
    public static void DrawConnection (Vector2 from, Vector2 to)
	// Render a node connection between the two given points
	{
		bool left = from.x > to.x;

		Handles.DrawBezier(
			new Vector3 (from.x + (left ? -kNodeSize : kNodeSize) * 0.5f, from.y, 0.0f),
			new Vector3 (to.x + (left ? kNodeSize : -kNodeSize) * 0.5f, to.y, 0.0f),
			new Vector3 (from.x, from.y, 0.0f) + Vector3.right * 50.0f * (left ? -1.0f : 1.0f),
			new Vector3 (to.x, to.y, 0.0f) + Vector3.right * 50.0f * (left ? 1.0f : -1.0f),
			GUI.color,
			null,
			2.0f
		);
	}
    */
}
