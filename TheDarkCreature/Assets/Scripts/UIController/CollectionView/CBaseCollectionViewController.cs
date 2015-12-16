using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu ("UI/Base CollectionView")]
public class CBaseCollectionViewController : CBaseList {

	protected CUIGridViewCustom m_GridView;
	
	public override void Init() {
		base.Init();
		m_GridView = m_ScrollRect.content.GetComponent<CUIGridViewCustom>();
		stepLoad = 10;
	}
	
	protected override int CountItem() {
		return 20;
	}
	
	protected override CBaseCell CellForIndex(int index) {
		return base.CellForIndex (index);
	} 
	
	protected override void DidSelectedCell (int index, CBaseCell cell = null) {
		base.DidSelectedCell(index, cell);
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
	
	protected override IEnumerator LoadNext() {
		loadingListItem = true;
		if (itemLoaded < this.CountItem()) {
			var cell = m_ListViewItems[itemLoaded];
			cell.gameObject.SetActive (true);
			itemLoaded++;
			yield return new WaitForSeconds (0.001f);
			loadingListItem = false;
		}
	}
}
