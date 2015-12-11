using UnityEngine;
using System.Collections;
using ObjectPool;

public class TDCGameManager : MonoBehaviour {

    #region Singleton

    public static object m_singletonObject = new object();
    public static TDCGameManager m_Instance;

    public static TDCGameManager Instance {
        get {
            lock (m_singletonObject) {
                if (m_Instance == null) {
                    GameObject go = new GameObject("GameManager");
                    m_Instance = go.AddComponent<TDCGameManager>();
                }
                return m_Instance;
            }
        }
    }

    public static TDCGameManager GetInstance() {
        return Instance;
    }

    public TDCGameManager()
    {
        m_Instance = this;
    }

    #endregion

    #region Properties

    private TDCObjectPool<TDCTrapController> m_TrapsPool;
	private TDCDataLoader m_DataLoader;

    #endregion

    #region Implementation Mono

    void Awake() {
		m_TrapsPool = new TDCObjectPool<TDCTrapController>();
		m_DataLoader = new TDCDataLoader();
    }

	void Start() {
		CreateCreature (TDCEnum.EGameType.PlayerSatla, Vector3.zero, Quaternion.identity);

		CreateGroup (TDCEnum.EGameType.GroupDodono, Vector3.zero, Quaternion.identity);
		CreateGroup (TDCEnum.EGameType.GroupSatla, new Vector3 (10f, 0f, 20f), Quaternion.identity);

		CreateGroup (TDCEnum.EGameType.GroupGrass, new Vector3 (20f, 0f, 20f), Quaternion.identity);
		CreateGroup (TDCEnum.EGameType.GroupMushRoom, new Vector3 (10f, 0f, 20f), Quaternion.identity);
	}

    #endregion

    #region Main method

	public TDCGroupCreatureController CreateGroup(TDCEnum.EGameType type, 
	                                               Vector3 position, 
	                                               Quaternion rotation, 
	                                               GameObject parent = null) {
		var random = Random.Range (0, 9999);
		TDCBaseData data = m_DataLoader.GetGroup (type);
		GameObject gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath [random % data.ModelPath.Length]), position, rotation) as GameObject;
		TDCGroupCreatureController controller = gObject.AddComponent<TDCGroupCreatureController> ();
		var groupController = controller as TDCGroupCreatureController;

		controller.SetData (data);
		controller.Init ();

		switch (type) {
		case TDCEnum.EGameType.GroupDodono:
			groupController.CreatePoolMember (TDCEnum.EGameType.Dodono);
			break;
		case TDCEnum.EGameType.GroupSatla:
			groupController.CreatePoolMember (TDCEnum.EGameType.Satla);
			break;
		case TDCEnum.EGameType.GroupBob: 
			groupController.CreatePoolMember (TDCEnum.EGameType.Bob);
			break;
		case TDCEnum.EGameType.GroupGrass:
			groupController.CreatePoolMember (TDCEnum.EGameType.EnviromentGrass);
			break;
		case TDCEnum.EGameType.GroupMushRoom:
			groupController.CreatePoolMember (TDCEnum.EGameType.EnviromentMushroom);
			break;
		}
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.SetData (data);
		controller.SetCreatureType (type);
		controller.name = type.ToString ();
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		return controller;
	}

	public TDCBaseController CreateCreature(TDCEnum.EGameType type, 
	                                        Vector3 position, 
	                                        Quaternion rotation, 
	                                        GameObject parent = null) {
		TDCBaseData data = null;
		GameObject gObject = null;
		TDCBaseController controller = null;
		var random = Random.Range (0, 9999);
		switch (type) { 
		case TDCEnum.EGameType.PlayerDodono:
		case TDCEnum.EGameType.PlayerSatla:
		case TDCEnum.EGameType.PlayerBob: {
			data = m_DataLoader.GetPlayer (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCPlayerController> ();
			CameraController.Instance.Target = gObject.transform;
			break;
		}
		case TDCEnum.EGameType.Dodono: 
		case TDCEnum.EGameType.Satla:
		case TDCEnum.EGameType.Bob: {
			data = m_DataLoader.GetCreature (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCEasyAIController> ();
			break;
		}
		case TDCEnum.EGameType.Meat: {

			break;
		}
		case TDCEnum.EGameType.Trap: {
			data = m_DataLoader.GetWeapon (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCTrapController> ();
			controller.Init ();
			break;
		}
		case TDCEnum.EGameType.EnviromentGrass: {
			data = m_DataLoader.GetResource (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCResourceController> ();
			break;
		}
		case TDCEnum.EGameType.EnviromentMushroom: {
			data = m_DataLoader.GetResource (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCResourceController> ();
			break;
		}
		case TDCEnum.EGameType.CampFire:
			data = m_DataLoader.GetGObject (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath [0]), position, rotation) as GameObject;
			controller = gObject.AddComponent <TDCCampFireController>();
			break;
		default:

			break;
		}
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.SetData (data);
		controller.SetCreatureType (type);
		controller.Init ();
		controller.name = type.ToString ();
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		return controller;
	}

    #endregion
}
