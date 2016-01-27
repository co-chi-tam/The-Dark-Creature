using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Map
{
    public class MapTexture
    {
        int _textureScale;
		float mapScale;
		private GameObject m_Plane;
		private GameObject m_Water;

        public MapTexture(int textureScale)
        {
            _textureScale = textureScale;

			m_Plane = new GameObject("Plane");
			m_Water = new GameObject("Water");
        }

        public void AttachTexture(GameObject plane, Map map)
        {
			int _textureWidth = (int)Map.Width * _textureScale;
			int _textureHeight = (int)Map.Height * _textureScale;

			Texture2D texture = new Texture2D(_textureWidth, _textureHeight, TextureFormat.RGBA32, false);
			texture.wrapMode = TextureWrapMode.Clamp;

			foreach (var c in map.Graph.centers)
			{
				var corner = new Vector2[c.corners.Count];
				for (int i = 0; i < corner.Length; i++)
				{
					var p = c.corners[i];
					corner[i] = new Vector2(p.point.x * _textureScale, p.point.y * _textureScale);
				}
				texture.FillPolygon(corner, BiomeProperties.Colors[c.biome]);
			}

            texture.Apply();

			var render = plane.GetComponent<Renderer>();
			render.material.mainTexture = texture;

			mapScale = render.bounds.size.x;
        }

        private void DrawLine(Texture2D texture, float x0, float y0, float x1, float y1, Color color)
        {
            texture.DrawLine((int)(x0 * _textureScale), (int)(y0 * _textureScale), (int)(x1 * _textureScale), (int)(y1 * _textureScale), color);
        }

		public void GenerateWaterMesh(Map map) {
			foreach (var c in map.Graph.centers)
			{
				if (!c.water)
					continue;
				var corner = new Vector2[c.corners.Count];
				HashSet<Vector2> uv      = new HashSet<Vector2>();
				HashSet<Vector2> uv2     = new HashSet<Vector2>();

				for (int i = 0; i < corner.Length; i++)
				{
					var point = c.corners[i].point; 
					var _X = (point.x - (Map.Width / 2f)) * 32f;
					var _Y = (point.y - (Map.Height / 2f)) * 32f;
					corner[i] = new Vector2(_X, _Y);
				}

				for (int i = 0; i < c.corners.Count; i++)
				{
					var p = c.corners[i];
					uv2.Add(new Vector2(p.moisture, UnityEngine.Random.value));
					uv.Add(p.point);
				}

				GenerateMesh(corner, uv.ToArray(), uv2.ToArray(), m_Water);
			}
		}

		public void GeneratePlaneMesh(Map map) {
			foreach (var c in map.Graph.centers)
			{
				if (c.water)
					continue;
				var corner = new Vector2[c.corners.Count];
				HashSet<Vector2> uv      = new HashSet<Vector2>();
				HashSet<Vector2> uv2     = new HashSet<Vector2>();

				for (int i = 0; i < corner.Length; i++)
				{
					var point = c.corners[i].point; 
					var _X = (point.x - (Map.Width / 2f)) * 32f;
					var _Y = (point.y - (Map.Height / 2f)) * 32f;
					corner[i] = new Vector2(_X, _Y);
				}

				for (int i = 0; i < c.corners.Count; i++)
				{
					var p = c.corners[i];
					uv2.Add(new Vector2(p.moisture, UnityEngine.Random.value));
					uv.Add(p.point);
				}

				GenerateMesh(corner, uv.ToArray(), uv2.ToArray(), m_Plane);
			}
		}

		private void GenerateMesh(Vector2[] vertices2D, Vector2[] uv, Vector2[] uv2, GameObject parent)
		{
			// Use the triangulator to get indices for creating triangles
			Triangulator tr = new Triangulator(vertices2D);
			int[] indices = tr.Triangulate();

			// Create the Vector3 vertices
			Vector3[] vertices = new Vector3[vertices2D.Length];
			for (int i=0; i<vertices.Length; i++) {
				vertices[i] = new Vector3(vertices2D[i].x, 0, vertices2D[i].y);
			}

			// Create the mesh
			Mesh msh = new Mesh();
			msh.vertices = vertices;
			msh.uv = uv;
			msh.uv2 = uv2;
			msh.triangles = indices;
			msh.RecalculateNormals();
			msh.RecalculateBounds();
			msh.Optimize();

			GameObject go = new GameObject("Mesh");
			var filter = go.AddComponent<MeshFilter>();
			var render = go.AddComponent<MeshRenderer>();
			var collider = go.AddComponent<BoxCollider>();

			filter.mesh = msh;
			render.material = new Material(Shader.Find("Diffuse"));
			go.transform.SetParent(parent.transform);
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;

			collider.center = msh.bounds.center;
			collider.size = msh.bounds.size;
		}

    }
}
