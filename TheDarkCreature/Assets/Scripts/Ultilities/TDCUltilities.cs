using UnityEngine;
using System.Collections;
using System.Text;

public static class TDCUltilities {
	
	public static Sprite LoadImage(string name) {
		var sprites = Resources.LoadAll <Sprite> ("Images");
		for (int i = 0; i < sprites.Length; i++) {
			if (sprites[i].name.Equals (name)) {
				return sprites[i];
			}
		}
		throw new System.Exception (string.Format ("[TDCUltilities] Can not found sprite name {0}", name));
	}

	public static byte[] GetBytes(this string value) {
		return Encoding.ASCII.GetBytes(value);
	}
}
