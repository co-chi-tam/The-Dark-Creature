using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	private TDCDataReader m_DataLoader;
	private List<TDCBaseController> m_ListController;

    #endregion

    #region Implementation Mono

    void Awake() {
		DontDestroyOnLoad(this.gameObject);
		m_DataLoader = new TDCDataReader();
		m_ListController = new List<TDCBaseController>();

#if UNITY_ANDROID	
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
    }

	void Start() {
		LoadMap("World1");
    }

    #endregion

    #region Main method

	public void LoadMap(string mapName) {
		CreateCreature (TDCEnum.EGameType.PlayerSatla, Vector3.zero, Quaternion.identity);
		var map = m_DataLoader.GetMap(mapName);
		for (int i = 0; i < map.Count; i++)
		{
			var mapObj = map[i];
			CreateGroup(mapObj.GameType, mapObj.Position, mapObj.Rotation);
		}
	}

	public TDCItemController CreateItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, TDCBaseController owner, int amount) {
		TDCItemData itemData = null;
		switch (itemType)
		{
			default:
			case TDCEnum.EItemType.Food:
				itemData = m_DataLoader.GetFood(gameType);
				break;
			case TDCEnum.EItemType.Weapon:
				itemData = m_DataLoader.GetWeapon(gameType);
				break;
		}
		if (itemData == null)
			return null;
		var item = new TDCItemController(itemData);
		item.GetData().Owner = owner;
		item.GetData().Amount = amount;
		return item;
	}

	public TDCGroupCreatureController CreateGroup(TDCEnum.EGameType type, 
	                                               Vector3 position, 
	                                               Quaternion rotation, 
	                                               GameObject parent = null) {
		var random = Random.Range (0, 9999);
		var data = m_DataLoader.GetGroup (type);
		var gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath [random % data.ModelPath.Length]), position, rotation) as GameObject;
		var controller = gObject.AddComponent<TDCGroupCreatureController> ();
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
		case TDCEnum.EGameType.GroupGrass:
			groupController.CreatePoolMember (TDCEnum.EGameType.Grass);
			break;
		case TDCEnum.EGameType.GroupMushRoom:
			groupController.CreatePoolMember (TDCEnum.EGameType.Mushroom);
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
		m_ListController.Add(controller);
		return controller;
	}

	public TDCBaseController CreateCreature(TDCEnum.EGameType type, 
	                                        Vector3 position, 
	                                        Quaternion rotation, 
	                                        GameObject parent = null) {
		TDCCreatureData data = null;
		GameObject gObject = null;
		TDCBaseController controller = null;
		var random = Random.Range (0, 9999);
		switch (type) { 
		case TDCEnum.EGameType.PlayerDodono:
		case TDCEnum.EGameType.PlayerSatla: {
			data = m_DataLoader.GetPlayer (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCPlayerController> ();
//			controller.gameObject.layer = (int)TDCEnum.ELayer.LayerPlayer;
			CameraController.Instance.Target = gObject.transform;
			break;
		}
		case TDCEnum.EGameType.Dodono: 
		case TDCEnum.EGameType.Satla: {
			data = m_DataLoader.GetCreature (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCEasyAIController> ();
			break;
		}
		case TDCEnum.EGameType.Meat: 
		case TDCEnum.EGameType.Trap: {
			//TODO
			break;
		}
		case TDCEnum.EGameType.Grass:
		case TDCEnum.EGameType.Mushroom: {
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
		m_ListController.Add(controller);
		return controller;
	}

    #endregion
}
