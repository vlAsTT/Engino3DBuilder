using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridBaseObject : MonoBehaviour
    {
        #region Variables

        private const int sideSize = 5;

        #endregion

        #region Functions
        
        void Start()
        {
            // Make sure that scale of the grid object is known and static
            transform.localScale = new Vector3(sideSize, sideSize, sideSize);
            
            // Assign name to the grid object
            name = "(" + transform.position.x + "," + transform.position.z + ")";
        }

        public int GetSideSize()
        {
            return sideSize;
        }

        #endregion
    }
}
