using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// Ruler
    /// </summary>
    [AddComponentMenu("WirePrimitives/Ruler")]
    public class WireRuler : WirePrimitiveBase
    {       
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        /// <summary>
        /// Pivot
        /// </summary>
        public enum Align
        {
            start,
            middle,
            end
        }

        private const int TickLimit = 32000;//Because a mesh may not have more than 65000 vertices.

        [SerializeField]
        private Direction direction;

        /// <summary>
        /// Where ruler pivot is placed
        /// </summary>
        [Header("ルーラーピボットが配置される場所")]
        [SerializeField]
        protected Align align;

        /// <summary>
        /// Ruler length
        /// </summary>
        [Header("定規の長さ")]
        [SerializeField]
        private float rulerLength;

        /// <summary>
        /// Distance between two adjustent ticks
        /// </summary>
        [Header("2つの調整ティック間の距離")]
        [SerializeField]
        private float tickStep;

        /// <summary>
        /// Length of ordinary (short) ticks
        /// </summary>
        [Header("通常の（短い）ティックの長さ")]
        [SerializeField]
        private float tickSize;

        /// <summary>
        /// Length of mid size ticks
        /// </summary>
        [Header("中サイズのティックの長さ")]
        [SerializeField]
        private float midsTickSize;

        /// <summary>
        /// Number of steps between mid ticks
        /// </summary>
        [Header("ミッドティック間のステップ数")]
        [SerializeField]
        private int midsTickPerSteps;

        /// <summary>
        /// Length of big ticks
        /// </summary>
        [Header("ミッドティック間のステップ数")]
        [SerializeField]
        private float bigTickSize;

        /// <summary>
        /// Number of steps between big ticks
        /// </summary>
        [Header("大目盛り間のステップ数")]
        [SerializeField]
        private int bigTickPerSteps;

        /// <summary>
        /// Line from first tick to the last
        /// </summary>
        [Header("最初の目盛りから最後の目盛りまでの線")]
        [SerializeField]
        private bool baseLine;

        /// <summary>
        /// Distance from baseLine to ticks start point
        /// </summary>
        [Header("baseLineからティックの開始点までの距離")]
        [SerializeField]
        private float baseLineGap;

        private int spanCount;
        private int tickCount;

        private Func<float, float, Vector3> CreateVertex;

        /// <summary>
        /// Callback that is called after ruler rebuilding
        /// </summary>
        public Action OnRulerBuld { get; set; }

        protected Direction RulerDirection
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
        /// Where ruler pivot is placed
        /// </summary>
        public Align RulerAlign
        {
            get { return align; }
            set
            {
                if (align != value)
                {
                    align = value;
                    Build();
                }
            }
        }

        public float RulerLength
        {
            get { return rulerLength; }
            set
            {
                if (rulerLength != value && (int)(value / tickStep) < TickLimit)
                {
                    rulerLength = value;
                    CustomStart();
                    Build();
                }
            }
        }

        public float TickStep
        {
            get { return tickStep; }
            set
            {
                if (tickStep != value && (int)(rulerLength / value) < TickLimit)
                {
                    tickStep = value;
                    CustomStart();
                    Build();
                }
            }
        }

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

        public bool BaseLine
        {
            get { return baseLine; }
            set
            {
                if (baseLine != value)
                {
                    baseLine = value;
                    Build();
                }
            }
        }

        public float BaseLineGap
        {
            get { return baseLineGap; }
            set
            {
                if (baseLineGap != value)
                {
                    baseLineGap = value;
                    Build();
                }
            }
        }

        protected override void CustomStart()
        {
            spanCount = (int)(Mathf.Abs(rulerLength / tickStep));
            int vertexCount = (spanCount + 1) * 2;
            if (baseLine)
            {
                vertexCount = (spanCount + 1) * 2 + 2;
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

            if (generateUV)
            {
                uv = new Vector2[vertexCount];
            }
        }


        [ContextMenu("Buld")]
        protected override void Build()
        {
            if (tickStep == 0 || rulerLength == 0) return;

            switch (direction)
            {
                case Direction.Horizontal:
                    CreateVertex = CreateXYVertex;
                    break;
                case Direction.Vertical:
                    CreateVertex = CreateYXVertex;
                    break;
            }

            int stepDirection = 1;
            if (align == Align.end)
            {
                stepDirection = -1;
            }

            List<int> ticks = new List<int>();
            List<int> midsTicks = new List<int>();
            List<int> bigTicks = new List<int>();
            float x, y;

            spanCount = (int)(rulerLength / tickStep);
            tickCount = spanCount + 1;
            int vertexIndex = 0;
            int indexShift = 0;
            for (int i = 0; i < tickCount; i++)
            {
                x = stepDirection * tickStep * (i - indexShift);
                y = baseLineGap;
                vertices[vertexIndex] = CreateVertex(x, y);
                if (bigTickPerSteps != 0 && (i - indexShift) % bigTickPerSteps == 0)
                {
                    y = 0 + baseLineGap + bigTickSize;
                    bigTicks.Add(vertexIndex);
                    bigTicks.Add(vertexIndex + 1);
                }
                else if (midsTickPerSteps != 0 && (i - indexShift) % midsTickPerSteps == 0)
                {
                    y = 0 + baseLineGap + midsTickSize;
                    midsTicks.Add(vertexIndex);
                    midsTicks.Add(vertexIndex + 1);
                }
                else
                {
                    y = 0 + baseLineGap + tickSize;
                    ticks.Add(vertexIndex);
                    ticks.Add(vertexIndex + 1);
                }
                vertices[vertexIndex + 1] = CreateVertex(x, y);

                if (generateUV)
                {
                    uv[vertexIndex] = Vector2.zero;
                    uv[vertexIndex + 1] = new Vector2(y - baseLineGap, 0);
                }

                vertexIndex += 2;

                if (align == Align.middle && 0 < stepDirection && (tickStep * spanCount) / 2f <= i * tickStep)
                {
                    stepDirection = -1;
                    indexShift = i;
                }
            }

            if (baseLine)
            {
                if (align == Align.middle)
                {
                    y = 0;
                    x = -tickStep * (spanCount - Mathf.Ceil(spanCount / 2f));
                    vertices[vertexIndex] = CreateVertex(x, y);
                    x = tickStep * Mathf.Ceil(spanCount / 2f);
                    vertices[vertexIndex + 1] = CreateVertex(x, y);
                }
                else
                {
                    vertices[vertexIndex] = Vector3.zero;
                    x = stepDirection * tickStep * spanCount;
                    y = 0;
                    vertices[vertexIndex + 1] = CreateVertex(x, y);
                }


                ticks.Add(vertexIndex);
                ticks.Add(vertexIndex + 1);

                if (generateUV)
                {
                    uv[vertexIndex] = Vector2.zero;
                    uv[vertexIndex + 1] = new Vector2(stepDirection * tickStep * spanCount, 0);
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

            mesh.SetIndices(ticks.ToArray(), MeshTopology.Lines, 0);
            mesh.SetIndices(midsTicks.ToArray(), MeshTopology.Lines, 1);
            mesh.SetIndices(bigTicks.ToArray(), MeshTopology.Lines, 2);
            mesh.RecalculateBounds();

            if (OnRulerBuld != null)
            {
                OnRulerBuld();
            }
        }

        private Vector3 CreateXYVertex(float x, float y)
        {
            return new Vector3(x, y, 0);
        }

        private Vector3 CreateYXVertex(float x, float y)
        {
            return new Vector3(y, x, 0);
        }


        internal List<Vector3> GetBigTicksPositions()
        {
            List<Vector3> points = new List<Vector3>();
            int stepDirection = 1;
            if (align == Align.end)
            {
                stepDirection = -1;
            }

            float x, y;
            spanCount = (int)(rulerLength / tickStep);
            tickCount = spanCount / bigTickPerSteps + 1;
            int indexShift = 0;
            for (int i = 0; i < tickCount; i++)
            {
                x = stepDirection * tickStep * bigTickPerSteps * (i - indexShift);
                y = baseLineGap + bigTickSize;
                if (align != Align.middle || (align == Align.middle && Mathf.Abs(x) <= (tickStep * spanCount) / 2f))
                {
                    points.Add(CreateVertex(x, y));
                    if (align == Align.middle && 0 < stepDirection && (tickStep * spanCount) / 2f < (i + 1) * tickStep * bigTickPerSteps)
                    {
                        stepDirection = -1;
                        indexShift = i;
                    }
                }
            }
            return points;
        }
    }
}