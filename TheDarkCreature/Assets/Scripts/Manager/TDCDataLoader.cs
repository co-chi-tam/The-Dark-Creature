using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using System.Linq;

public class TDCDataLoader {

	#region Property

	private Dictionary<TDCEnum.EGameType, TDCCreatureData> m_ListCreatureData;
	private Dictionary<TDCEnum.EGameType, TDCItemData> m_ListItemData;
	private Dictionary<TDCEnum.EGameType, TDCGroupData> m_ListGroupData;


	#endregion

	#region Contructor

	public TDCDataLoader() {
		m_ListCreatureData = new Dictionary<TDCEnum.EGameType, TDCCreatureData> ();
		m_ListGroupData = new Dictionary<TDCEnum.EGameType, TDCGroupData> ();
		m_ListItemData = new Dictionary<TDCEnum.EGameType, TDCItemData> ();

		LoadData ();
	}

	#endregion

	#region Main methods
	private void LoadData() {
		var creatureAsset = Resources.Load<TextAsset> ("Data/Creature/CreatureData");
		var foodAsset = Resources.Load<TextAsset> ("Data/Item/FoodData");
		var groupAsset = Resources.Load<TextAsset> ("Data/Group/GroupData");
		var playerAsset = Resources.Load<TextAsset> ("Data/Creature/PlayerData");
		var weaponAsset = Resources.Load<TextAsset> ("Data/Item/WeaponData");
		var resourceTextAsset = Resources.Load<TextAsset> ("Data/Creature/ResourceData");
		var objectTextAsset = Resources.Load<TextAsset> ("Data/Creature/ObjectData");

		var jsonCreature = Json.Deserialize (creatureAsset.text) as Dictionary<string, object>;
		var jsonfood = Json.Deserialize (foodAsset.text) as Dictionary<string, object>;
		var jsonGroup = Json.Deserialize (groupAsset.text) as Dictionary<string, object>;
		var jsonPlayer = Json.Deserialize (playerAsset.text) as Dictionary<string, object>;
		var jsonWeapon = Json.Deserialize (weaponAsset.text) as Dictionary<string, object>;
		var jsonResource = Json.Deserialize (resourceTextAsset.text) as Dictionary<string, object>;
		var jsonObject = Json.Deserialize (objectTextAsset.text) as Dictionary<string, object>;

		LoadFood (jsonfood["foods"] as List<object>);
		LoadGroup (jsonGroup["groups"] as List<object>);
		LoadWeapon (jsonWeapon["weapons"] as List<object>);
		LoadResource (jsonResource["resources"] as List<object>);
		LoadCreature (jsonCreature["creatures"] as List<object>);
		LoadObject (jsonObject["objects"] as List<object>);
		LoadPlayer (jsonPlayer["players"] as List<object>);
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
			creature.MinDamage = int.Parse (instance["MinDamage"].ToString());
			creature.MaxDamage = int.Parse (instance["MaxDamage"].ToString());
			creature.DetectRange = float.Parse (instance["DetectRange"].ToString());
			creature.AttackRange = float.Parse (instance["AttackRange"].ToString());
			creature.RunSpeed = float.Parse (instance["RunSpeed"].ToString());
			creature.WalkSpeed = float.Parse (instance["WalkSpeed"].ToString());
			ConvertToEnum (instance["Enemies"] as List<object>, creature.TypeEnemies);
			ConvertToEnum (instance["Foods"] as List<object>, creature.TypeFoods);
			m_ListCreatureData.Add (creature.GameType, creature);
		}
	}

	private void LoadFood(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var food = new TDCFoodData ();
			food.ID = int.Parse (instance ["ID"].ToString ());
			food.Name = instance ["Name"].ToString ();
			food.Description = instance ["Description"].ToString ();
			food.Icon = instance["IconPath"].ToString();
			food.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			food.ItemType = (TDCEnum.EItemType)int.Parse (instance ["ItemType"].ToString ());
			food.EffectName = instance ["EffectName"].ToString ();
			m_ListItemData.Add (food.GameType, food);
		}
	}

	private void LoadWeapon(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var weapon = new TDCWeaponData ();
			weapon.ID = int.Parse (instance ["ID"].ToString ());
			weapon.Name = instance ["Name"].ToString ();
			weapon.Description = instance ["Description"].ToString ();
			weapon.Icon = instance["IconPath"].ToString();
			weapon.EffectName = instance["EffectName"].ToString();
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
			group.Radius = float.Parse (instance ["Radius"].ToString ());
			group.MinMember = int.Parse (instance ["MinMember"].ToString ());
			group.MaxMember = int.Parse (instance ["MaxMember"].ToString ());
			group.TimeRespawnMember = float.Parse (instance ["TimeRespawnMember"].ToString ());
			m_ListGroupData.Add (group.GameType, group);
		}
	}

	private void LoadResource (List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var resource = new TDCResourceData ();
			resource.ID = int.Parse (instance ["ID"].ToString ());
			resource.Name = instance ["Name"].ToString ();
			resource.Description = instance ["Description"].ToString ();
			resource.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			resource.FSMPath = instance["FSMPath"].ToString ();
			resource.Icon = instance["IconPath"].ToString();
			resource.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			resource.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance["CreatureType"].ToString());
			resource.CurrentHP = int.Parse (instance["CurrentHP"].ToString());
			resource.MaxHP = int.Parse (instance["MaxHP"].ToString());
			m_ListCreatureData.Add (resource.GameType, resource);
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
			player.MinDamage = int.Parse (instance["MinDamage"].ToString());
			player.MaxDamage = int.Parse (instance["MaxDamage"].ToString());
			player.RunSpeed = float.Parse (instance["RunSpeed"].ToString());
			player.WalkSpeed = float.Parse (instance["WalkSpeed"].ToString());
			player.AttackRange = float.Parse (instance["AttackRange"].ToString());
			player.Level = int.Parse (instance["Level"].ToString());
			var inventory = instance["Inventory"] as List<object>;
			for (int x = 0; x < inventory.Count; x++) {
				var invenData = inventory[x] as Dictionary<string, object>;
				var gameType = (TDCEnum.EGameType)int.Parse (invenData["GameType"].ToString());
				var amount = int.Parse (invenData["Amount"].ToString());
				var itemType = m_ListItemData [gameType].ItemType;
				TDCItemData item = null;
				switch (itemType) {
				case TDCEnum.EItemType.Food:
					item = GetFood (gameType);
					break;
				case TDCEnum.EItemType.Weapon:
					item = GetWeapon (gameType);
					break;
				}
				player.Inventory[x] = item;
				player.Inventory[x].Amount = amount;
			}
			m_ListCreatureData.Add (player.GameType, player);
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
			m_ListCreatureData.Add (gObject.GameType, gObject);
		}
	}

	public TDCCreatureData GetCreature(TDCEnum.EGameType creature) {
		var creatureData = TDCCreatureData.Clone (m_ListCreatureData [creature]);
		return creatureData;
	}

	public TDCFoodData GetFood (TDCEnum.EGameType food) {
		var foodData = TDCFoodData.Clone (m_ListItemData[food] as TDCFoodData);
		foodData.Owner = null;
		return foodData;
	}

	public TDCGroupData GetGroup(TDCEnum.EGameType group) {
		var groupData = TDCGroupData.Clone (m_ListGroupData[group]);
		return groupData;
	}

	public TDCPlayerData GetPlayer(TDCEnum.EGameType player) {
		var playerData = TDCPlayerData.Clone (m_ListCreatureData[player] as TDCPlayerData);
		return playerData;
	}

	public TDCWeaponData GetWeapon(TDCEnum.EGameType weapon) {
		var weapondata = TDCWeaponData.Clone (m_ListItemData[weapon] as TDCWeaponData);
		weapondata.Owner = null;
		return weapondata;
	}

	public TDCResourceData GetResource(TDCEnum.EGameType resource) {
		var resourceData = TDCResourceData.Clone (m_ListCreatureData[resource] as TDCResourceData);
		return resourceData;
	}

	public TDCGObjectData GetGObject(TDCEnum.EGameType gObject) {
		var objectData = TDCGObjectData.Clone (m_ListCreatureData[gObject] as TDCGObjectData);
		return objectData;
	}

	#endregion

	#region Ultilities

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










