using UnityEngine;
using System.Collections;
using ObjectPool;

public class TDCGameManager : MonoBehaviour {

    #region Singleton

    public static object m_singletonObject = new object();
    public static TDCGameManager m_Instance;

    public static TDCGameManager Instance {
        get {
            lock (m_singletonObject) {
                if (m_Instance == null) {
                    GameObject go = new GameObject("GameManager");
                    m_Instance = go.AddComponent<TDCGameManager>();
                }
                return m_Instance;
            }
        }
    }

    public static TDCGameManager GetInstance() {
        return Instance;
    }

    public TDCGameManager()
    {
        m_Instance = this;
    }

    #endregion

    #region Properties

    private TDCObjectPool<TDCTrapController> m_TrapsPool;
	private TDCDataLoader m_DataLoader;

    #endregion

    #region Implementation Mono

    void Awake() {
		m_TrapsPool = new TDCObjectPool<TDCTrapController>();
		m_DataLoader = TDCDataLoader.GetInstance ();
		
		CreatTrapPool(10);
		m_DataLoader.Init ();
    }

	void Start() {
		CreateCreature (TDCEnum.EGameType.PlayerSatla, Vector3.zero, Quaternion.identity);

		CreateCreature (TDCEnum.EGameType.GroupDodono, Vector3.zero, Quaternion.identity);
		CreateCreature (TDCEnum.EGameType.GroupSatla, new Vector3 (10f, 0f, 20f), Quaternion.identity);

		CreateCreature (TDCEnum.EGameType.GroupGrass, new Vector3 (20f, 0f, 20f), Quaternion.identity);
	}

    #endregion

    #region Main method

	public TDCBaseController CreateCreature(TDCEnum.EGameType type, 
	                                        Vector3 position, 
	                                        Quaternion rotation, 
	                                        GameObject parent = null) {
		TDCBaseData data = null;
		GameObject gObject = null;
		TDCBaseController controller = null;
		var random = Random.Range (0, 100); 

		switch (type) { 
		case TDCEnum.EGameType.PlayerDodono:
		case TDCEnum.EGameType.PlayerSatla:
		case TDCEnum.EGameType.PlayerBob: {
			data = m_DataLoader.GetPlayer (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]));
			controller = gObject.AddComponent<TDCPlayerController> ();
			CameraController.Instance.Target = gObject.transform;
			controller.Init ();
			break;
		}
		case TDCEnum.EGameType.Dodono: 
		case TDCEnum.EGameType.Satla:
		case TDCEnum.EGameType.Bob: {
			data = m_DataLoader.GetCreature (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]));
			controller = gObject.AddComponent<TDCEasyAIController> ();
			controller.Init ();
			break;
		}
		case TDCEnum.EGameType.Food:
		case TDCEnum.EGameType.Meat: {

			break;
		}
		case TDCEnum.EGameType.Weapon:
		case TDCEnum.EGameType.Trap: {
			data = m_DataLoader.GetWeapon (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]));
			controller = gObject.AddComponent<TDCTrapController> ();
			controller.Init ();
			break;
		}
		case TDCEnum.EGameType.Group:
		case TDCEnum.EGameType.GroupDodono:
		case TDCEnum.EGameType.GroupSatla:
		case TDCEnum.EGameType.GroupBob: 
		case TDCEnum.EGameType.GroupGrass: {
			data = m_DataLoader.GetGroup (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]));
			controller = gObject.AddComponent<TDCGroupCreatureController> ();
			var groupController = controller as TDCGroupCreatureController;
			controller.SetData (data);
			controller.Init ();
			if (type == TDCEnum.EGameType.GroupDodono) {
				groupController.CreatePoolMember (TDCEnum.EGameType.Dodono);
			} else if (type == TDCEnum.EGameType.GroupSatla) {
				groupController.CreatePoolMember (TDCEnum.EGameType.Satla);
			} else if (type == TDCEnum.EGameType.GroupBob) {
				groupController.CreatePoolMember (TDCEnum.EGameType.Bob);
			} else if (type == TDCEnum.EGameType.GroupGrass) {
				groupController.CreatePoolMember (TDCEnum.EGameType.EnviromentGrass);
			}
			break;
		}
		case TDCEnum.EGameType.EnviromentGrass: {
			data = m_DataLoader.GetResource (type);
			gObject = GameObject.Instantiate (Resources.Load<GameObject> (data.ModelPath[random % data.ModelPath.Length]));
			controller = gObject.AddComponent<TDCResourceController> ();
			controller.Init ();
			break;
		}
		}
		gObject.transform.position = position;
		gObject.transform.rotation = rotation;
		controller.SetData (data);
		controller.SetCreatureType (type);
		controller.name = type.ToString ();
		if (parent != null) {
			gObject.transform.SetParent (parent.transform);		
		}
		return controller;
	}

    private void CreatTrapPool(int amout) {
        for (int i = 0; i < amout; i++)
        {
            GameObject trapObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Trap"));
            trapObject.name = string.Format("Trap {0}", i);
            var trapController = trapObject.AddComponent<TDCTrapController>();
            trapController.transform.SetParent(this.transform);
			trapController.gameObject.SetActive (false);
            m_TrapsPool.Create(trapController);
        }
    }

    public void SpawnTrap(Vector3 position) {
        var trap = m_TrapsPool.Get();
        trap.transform.position = position;
    }

    public void ReturnTrap(TDCTrapController item)
    {
		item.gameObject.SetActive (false);
        m_TrapsPool.Set(item);
    }

    #endregion
}
