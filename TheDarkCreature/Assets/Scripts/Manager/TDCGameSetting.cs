using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TDCGameSetting : MonoBehaviour
{
	#region Singleton

	private static object m_SingletonLock = new object();
	private static TDCGameSetting m_Instance = null;

	public static TDCGameSetting Instance {
		get {
			lock (m_SingletonLock) {
				if (m_Instance == null) {
					var m_SingletonObject = new GameObject("GameSetting");
					m_Instance = m_SingletonObject.AddComponent<TDCGameSetting>();
				}
				return m_Instance;
			}
		}
	}

	public static TDCGameSetting GetInstance() {
		return Instance;
	}

	#endregion

	#region Properties

	private static TDCEnum.EGameType m_PlayerType = TDCEnum.EGameType.Dodono;

	#endregion

	#region Implementation MonoBeahviour

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	#region Main methods

	public void LoadMainScene() {
		SceneManager.LoadScene("World1");
	}

	#endregion

	#region Getter && Setter

	public TDCEnum.EGameType GetPlayerType() {
		return m_PlayerType;
	}

	public void SetPlayerType(int player) {
		m_PlayerType = (TDCEnum.EGameType)player;
		LoadMainScene();
	}

	#endregion
}

