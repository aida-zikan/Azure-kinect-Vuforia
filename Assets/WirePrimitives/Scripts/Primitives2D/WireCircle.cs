using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// Circle
    /// </summary>
    [AddComponentMenu("WirePrimitives/Circle")]
    public class WireCircle : WirePrimitive2D
    {
        [SerializeField]
        private float radius;

        /// <summary>
        /// Delegate for computing vertex on circle for a selected plane
        /// </summary>
        private Func<Vector3> CreateVertex;

        /// <summary>
        /// Unit circle
        /// </summary>
        private CircleMath circleMath;

        /// <summary>
        /// Count of point on unit circle. This affects how coarse would look circular parts of produced meshes.
        /// </summary>
        private readonly int vertexCountPerCircle = 64;

        public int VertexCount
        {
            get { return vertexCountPerCircle; }
        }

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

        protected override void CustomStart()
        {
            circleMath = new CircleMath(vertexCountPerCircle);

            vertices = new Vector3[vertexCountPerCircle + 1];

            indices = new int[vertexCountPerCircle * 2];
            for (int i = 0; i < vertices.Length; i++)
            {
                if (i != 0)
                {
                    indices[i * 2 - 2] = i - 1;
                    indices[i * 2 - 1] = i;
                }
            }

            if (generateUV)
            {
                uv = new Vector2[vertexCountPerCircle + 1];
                float uvStep = 1f / vertices.Length;
                for (int i = 0; i < vertices.Length; i++)
                {
                    uv[i] = new Vector2(i * uvStep, 0);
                }
            }
        }

        protected override void Build()
        {
            CreateCircle();
        }

        [ContextMenu("CreateCircle")]
        void CreateCircle()
        {
            switch (axes)
            {
                case PlaneAxes.XY:
                    CreateVertex = CreateXYVertex;
                    break;
                case PlaneAxes.XZ:
                    CreateVertex = CreateXZVertex;
                    break;
                case PlaneAxes.ZY:
                    CreateVertex = CreateZYVertex;
                    break;
            }
            CreateWireCircleMesh(Vector3.zero, Radius);
        }

        private void CreateWireCircleMesh(Vector3 center, float radius)
        {
            circleMath.StartFromZero();
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = CreateVertex();
            }
            ApplyToMesh();
        }


        /// <summary>
        /// Calculates position of a vertex for circle on XY plane
        /// </summary>
        /// <param name="i">Index of vertex on unit circle</param>
        /// <returns> Positon </returns>
        private Vector3 CreateXYVertex()
        {
            Vector2 circle = circleMath.Current();
            return new Vector3(circle.x * radius, circle.y * radius, 0);
        }

        /// <summary>
        /// Calculates position of a vertex for circle on XZ plane
        /// </summary>
        /// <param name="i">Index of vertex on unit circle</param>
        /// <returns> Positon </returns>
        private Vector3 CreateXZVertex()
        {
            Vector2 circle = circleMath.Current();
            return new Vector3(circle.x * radius, 0, circle.y * radius);
        }

        /// <summary>
        /// Calculates position of a vertex for circle on ZY plane
        /// </summary>
        /// <param name="i">Index of vertex on unit circle</param>
        /// <returns> Positon </returns>
        private Vector3 CreateZYVertex()
        {
            Vector2 circle = circleMath.Current();
            return new Vector3(0, circle.x * radius, circle.y * radius);
        }
    }
}
