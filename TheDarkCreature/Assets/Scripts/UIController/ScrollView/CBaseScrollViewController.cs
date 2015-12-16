using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu ("UI/Base Scrollview")]
public class CBaseScrollViewController : CBaseList {	
	
	public override void Init() {
		LoadLate = false;
		base.Init();
	}
	
	protected override int CountItem() {
		return 10;
	}

	protected override CBaseCell CellForIndex(int index) {
		return base.CellForIndex (index);
	} 

	protected override void DidSelectedCell (int index, CBaseCell cell = null)
	{
		Debug.LogError (index + " / " + cell.name);
	}
	
	public override void OnBeginDrag() {
		base.OnBeginDrag();
	}
	
	public override void OnDrag(float value) {
		base.OnDrag(value);
	}
	
	public override void OnScroll(float value) {
		base.OnScroll(value);
	}
	
	public override void OnEndDrag() {
//		base.OnEndDrag();
	}
}




