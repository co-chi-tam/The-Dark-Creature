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
	private Dictionary<string, TDCBaseController> m_ListControllers;
	private Dictionary<string, TDCEntity> m_ListEntities;
	private Dictionary<TDCEnum.EGameType, TDCObjectPool<TDCEntity>> m_ObjectPool;

    #endregion

    #region Implementation Mono

    void Awake() {
		m_Instance = this;

		DontDestroyOnLoad(this.gameObject);
		m_DataReader = new TDCDataReader();
		m_ListControllers = new Dictionary<string, TDCBaseController>();
		m_ListEntities = new Dictionary<string, TDCEntity>();
		m_ObjectPool = new Dictionary<TDCEnum.EGameType, TDCObjectPool<TDCEntity>>();

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
			m_ObjectPool.Add(poolData.GameType, new TDCObjectPool<TDCEntity>());
			for (int x = 0; x < poolData.Amount; x++) {
				var obj = CreateCreature(poolData.GameType, Vector3.zero, Quaternion.identity, this.gameObject);
				obj.SetActive(false);
				m_ObjectPool[poolData.GameType].Create(obj);
			}
		}
	}

	public void LoadMap(string mapName) {
		var player = CreatePlayer (TDCEnum.EGameType.Satla, Vector3.zero, Quaternion.identity);
		player.SetActive(true);
		var map = m_DataReader.GetMap(mapName);
		for (int i = 0; i < map.Count; i++)
		{
			var mapObj = map[i];
			var group = CreateGroup(mapObj.GameType, mapObj.Position, mapObj.Rotation);
			group.SetActive(true);
		}
	}

	public TDCBaseController GetControllerByIndex(int index) {
		return m_ListControllers.ElementAt (index).Value;
	}

	public TDCBaseController GetControllerByName(string name) {
		if (m_ListControllers.ContainsKey(name))
		{
			return m_ListControllers[name];
		}
		return null;
	}

	public TDCEntity GetEntityByIndex(int index) {
		return m_ListEntities.ElementAt (index).Value;
	}

	public TDCEntity GetEntityByName(string name) {
		if (m_ListEntities.ContainsKey(name))
		{
			return m_ListEntities[name];
		}
		return null;
	}

	public TDCSkillData GetSkillData(TDCEnum.EGameType skill) {
		return m_DataReader.GetSkillData(skill);
	}

	public TDCEntity GetObjectPool(TDCEnum.EGameType type) {
		TDCEntity result = null;
		if (m_ObjectPool[type].Get(ref result))
		{
			return result;
		}
		return null;
	}

	public bool GetObjectPool(TDCEnum.EGameType type, ref TDCEntity obj) {
		if (m_ObjectPool[type].Get(ref obj))
		{
			return true;
		}
		return false;
	}

	public void SetObjectPool(TDCEntity obj) {
		var gameType = obj.GetGameType();
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

	public TDCEntity CreateGroup(TDCEnum.EGameType type, 
	                                               Vector3 position, 
	                                               Quaternion rotation, 
	                                               GameObject parent = null) {
		var random = Random.Range (0, 9999);
		var data = m_DataReader.GetGroup (type);
		var gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath [random % data.ModelPath.Length]), position, rotation) as GameObject;
		var controller = gObject.AddComponent<TDCGroupCreatureController> ();
		var entity = new TDCGroup(controller, data);
		entity.SetActive(false);
		controller.SetEntity(entity);
		controller.Init ();
		controller.CreatePoolMember ();
		controller.name = string.Format("{0}-{1}", type, m_ListControllers.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		m_ListEntities.Add(controller.name, entity);
		m_ListControllers.Add(controller.name, controller);
		return entity;
	}

	public TDCEntity CreatePlayer(TDCEnum.EGameType type, 
											Vector3 position, 
											Quaternion rotation, 
											GameObject parent = null) {
		TDCBaseData data = null;
		GameObject gObject = null;
		TDCBaseController controller = null;
		TDCEntity entity = null;
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
		entity = new TDCCreature(controller, data);
		entity.SetActive(false);
		controller.SetEntity(entity);
		controller.Init ();
		controller.name = string.Format("{0}-{1}", type, m_ListControllers.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}

		m_ListEntities.Add(controller.name, entity);
		m_ListControllers.Add(controller.name, controller);
		return entity;
	}

	public TDCEntity CreateCreature(TDCEnum.EGameType type, 
	                                        Vector3 position, 
	                                        Quaternion rotation, 
	                                        GameObject parent = null) {
		TDCBaseData data = null;
		TDCBaseController controller = null;
		TDCEntity entity = null;
		GameObject gObject = null;
		var random = Random.Range (0, 9999);
		switch (type) { 
		case TDCEnum.EGameType.Dodono: 
		case TDCEnum.EGameType.Satla: {
			data = m_DataReader.GetCreature (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCEasyAIController> ();
			entity = new TDCCreature(controller, data);
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
			controller = gObject.AddComponent<TDCEnviromentController> ();
			entity = new TDCEnviroment(controller, data);
			break;
		}
		case TDCEnum.EGameType.CampFire:
			data = m_DataReader.GetGObject (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath [0]), position, rotation) as GameObject;
			controller = gObject.AddComponent <TDCCampFireController>();
			entity = new TDCObject(controller, data);
			break;
		case TDCEnum.EGameType.FlameBodySkill:
			data = m_DataReader.GetSkillData (type);	
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[0]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCPasiveSkillController>();
			entity = new TDCSkill(controller, data);
			break;
		case TDCEnum.EGameType.NormalMeleeSkill:
		case TDCEnum.EGameType.NormalRangeSkill:
		case TDCEnum.EGameType.EffectPerSecondSkill:
		case TDCEnum.EGameType.EffectUpdateSkill:
			data = m_DataReader.GetSkillData (type);	
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[0]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCActiveSkillController>();
			entity = new TDCSkill(controller, data);
			break;
		default:

			break;
		}
		entity.SetActive(false);
		controller.SetEntity(entity);
		controller.Init ();
		controller.name = string.Format("{0}-{1}", type, m_ListControllers.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}

		m_ListEntities.Add(controller.name, entity);
		m_ListControllers.Add(controller.name, controller);
		return entity;
	}

    #endregion


}
