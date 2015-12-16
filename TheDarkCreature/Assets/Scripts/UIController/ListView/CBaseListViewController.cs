using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu ("UI/Base ListView")]
public class CBaseListViewController : CBaseList {	

	public override void Init() {
		base.Init();
	}

	protected override int CountItem() {
		return 100;
	}

	protected override CBaseCell CellForIndex(int index) {
		return null;
	} 

	protected override void DidSelectedCell (int index, CBaseCell cell)
	{
		base.DidSelectedCell (index, cell);
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
		base.OnEndDrag();

	}
}

























