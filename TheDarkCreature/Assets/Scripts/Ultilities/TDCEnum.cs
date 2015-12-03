
public class TDCEnum{

	public enum EGameType : int {
		None 		= 0,
		// Player
		PlayerDodono	= 1,
		PlayerSatla		= 2,
		PlayerBob		= 3,

		// Creature
		Dodono 		= 5,
		Satla 		= 6,
		Bob 		= 7,

		// Food
		Meat 		= 51,
		Mushroom	= 52,

		// Weapon
		Trap		= 71,

		// Group
		GroupDodono	= 201,
		GroupSatla	= 202,
		GroupBob	= 203,
		GroupGrass	= 204,
		GroupMushRoom = 205,

		// Enviroment
		EnviromentGrass = 251,
		EnviromentMushroom = 252,

		// Object
		CampFire	= 301
	}

	public enum EItemType : int {
		None 		= 0,
		Food 		= 1,
		Weapon 		= 2,
		Resource	= 3,
		GObject		= 4
	}

	public enum ECreatureType : int {
		None		= 0,
		Creature	= 1,
		Player 		= 2,
		Resource	= 3
	}

	public enum EGroupType : int {
		None			= 0,
		GroupCreature	= 1
	}

	public enum ELayer: int {
		None 			= 0,
		LayerPlane 		= 9,
		LayerCreature 	= 8,
		LayerEnviroment = 10
	}

	public enum EGameSeason : byte {
		Spring = 0,
		Summer = 1,
		Autumn = 2,
		Winter = 3
	}
}
