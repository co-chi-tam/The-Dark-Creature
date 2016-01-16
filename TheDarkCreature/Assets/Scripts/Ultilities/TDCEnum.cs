
public class TDCEnum{

	public enum EGameType : int {
		None 		= 0,

		// Creature
		Dodono 		= 5,
		Satla 		= 6,
		Taurot 		= 7,
		Vulbat 		= 8,
		Crabystal	= 9,

		// Object
		Mushroom	= 52,
		Grass		= 53,
		Bush		= 54,
		BlueBerry	= 55,
		Crystal		= 56,

		// Weapon
		Trap		= 71,

		// Item
		ItemMeat 		= 101,
		ItemMushroom	= 102,
		ItemGrass 		= 103,
		ItemBush 		= 104,
		ItemBlueBerry	= 105,
		ItemCrystal		= 106,

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

		// Object
		CampFire	= 301,

		// Skill
		FlameBodySkill   = 501,
		NormalRangeSkill = 502,
		NormalMeleeSkill = 503,
		LifeNotEasySkill = 506,
		BurnObjectSkill = 507,

		// Egg
		EggDodono = 801
	}

	public enum ESkillType : int {
		None 		= 0,
		Passive 	= 1,
		Active 		= 2,
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
		LayerPlayer		= 31
	}

	public enum EGameSeason : byte {
		Spring = 0,
		Summer = 1,
		Autumn = 2,
		Winter = 3
	}
}
