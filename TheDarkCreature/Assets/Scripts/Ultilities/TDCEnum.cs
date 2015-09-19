
public class TDCEnum{

	public enum EGameType : int {
		None 		= 0,
		// Player
		PlayerDodono	= 1,
		PlayerSatla		= 2,
		PlayerBob		= 3,

		// Creature
		Creature	= 4,
		Dodono 		= 5,
		Satla 		= 6,
		Bob 		= 7,

		// Food
		Food 		= 50,
		Meat 		= 51,

		// Weapon
		Weapon		= 70,
		Trap		= 71,

		// Group
		Group 		= 200,
		GroupDodono	= 201,
		GroupSatla	= 202,
		GroupBob	= 203,
		GroupGrass	= 204,

		// Enviroment
		Enviroment  = 250,
		EnviromentGrass = 251
	}

	public enum EItemType : int {
		None 		= 0,
		Food 		= 1,
		Weapon 		= 2,
		Resource	= 3
	}

	public enum ECreatureType : int {
		None		= 0,
		Creature	= 1,
		Player 		= 2,
		Resource	= 3
	}

	public enum EGroupType : int {
		None		= 0,
		GroupCreature	= 1
	}

	public enum ELayer: int {
		None = 0,
		LayerPlane = 9,
		LayerCreature = 8,
		LayerEnviroment = 10
	}
}
