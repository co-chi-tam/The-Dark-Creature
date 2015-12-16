using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CNetResponse {

	public WWW www;
	public Dictionary<string, string> Header;

	public Texture2D Texture {
		get { return www.texture; }
	}

	public Sprite Sprite {
		get { return Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), Vector2.zero, 1024); }
	}

	public string Text {
		get { return www.text; }
	}

	public byte[] Bytes {
		get { return www.bytes; }
	}

	public string Error {
		get { return www.error; }
	}

	public Dictionary<string, object> GetDictionary() {
		try {
			var jsonData = MiniJSON.Json.Deserialize (www.text) as Dictionary<string, object>;
			return jsonData;
		} catch (System.Exception ex) {
			Debug.LogError (ex.Message);
		}
		return null;
	}
}

public class WWWFormSimple {
	public string  URL;
	public Dictionary<string, string> Header;
	public byte[] Data {
		get { return m_Form.data; }
	}

	private WWWForm m_Form;

	public WWWFormSimple ()
	{
		Header = new Dictionary<string, string>();
		m_Form = new WWWForm ();
	}

	public void AddHeader(string name, string value) {
		if (!Header.ContainsKey(name)) {
			Header[name] = value;
		}
	}

	public void SetHeader(string name, string value) {
		if (Header.ContainsKey(name)) {
			Header[name] = value;
		} else {
			AddHeader (name, value);
		}
	}

	public void AddField(string name, string value) {
		m_Form.AddField (name, value);
	}

	public void AddField(string name, int value) {
		m_Form.AddField (name, value);
	}
}












