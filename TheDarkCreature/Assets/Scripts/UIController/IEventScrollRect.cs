using UnityEngine;
using System.Collections;

public interface IEventScrollRect {
	void OnBeginDrag ();
	void OnDrag (float value);
	void OnScroll (float value);
	void OnEndDrag ();
}
