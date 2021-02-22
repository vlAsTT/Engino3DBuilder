using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blocks
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(RawImage))]
    public class BlockUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Variables

        [SerializeField] private Canvas canvas;
        private RectTransform _rectTransform;
        private RawImage _image;

        #endregion

        #region Events

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var obj = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
            obj.name = name;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            RaycastHit hit;

            if (Camera.main is null) return;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                var obj = hit.transform.GetComponent<GridBaseObject>();

                if (obj)
                {
                    obj.AttachBlock(_image.texture);
                }
            }
            
            Destroy(gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        #endregion

        #region Functions

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<RawImage>();
        }

        #endregion

    }
}
