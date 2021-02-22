using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blocks
{
    /// <summary>
    /// A base class for UI representation of the Block that contains Image of a texture
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class BlockUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region References

        /// <summary>
        /// Reference to the Canvas of a general UI
        /// </summary>
        [SerializeField] private Canvas canvas;
        /// <summary>
        /// Reference to the RectTransform component of the block
        /// </summary>
        private RectTransform _rectTransform;
        /// <summary>
        /// Reference to the Raw Image Component of the block
        /// </summary>
        private RawImage _image;

        #endregion

        #region Events

        /// <summary>
        /// Instantiates a copy of the object that was dragged
        /// </summary>
        /// <param name="eventData">Event Data</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            var obj = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
            obj.name = name;
        }

        /// <summary>
        /// Checks if the object was on top of any GridBaseObject object
        /// If yes - attaches a block to it, at the end - self-destroys
        /// </summary>
        /// <param name="eventData">Event Data</param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (Camera.main is null) return;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity))
            {
                var obj = hit.transform.GetComponent<GridBaseObject>();

                if (obj)
                {
                    obj.AttachBlock(_image.texture);
                }
            }
            
            Destroy(gameObject);
        }

        /// <summary>
        /// Moves image with mouse pointer
        /// </summary>
        /// <param name="eventData">Event Data</param>
        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Initializes references
        /// </summary>
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<RawImage>();
        }

        #endregion

    }
}
