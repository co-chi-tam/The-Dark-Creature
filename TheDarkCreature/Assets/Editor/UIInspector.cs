using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CUIGridLayout))]
public class GridviewInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		var myTarget = (CUIGridLayout)target;
		if (GUILayout.Button ("FitContent")) {
			myTarget.FitContent ();
		}
		if (GUILayout.Button ("Calculate Content")) {
			myTarget.CalculateContent ();
		}
		if (GUILayout.Button ("Fit cell")) {
			myTarget.FitCellContent ();
		}
	}
}

[CustomEditor(typeof(CUIImage))]
public class CUIImageInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		var myTarget = (CUIImage)target;
		if (GUILayout.Button ("FitContent")) {
			myTarget.FitContent ();
		}
		if (GUILayout.Button ("Calculate Content")) {
			myTarget.CalculateContent ();
		}
	}
}

[CustomEditor(typeof(UIInventory))]
public class TDCInventoryInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		var myTarget = (UIInventory)target;
		if (GUILayout.Button ("Load All Slot")) {
			myTarget.LoadAllSlots ();
		}
	}
}






