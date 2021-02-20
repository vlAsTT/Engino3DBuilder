using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    /// <summary>
    /// Responsible for all the operations with Grid creation
    /// </summary>
    public class GridCreator : MonoBehaviour
    {
        #region Variables

        /// <summary>
        /// A default object that serves as a base for the grid
        /// </summary>
        [Tooltip("A default object that serves as a base for the grid")][SerializeField] private GridBaseObject defaultGridBaseObject;
        /// <summary>
        /// Reference to the Input Field of X Grid
        /// </summary>
        [Space(10)]
        [Header("UI")]
        [Tooltip("Reference to the Input Field of X Grid")] [SerializeField] private TMP_InputField gridXInputField;
        /// <summary>
        /// Reference to the Input Field of X Grid
        /// </summary>
        [Tooltip("Reference to the Input Field of X Grid")] [SerializeField] private TMP_InputField gridYInputField;
        /// <summary>
        /// Reference to the Apply Button
        /// </summary>
        [Tooltip("Reference to the Apply Button")] [SerializeField] private Button applyButton;
        /// <summary>
        /// Reference to the UI Notification Text Element
        /// </summary>
        [Tooltip("Reference to the UI Notification Text Element")] [SerializeField] private TextMeshProUGUI notificationText;

        /// <summary>
        /// Duration of the notification pop-up
        /// </summary>
        [Space(10)] 
        [Tooltip("Duration of the notification pop-up")] [Range(1f, 5f)] [SerializeField] private float notificationDuration = 1.5f;

        /// <summary>
        /// Reference to the parent object of grid objects
        /// </summary>
        [Header("Spawner")] 
        [Tooltip("Reference to the parent object of grid objects")] [SerializeField] private Transform parentReference;
        
        #endregion

        #region Events

        /// <summary>
        /// The method that initializes the call to start building the grid, called from UI
        /// </summary>
        public void StartBuildingGrid()
        {
            if (gridXInputField.text.Equals("") || gridYInputField.text.Equals("") || int.Parse(gridXInputField.text) <= 0 || int.Parse(gridYInputField.text) <= 0)
            {
                NotifyUser(false);
            }
            else
            {
                BuildGrid();
                NotifyUser(true);
            }
        }

        #endregion

        #region Functions

        #region Grid

        /// <summary>
        /// Builds the grid of the user's choice
        /// </summary>
        private void BuildGrid()
        {
            ClearGrid();
            
            Vector3 WorldRoot = new Vector3(0, 0, 0);
            int gridX = int.Parse(gridXInputField.text), gridY = int.Parse(gridYInputField.text);

            // Special case - x & y == 1
            if (gridX == 1 && gridY == 1)
            {
                Instantiate(defaultGridBaseObject, WorldRoot, Quaternion.identity, parentReference);
                return;
            }
 
            // Calculate spawn position for first block
            float xStart = -((gridX - 1) / 2) * (float)defaultGridBaseObject.GetSideSize(),
            yStart = -((gridY - 1) / 2) * (float)defaultGridBaseObject.GetSideSize();

            // Spawn grid blocks
            for (var horizontal = 0; horizontal < gridX; horizontal++)
            {
                for (var vertical = 0; vertical < gridY; vertical++)
                {
                    Instantiate(defaultGridBaseObject, new Vector3
                        (xStart + defaultGridBaseObject.GetSideSize() * (float)horizontal,0f,
                            yStart + defaultGridBaseObject.GetSideSize() * (float)vertical), 
                        Quaternion.identity,
                        parentReference);
                }
            }
        }

        /// <summary>
        /// Destroys current grid
        /// </summary>
        private void ClearGrid()
        {
            for (var i = 0; i < parentReference.childCount; i++)
            {
                Destroy(parentReference.GetChild(i).gameObject);
            }
        }
        
        #endregion

        #region UI Notification

        /// <summary>
        /// Sets notification of the grid build status succession depending on the flag
        /// </summary>
        /// <param name="isSuccessful">Flag that indicates if the build process was successful</param>
        private void NotifyUser(bool isSuccessful)
        {
            notificationText.text = isSuccessful ? "Grid created" : "Enter correct data";
            notificationText.color = isSuccessful ? Color.green : Color.red;
            
            // Disable button to prevent unexpected behavior
            applyButton.enabled = false;

            StartCoroutine(ShowNotification());
        }

        /// <summary>
        /// Enables Notification UI Text and the button after certain delay
        /// </summary>
        /// <returns>Coroutine IEnumerator</returns>
        private IEnumerator ShowNotification()
        {
            yield return new WaitForSeconds(notificationDuration);
            notificationText.text = "";
            applyButton.enabled = true;
        }

        #endregion

        #endregion
    }
}

