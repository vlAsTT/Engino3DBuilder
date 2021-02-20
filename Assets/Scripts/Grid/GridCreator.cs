using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    public class GridCreator : MonoBehaviour
    {
        #region Variables

        [Tooltip("A default object that serves as a base for the grid")][SerializeField] private GridBaseObject defaultGridBaseObject;
        [Space(10)]
        [Header("UI")]
        [Tooltip("Reference to the Input Field of X Grid")] [SerializeField] private TMP_InputField gridXInputField;
        [Tooltip("Reference to the Input Field of X Grid")] [SerializeField] private TMP_InputField gridYInputField;
        [Tooltip("Reference to the Apply Button")] [SerializeField] private Button applyButton;
        [Tooltip("Reference to the UI Notification Text Element")] [SerializeField] private TextMeshProUGUI notificationText;

        [Space(10)] 
        [Tooltip("Duration of the notification pop-up")] [Range(1f, 5f)] [SerializeField] private float notificationDuration = 1.5f;

        [Header("Spawner")] 
        [Tooltip("Reference to the parent object of grid objects")] [SerializeField] private Transform parentReference;
        
        #endregion

        #region Events

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

        private void ClearGrid()
        {
            for (var i = 0; i < parentReference.childCount; i++)
            {
                Destroy(parentReference.GetChild(i).gameObject);
            }
        }
        
        #endregion

        #region UI Notification

        private void NotifyUser(bool isSuccessful)
        {
            notificationText.text = isSuccessful ? "Grid created" : "Enter correct data";
            notificationText.color = isSuccessful ? Color.green : Color.red;
            
            // Disable button to prevent unexpected behavior
            applyButton.enabled = false;

            StartCoroutine(ShowNotification());
        }

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

