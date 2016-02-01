using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using Assets.Map;

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
	private Map m_Map;

	private TDCEntity m_PlaneEntity;
	private TDCEntity m_SeasonEntity;
	private TDCEntity m_SunEntity;

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
		UnityEngine.Random.seed = (int) System.DateTime.Now.Ticks;
	
		var ui = UIManager.GetInstance();
		ui.EnableLoadingScreen(true);
		Debug.Log ("Loading " + Time.time);
		HandleLoadObjectPool(() => {
			HandleLoadMap("World1", () => {
				Debug.Log ("Complete " + Time.time);
				ui.EnableLoadingScreen(false);
			});
		});
    }

    #endregion

    #region Main method

	private void HandleLoadMap(string name, Action complete = null) {
		StartCoroutine (LoadMap(name, complete));
	}

	private void HandleLoadObjectPool(Action complete = null) {
		StartCoroutine (LoadObjectPool(complete));
	}

	public IEnumerator LoadObjectPool(Action complete = null) {
		if (m_ObjectPool.Count > 0)
			yield break; // Once times
		var objPool = m_DataReader.GetObjectPoolData();
		for (int i = 0; i < objPool.Count; i++)
		{
			var poolData = objPool[i];
			m_ObjectPool.Add(poolData.GameType, new TDCObjectPool<TDCEntity>());
			for (int x = 0; x < poolData.Amount; x++) {
				var obj = CreateCreature(poolData.GameType, Vector3.zero, Quaternion.identity, this.gameObject);
				obj.SetActive(false);
				m_ObjectPool[poolData.GameType].Create(obj);
				yield return obj != null;
			}
		}
		yield return null;
		if (complete != null)
		{
			complete();
		}
	}

	public IEnumerator LoadMap(string mapName, Action complete = null) {
		var sun = CreateCreature(TDCEnum.EGameType.Sun, new Vector3 (0f, 200f, 0f), Quaternion.Euler(new Vector3(75f, -30f, 0f)));
		sun.SetActive(true);

		var plane = CreateCreature(TDCEnum.EGameType.GrassLandPlane, Vector3.zero, Quaternion.Euler(new Vector3(270f, 180f, 0f)));
		plane.SetActive(true);

		var season = CreateCreature(TDCEnum.EGameType.SeasonGrassLand, Vector3.zero, Quaternion.identity);
		season.SetActive(true);

		var playerType = TDCGameSetting.Instance.GetPlayerType();
		var player = CreatePlayer (playerType, Vector3.zero, Quaternion.identity);
		player.SetActive(true);

		var map = m_DataReader.GetMap(mapName);
		for (int i = 0; i < map.Count; i++)
		{
			var mapObj = map[i];
			var obj = CreateCreature(mapObj.GameType, mapObj.Position, mapObj.Rotation);
			obj.SetActive(true);
			yield return obj != null;
		}
		yield return null;
		if (complete != null)
		{
			complete();
		}

//		m_Map = new Map(150);
//		var bound = plane.GetController().GetComponent<MeshFilter>().mesh.bounds;
//		yield return m_Map != null && plane != null;
//		var centers = m_Map.Graph.centers;
//		var offsetPointX = bound.size.x / Map.Width;
//		var offsetPointY = bound.size.y / Map.Height;
//		for (int i = 0; i < centers.Count; i++)
//		{
//			var centerPoint = centers[i];
//			if (centerPoint.biome == Biome.Ocean)
//				continue;
//			
//			var gameType = TDCEnum.EGameType.None;
//			switch (centerPoint.biome)
//			{
//				case Biome.Grassland:
//					var grassRegion = new TDCEnum.EGameType[] { TDCEnum.EGameType.None, TDCEnum.EGameType.GroupDodono, TDCEnum.EGameType.GroupGrass, TDCEnum.EGameType.GroupSatla };
//					gameType = grassRegion[i % grassRegion.Length];
//					break;
//				case Biome.Marsh:
//					var marshRegion = new TDCEnum.EGameType[] { TDCEnum.EGameType.None, TDCEnum.EGameType.GroupMushRoom };
//					gameType = marshRegion[i % marshRegion.Length];
//					break;
//				case Biome.TemperateDesert:
//					var desertRegion = new TDCEnum.EGameType[] { TDCEnum.EGameType.None, TDCEnum.EGameType.GroupBush, TDCEnum.EGameType.GroupVulbat };
//					gameType = desertRegion[i % desertRegion.Length];
//					break;
//				case Biome.Lava:
//					var lavaRegion = new TDCEnum.EGameType[] { TDCEnum.EGameType.None, TDCEnum.EGameType.GroupBlueBerry, TDCEnum.EGameType.GroupFireBuggy };
//					gameType = lavaRegion[i % lavaRegion.Length];
//					break;
//				case Biome.RockLand:
//					var rockRegion = new TDCEnum.EGameType[] { TDCEnum.EGameType.None, TDCEnum.EGameType.GroupCrystal, TDCEnum.EGameType.GroupCrabystal, TDCEnum.EGameType.GroupTaurot };
//					gameType = rockRegion[i % rockRegion.Length];
//					break;
//				default:
////					var defaultRegion = new TDCEnum.EGameType[] { TDCEnum.EGameType.None, TDCEnum.EGameType.GroupGrass, TDCEnum.EGameType.GroupBush, TDCEnum.EGameType.GroupMushRoom, TDCEnum.EGameType.GroupBlueBerry };
////					gameType = defaultRegion[i % defaultRegion.Length];
//					break;
//			}
//			if (gameType == TDCEnum.EGameType.None)
//				continue;
//			var point = centerPoint.point;
//			var _X = (point.x - (Map.Width / 2f)) * offsetPointX;
//			var _Y = (point.y - (Map.Height / 2f)) * offsetPointY;
//			var obj = CreateCreature(gameType, new Vector3 (_X, 0f, _Y), Quaternion.identity);
//			obj.SetActive(true);
//			yield return obj != null;
//		}
//		yield return null;
//		if (complete != null)
//		{
//			complete();
//		}

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
		else
		{
			Debug.LogError("[GameManager] Set more pool " + type);
		}
		return null;
	}

	public bool GetObjectPool(TDCEnum.EGameType type, ref TDCEntity obj) {
		if (m_ObjectPool[type].Get(ref obj))
		{
			return true;
		}
		else
		{
			Debug.LogError("[GameManager] Set more pool " + type);
		}
		return false;
	}

	public void SetObjectPool(TDCEntity obj) {
		var gameType = obj.GetGameType();
		if (m_ObjectPool.ContainsKey(gameType))
		{
			// TODO
		}
		else
		{
			m_ObjectPool[gameType] = new TDCObjectPool<TDCEntity>();
			Debug.LogError("[GameManager] Pool not instance " + gameType);
		}
		m_ObjectPool[gameType].Set(obj);
		obj.GetController().transform.SetParent(this.transform);
	}

	#endregion

	#region Create Game Creature

	public UIItemController CreateItem(TDCEnum.EGameType gameType, TDCEnum.EItemType itemType, TDCBaseController owner, int amount) {
		UIItemData itemData = null;
		switch (itemType)
		{
			default:
			case TDCEnum.EItemType.Food:
				itemData = m_DataReader.GetUIFoodData(gameType);
				break;
			case TDCEnum.EItemType.Weapon:
				itemData = m_DataReader.GetUIWeaponData(gameType);
				break;
			case TDCEnum.EItemType.GObject:
			case TDCEnum.EItemType.Item:
				itemData = m_DataReader.GetUIItemData(gameType);
				break;
		}
		if (itemData == null)
			return null;
		var item = new UIItemController(itemData);
		item.GetData().Owner = owner;
		item.GetData().Amount = amount;
		return item;
	}

	public TDCEntity CreatePlayer(TDCEnum.EGameType type, 
											Vector3 position, 
											Quaternion rotation, 
											GameObject parent = null) {
		TDCBaseData data = null;
		GameObject gObject = null;
		TDCBaseController controller = null;
		TDCEntity entity = null;
		var random = UnityEngine.Random.Range (0, 9999);
		data = m_DataReader.GetPlayerData (type);
		gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
		switch ((data as TDCPlayerData).CreatureType)
		{
			case TDCEnum.ECreatureType.GroundPlayer:
				controller = gObject.AddComponent<TDCPlayerController> ();
				break;
			case TDCEnum.ECreatureType.FlyPlayer:
				controller = gObject.AddComponent<TDCFlyPlayerController> ();
				break;
		}
		CameraController.Instance.Target = gObject.transform;
		data.ID = m_ListEntities.Count + 1;
		entity = new TDCPlayer(controller, data);
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
		var random = UnityEngine.Random.Range (0, 9999);
		switch (type) { 
		case TDCEnum.EGameType.Sun:
			data = m_DataReader.GetSunData (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCSunController> ();
			entity = new TDCSun(controller, data);
			m_SunEntity = entity;
			break;
		case TDCEnum.EGameType.GrassLandPlane: {
			data = m_DataReader.GetPlaneData (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCPlaneController> ();
			entity = new TDCPlane(controller, data);
			m_PlaneEntity = entity;
			break;
		}
		case TDCEnum.EGameType.Dodono: 
		case TDCEnum.EGameType.Satla: 
		case TDCEnum.EGameType.Taurot: 
		case TDCEnum.EGameType.Vulbat:
		case TDCEnum.EGameType.Crabystal: 
		case TDCEnum.EGameType.FireBuggy: {
			data = m_DataReader.GetCreatureData (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			switch ((data as TDCCreatureData).CreatureType){
			case TDCEnum.ECreatureType.GroundCreature:
				controller = gObject.AddComponent<TDCGroundAIController> ();
				break;
			case TDCEnum.ECreatureType.FlyCreature:
				controller = gObject.AddComponent<TDCFlyAIController> ();
				break;
			}
			entity = new TDCCreature(controller, data);
			break;
		}
		case TDCEnum.EGameType.ItemMeat: 
		case TDCEnum.EGameType.ItemMushroom:
		case TDCEnum.EGameType.ItemGrass:
		case TDCEnum.EGameType.ItemBush:
		case TDCEnum.EGameType.ItemBlueBerry:
		case TDCEnum.EGameType.ItemCrystal: {
			data = m_DataReader.GetItemData(type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCItemController> ();
			entity = new TDCItem(controller, data);
			break;
		}
		case TDCEnum.EGameType.Trap: {
			//TODO
			break;
		}
		case TDCEnum.EGameType.GroupDodono:
		case TDCEnum.EGameType.GroupSatla: 
		case TDCEnum.EGameType.GroupTaurot: 
		case TDCEnum.EGameType.GroupVulbat: 
		case TDCEnum.EGameType.GroupGrass:
		case TDCEnum.EGameType.GroupMushRoom:
		case TDCEnum.EGameType.GroupBlueBerry: 
		case TDCEnum.EGameType.GroupBush:
		case TDCEnum.EGameType.GroupCrystal:
		case TDCEnum.EGameType.GroupCrabystal:
		case TDCEnum.EGameType.GroupFireBuggy: {
			data = m_DataReader.GetGroupData(type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			switch ((data as TDCGroupData).GroupType) {
			case TDCEnum.EGroupType.GroupNestCreature:
				controller = gObject.AddComponent<TDCNestGroupCreatureController> ();
				break;
			case TDCEnum.EGroupType.GroupCreature:
				controller = gObject.AddComponent<TDCGroupCreatureController> ();
				break;
			}
			entity = new TDCGroup(controller, data);
			break;
		}
		case TDCEnum.EGameType.Grass:
		case TDCEnum.EGameType.Mushroom: 
		case TDCEnum.EGameType.BlueBerry:		
		case TDCEnum.EGameType.Bush: 
		case TDCEnum.EGameType.Crystal: {
			data = m_DataReader.GetEnviromentData (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCEnviromentController> ();
			entity = new TDCEnviroment(controller, data);
			break;
		}
		case TDCEnum.EGameType.CampFire: {
			data = m_DataReader.GetGObjectData(type);
			gObject = GameObject.Instantiate(Resources.Load<GameObject>(data.ModelPath[0]), position, rotation) as GameObject;
			controller = gObject.AddComponent <TDCCampFireController>();
			entity = new TDCCampFire(controller, data);
			break;
		}
		case TDCEnum.EGameType.FlameBodySkill: 
		case TDCEnum.EGameType.LifeNotEasySkill:
		case TDCEnum.EGameType.BurnObjectSkill:
		case TDCEnum.EGameType.AfraidTheDarkSkill:
		case TDCEnum.EGameType.NormalMeleeSkill:
		case TDCEnum.EGameType.NormalRangeSkill:
		case TDCEnum.EGameType.WeatherNormalSkill:
		case TDCEnum.EGameType.WeatherRainySkill: 
		case TDCEnum.EGameType.WeatherOverHeatSkill:
		case TDCEnum.EGameType.WeatherWindySkill:
		case TDCEnum.EGameType.WeatherSnowySkill: 
		case TDCEnum.EGameType.EyeOfNightSkill: {
			data = m_DataReader.GetSkillData(type);	
			gObject = GameObject.Instantiate(Resources.Load<GameObject>(data.ModelPath[0]), position, rotation) as GameObject;
			switch ((data as TDCSkillData).SkillType) {
			case TDCEnum.ESkillType.Active:
				controller = gObject.AddComponent<TDCActiveSkillController>();
				break;
			case TDCEnum.ESkillType.Passive:
				controller = gObject.AddComponent<TDCPasiveSkillController>();
				break;
			}
			entity = new TDCSkill(controller, data);
			break;
		}
		case TDCEnum.EGameType.EggDodono:{
			data = m_DataReader.GetEggData(type);	
			gObject = GameObject.Instantiate(Resources.Load<GameObject>(data.ModelPath[0]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCEggController>();
			entity = new TDCEgg(controller, data);
			break;
		}
		case TDCEnum.EGameType.SeasonGrassLand: {
			data = m_DataReader.GetSeasonData (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]), position, rotation) as GameObject;
			controller = gObject.AddComponent<TDCSeasonController> ();
			entity = new TDCSeason(controller, data);
			m_SeasonEntity = entity;
			break;
		}
		default:

			break;
		}
		data.ID = m_ListEntities.Count + 1;
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

	#region Getter && Setter

	public TDCEntity GetPlane() {
		return m_PlaneEntity;
	}

	public void SetSunEntity(TDCEntity value) {
		m_SunEntity = value;
	}

	public TDCEntity GetSunEntity() {
		return m_SunEntity;
	}

	#endregion

}
