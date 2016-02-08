
public class TDCEnum{

	public enum EGameType : int {
		None 		= 0,
		Sun			= 1,

		// Creature
		Dodono 		= 5,
		Satla 		= 6,
		Taurot 		= 7,
		Vulbat 		= 8,
		Crabystal	= 9,
		FireBuggy	= 10,
		Wobell		= 11,

		// Enviroment
		Mushroom	= 52,
		Grass		= 53,
		Bush		= 54,
		BlueBerry	= 55,
		Crystal		= 56,
		LetoTree	= 57,
		Rock		= 58,

		// Weapon
		Trap		= 71,

		// Item
		ItemMeat 		= 101,
		ItemMushroom	= 102,
		ItemGrass 		= 103,
		ItemBush 		= 104,
		ItemBlueBerry	= 105,
		ItemCrystal		= 106,
		ItemLog			= 107,
		ItemCampfire	= 108,
		ItemRock		= 109,
		ItemBigCampfire	= 110,

		// Group
		GroupDodono	= 201,
		GroupSatla	= 202,
		GroupGrass	= 204,
		GroupMushRoom = 205,
		GroupBush 	= 206,
		GroupBlueBerry = 207,
		GroupTaurot = 208,
		GroupVulbat = 209,
		GroupCrystal = 210,
		GroupCrabystal = 211,
		GroupFireBuggy = 212,
		GroupLetoTree = 213,
		GroupRock 	= 214,
		GroupWobell 	= 215,

		// Object
		Campfire	= 301,
		BigCampfire	= 302,

		// Skill
		FlameBodySkill   	= 501,
		NormalRangeSkill 	= 502,
		NormalMeleeSkill 	= 503,
		LavaSpotSkill 		= 504,
		FreezeSkill     	= 505,
		LifeNotEasySkill 	= 506,
		BurnObjectSkill  	= 507,
		AfraidTheDarkSkill 	= 508,
		EyeOfNightSkill 	= 509,
		DarkSwallowSkill 	= 510,
		ColdAsIceSkill 		= 511,

		// Weather
		WeatherNormalSkill 	= 601,
		WeatherRainySkill 	= 602,
		WeatherOverHeatSkill = 603,
		WeatherWindySkill 	= 604,
		WeatherSnowySkill 	= 605,

		// Egg
		EggDodono = 801,

		// Map
		SeasonGrassLand = 1001,

		// Plane
		GrassLandPlane	= 1101,
	}

	public enum ESkillType : int {
		None 		= 0,
		Passive 	= 1,
		Active 		= 2
	}

	public enum EItemType : int {
		None 		= 0,
		Food 		= 1,
		Weapon 		= 2,
		Item		= 3,
		GObject		= 4
	}

	public enum ECreatureType : int {
		None		= 0,
		GroundCreature	= 1,
		GroundPlayer	= 2,
		Enviroment	= 3,
		GObject		= 4,
		FlyCreature	= 5,
		FlyPlayer	= 6,
		Item		= 7
	}

	public enum EGroupType : int {
		None			= 0,
		GroupCreature	= 1,
		GroupNestCreature = 2
	}

	public enum EGroupSpawnType : int {
		None			= 0,
		Random			= 1,
		Center			= 2
	}

	public enum ELayer: int {
		None 			= 0,
		LayerCreature 	= 8,
		LayerPlane 		= 9,
		LayerEnviroment = 10,
		LayerGObject	= 11,
		LayerItem		= 12,
		LayerPlayer		= 31
	}

	public enum EGameSeason : byte {
		Spring = 0,
		Summer = 1,
		Autumn = 2,
		Winter = 3
	}

	public enum ECraftingTab : byte {
		None = 0,
		Survival = 1,
		Weapon = 2,
		Food = 3,
		Other = 10
	}

}
