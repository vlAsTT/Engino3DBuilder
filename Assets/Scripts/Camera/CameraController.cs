using UnityEngine;

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
        /// Speed of zoom
        /// </summary>
        [Space(10)] 
        [Tooltip("Speed of zoom")] [SerializeField] private float zoomSpeed = 10f;

        /// <summary>
        /// Max Possible Zoom Limit for Isometric Camera
        /// </summary>
        [Tooltip("Max Possible Zoom Limit for Isometric Camera")] [SerializeField] [Range(60, 120)] private int IsometricCameraLimit = 100;

        /// <summary>
        /// Indicates if the program should start with 2D Camera as default
        /// </summary>
        [Space(10)] 
        [SerializeField] private bool StartWith2DCameraByDefault = true;
        /// <summary>
        /// Indicates what camera is currently enabled
        /// </summary>
        private bool isIsometricCameraEnabled;
        
        #endregion

        #region Functions

        /// <summary>
        /// Enables selected camera by default
        /// </summary>
        private void Start()
        {
            if (StartWith2DCameraByDefault)
            {
                Camera2D.gameObject.SetActive(true);
                CameraIsometric.gameObject.SetActive(false);

                isIsometricCameraEnabled = false;
            }
            else
            {
                Camera2D.gameObject.SetActive(false);
                CameraIsometric.gameObject.SetActive(true);

                isIsometricCameraEnabled = true;
            }
        }

        /// <summary>
        /// Responsible for Camera Zoom
        /// </summary>
        private void Update()
        {
            // Isometric Camera needs to be checked for the top limit, while 2D Camera's FOV is limited to 179
            if (isIsometricCameraEnabled)
            {
                CameraIsometric.orthographicSize = Mathf.Clamp(CameraIsometric.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 1f, IsometricCameraLimit);
            }
            else
            {
                Camera2D.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            }
        }

        /// <summary>
        /// Changes Camera Perspective between 2D and Isometric
        /// </summary>
        public void ChangeCamera()
        {
            if (isIsometricCameraEnabled)
            {
                Camera2D.gameObject.SetActive(true);
                CameraIsometric.gameObject.SetActive(false);
            }
            else
            {
                Camera2D.gameObject.SetActive(false);
                CameraIsometric.gameObject.SetActive(true);
            }
            
            isIsometricCameraEnabled = !isIsometricCameraEnabled;
        }

        /// <summary>
        /// Centralizes camera to look at (0,0,0)
        /// </summary>
        public void CentralizeCamera()
        {
            if (isIsometricCameraEnabled)
            {
                CameraIsometric.transform.LookAt(Vector3.zero);
            }
            else
            {
                Camera2D.transform.LookAt(Vector3.zero);
            }
        }

        #endregion
    }   
}
