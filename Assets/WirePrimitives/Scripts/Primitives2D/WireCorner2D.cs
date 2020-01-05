using UnityEngine;
using System.Collections;
namespace WirePrimitives
{
    /// <summary>
    /// Wire mesh "L" shape
    /// </summary>
    [AddComponentMenu("WirePrimitives/Corner 2D")]
    public class WireCorner2D : WirePrimitive2D
    {
        /// <summary>
        /// Size of each "L" segment
        /// </summary>
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
            vertices = new Vector3[3];
            indices = new int[4];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 1;
            indices[3] = 2;

            if (generateUV)
            {
                uv = new Vector2[3];
            }
        }

        [ContextMenu("Buld")]
        protected override void Build()
        {
            switch (axes)
            {
                case PlaneAxes.XY:
                    vertices[0] = new Vector3(size, 0, 0);
                    vertices[1] = new Vector3(0, 0, 0);
                    vertices[2] = new Vector3(0, size, 0);
                    break;
                case PlaneAxes.XZ:
                    vertices[0] = new Vector3(size, 0, 0);
                    vertices[1] = new Vector3(0, 0, 0);
                    vertices[2] = new Vector3(0, 0, size);
                    break;
                case PlaneAxes.ZY:
                    vertices[0] = new Vector3(0, size, 0);
                    vertices[1] = new Vector3(0, 0, 0);
                    vertices[2] = new Vector3(0, 0, size);
                    break;
            }

            if (generateUV)
            {
                uv[0] = new Vector2(0, 0);
                uv[1] = new Vector2(0.5f, 0);
                uv[2] = new Vector2(1f, 0);
            }

            ApplyToMesh();
        }
    }
}
