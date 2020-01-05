using UnityEngine;
using System.Collections;
using WirePrimitives;

namespace WirePrimitives
{
    /// <summary>
    /// Sample script to show how use different primitives as selecion highlight
    /// </summary>
    public class BoxSelection : MonoBehaviour
    {
        public enum SelectionType
        {
            Box,
            CornerBox,
            DashedBox,
            Circle
        }

        /// <summary>
        /// Primitives used as highlight
        /// </summary>
        [SerializeField]
        private WireCornerBox cornerBox;
        [SerializeField]
        private WireBox box;
        [SerializeField]
        private WireBox dashedBox;
        [SerializeField]
        private WireCircle circle;
        [SerializeField]
        private WireChamferRectangle chamferRect;

        [SerializeField]
        private SelectionType selectionType;

        private Ray ray;
        private Collider prevCollider;

        public SelectionType Type
        {
            get { return selectionType; }
            set
            {
                selectionType = value;

                if (prevCollider != null)
                {
                    if (selectionType == SelectionType.Box)
                    {
                        DrawBoxSelection(prevCollider);
                    }
                    else if (selectionType == SelectionType.CornerBox)
                    {
                        DrawCornersSelection(prevCollider);
                    }
                    else if (selectionType == SelectionType.DashedBox)
                    {
                        DrawDashedBoxSelection(prevCollider);
                    }
                    else if (selectionType == SelectionType.Circle)
                    {
                        DrawCircleSelection(prevCollider);
                    }
                }
            }
        }

        void Update()
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider != prevCollider)
                {
                    prevCollider = hit.collider;
                    if (selectionType == SelectionType.Box)
                    {
                        DrawBoxSelection(hit.collider);
                    }
                    else if (selectionType == SelectionType.CornerBox)
                    {
                        DrawCornersSelection(hit.collider);
                    }
                    else if (selectionType == SelectionType.DashedBox)
                    {
                        DrawDashedBoxSelection(hit.collider);
                    }
                    else if (selectionType == SelectionType.Circle)
                    {
                        DrawCircleSelection(hit.collider);
                    }
                }
            }
        }

        /// <summary>
        /// Set position and size of box highlight for selected Collider
        /// </summary>
        /// <param name="collider">Collider</param>
        private void DrawBoxSelection(Collider collider)
        {
            box.gameObject.SetActive(true);
            cornerBox.gameObject.SetActive(false);
            dashedBox.gameObject.SetActive(false);
            circle.gameObject.SetActive(false);

            box.SetSize(collider.bounds.size);
            box.transform.position = collider.bounds.center;
        }

        /// <summary>
        /// Set position and size of corner box highlight for selected Collider
        /// </summary>
        /// <param name="collider">Collider</param>
        private void DrawCornersSelection(Collider collider)
        {
            box.gameObject.SetActive(false);
            cornerBox.gameObject.SetActive(true);
            dashedBox.gameObject.SetActive(false);
            circle.gameObject.SetActive(false);

            cornerBox.SetSize(collider.bounds.size);
            cornerBox.transform.position = collider.bounds.center;
        }

        private void DrawDashedBoxSelection(Collider collider)
        {
            box.gameObject.SetActive(false);
            cornerBox.gameObject.SetActive(false);
            dashedBox.gameObject.SetActive(true);
            circle.gameObject.SetActive(false);

            dashedBox.SetSize(collider.bounds.size);
            dashedBox.transform.position = collider.bounds.center;
        }

        private void DrawCircleSelection(Collider collider)
        {
            box.gameObject.SetActive(false);
            cornerBox.gameObject.SetActive(false);
            dashedBox.gameObject.SetActive(false);
            circle.gameObject.SetActive(true);

            Vector2 xyExtents = new Vector2(collider.bounds.extents.x, collider.bounds.extents.z);
            circle.Radius = xyExtents.magnitude;
            Vector3 pos = collider.bounds.center;
            pos.y = collider.bounds.min.y;
            circle.transform.position = pos;
        }

        private void DrawChamferRectSelection(Collider collider)
        {
            box.gameObject.SetActive(false);
            cornerBox.gameObject.SetActive(false);
            dashedBox.gameObject.SetActive(false);
            circle.gameObject.SetActive(true);

            Rect rect = GUIRectWithObject(collider.bounds);
            chamferRect.SizeX = rect.width;
            chamferRect.SizeY = rect.height;
            circle.transform.position = rect.center;
        }

        public static Rect GUIRectWithObject(Bounds bb)
        {
            Vector3 cen = bb.center;
            Vector3 ext = bb.extents;
            Vector2[] extentPoints = new Vector2[8]
             {
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
               WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
               WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
             };
            Vector2 min = extentPoints[0];
            Vector2 max = extentPoints[0];
            foreach (Vector2 v in extentPoints)
            {
                min = Vector2.Min(min, v);
                max = Vector2.Max(max, v);
            }
            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }

        public static Vector2 WorldToGUIPoint(Vector3 world)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(world);
            screenPoint.y = (float)Screen.height - screenPoint.y;
            return screenPoint;
        }
    }
}
