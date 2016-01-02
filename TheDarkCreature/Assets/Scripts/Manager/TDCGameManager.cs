using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

public class TDCGameManager : MonoBehaviour {

    #region Singleton

	private static object m_SingletonLock = new object();
	private static TDCGameManager m_Instance = null;

    public static TDCGameManager Instance {
        get {
            lock (m_SingletonLock) {
                if (m_Instance == null) {
					var m_SingletonObject = new GameObject("GameManager");
					m_Instance = m_SingletonObject.AddComponent<TDCGameManager>();
                }
                return m_Instance;
            }
        }
    }

    public static TDCGameManager GetInstance() {
        return Instance;
    }

    #endregion

    #region Properties

	private TDCDataReader m_DataReader;
	private Dictionary<string, TDCBaseController> m_ListController;
	private Dictionary<TDCEnum.EGameType, TDCObjectPool<TDCBaseController>> m_ObjectPool;

    #endregion

    #region Implementation Mono

    void Awake() {
		m_Instance = this;

		DontDestroyOnLoad(this.gameObject);
		m_DataReader = new TDCDataReader();
		m_ListController = new Dictionary<string, TDCBaseController>();
		m_ObjectPool = new Dictionary<TDCEnum.EGameType, TDCObjectPool<TDCBaseController>>();

#if UNITY_ANDROID	
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
#endif
    }

	void Start() {
		LoadMap("World1");
		LoadObjectPool();
    }

    #endregion

    #region Main method

	public void LoadObjectPool() {
		if (m_ObjectPool.Count > 0)
			return; // Once times
		var objPool = m_DataReader.GetObjectPoolData();
		for (int i = 0; i < objPool.Count; i++)
		{
			var poolData = objPool[i];
			m_ObjectPool.Add(poolData.GameType, new TDCObjectPool<TDCBaseController>());
			for (int x = 0; x < poolData.Amount; x++) {
				var obj = CreateCreature(poolData.GameType, Vector3.zero, Quaternion.identity, this.gameObject);
				obj.SetActive(false);
				m_ObjectPool[poolData.GameType].Create(obj);
			}
		}
	}

	public void LoadMap(string mapName) {
		CreatePlayer (TDCEnum.EGameType.Satla, Vector3.zero, Quaternion.identity);
		var map = m_DataReader.GetMap(mapName);
		for (int i = 0; i < map.Count; i++)
		{
			var mapObj = map[i];
			CreateGroup(mapObj.GameType, mapObj.Position, mapObj.Rotation);
		}
	}

	public TDCBaseController GetControllerByIndex(int index) {
		return m_ListController.ElementAt (index).Value;
	}

	public TDCBaseController GetControllerByName(string name) {
		if (m_ListController.ContainsKey(name))
		{
			return m_ListController[name];
		}
		return null;
	}

	public TDCSkillData GetSkillData(TDCEnum.EGameType skill) {
		return m_DataReader.GetSkillData(skill);
	}

	public TDCBaseController GetObjectPool(TDCEnum.EGameType type) {
		TDCBaseController result = null;
		if (m_ObjectPool[type].Get(ref result))
		{
			return result;
		}
		return null;
	}

	public bool GetObjectPool(TDCEnum.EGameType type, ref TDCBaseController obj) {
		if (m_ObjectPool[type].Get(ref obj))
		{
			return true;
		}
		return false;
	}

	public void SetObjectPool(TDCBaseController obj) {
		var gameType = obj.GetData().GameType;
		m_ObjectPool[gameType].Set(obj);
	}

	#endregion

	#region Create Game Creature

	public TDCItemController CreateItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, TDCBaseController owner, int amount) {
		TDCItemData itemData = null;
		switch (itemType)
		{
			default:
			case TDCEnum.EItemType.Food:
				itemData = m_DataReader.GetFood(gameType);
				break;
			case TDCEnum.EItemType.Weapon:
				itemData = m_DataReader.GetWeapon(gameType);
				break;
			case TDCEnum.EItemType.GObject:
			case TDCEnum.EItemType.Item:
				itemData = m_DataReader.GetItem(gameType);
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
		var data = m_DataReader.GetGroup (type);
		var gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath [random % data.ModelPath.Length]), position, rotation) as GameObject;
		var controller = gObject.AddComponent<TDCGroupCreatureController> ();
		var groupController = controller as TDCGroupCreatureController;

		controller.SetActive(true);
		controller.SetData (data);
		controller.Init ();
		groupController.CreatePoolMember ();
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.name = string.Format("{0}-{1}", type, m_ListController.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		m_ListController.Add(controller.name, controller);
		return controller;
	}

	public TDCBaseController CreatePlayer(TDCEnum.EGameType type, 
											Vector3 position, 
											Quaternion rotation, 
											GameObject parent = null) {
		TDCBaseData data = null;
		GameObject gObject = null;
		TDCBaseController controller = null;
		var random = Random.Range (0, 9999);
		switch (type) { 
			case TDCEnum.EGameType.Dodono:
			case TDCEnum.EGameType.Satla: {
					data = m_DataReader.GetPlayer (type);
					gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
					controller = gObject.AddComponent<TDCPlayerController> ();
					CameraController.Instance.Target = gObject.transform;
					break;
				}
			default:

				break;
		}
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.SetActive(true);
		controller.SetData (data);
		controller.Init ();
		controller.name = string.Format("{0}-{1}", type, m_ListController.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		m_ListController.Add(controller.name, controller);
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
		case TDCEnum.EGameType.Dodono: 
		case TDCEnum.EGameType.Satla: {
			data = m_DataReader.GetCreature (type);
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
		case TDCEnum.EGameType.Mushroom: 
		case TDCEnum.EGameType.BlueBerry:		
		case TDCEnum.EGameType.Bush: {
			data = m_DataReader.GetResource (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCResourceController> ();
			break;
		}
		case TDCEnum.EGameType.CampFire:
			data = m_DataReader.GetGObject (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath [0]), position, rotation) as GameObject;
			controller = gObject.AddComponent <TDCCampFireController>();
			break;
		case TDCEnum.EGameType.FlameBody:
			data = m_DataReader.GetSkillData (type);	
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[0]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCFlameBodyController>();
			break;
		case TDCEnum.EGameType.NormalRangeAttack:
			data = m_DataReader.GetSkillData (type);	
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[0]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCNormalRangeAttackController>();
			break;
		default:

			break;
		}
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.SetActive(true);
		controller.SetData (data);
		controller.Init ();
		controller.name = string.Format("{0}-{1}", type, m_ListController.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		m_ListController.Add(controller.name, controller);
		return controller;
	}

    #endregion


}
