using UnityEngine;
using UnityEngine.Serialization;

namespace CameraController
{
    /// <summary>
    /// Responsible for Camera Switch, Zoom & Centralize
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region Variables

        /// <summary>
        /// Reference to the 2D Camera
        /// </summary>
        [Header("Cameras")]
        [Tooltip("Reference to the 2D Camera")][SerializeField] private Camera Camera2D;
        /// <summary>
        /// Reference to the Isometric Camera
        /// </summary>
        [Tooltip("Reference to the Isometric Camera")][SerializeField] private Camera CameraIsometric;

        /// <summary>
        /// Speed of Camera Zoom
        /// </summary>
        [Space(10)] 
        [Tooltip("Speed of zoom")] [SerializeField][Range(0f, 100f)] private float zoomSpeed = 10f;
        /// <summary>
        /// Speed of Camera Movement
        /// </summary>
        [Tooltip("Speed of Camera Movement")] [SerializeField][Range(0f, 100f)] private float movementSpeed = 10f;

        /// <summary>
        /// Max Possible Zoom Limit for Isometric Camera
        /// </summary>
        [FormerlySerializedAs("IsometricCameraLimit")] [Tooltip("Max Possible Zoom Limit for Isometric Camera")] [SerializeField] [Range(60, 120)] private int isometricCameraLimit = 100;

        /// <summary>
        /// Indicates if the program should start with 2D Camera as default
        /// </summary>
        [FormerlySerializedAs("StartWith2DCameraByDefault")]
        [Space(10)] 
        [SerializeField] private bool startWith2DCameraByDefault = true;
        /// <summary>
        /// Indicates what camera is currently enabled
        /// </summary>
        private bool _isIsometricCameraEnabled;

        /// <summary>
        /// Original position of 2D camera
        /// </summary>
        private Vector3 _originCamera2D;
        /// <summary>
        /// Original position of Isometric Camera
        /// </summary>
        private Vector3 _originCameraIsometric;
        
        #endregion

        #region Functions

        /// <summary>
        /// Enables selected camera by default
        /// </summary>
        private void Start()
        {
            // Extra enable, in case if it was turned off in the editor
            Camera2D.gameObject.SetActive(true);
            _originCamera2D = Camera2D.transform.position;
            
            CameraIsometric.gameObject.SetActive(true);
            _originCameraIsometric = CameraIsometric.transform.position;
            
            if (startWith2DCameraByDefault)
            {
                Camera2D.gameObject.SetActive(true);
                CameraIsometric.gameObject.SetActive(false);

                _isIsometricCameraEnabled = false;
            }
            else
            {
                Camera2D.gameObject.SetActive(false);
                CameraIsometric.gameObject.SetActive(true);

                _isIsometricCameraEnabled = true;
            }
        }

        /// <summary>
        /// Responsible for Camera Zoom
        /// </summary>
        private void Update()
        {
            // Isometric Camera needs to be checked for the top limit, while 2D Camera's FOV is limited to 179
            if (_isIsometricCameraEnabled)
            {
                CameraIsometric.orthographicSize = Mathf.Clamp(CameraIsometric.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 1f, isometricCameraLimit);
            }
            else
            {
                Camera2D.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }
        }

        /// <summary>
        /// Responsible for Camera Movement
        /// </summary>
        private void FixedUpdate()
        {
            if (_isIsometricCameraEnabled)
            {
                var position = CameraIsometric.transform.position;
                
                position = new Vector3
                (position.x + Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime,
                    position.y,
                    position.z +
                    Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);
                CameraIsometric.transform.position = position;
            }
            else
            {
                var position = Camera2D.transform.position;
                
                position = new Vector3
                (position.x + Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime,
                    CameraIsometric.transform.position.y,
                    position.z + Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime);
                Camera2D.transform.position = position;
            }

        }

        /// <summary>
        /// Changes Camera Perspective between 2D and Isometric
        /// </summary>
        public void ChangeCamera()
        {
            if (_isIsometricCameraEnabled)
            {
                Camera2D.gameObject.SetActive(true);
                CameraIsometric.gameObject.SetActive(false);
            }
            else
            {
                Camera2D.gameObject.SetActive(false);
                CameraIsometric.gameObject.SetActive(true);
            }
            
            _isIsometricCameraEnabled = !_isIsometricCameraEnabled;
        }

        /// <summary>
        /// Centralizes camera to have an original transform values
        /// </summary>
        public void CentralizeCamera()
        {
            if (_isIsometricCameraEnabled)
            {
                var transformIsometric = CameraIsometric.transform;

                transformIsometric.position = _originCameraIsometric;
                transformIsometric.LookAt(Vector3.zero);
            }
            else
            {
                var transform2D = Camera2D.transform;

                transform2D.position = new Vector3(_originCamera2D.x, transform2D.position.y, _originCamera2D.z);
                transform2D.LookAt(Vector3.zero);
            }
        }

        #endregion
    }   
}
