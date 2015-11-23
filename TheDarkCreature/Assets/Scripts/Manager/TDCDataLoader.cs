using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class TDCDataLoader {

	#region Property

	private Dictionary<TDCEnum.EGameType, TDCCreatureData> m_ListCreatureData;
	private Dictionary<TDCEnum.EGameType, TDCFoodData> m_ListFoodData;
	private Dictionary<TDCEnum.EGameType, TDCGroupData> m_ListGroupData;
	private Dictionary<TDCEnum.EGameType, TDCPlayerData> m_ListPlayerData;
	private Dictionary<TDCEnum.EGameType, TDCWeaponData> m_ListWeaponData;
	private Dictionary<TDCEnum.EGameType, TDCResourceData> m_ListResourceData;

	#endregion

	#region Contructor

	public TDCDataLoader() {
		m_ListCreatureData = new Dictionary<TDCEnum.EGameType, TDCCreatureData> ();
		m_ListFoodData = new Dictionary<TDCEnum.EGameType, TDCFoodData> ();
		m_ListGroupData = new Dictionary<TDCEnum.EGameType, TDCGroupData> ();
		m_ListPlayerData = new Dictionary<TDCEnum.EGameType, TDCPlayerData> ();
		m_ListWeaponData = new Dictionary<TDCEnum.EGameType, TDCWeaponData> ();
		m_ListResourceData = new Dictionary<TDCEnum.EGameType, TDCResourceData> ();

		LoadData ();
	}

	#endregion

	#region Main methods
	private void LoadData() {
		var creatureAsset = Resources.Load<TextAsset> ("Data/CreatureData");
		var foodAsset = Resources.Load<TextAsset> ("Data/FoodData");
		var groupAsset = Resources.Load<TextAsset> ("Data/GroupData");
		var playerAsset = Resources.Load<TextAsset> ("Data/PlayerData");
		var weaponAsset = Resources.Load<TextAsset> ("Data/WeaponData");
		var resourceTextAsset = Resources.Load<TextAsset> ("Data/ResourceData");

		var jsonCreature = Json.Deserialize (creatureAsset.text) as Dictionary<string, object>;
		var jsonfood = Json.Deserialize (foodAsset.text) as Dictionary<string, object>;
		var jsonGroup = Json.Deserialize (groupAsset.text) as Dictionary<string, object>;
		var jsonPlayer = Json.Deserialize (playerAsset.text) as Dictionary<string, object>;
		var jsonWeapon = Json.Deserialize (weaponAsset.text) as Dictionary<string, object>;
		var jsonResource = Json.Deserialize (resourceTextAsset.text) as Dictionary<string, object>;

		LoadFood (jsonfood["foods"] as List<object>);
		LoadGroup (jsonGroup["groups"] as List<object>);
		LoadWeapon (jsonWeapon["weapons"] as List<object>);
		LoadResource (jsonResource["resources"] as List<object>);
		LoadCreature (jsonCreature["creatures"] as List<object>);
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
			food.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			food.Icon = instance["IconPath"].ToString();
			food.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			food.ItemType = (TDCEnum.EItemType)int.Parse (instance ["ItemType"].ToString ());
			food.RecoverPoint = int.Parse (instance ["RecoverPoint"].ToString ());
			m_ListFoodData.Add (food.GameType, food);
		}
	}

	private void LoadWeapon(List<object> values) {
		for (int i = 0; i < values.Count; i++) {
			var instance = values [i] as Dictionary<string, object>;
			var weapon = new TDCWeaponData ();
			weapon.ID = int.Parse (instance ["ID"].ToString ());
			weapon.Name = instance ["Name"].ToString ();
			weapon.Description = instance ["Description"].ToString ();
			weapon.ModelPath = ConvertTo<string> (instance["ModelPath"] as List<object>);
			weapon.FSMPath = instance["FSMPath"].ToString ();
			weapon.Icon = instance["IconPath"].ToString();
			weapon.GameType = (TDCEnum.EGameType)int.Parse (instance ["GameType"].ToString ());
			weapon.ItemType = (TDCEnum.EItemType)int.Parse (instance ["ItemType"].ToString ());
			weapon.MinDamage = int.Parse (instance ["MinDamage"].ToString ());
			weapon.MaxDamage = int.Parse (instance ["MaxDamage"].ToString ());
			weapon.AttackRange = float.Parse (instance ["AttackRange"].ToString ());
			weapon.Duration = int.Parse (instance ["Duration"].ToString ());
			weapon.DecreaseDuration = int.Parse (instance ["DecreaseDuration"].ToString ());
			m_ListWeaponData.Add (weapon.GameType, weapon);
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
			resource.CreatureType = (TDCEnum.ECreatureType)int.Parse (instance ["CreatureType"].ToString ());
			resource.CurrentHP = int.Parse (instance["CurrentHP"].ToString());
			resource.MaxHP = int.Parse (instance["MaxHP"].ToString());
			m_ListResourceData.Add (resource.GameType, resource);
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
			player.Level = int.Parse (instance["Level"].ToString());
			var inventory = instance["Inventory"] as List<object>;
			for (int x = 0; x < inventory.Count; x++) {
				var invenData = inventory[x] as Dictionary<string, object>;
				var itemType = (TDCEnum.EItemType)int.Parse (invenData["ItemType"].ToString());
				var gameType = (TDCEnum.EGameType)int.Parse (invenData["GameType"].ToString());
				switch (itemType) {
				case TDCEnum.EItemType.Food:
					player.Inventory[x] = GetFood (gameType);
					break;
				case TDCEnum.EItemType.Weapon:
					player.Inventory[x] = GetWeapon (gameType);
					break;
				case TDCEnum.EItemType.Resource:
					player.Inventory[x] = GetResource (gameType);
					break;
				}
				player.Inventory[x].Amount = 1;
			}
			m_ListPlayerData.Add (player.GameType, player);
		}
	}

	public TDCCreatureData GetCreature(TDCEnum.EGameType creature) {
		return TDCCreatureData.Clone (m_ListCreatureData[creature]);
	}

	public TDCFoodData GetFood (TDCEnum.EGameType food) {
		var foodData = TDCFoodData.Clone (m_ListFoodData[food]);
		foodData.Amount += 1;
		return foodData;
	}

	public TDCGroupData GetGroup(TDCEnum.EGameType group) {
		return TDCGroupData.Clone (m_ListGroupData[group]);
	}

	public TDCPlayerData GetPlayer(TDCEnum.EGameType player) {
		return TDCPlayerData.Clone (m_ListPlayerData[player]);
	}

	public TDCWeaponData GetWeapon(TDCEnum.EGameType weapon) {
		return TDCWeaponData.Clone (m_ListWeaponData[weapon]);
	}

	public TDCResourceData GetResource(TDCEnum.EGameType resource) {
		return TDCResourceData.Clone (m_ListResourceData[resource]);
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










