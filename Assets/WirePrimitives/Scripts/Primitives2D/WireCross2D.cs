using UnityEngine;
using System.Collections;

namespace WirePrimitives
{
    /// <summary>
    /// Wire cross like "+" sign
    /// </summary>
    [AddComponentMenu("WirePrimitives/Cross 2D")]
    public class WireCross2D : WirePrimitive2D
    {
        [SerializeField]
        private float size;

        public float Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    Build();
                }
            }
        }

        protected override void CustomStart()
        {
            vertices = new Vector3[4];
            indices = new int[4];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 3;

            if (generateUV)
            {
                uv = new Vector2[4];
            }
        }

        [ContextMenu("Buld")]
        protected override void Build()
        {
            switch (axes)
            {
                case PlaneAxes.XY:
                    vertices[0] = new Vector3(-0.5f * size, 0, 0);
                    vertices[1] = new Vector3(0.5f * size, 0, 0);
                    vertices[2] = new Vector3(0, -0.5f * size, 0);
                    vertices[3] = new Vector3(0, 0.5f * size, 0);
                    break;
                case PlaneAxes.XZ:
                    vertices[0] = new Vector3(-0.5f * size, 0, 0);
                    vertices[1] = new Vector3(0.5f * size, 0, 0);
                    vertices[2] = new Vector3(0, 0, -0.5f * size);
                    vertices[3] = new Vector3(0, 0, 0.5f * size);
                    break;
                case PlaneAxes.ZY:
                    vertices[0] = new Vector3(0, -0.5f * size, 0);
                    vertices[1] = new Vector3(0, 0.5f * size, 0);
                    vertices[2] = new Vector3(0, 0, -0.5f * size);
                    vertices[3] = new Vector3(0, 0, 0.5f * size);
                    break;
            }

            if (generateUV)
            {
                uv[0] = new Vector2(0, 0);
                uv[1] = new Vector2(1, 0);
                uv[2] = uv[0];
                uv[3] = uv[1];
            }

            ApplyToMesh();
        }
    }
}
