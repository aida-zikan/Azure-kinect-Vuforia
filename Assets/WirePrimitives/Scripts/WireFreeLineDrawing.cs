using UnityEngine;
using System.Collections;
using System;

namespace WirePrimitives
{
    public class WireFreeLineDrawing : MonoBehaviour
    {
        [SerializeField]
        private GameObject wireLineProto;
        [SerializeField]
        private bool drawOnCollision;
        [SerializeField]
        private bool enableDrawing;

        public bool EnableDrawing
        {
            get { return enableDrawing; }
            set { enableDrawing = value; }
        }

        private WireLine wireLine;
        private float distance = 250;
        private Vector3 prevMousePosition;
        private float addVertexThreshold = 1.4f;

        private Ray ray;
        private Collider prevCollider;
        private float margin = 0.01f;

        void Update()
        {
            //{
            //    if (wireLine == null)
            //    {
            //        wireLine = CreateNewLine();
            //    }
            //    ray = Camera.main.ScreenPointToRay(new Vector3(UnityEngine.Random.Range(0, Screen.width), UnityEngine.Random.Range(0, Screen.height), 1000));
            //    var p = ray.GetPoint(distance);
            //    wireLine.AddVertex(p);
            //    return;
            //}

            if (EnableDrawing)
            {
                if (drawOnCollision)
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider != prevCollider)
                        {
                            wireLine = CreateNewLine();
                        }

                        if (wireLine != null && addVertexThreshold < Vector2.Distance(Input.mousePosition, prevMousePosition))
                        {
                            //prevCollider = hit.collider;
                            wireLine.AddVertex(ray.GetPoint(hit.distance - margin));
                            prevMousePosition = Input.mousePosition;
                        }
                        prevCollider = hit.collider;
                    }
                    else
                    {
                        prevCollider = null;
                        wireLine = null;
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (wireLine == null)
                        {
                            wireLine = CreateNewLine();
                        }
                        else if (wireLine != null && addVertexThreshold < Vector2.Distance(Input.mousePosition, prevMousePosition))
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            var p = ray.GetPoint(distance);
                            wireLine.AddVertex(p);
                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        wireLine = null;
                    }
                }
            }
            prevMousePosition = Input.mousePosition;
        }

        private WireLine CreateNewLine()
        {
            GameObject newLineGO = Instantiate(wireLineProto);
            newLineGO.transform.SetParent(this.transform);
            WireLine wl = newLineGO.GetComponent<WireLine>();
            wl.Initialize();
            wl.UseWorldSpace = true;
            return wl;
        }
    }
}
