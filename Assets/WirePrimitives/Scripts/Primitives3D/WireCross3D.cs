using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// Wire cross along 3 axis
    /// </summary>
    [AddComponentMenu("WirePrimitives/Cross 3D")]
    public class WireCross3D : WirePrimitiveBase
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
            vertices = new Vector3[6];
            indices = new int[6];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 3;
            indices[4] = 4;
            indices[5] = 5;

            if (generateUV)
            {
                uv = new Vector2[6];
            }
        }

        [ContextMenu("Buld")]
        protected override void Build()
        {
            vertices[0] = new Vector3(-0.5f * size, 0, 0);
            vertices[1] = new Vector3(0.5f * size, 0, 0);

            vertices[2] = new Vector3(0, -0.5f * size, 0);
            vertices[3] = new Vector3(0, 0.5f * size, 0);

            vertices[4] = new Vector3(0, 0, -0.5f * size);
            vertices[5] = new Vector3(0, 0, 0.5f * size);

            if (generateUV)
            {
                uv[0] = uv[2] = uv[4] = new Vector2(0, 0);
                uv[1] = uv[3] = uv[5] = new Vector2(1, 0);
            }

            ApplyToMesh();
        }
    }
}
