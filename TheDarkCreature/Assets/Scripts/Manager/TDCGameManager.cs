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
	private TDCObjectPool<TDCSkillController> m_SkillPool;

    #endregion

    #region Implementation Mono

    void Awake() {
		m_Instance = this;

		DontDestroyOnLoad(this.gameObject);
		m_DataReader = new TDCDataReader();
		m_ListController = new Dictionary<string, TDCBaseController>();
		m_SkillPool = new TDCObjectPool<TDCSkillController>();

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

	public TDCSkillController GetSkillPool() {
		TDCSkillController skill = null;
		if (m_SkillPool.Get(ref skill))
		{
			return skill;
		}
		return null;
	}

	public void SetSkillPool(TDCSkillController skill) {
		m_SkillPool.Set(skill);
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

		controller.SetData (data);
		controller.Init ();
		groupController.CreatePoolMember ();
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.SetData (data);
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
		TDCCreatureData data = null;
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
		controller.SetData (data);
		controller.Init ();
		controller.name = string.Format("{0}-{1}", type, m_ListController.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		m_ListController.Add(controller.name, controller);
		controller.SetActive(true);
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
		default:

			break;
		}
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.SetData (data);
		controller.Init ();
		controller.name = string.Format("{0}-{1}", type, m_ListController.Count);
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		m_ListController.Add(controller.name, controller);
		return controller;
	}

	public TDCSkillController CreateSkill(	TDCSkillData data,
											TDCEnum.EGameType skillType,  
											Vector3 position, 
											Quaternion rotation, 
											GameObject parent = null) {
		TDCSkillController controller = null;
		var gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[0]), position, rotation) as GameObject;
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		gObject.name = string.Format("{0}-{1}", skillType, m_ListController.Count);
		switch (skillType)
		{
			case TDCEnum.EGameType.FlameBody:
				controller = gObject.AddComponent<TDCFlameBodyController>();
				break;
			case TDCEnum.EGameType.NormalRangeAttack:
				controller = gObject.AddComponent<TDCNormalRangeAttackController>();
				break;
			default:
				controller = gObject.AddComponent<TDCSkillController>();
				break;
		}
		data.Owner = null;
		controller.SetData(data);
		controller.Init();
		if (parent != null)
		{
			gObject.transform.SetParent(parent.transform);		
		}
		else
		{
			gObject.transform.SetParent(this.transform);
		}
		m_ListController.Add(controller.name, controller);
		m_SkillPool.Create(controller);
		return controller;
	}

    #endregion
}
