using UnityEngine;

namespace Grid
{
    /// <summary>
    /// Default Object of the Grid
    /// Can be expanded in future to include different shapes, etc.
    /// </summary>
    public class GridBaseObject : MonoBehaviour
    {
        #region Variables

        /// <summary>
        /// Size of one of the sides of the Grid Base Object
        /// </summary>
        private const int sideSize = 5;

        #endregion

        #region Functions
        
        /// <summary>
        /// Initializes the object
        /// </summary>
        void Start()
        {
            // Make sure that scale of the grid object is known and static
            transform.localScale = new Vector3(sideSize, sideSize, sideSize);
            
            // Assign name to the grid object
            name = "(" + transform.position.x + "," + transform.position.z + ")";
        }

        /// <summary>
        /// Getter for <see cref="sideSize"/>
        /// </summary>
        /// <returns>Side Size of the Object</returns>
        public int GetSideSize()
        {
            return sideSize;
        }

        #endregion
    }
}
