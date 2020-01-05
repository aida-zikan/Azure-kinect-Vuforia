using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// Protractor
    /// </summary>
    [AddComponentMenu("WirePrimitives/Protractor")]
    public class WireProtractor : WirePrimitiveBase
    {
        /// <summary>
        /// Direction of ticks 
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// Ticks points inside circle
            /// </summary>
            In,
            /// <summary>
            /// Ticks points outside circle
            /// </summary>
            Out
        }

        private const int TickLimit = 32000;//Because a mesh may not have more than 65000 vertices. 65000 / 2 ~ 32000 (two vertices for each tick)

        /// <summary>
        /// Direction of ticks 
        /// </summary>
        [SerializeField]
        private Direction direction;

        /// <summary>
        /// Protractor radius (base arc radius)
        /// </summary>
        [SerializeField]
        private float radius;

        /// <summary>
        /// Start angle of arc in degrees (0 degrees at 3 o'clock)
        /// </summary>
        [SerializeField]
        private float startAngle;

        /// <summary>
        /// End angle of arc in degrees
        /// </summary>
        [SerializeField]
        private float endAngle;

        /// <summary>
        /// Ticks step in degrees
        /// </summary>
        [SerializeField]
        private float tickStep;

        /// <summary>
        /// Length of ordinary ticks
        /// </summary>
        [SerializeField]
        private float tickSize;

        /// <summary>
        /// Length of mid size ticks
        /// </summary>
        [SerializeField]
        private float midsTickSize;

        /// <summary>
        /// Number of steps between mid ticks
        /// </summary>
        [SerializeField]
        private int midsTickPerSteps;

        /// <summary>
        /// Length of big ticks
        /// </summary>
        [SerializeField]
        private float bigTickSize;

        /// <summary>
        /// Number of steps between big ticks
        /// </summary>
        [SerializeField]
        private int bigTickPerSteps;

        /// <summary>
        /// Does we need to draw arc for protractor
        /// </summary>
        [SerializeField]
        private bool baseArc;

        /// <summary>
        /// Distance from arc to ticks
        /// </summary>
        [SerializeField]
        private float baseArcGap;

        /// <summary>
        /// Count of full steps in sector angle
        /// </summary>
        private int spanCount;

        private int tickCount;

        /// <summary>
        /// Callback that is called after protractor rebuilding
        /// </summary>
        public Action OnRulerBuld { get; set; }

        /// <summary>
        /// Direction of ticks 
        /// </summary>
        protected Direction ProtractorDirection
        {
            get { return direction; }
            set
            {
                if (direction != value)
                {
                    direction = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Protractor radius (base arc radius)
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set
            {
                if (radius != value)
                {
                    radius = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Start angle of arc in degrees (0 degrees at 3 o'clock)
        /// </summary>
        public float StartAngle
        {
            get { return startAngle; }
            set
            {
                if (startAngle != value && value <= endAngle && (int)((endAngle - value) / tickStep) < TickLimit)
                {
                    startAngle = value;
                    CustomStart();
                    Build();
                }
            }
        }

        /// <summary>
        /// End angle of arc in degrees
        /// </summary>
        public float EndAngle
        {
            get { return endAngle; }
            set
            {
                if (endAngle != value && startAngle <= value && (int)((value - startAngle) / tickStep) < TickLimit)
                {
                    endAngle = value;
                    CustomStart();
                    Build();
                }
            }
        }

        /// <summary>
        /// Ticks step in degrees
        /// </summary>
        public float TickStep
        {
            get { return tickStep; }
            set
            {
                if (tickStep != value && (int)(FullAngle / value) < TickLimit)
                {
                    tickStep = value;
                    CustomStart();
                    Build();
                }
            }
        }

        /// <summary>
        /// Length of short ticks
        /// </summary>
        public float TickSize
        {
            get { return tickSize; }
            set
            {
                if (tickSize != value)
                {
                    tickSize = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Length of mid size ticks
        /// </summary>
        public float MidsTickSize
        {
            get { return midsTickSize; }
            set
            {
                if (midsTickSize != value)
                {
                    midsTickSize = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Number of steps between mid ticks
        /// </summary>
        public int MidsTickPerSteps
        {
            get { return midsTickPerSteps; }
            set
            {
                if (midsTickPerSteps != value)
                {
                    midsTickPerSteps = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Length of big ticks
        /// </summary>
        public float BigTickSize
        {
            get { return bigTickSize; }
            set
            {
                if (bigTickSize != value)
                {
                    bigTickSize = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Number of steps between big ticks
        /// </summary>
        public int BigTickPerSteps
        {
            get { return bigTickPerSteps; }
            set
            {
                if (bigTickPerSteps != value)
                {
                    bigTickPerSteps = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Does we need to draw arc for protractor
        /// </summary>
        public bool BaseArc
        {
            get { return baseArc; }
            set
            {
                if (baseArc != value)
                {
                    baseArc = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Distance from arc to ticks
        /// </summary>
        public float BaseLineGap
        {
            get { return baseArcGap; }
            set
            {
                if (baseArcGap != value)
                {
                    baseArcGap = value;
                    Build();
                }
            }
        }

        public float FullAngle
        {
            get
            {
                float fullAngle = endAngle - startAngle;
                if (360 < fullAngle)
                {
                    endAngle = startAngle + 360;
                    fullAngle = 360;
                }
                return fullAngle;
            }
        }
        
        protected override void CustomStart()
        {
            spanCount = (int)(FullAngle / tickStep);
            int vertexCount = (spanCount + 1) * 2;
            if (baseArc)
            {
                vertexCount = (spanCount + 1) * 2 + BaseArcVertexCount();
            }
            else
            {
                vertexCount = (spanCount + 1) * 2;
            }

            vertices = new Vector3[vertexCount];
            indices = new int[vertexCount * 2];
            for (int i = 0; i < vertexCount; i += 2)
            {
                indices[i] = i;
                indices[i + 1] = i + 1;
            }

            //if (generateUV)
            //{
            //    uv = new Vector2[vertexCount];
            //}
        }


        [ContextMenu("Buld")]
        protected override void Build()
        {
            if (tickStep <= 0 || FullAngle <= 0) return;

            int directionSign = 1;
            switch (direction)
            {
                case Direction.In:
                    directionSign = -1;
                    break;
            }            

            List<int> ticks = new List<int>();
            List<int> midsTicks = new List<int>();
            List<int> bigTicks = new List<int>();

            spanCount = (int)(FullAngle / tickStep);
            tickCount = spanCount + 1;
            int vertexIndex = 0;
            float startAngleRad = Mathf.Deg2Rad * startAngle;
            float tickStepRad = Mathf.Deg2Rad * tickStep;
            float angle;
            for (int i = 0; i < tickCount; i++)
            {
                angle = startAngleRad + tickStepRad * i;
                Vector2 unitCircle = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                vertices[vertexIndex] = unitCircle * (radius + directionSign * baseArcGap);

                if (bigTickPerSteps != 0 && i % bigTickPerSteps == 0)
                {
                    vertices[vertexIndex + 1] = unitCircle * (radius + directionSign * (baseArcGap + bigTickSize));
                    bigTicks.Add(vertexIndex);
                    bigTicks.Add(vertexIndex + 1);
                }
                else if (midsTickPerSteps != 0 && i % midsTickPerSteps == 0)
                {
                    vertices[vertexIndex + 1] = unitCircle * (radius + directionSign * (baseArcGap + midsTickSize));
                    midsTicks.Add(vertexIndex);
                    midsTicks.Add(vertexIndex + 1);
                }
                else
                {
                    vertices[vertexIndex + 1] = unitCircle * (radius + directionSign * (baseArcGap + tickSize));
                    ticks.Add(vertexIndex);
                    ticks.Add(vertexIndex + 1);
                }

                if (generateUV)
                {
                    uv[vertexIndex] = Vector2.zero;
                    uv[vertexIndex + 1] = new Vector2(1, 0);
                }

                vertexIndex += 2;
            }

            if (baseArc)
            {
                int arcVertexCount = BaseArcVertexCount();
                float angleStep = (Mathf.Deg2Rad * FullAngle) / (arcVertexCount - 1);
                for (int i = 0; i < arcVertexCount; i++)
                {
                    angle = startAngleRad + angleStep * i;
                    vertices[vertexIndex] = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));

                    if (i != 0)
                    {
                        ticks.Add(vertexIndex);
                        ticks.Add(vertexIndex - 1);
                    }
                    vertexIndex++;
                }
            }

            mesh.Clear();
            mesh.subMeshCount = 3;
            mesh.vertices = vertices;
            if (generateUV)
            {
                mesh.uv = uv;
            }
            else
            {
                mesh.uv = null;
            }

            ///protractor mesh consist of 3 submeshes for different materials for different size ticks
            mesh.SetIndices(ticks.ToArray(), MeshTopology.Lines, 0);
            mesh.SetIndices(midsTicks.ToArray(), MeshTopology.Lines, 1);
            mesh.SetIndices(bigTicks.ToArray(), MeshTopology.Lines, 2);
            mesh.RecalculateBounds();

            if (OnRulerBuld != null)
            {
                OnRulerBuld();
            }
        }

        private int BaseArcVertexCount()
        {
            return (int)(FullAngle / 3) + 2;
        }

        private Vector3 CreateXYVertex(float x, float y)
        {
            return new Vector3(x, y, 0);
        }

        private Vector3 CreateYXVertex(float x, float y)
        {
            return new Vector3(y, x, 0);
        }
    }
}