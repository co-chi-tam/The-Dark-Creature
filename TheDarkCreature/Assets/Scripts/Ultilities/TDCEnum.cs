
public class TDCEnum{

	public enum EGameType : int {
		None 		= 0,

		// Creature
		Dodono 		= 5,
		Satla 		= 6,

		// Object
		Meat 		= 51,
		Mushroom	= 52,
		Grass		= 53,
		Bush		= 54,
		BlueBerry	= 55,

		// Weapon
		Trap		= 71,

		// Group
		GroupDodono	= 201,
		GroupSatla	= 202,
		GroupGrass	= 204,
		GroupMushRoom = 205,
		GroupBush 	= 206,
		GroupBlueBerry = 207,

		// Object
		CampFire	= 301,

		// Skill
		FlameBodySkill   = 501,
		NormalRangeSkill = 502,
		NormalMeleeSkill = 503,
		EffectPerSecondSkill = 504,
		EffectUpdateSkill = 505
	}

	public enum ESkillType : int {
		None 		= 0,
		Passive 	= 1,
		Range 		= 2,
		Melee 		= 3,
		EffectPerSecond	= 4,
		EffectUpdate = 5
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
		Creature	= 1,
		Player 		= 2,
		Enviroment	= 3,
		GObject		= 4
	}

	public enum EGroupType : int {
		None			= 0,
		GroupCreature	= 1
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
