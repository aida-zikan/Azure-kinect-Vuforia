using UnityEngine;
using System;

namespace WirePrimitives
{
    [AddComponentMenu("WirePrimitives/Circular arc")]
    public class WireArc : WirePrimitive2D
    {
        /// <summary>
        /// Arc radius
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
        /// Delegate for creating vertices in XY, XZ or YZ planes
        /// </summary>
        private Func<int, Vector3> CreateVertex;

        /// <summary>
        /// Count of point on unit circle arc.
        /// </summary>
        private int arcVertexCount = 16;
        private Vector2[] unitCircleArc;

        /// <summary>
        /// Count of point on unit circle. This affects how coarse would look circular parts of produced meshes.
        /// </summary>
        private readonly int vertexCountPerCircle = 120;

        public int VertexCount
        {
            get { return vertexCountPerCircle; }
        }

        /// <summary>
        /// Arc radius
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
                if (startAngle != value && value <= endAngle)
                {
                    startAngle = value;
                    FillUnitCircleVertices();
                    CreateArrays();
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
                if (endAngle != value && startAngle <= value)
                {
                    endAngle = value;
                    FillUnitCircleVertices();
                    CreateArrays();
                    Build();
                }
            }
        }

        private void Rebuild()
        {
            FillUnitCircleVertices();
            CreateArrays();
            Build();
        }

        /// <summary>
        /// Fill vertices on unit circle (radius = 1) for arc from "startAngle" to "endAngle"
        /// </summary>
        public void FillUnitCircleVertices()
        {
            float fullAngle = endAngle - startAngle;
            if (360 < fullAngle)
            {
                endAngle = startAngle + 360;
            }
            arcVertexCount = (int)(fullAngle / 3) + 2;
            float angleStep = (Mathf.Deg2Rad * fullAngle) / (arcVertexCount - 1);
            unitCircleArc = new Vector2[arcVertexCount];
            float startAngleRad = Mathf.Deg2Rad * startAngle;
            float angle;
            for (int i = 0; i < unitCircleArc.Length; i++)
            {
                angle = startAngleRad + angleStep * i;
                unitCircleArc[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            }
        }


        protected override void CustomStart()
        {
            FillUnitCircleVertices();
            CreateArrays();
        }

        protected override void Build()
        {
            CreateArc();
        }

        /// <summary>
        /// Creates new arrays for vertices, indices and uv
        /// </summary>
        private void CreateArrays()
        {
            vertices = new Vector3[arcVertexCount];

            indices = new int[arcVertexCount * 2];
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
                uv = new Vector2[arcVertexCount];
                float uvStep = (endAngle - startAngle) / vertices.Length;
                for (int i = 0; i < vertices.Length; i++)
                {
                    uv[i] = new Vector2(i * uvStep, 0);
                }
            }
        }

        /// <summary>
        /// Creates arc for selected plane
        /// </summary>
        [ContextMenu("CreateArc")]
        void CreateArc()
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
            CreateWireCircleMesh(Radius);
        }

        /// <summary>
        /// Calculates position of a vertex on arc for XY plane
        /// </summary>
        /// <param name="i">Index of vertex on unit circle</param>
        /// <returns> Positon </returns>
        private void CreateWireCircleMesh(float radius)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = CreateVertex(i);
            }
            mesh.Clear();
            ApplyToMesh();
        }

        /// <summary>
        /// Calculates position of a vertex on arc for XY plane
        /// </summary>
        /// <param name="i">Index of vertex on unit circle</param>
        /// <returns> Positon </returns>
        private Vector3 CreateXYVertex(int i)
        {
            Vector2 circle = unitCircleArc[i];
            return new Vector3(circle.x * radius, circle.y * radius, 0);
        }

        private Vector3 CreateXZVertex(int i)
        {
            Vector2 circle = unitCircleArc[i];
            return new Vector3(circle.x * radius, 0, circle.y * radius);
        }

        private Vector3 CreateZYVertex(int i)
        {
            Vector2 circle = unitCircleArc[i];
            return new Vector3(0, circle.x * radius, circle.y * radius);
        }
    }
}
