using UnityEngine;
using System;
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

	public static T LoadImage<T>(string name, string path) where T : UnityEngine.Object {
		var sprites = Resources.LoadAll <T> (path);
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

	public static Vector2 RandomInsideCircle(float radius) {
		var randomRadius = UnityEngine.Random.Range(0f, radius);
		var randomDegree = UnityEngine.Random.Range(0f, 360f);
		var angleRadian = randomDegree * Mathf.PI / 180f; // Deg2Rad
		Vector2 returnVetor = Vector2.zero;
		returnVetor.x = randomRadius * Mathf.Sin(angleRadian);
		returnVetor.y = randomRadius * Mathf.Cos(angleRadian);
		return returnVetor;
	}

	public static Vector2 RandomAroundCircle(float radius) {
		var randomDegree = UnityEngine.Random.Range(0f, 360f);
		var angleRadian = randomDegree * Mathf.PI / 180f; // Deg2Rad
		Vector2 returnVetor = Vector2.zero;
		returnVetor.x = radius * Mathf.Sin(angleRadian);
		returnVetor.y = radius * Mathf.Cos(angleRadian);
		return returnVetor;
	}

	public static Vector3 V3Forward(this Transform trans, float length = 1f) {
		return trans.position + trans.rotation * Vector3.forward * length;
	}

	public static bool IsPlayer(TDCEntity entity) {
		return entity.GetCreatureType() == TDCEnum.ECreatureType.GroundPlayer ||
			entity.GetCreatureType() == TDCEnum.ECreatureType.FlyPlayer;
	}

	public static bool IsCreature(TDCEntity entity) {
		return entity.GetCreatureType() == TDCEnum.ECreatureType.GroundCreature ||
			entity.GetCreatureType() == TDCEnum.ECreatureType.FlyCreature || 
			IsPlayer(entity);
	}

	public static Vector3 GetPositionByRadius(Vector3 center, Vector3 position, float radius, float radius2 = 0f) {
		var direction = (center - position).normalized;
		var point = center;
		point.x -= direction.x * (radius + radius2);
		point.y -= direction.y * (radius + radius2);
		point.z -= direction.z * (radius + radius2);
		return point;
	}

}
