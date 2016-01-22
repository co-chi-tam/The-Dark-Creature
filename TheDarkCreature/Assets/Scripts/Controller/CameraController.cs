using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public Transform _target;						//Target transform

    private float _rotationUpDownSpeed = 120f;
    private float _rotationLeftRightSpeed = 120f;
    private float _maxUpdownRange = 60f;
    private float _minUpdownRange = 45f;
    private float _x;
    private float _y;
	[SerializeField]
    private float _distance = 50f;
	[SerializeField]
    private float _zoom = 5f;
    private float _minZoom = 10f;
    private const float _maxZoom = 60f;
    private float _commonY = 2f;					//Y 
    private Quaternion _rotation;
    private Vector3 _position;
    private bool _isFirstSetCamra = true;

	#region Singleton
	
	public static object m_singletonObject = new object();
	public static CameraController m_Instance;
	
	public static CameraController Instance {
		get {
			lock (m_singletonObject) {
				if (m_Instance == null) {
					GameObject go = new GameObject("CameraController");
					m_Instance = go.AddComponent<CameraController>();
				}
				return m_Instance;
			}
		}
	}
	
	public static CameraController GetInstance() {
		return Instance;
	}
	
	#endregion

	void Awake() {
		m_Instance = this;
	}

    void Start()
    {
        SetupCamera(); 								//Setup camera parameter
    }

    void Update()
    {
		if (_target == null)
			return;
#if UNITY_EDITOR
        RotationAroundObjectByCamera();
//        ZoomCamera();
#endif
        MoveCamera();
    }

    void FixedUpdate()
    {
        if (_target != null && _isFirstSetCamra)
        {
            FirstSetCamera();
        }
    }

    //Setup Camera
    private void SetupCamera()
    {
        try
        {
            Vector3 angles = transform.eulerAngles;
            transform.position = new Vector3(transform.position.x, transform.position.y, -_distance);
            _x = angles.y;
            _y = angles.x;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
	
	Vector2 deltaTouch = Vector2.zero;
    //Rotation camera around Target
    private void RotationAroundObjectByCamera()
    {
        if (Input.GetMouseButton(1) && _target != null)
        {
            _x += Input.GetAxis("Mouse X") * _rotationLeftRightSpeed * 0.02f;
            _y -= Input.GetAxis("Mouse Y") * _rotationUpDownSpeed * 0.02f;

            _y = ClampAngle(_y, _minUpdownRange, _maxUpdownRange);
            _rotation = Quaternion.Euler(_y, _x, 0);
            _position = _rotation * new Vector3(0, _commonY, -_distance) + _target.position;
            transform.rotation = _rotation;
            transform.position = _position;
        }
        else if (Input.touchCount == 2) {
            deltaTouch = Input.GetTouch(0).deltaPosition;
            //for (int i = 0; i < Input.touchCount; i++) {
            //	deltaTouch = deltaTouch + Input.GetTouch(i).deltaPosition;
            //}
            //deltaTouch = deltaTouch / 2f;

            _x += deltaTouch.x * _rotationLeftRightSpeed * 0.02f;
			_y -= deltaTouch.y * _rotationUpDownSpeed * 0.02f;

            _y = ClampAngle(_y, _minUpdownRange, _maxUpdownRange);
            _rotation = Quaternion.Euler(_y, _x, 0);
            _position = _rotation * new Vector3(0, _commonY, -_distance) + _target.position;
            transform.rotation = _rotation;
            transform.position = _position;
        }
    }

    //FirstCamera
    private void FirstSetCamera()
    {
        _x += 1 * _rotationLeftRightSpeed * 0.02f;
        _y -= 1 * _rotationUpDownSpeed * 0.02f;

        _y = ClampAngle(_y, _minUpdownRange, _maxUpdownRange);
        _rotation = Quaternion.Euler(_y, _x, 0);
        _position = _rotation * new Vector3(0, _commonY, -_distance) + _target.position;
        transform.rotation = _rotation;
        transform.position = _position;

        _isFirstSetCamra = false;
    }

    private Vector2 touchPos1;
    private float touchDistance;
    //Zoom camera by Target Distance
    private void ZoomCamera()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && _target != null)
        {
            _distance = Mathf.Clamp((_distance -= _zoom), _minZoom, _maxZoom);
            transform.position = _rotation * new Vector3(0, _commonY, -_distance) + _target.position;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && _target != null)
        {
            _distance = Mathf.Clamp((_distance += _zoom), _minZoom, _maxZoom);
            transform.position = _rotation * new Vector3(0, _commonY, -_distance) + _target.position;
        }
        

        if (Input.touchCount == 1)
        {
           if (Input.GetTouch(0).phase == TouchPhase.Began) {
                touchPos1 = Input.GetTouch(0).position;
           }

        } else if (Input.touchCount == 2) {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touchPhase = Input.GetTouch(i).phase;
                switch (touchPhase)
                {
                    case TouchPhase.Began:
                        touchDistance = (touchPos1 - Input.GetTouch(1).position).sqrMagnitude;
                        break;
                    case TouchPhase.Moved:
                        var nextDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).sqrMagnitude - touchDistance;
                        if (nextDistance > 0 && _target != null)
                        {
                            _distance = Mathf.Clamp((_distance -= _zoom), _minZoom, _maxZoom);
                            transform.position = _rotation * new Vector3(0, _commonY, -_distance) + _target.position;
                        }
                        else if (nextDistance < 0 && _target != null)
                        {
                            _distance = Mathf.Clamp((_distance += _zoom), _minZoom, _maxZoom);
                            transform.position = _rotation * new Vector3(0, _commonY, -_distance) + _target.position;
                        }
                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Ended:
                        touchDistance = 0f;
                        break;
                    case TouchPhase.Canceled:
                        touchDistance = 0f;
                        break;
                }
            }
        }

    }

    //Max rotation Angle
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private void MoveCamera()
    {
		var target = _target.position;
		target.y = 0f;
        _position = _rotation * new Vector3(0.0f, _commonY, -_distance) + target;
		transform.position = Vector3.Lerp(transform.position, _position, 5);
    }

    #region Get and Set basic
    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }
    #endregion
}