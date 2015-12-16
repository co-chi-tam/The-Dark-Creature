using UnityEngine;
using System.Collections;

public interface IUserInterface {
	void FitContent();
	void CalculateContent();
	void MoveToFirst();

	float GetPercentX();
	float GetPercentY();
	float GetPercentWidth();
	float GetPercentHeight();

}
