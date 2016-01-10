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

[CustomEditor(typeof(uObjectCurrentValue))]
public class ObjectCurrentValueInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		var myTarget = (uObjectCurrentValue)target;
		var ctrl = myTarget.GetComponent<TDCBaseController>();
		if (ctrl != null)
		{
			var objectValues = ctrl.GetObjectCurrentValue();
			foreach (var item in objectValues)
			{
				EditorGUILayout.LabelField(item.Key.ToString(), item.Value == null ? "None" : item.Value.ToString());
			}
		}
	}
}






