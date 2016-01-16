using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System.Linq;

public class TDCDataReader {

	#region Property

	private Dictionary<TDCEnum.EGameType, TDCCreatureData> m_ListCreatureData;
	private Dictionary<TDCEnum.EGameType, TDCPlayerData> m_ListPlayerData;
	private Dictionary<TDCEnum.EGameType, UIItemData> m_ListItemData;
	private Dictionary<TDCEnum.EGameType, TDCGroupData> m_ListGroupData;
	private Dictionary<TDCEnum.EGameType, TDCSkillData> m_ListSkillData;
	private Dictionary<string, List<MapObjectData>> m_MapData;
	private List<TDCObjectPoolData> m_ListObjectPoolData;

	#endregion

	#region Contructor

	public TDCDataReader() {
		m_ListCreatureData = new Dictionary<TDCEnum.EGameType, TDCCreatureData> ();
		m_ListPlayerData = new Dictionary<TDCEnum.EGameType, TDCPlayerData> ();
		m_ListGroupData = new Dictionary<TDCEnum.EGameType, TDCGroupData> ();
		m_ListItemData = new Dictionary<TDCEnum.EGameType, UIItemData> ();
		m_ListSkillData = new Dictionary<TDCEnum.EGameType, TDCSkillData>();
		m_MapData = new Dictionary<string, List<MapObjectData>>();
		m_ListObjectPoolData = new List<TDCObjectPoolData>();

		LoadData ();
	}

	#endregion

	#region Main methods
	private void LoadData() {
		var uiFoodAsset = Resources.Load<TextAsset> ("Data/UIItems/UIFoodData");
		var uiWeaponAsset = Resources.Load<TextAsset> ("Data/UIItems/UIWeaponData");
		var uiItemAsset = Resources.Load<TextAsset> ("Data/UIItems/UIItemData");
		var groupAsset = Resources.Load<TextAsset> ("Data/Group/GroupData");
		var creatureAsset = Resources.Load<TextAsset> ("Data/Creature/CreatureData");
		var playerAsset = Resources.Load<TextAsset> ("Data/Player/PlayerData");
		var enviromentTextAsset = Resources.Load<TextAsset> ("Data/Creature/EnviromentData");
		var objectTextAsset = Resources.Load<TextAsset> ("Data/Creature/ObjectData");
		var itemTextAsset = Resources.Load<TextAsset> ("Data/Creature/ItemData");
		var skillTextAsset = Resources.Load<TextAsset>("Data/Skill/SkillData");
		var eggTextAsset = Resources.Load<TextAsset>("Data/Egg/EggData");
		var mapTextAsset = Resources.Load<TextAsset>("Data/Map/WorldMap");
		var poolTextAsset = Resources.Load<TextAsset>("ObjectPool/ObjectPoolData");

		var jsonUIItem = Json.Deserialize (uiItemAsset.text) as Dictionary<string, object>;
		var jsonUIFood = Json.Deserialize (uiFoodAsset.text) as Dictionary<string, object>;
		var jsonUIWeapon = Json.Deserialize (uiWeaponAsset.text) as Dictionary<string, object>;
		var jsonCreature = Json.Deserialize (creatureAsset.text) as Dictionary<string, object>;
		var jsonGroup = Json.Deserialize (groupAsset.text) as Dictionary<string, object>;
		var jsonPlayer = Json.Deserialize (playerAsset.text) as Dictionary<string, object>;
		var jsonResource = Json.Deserialize (enviromentTextAsset.text) as Dictionary<string, object>;
		var jsonObject = Json.Deserialize (objectTextAsset.text) as Dictionary<string, object>;
		var jsonItem = Json.Deserialize (itemTextAsset.text) as Dictionary<string, object>;
		var jsonSkill = Json.Deserialize (skillTextAsset.text) as Dictionary<string, object>;
		var jsonEgg = Json.Deserialize (eggTextAsset.text) as Dictionary<string, object>;
		var jsonMap = Json.Deserialize(mapTextAsset.text) as Dictionary<string, object>;
		var jsonPool = Json.Deserialize(poolTextAsset.text) as Dictionary<string, object>;

		LoadUIItem(jsonUIItem["items"] as List<object>);
		LoadUIFood (jsonUIFood["foods"] as List<object>);
		LoadUIWeapon (jsonUIWeapon["weapons"] as List<object>);
		LoadEnviroment (jsonResource["enviroments"] as List<object>);
		LoadCreature (jsonCreature["creatures"] as List<object>);
		LoadObject (jsonObject["objects"] as List<object>);
		LoadGroup (jsonGroup["groups"] as List<object>);
		LoadPlayer (jsonPlayer["players"] as List<object>);
		LoadItem (jsonItem["items"] as List<object>);
		LoadSkill(jsonSkill["skills"] as List<object>);
		LoadEgg(jsonEgg["eggs"] as List<object>);
		LoadMap(jsonMap["map"] as List<object>);
		LoadObjectPool(jsonPool);
	}

