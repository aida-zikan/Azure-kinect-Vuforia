using UnityEngine;
using System.Collections;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// 
    /// </summary>
    [AddComponentMenu("WirePrimitives/Line")]
    public class WireLine : WirePrimitiveBase
    {
        [SerializeField]
        // Summary:
        //     ///
        //     If enabled, the lines are defined in world space.
        //     ///
        private bool useWorldSpace;

        private int initCapacity = 2;
        private int capacityLimit = 65000;
        private int vertexCount;
        public bool UseWorldSpace
        {
            get { return useWorldSpace; }
            set { useWorldSpace = value; }
        }

        public int Capacity
        {
            get { return vertices.Length; }
        }

        // Summary:
        //     ///
        //     Set the position of the vertex in the line.
        //     ///
        //
        // Parameters:
        //   index:
        //
        //   position:
        public void SetPosition(int index, Vector3 position)
        {
            if (0 <= index && index < vertexCount)
            {
                if (UseWorldSpace)
                {
                    vertices[index] = transform.InverseTransformPoint(position);
                }
                else
                {
                    vertices[index] = position;
                }
            }
        }

        public void AddVertex(Vector3 position)
        {
            if (0 <= vertexCount)
            {
                if (Capacity <= vertexCount)
                {
                    Grow();
                }

                if (vertexCount < Capacity)
                {
                    if (UseWorldSpace)
                    {
                        vertices[vertexCount] = transform.InverseTransformPoint(position);
                    }
                    else
                    {
                        vertices[vertexCount] = position;
                    }
                    vertexCount++;
                    FillLineIndices();
                    Build();
                }
            }
        }

        private void Grow()
        {
            int newCapacity = Capacity * 2;
            if (capacityLimit < newCapacity)
            {
                newCapacity = capacityLimit;
            }
            Vector3[] newVertices = new Vector3[newCapacity];
            vertices.CopyTo(newVertices, 0);
            vertices = newVertices;
        }


        protected void FillLineIndices()
        {
            if (1 < vertexCount)
            {
                indices = new int[2 * vertexCount - 2];
                ///For all segments fill vertex indices
                int vertexIndex = 0;
                for (int i = 0; i < indices.Length; i += 2)
                {
                    indices[i] = vertexIndex;
                    vertexIndex += 1;
                    indices[i + 1] = vertexIndex;
                }
            }
        }

        public void Initialize()
        {
            vertexCount = 0;
            vertices = new Vector3[initCapacity];
        }
        //protected override void CustomStart()
        //{
        //    vertexCount = 0;
        //    vertices = new Vector3[initCapacity];

        //    //if (generateUV)
        //    //{
        //    //    uv = new Vector2[8];
        //    //}
        //}


        [ContextMenu("Buld")]
        protected override void Build()
        {
            ApplyToMesh();
        }
    }
}
