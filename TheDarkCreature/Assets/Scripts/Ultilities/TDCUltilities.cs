using UnityEngine;
using System.Collections;

public class TDCUltilities {
	public static Sprite LoadImage(string name) {
		var sprites = Resources.LoadAll <Sprite> ("Images");
		for (int i = 0; i < sprites.Length; i++) {
			if (sprites[i].name.Equals (name)) {
				return sprites[i];
			}
		}
		throw new System.Exception (string.Format ("[TDCUltilities] Can not found sprite name {0}", name));
	}
}