	private void LoadCreature(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var creature = new TDCCreatureData ();
			creature.ID = int.Parse (instance["ID"].ToString());
			creature.Name = instance["Name"].ToString();
			creature.Description = instance["Description"].ToString();
			creature.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			creature.FSMPath = instance["FSMPath"].ToString ();
			creature.Icon = instance["IconPath"].ToString();
			creature.GameType = (TDCEnum.EGameType)int.Parse (instance["GameType"].ToString());
			creature.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance["CreatureType"].ToString());
			creature.CurrentHP = int.Parse (instance["CurrentHP"].ToString());
			creature.MaxHP = int.Parse (instance["MaxHP"].ToString());
			creature.Damage = int.Parse (instance["Damage"].ToString());
			creature.CurrentHeatPoint = int.Parse (instance["CurrentHeatPoint"].ToString());
			creature.MaxHeatPoint = int.Parse (instance["MaxHeatPoint"].ToString());
			creature.DetectRange = float.Parse (instance["DetectRange"].ToString());
			creature.AttackRange = float.Parse (instance["AttackRange"].ToString());
			creature.MoveSpeed = float.Parse (instance["MoveSpeed"].ToString());
			creature.NormalSkill = (TDCEnum.EGameType)int.Parse (instance["NormalSkill"].ToString());
			ConvertToEnum (instance["Enemies"] as List<object>, creature.TypeEnemies);
			ConvertToEnum (instance["Foods"] as List<object>, creature.TypeFoods);
			var inventory = instance["Inventory"] as List<object>;
			for (int x = 0; x < inventory.Count; x++) {
				var invenData = inventory[x] as Dictionary<string, object>;
				var gameType = (TDCEnum.EGameType)int.Parse (invenData["GameType"].ToString());
				var amount = int.Parse (invenData["Amount"].ToString());
				var itemType = m_ListItemData [gameType].ItemType;
				var item = LoadItemData(itemType, gameType);
				creature.Inventory[x] = new UIItemController (item);
				creature.Inventory[x].GetData().Amount = amount;
			}
			m_ListCreatureData.Add (creature.GameType, creature);
		}
	}

	private void LoadUIItem(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var item = new UIItemData ();
			item.ID = int.Parse (instance ["ID"].ToString ());
			item.Name = instance ["Name"].ToString ();
			item.Description = instance ["Description"].ToString ();
			item.Icon = instance["IconPath"].ToString();
			item.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			item.ItemType = (TDCEnum.EItemType)int.Parse (instance ["ItemType"].ToString ());
			item.EffectPath = instance ["EffectPath"].ToString ();
			m_ListItemData.Add (item.GameType, item);
		}
	}

	private void LoadUIFood(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var food = new UIFoodData ();
			food.ID = int.Parse (instance ["ID"].ToString ());
			food.Name = instance ["Name"].ToString ();
			food.Description = instance ["Description"].ToString ();
			food.Icon = instance["IconPath"].ToString();
			food.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			food.ItemType = (TDCEnum.EItemType)int.Parse (instance ["ItemType"].ToString ());
            food.EffectPath = instance ["EffectPath"].ToString ();
			m_ListItemData.Add (food.GameType, food);
		}
	}

	private void LoadUIWeapon(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var weapon = new UIWeaponData ();
			weapon.ID = int.Parse (instance ["ID"].ToString ());
			weapon.Name = instance ["Name"].ToString ();
			weapon.Description = instance ["Description"].ToString ();
			weapon.Icon = instance["IconPath"].ToString();
			weapon.EffectPath = instance["EffectPath"].ToString();
			weapon.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			weapon.ItemType = (TDCEnum.EItemType)int.Parse (instance ["ItemType"].ToString ());
			weapon.MinDamage = int.Parse (instance ["MinDamage"].ToString ());
			weapon.MaxDamage = int.Parse (instance ["MaxDamage"].ToString ());
			weapon.AttackRange = float.Parse (instance ["AttackRange"].ToString ());
			weapon.Duration = int.Parse (instance ["Duration"].ToString ());
			weapon.DecreaseDuration = int.Parse (instance ["DecreaseDuration"].ToString ());
			m_ListItemData.Add (weapon.GameType, weapon);
		}
	}

	private void LoadGroup (List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var group = new TDCGroupData ();
			group.ID = int.Parse (instance ["ID"].ToString ());
			group.Name = instance ["Name"].ToString ();
			group.Description = instance ["Description"].ToString ();
			group.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			group.FSMPath = instance["FSMPath"].ToString ();
			group.Icon = instance["IconPath"].ToString();
			group.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			group.GroupType = (TDCEnum.EGroupType)int.Parse (instance ["GroupType"].ToString ());
			group.GroupSpawnType = (TDCEnum.EGroupSpawnType)Enum.Parse (typeof(TDCEnum.EGroupSpawnType), instance ["GroupSpawnType"].ToString ());
			group.Radius = float.Parse (instance ["Radius"].ToString ());
			group.MemberType = (TDCEnum.EGameType) int.Parse (instance["MemberType"].ToString());
			group.MaxMember = int.Parse (instance ["MaxMember"].ToString());
			group.TimeRespawnMember = float.Parse (instance ["TimeRespawnMember"].ToString ());
			group.CurrentHP = int.Parse (instance ["CurrentHP"].ToString ());
			group.MaxHP = int.Parse (instance ["MaxHP"].ToString ());
			m_ListGroupData.Add (group.GameType, group);
		}
	}

	private void LoadEnviroment (List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var enviroment = new TDCEnviromentData ();
			enviroment.ID = int.Parse (instance ["ID"].ToString ());
			enviroment.Name = instance ["Name"].ToString ();
			enviroment.Description = instance ["Description"].ToString ();
			enviroment.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			enviroment.FSMPath = instance["FSMPath"].ToString ();
			enviroment.Icon = instance["IconPath"].ToString();
			enviroment.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			enviroment.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance["CreatureType"].ToString());
			enviroment.CurrentHP = int.Parse (instance["CurrentHP"].ToString());
			enviroment.MaxHP = int.Parse (instance["MaxHP"].ToString());
			var inventory = instance["Inventory"] as List<object>;
			for (int x = 0; x < inventory.Count; x++) {
				var invenData = inventory[x] as Dictionary<string, object>;
				var gameType = (TDCEnum.EGameType)int.Parse (invenData["GameType"].ToString());
				var amount = int.Parse (invenData["Amount"].ToString());
				var itemType = m_ListItemData [gameType].ItemType;
				var item = LoadItemData(itemType, gameType);
				enviroment.Inventory[x] = new UIItemController (item);
				enviroment.Inventory[x].GetData().Amount = amount;
			}
			m_ListCreatureData.Add (enviroment.GameType, enviroment);
		}
	}

	private void LoadPlayer(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var player = new TDCPlayerData ();
			player.ID = int.Parse (instance["ID"].ToString());
			player.Name = instance["Name"].ToString();
			player.Description = instance["Description"].ToString();
			player.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			player.FSMPath = instance["FSMPath"].ToString ();
			player.Icon = instance["IconPath"].ToString();
			player.GameType = (TDCEnum.EGameType)int.Parse (instance["GameType"].ToString());
			player.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance["CreatureType"].ToString());
			player.CurrentHP = int.Parse (instance["CurrentHP"].ToString());
			player.MaxHP = int.Parse (instance["MaxHP"].ToString());
			player.CurrentHeatPoint = int.Parse (instance["CurrentHeatPoint"].ToString());
			player.MaxHeatPoint = int.Parse (instance["MaxHeatPoint"].ToString());
			player.CurrentHungerPoint = int.Parse (instance["CurrentHungerPoint"].ToString());
			player.MaxHungerPoint = int.Parse (instance["MaxHungerPoint"].ToString());
			player.CurrentSanityPoint = int.Parse (instance["CurrentSanityPoint"].ToString());
			player.MaxSanityPoint = int.Parse (instance["MaxSanityPoint"].ToString());
			player.Damage = int.Parse (instance["Damage"].ToString());
			player.MoveSpeed = float.Parse (instance["MoveSpeed"].ToString());
			player.AttackRange = float.Parse (instance["AttackRange"].ToString());
			player.DetectRange = float.Parse (instance["DetectRange"].ToString());
			player.NormalSkill = (TDCEnum.EGameType)int.Parse (instance["NormalSkill"].ToString());
			player.Level = int.Parse (instance["Level"].ToString());
			var inventory = instance["Inventory"] as List<object>;
			for (int x = 0; x < inventory.Count; x++) {
				var invenData = inventory[x] as Dictionary<string, object>;
				var gameType = (TDCEnum.EGameType)int.Parse (invenData["GameType"].ToString());
				var amount = int.Parse (invenData["Amount"].ToString());
				var itemType = m_ListItemData [gameType].ItemType;
				var item = LoadItemData(itemType, gameType);
                player.Inventory[x] = new UIItemController (item);
                player.Inventory[x].GetData().Amount = amount;
			}
			m_ListPlayerData.Add (player.GameType, player);
		}
	}

	public void LoadObject(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var gObject = new TDCGObjectData ();
			gObject.ID = int.Parse (instance ["ID"].ToString ());
			gObject.Name = instance ["Name"].ToString ();
			gObject.Description = instance ["Description"].ToString ();
			gObject.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			gObject.FSMPath = instance["FSMPath"].ToString();
			gObject.Icon = instance["IconPath"].ToString();
			gObject.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			gObject.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance["CreatureType"].ToString());
			gObject.Duration = float.Parse (instance ["Duration"].ToString ());
			var inventory = instance["Inventory"] as List<object>;
			for (int x = 0; x < inventory.Count; x++) {
				var invenData = inventory[x] as Dictionary<string, object>;
				var gameType = (TDCEnum.EGameType) int.Parse (invenData["GameType"].ToString());
				var amount = int.Parse (invenData["Amount"].ToString());
				var itemType = m_ListItemData [gameType].ItemType;
				var item = LoadItemData(itemType, gameType);
				gObject.Inventory[x] = new UIItemController (item);
				gObject.Inventory[x].GetData().Amount = amount;
			}
			m_ListCreatureData.Add (gObject.GameType, gObject);
		}
	}

	public void LoadItem(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var item = new TDCItemData ();
			item.ID = int.Parse (instance ["ID"].ToString ());
			item.Name = instance ["Name"].ToString ();
			item.Description = instance ["Description"].ToString ();
			item.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			item.FSMPath = instance["FSMPath"].ToString();
			item.Icon = instance["IconPath"].ToString();
			item.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			item.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance ["CreatureType"].ToString ());
			item.ItemType = (TDCEnum.EItemType)int.Parse (instance ["ItemType"].ToString ());
			m_ListCreatureData.Add (item.GameType, item);
		}
	}

	private void LoadSkill(List<object> skills) {
		for (int i = 0; i < skills.Count; i++)
		{
			var skill = skills[i] as Dictionary<string, object>;
			var skillData = new TDCSkillData();
			skillData.ID = int.Parse(skill["ID"].ToString());
			skillData.Name = skill["Name"].ToString();
			skillData.Description = skill["Description"].ToString();
			skillData.GameType = (TDCEnum.EGameType) int.Parse (skill["GameType"].ToString());
			skillData.Icon = skill["IconPath"].ToString();
			skillData.SkillLevel = int.Parse(skill["SkillLevel"].ToString());
			skillData.SkillType = (TDCEnum.ESkillType)int.Parse(skill["SkillType"].ToString());
			skillData.CostHealthPoint = int.Parse(skill["CostHealthPoint"].ToString());
			skillData.CostHeatPoint = int.Parse(skill["CostHeatPoint"].ToString());
			skillData.CostHungerPoint = int.Parse(skill["CostHungerPoint"].ToString());
			skillData.CostSanityPoint = int.Parse(skill["CostSanityPoint"].ToString());
			skillData.EffectRadius = float.Parse(skill["EffectRadius"].ToString());
			skillData.TimeDelay = float.Parse(skill["TimeDelay"].ToString());
			skillData.TimeEffect = float.Parse(skill["TimeEffect"].ToString());
			skillData.EffectPerTime = float.Parse(skill["EffectPerTime"].ToString());
			skillData.RepeatSkill = bool.Parse(skill["RepeatSkill"].ToString());
			skillData.AttachOwner = bool.Parse(skill["AttachOwner"].ToString());
			skillData.AttachEnemy = bool.Parse(skill["AttachEnemy"].ToString());
			skillData.ModelPath = ConvertTo<string> (skill["ModelPath"] as List<object>);
			skillData.FSMPath = skill["FSMPath"].ToString();
			skillData.EffectPath = skill["EffectPath"].ToString();
			skillData.TriggerEnvent = skill["TriggerEvent"].ToString();
			m_ListSkillData.Add(skillData.GameType, skillData);
		}
	}

	private void LoadEgg(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values[i] as Dictionary<string, object>;
			var egg = new TDCEggData ();
			egg.ID = int.Parse (instance["ID"].ToString());
			egg.Name = instance["Name"].ToString();
			egg.Description = instance["Description"].ToString();
			egg.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			egg.FSMPath = instance["FSMPath"].ToString ();
			egg.Icon = instance["IconPath"].ToString();
			egg.GameType = (TDCEnum.EGameType)int.Parse (instance["GameType"].ToString());
			egg.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance["CreatureType"].ToString());
			egg.CurrentHP = int.Parse (instance["CurrentHP"].ToString());
			egg.MaxHP = int.Parse (instance["MaxHP"].ToString());
			egg.CurrentHeatPoint = int.Parse (instance["CurrentHeatPoint"].ToString());
			egg.MaxHeatPoint = int.Parse (instance["MaxHeatPoint"].ToString());
			egg.Duration = int.Parse (instance["Duration"].ToString());
			egg.MemberType = (TDCEnum.EGameType)Enum.Parse (typeof(TDCEnum.EGameType), instance["MemberType"].ToString());
			var inventory = instance["Inventory"] as List<object>;
			for (int x = 0; x < inventory.Count; x++) {
				var invenData = inventory[x] as Dictionary<string, object>;
				var gameType = (TDCEnum.EGameType)int.Parse (invenData["GameType"].ToString());
				var amount = int.Parse (invenData["Amount"].ToString());
				var itemType = m_ListItemData [gameType].ItemType;
				var item = LoadItemData(itemType, gameType);
				egg.Inventory[x] = new UIItemController (item);
				egg.Inventory[x].GetData().Amount = amount;
			}
			m_ListCreatureData.Add (egg.GameType, egg);
		}
	}

	private void LoadMap(List<object> value) {
		for (int i = 0; i < value.Count; i++)
		{
			var map = value[i] as Dictionary<string, object>;
			var mapName = map["Name"].ToString();
			if (m_MapData.ContainsKey(mapName)) 
				continue;
			m_MapData[mapName] = new List<MapObjectData>();
			var mapObjects = map["MapObjects"] as List<object>;
			for (int j = 0; j < mapObjects.Count; j++) {
				var obj = new MapObjectData();
				var mapObj = mapObjects[j] as Dictionary<string, object>;
				obj.ID = int.Parse(mapObj["ID"].ToString());
				obj.GameType = (TDCEnum.EGameType)int.Parse(mapObj["GameType"].ToString());
				obj.Position = ConvertStringToV3(mapObj["Position"].ToString());
				obj.Rotation = Quaternion.Euler (ConvertStringToV3(mapObj["Rotation"].ToString()));
				m_MapData[mapName].Add(obj);
			}
		}
	}

	private UIItemData LoadItemData(TDCEnum.EItemType itemType, TDCEnum.EGameType gameType) {
		UIItemData item = null;
		switch (itemType) {
			case TDCEnum.EItemType.Food:
				item = GetUIFoodData (gameType);
				break;
			case TDCEnum.EItemType.Weapon:
				item = GetUIWeaponData (gameType);
				break;
			case TDCEnum.EItemType.GObject:
			case TDCEnum.EItemType.Item:
				item = GetUIItemData(gameType);
				break;
		}
		return item;
	}

	private void LoadObjectPool(Dictionary<string, object> data) {
		foreach (var item in data)
		{
			var obj = new TDCObjectPoolData();
			obj.GameType = (TDCEnum.EGameType) Enum.Parse(typeof (TDCEnum.EGameType), item.Key.ToString());
			obj.Amount = int.Parse(item.Value.ToString());
			m_ListObjectPoolData.Add(obj);
		}
	}

	#endregion

	#region Getter 

	public List<TDCObjectPoolData> GetObjectPoolData() {
		return m_ListObjectPoolData;
	}

	public TDCObjectPoolData GetObjectPoolData(int index) {
		return m_ListObjectPoolData[index];
	}

	public TDCObjectPoolData GetObjectPoolData(TDCEnum.EGameType type) {
		for (int i = 0; i < m_ListObjectPoolData.Count; i++)
		{
			if (m_ListObjectPoolData[i].GameType == type)
			{
				return m_ListObjectPoolData[i];
			}
		}
		return null;
	}

	public TDCSkillData GetSkillData(TDCEnum.EGameType skill) {
		var skillData = TDCSkillData.Parse (m_ListSkillData[skill] as TDCSkillData);
		return skillData;
	}

	public List<MapObjectData> GetMap(string name) {
		var map = m_MapData[name];
		return map;
	}

	public TDCCreatureData GetCreatureData(TDCEnum.EGameType creature) {
		var creatureData = TDCCreatureData.Clone (m_ListCreatureData [creature]);
		return creatureData;
	}

	public TDCEggData GetEggData(TDCEnum.EGameType creature) {
		var eggData = TDCEggData.Clone (m_ListCreatureData [creature] as TDCEggData);
		return eggData;
	}

	public UIItemData GetUIItemData (TDCEnum.EGameType item) {
		var itemData = UIItemData.Clone (m_ListItemData[item] as UIItemData);
		itemData.Owner = null;
		return itemData;
	}

	public UIFoodData GetUIFoodData (TDCEnum.EGameType food) {
		var foodData = UIFoodData.Clone (m_ListItemData[food] as UIFoodData);
		foodData.Owner = null;
		return foodData;
	}

	public UIWeaponData GetUIWeaponData(TDCEnum.EGameType weapon) {
		var weapondata = UIWeaponData.Clone (m_ListItemData[weapon] as UIWeaponData);
		weapondata.Owner = null;
		return weapondata;
	}

	public TDCGroupData GetGroupData(TDCEnum.EGameType group) {
		var groupData = TDCGroupData.Clone (m_ListGroupData[group]);
		return groupData;
	}

	public TDCPlayerData GetPlayerData(TDCEnum.EGameType player) {
		var playerData = TDCPlayerData.Clone (m_ListPlayerData[player] as TDCPlayerData);
		return playerData;
	}

	public TDCItemData GetItemData(TDCEnum.EGameType resource) {
		var item = TDCItemData.Clone (m_ListCreatureData[resource] as TDCItemData);
		return item;
	}

	public TDCEnviromentData GetEnviromentData(TDCEnum.EGameType resource) {
		var resourceData = TDCEnviromentData.Clone (m_ListCreatureData[resource] as TDCEnviromentData);
		return resourceData;
	}

	public TDCGObjectData GetGObjectData(TDCEnum.EGameType gObject) {
		var objectData = TDCGObjectData.Clone (m_ListCreatureData[gObject] as TDCGObjectData);
		return objectData;
	}

	#endregion

	#region Ultilities

	private Vector3 ConvertStringToV3(string value) {
		var result = Vector3.zero;
		var tmp = value.Split(',');
		result.x = float.Parse(tmp[0].ToString());
		result.y = float.Parse(tmp[1].ToString());
		result.z = float.Parse(tmp[2].ToString());
		return result;
	}

	private void ConvertToEnum (List<object> instance, List<TDCEnum.EGameType> listType) {
		for (int i = 0; i < instance.Count; i++) {
			var typeEnum = (TDCEnum.EGameType) int.Parse (instance[i].ToString());
			listType.Add (typeEnum);
		}
	}

	private T[] ConvertTo<T> (List<object> instance) {
		var result = new T[instance.Count];
		for (int i = 0; i < instance.Count; i++) {
			result[i] = (T)Convert.ChangeType (instance[i], typeof (T));
		}
		return result;
	}

	#endregion

}










