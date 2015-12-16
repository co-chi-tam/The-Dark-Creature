using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CTestCollectionController : CBaseCollectionViewController {

	private List<string> m_ListString;

	public override void Init ()
	{
		m_ListString = new List<string>();
		for (int i = 0; i < 1000; i++) {
			m_ListString.Add (string.Format ("Name {0}", i));
		}
		base.Init ();
	}

	protected override int CountItem ()
	{
		return m_ListString.Count;
	}

	protected override CBaseCell CellForIndex (int index)
	{
		var go 		= Instantiate (Cell);
		var cell 	= go.GetComponent<CTestCollectionCell>();
		var image 	= cell.BackgroundImage;
		var text 	= cell.Text;
		
		cell.SetupCell(m_ScrollRect.content);
		
		go.name 			= string.Format ("Item {0}", index);
		text.text			= m_ListString[index];
		text.color 			= Color.black;
		
		image.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));

		return cell;
	}

}
