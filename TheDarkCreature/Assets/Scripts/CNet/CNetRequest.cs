using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CNetRequest : MonoBehaviour {

	#region Singleton

	public static object m_singletonObject = new object();
	public static CNetRequest m_Instance;

	public static CNetRequest Instance {
		get {
			lock (m_singletonObject) {
				if (m_Instance == null) {
					GameObject go = new GameObject("NetRequest");
					m_Instance = go.AddComponent<CNetRequest>();
				}
				return m_Instance;
			}
		}
	}

	public static CNetRequest GetInstance() {
		return Instance;
	}

	public CNetRequest()
	{
		m_Instance = this;
	}

	#endregion

	#region Properties

	private WaitForEndOfFrame m_Waiting = new WaitForEndOfFrame ();

	#endregion

	#region Main methods

	public void Request(string url, Action<CNetResponse> complete, Action<string> error) {
		StartCoroutine(RequestWWW(url, null, null, complete, error));
	}

	public void Request(string url, WWWFormSimple form, Action<CNetResponse> complete, Action<string> error) {
		StartCoroutine(RequestWWW(url, form.Data, form.Header, complete, error));
	}

	public void Request(string url, byte[] data, Dictionary<string, string> headers, Action<CNetResponse> complete, Action<string> error) {
		StartCoroutine(RequestWWW(url, data, headers, complete, error));
	}

	private IEnumerator RequestWWW(string url, byte[] data, Dictionary<string, string> headers, 
		Action<CNetResponse> complete, Action<string> error) {
		var www = new WWW(url, data, headers);
		var responseTime = 10f;
		while (www.isDone && responseTime > 0f)
		{
			responseTime -= Time.deltaTime;
			yield return m_Waiting;
		}
		if (responseTime < 0f)
		{
			error("Request time out");
			yield break;
		}
		yield return www;
		var response = new CNetResponse();
		response.www = www;
		if (www.bytes.Length > 0)
		{
			complete(response);
		}
		else
		{
			error(www.error);
		}
	}

	#endregion

}
