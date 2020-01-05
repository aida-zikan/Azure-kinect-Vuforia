using UnityEngine;
using System.Collections;
using System;


namespace WirePrimitives
{
    /// <summary>
    /// Cross with square in center like AutoCAD cursor
    /// </summary>
    [AddComponentMenu("WirePrimitives/Autocad cursor")]
    public class ACADCursor : WirePrimitive2D
    {
        /// <summary>
        /// Length of lines for cross
        /// </summary>
        [SerializeField]
        private float sizeCross;

        /// <summary>
        /// Length of square side
        /// </summary>
        [SerializeField]
        private float sizeSquare;

        public float SizeCross
        {
            get { return sizeCross; }
            set
            {
                if (sizeCross != value)
                {
                    sizeCross = value;
                    Build();
                }
            }
        }

        public float SizeSquare
        {
            get { return sizeSquare; }
            set
            {
                if (sizeSquare != value)
                {
                    sizeSquare = value;
                    Build();
                }
            }
        }

        protected override void CustomStart()
        {
            vertices = new Vector3[8];
            indices = new int[12];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 3;

            indices[4] = 4;
            indices[5] = 5;
            indices[6] = 5;
            indices[7] = 6;
            indices[8] = 6;
            indices[9] = 7;
            indices[10] = 7;
            indices[11] = 4;

            if (generateUV)
            {
                uv = new Vector2[8];
            }
        }

        [ContextMenu("Buld")]
        protected override void Build()
        {
            switch (axes)
            {
                case PlaneAxes.XY:
                    vertices[0] = new Vector3(-0.5f * sizeCross, 0, 0);
                    vertices[1] = new Vector3(0.5f * sizeCross, 0, 0);
                    vertices[2] = new Vector3(0, -0.5f * sizeCross, 0);
                    vertices[3] = new Vector3(0, 0.5f * sizeCross, 0);

                    vertices[4] = new Vector3(-0.5f * sizeSquare, -0.5f * sizeSquare, 0);
                    vertices[5] = new Vector3(-0.5f * sizeSquare, 0.5f * sizeSquare, 0);
                    vertices[6] = new Vector3(0.5f * sizeSquare, 0.5f * sizeSquare, 0);
                    vertices[7] = new Vector3(0.5f * sizeSquare, -0.5f * sizeSquare, 0);

                    break;
                case PlaneAxes.XZ:
                    vertices[0] = new Vector3(-0.5f * sizeCross, 0, 0);
                    vertices[1] = new Vector3(0.5f * sizeCross, 0, 0);
                    vertices[2] = new Vector3(0, 0, -0.5f * sizeCross);
                    vertices[3] = new Vector3(0, 0, 0.5f * sizeCross);

                    vertices[4] = new Vector3(-0.5f * sizeSquare, 0, -0.5f * sizeSquare);
                    vertices[5] = new Vector3(-0.5f * sizeSquare, 0, 0.5f * sizeSquare);
                    vertices[6] = new Vector3(0.5f * sizeSquare, 0, 0.5f * sizeSquare);
                    vertices[7] = new Vector3(0.5f * sizeSquare, 0, -0.5f * sizeSquare);
                    break;
                case PlaneAxes.ZY:
                    vertices[0] = new Vector3(0, -0.5f * sizeCross, 0);
                    vertices[1] = new Vector3(0, 0.5f * sizeCross, 0);
                    vertices[2] = new Vector3(0, 0, -0.5f * sizeCross);
                    vertices[3] = new Vector3(0, 0, 0.5f * sizeCross);

                    vertices[4] = new Vector3(0, -0.5f * sizeSquare, -0.5f * sizeSquare);
                    vertices[5] = new Vector3(0, 0.5f * sizeSquare, -0.5f * sizeSquare);
                    vertices[6] = new Vector3(0, 0.5f * sizeSquare, 0.5f * sizeSquare);
                    vertices[7] = new Vector3(0, -0.5f * sizeSquare, 0.5f * sizeSquare);
                    break;
            }

            if (generateUV)
            {
                uv[0] = new Vector2(0, 0);
                uv[1] = new Vector2(1, 0);
                uv[2] = uv[0];
                uv[3] = uv[1];

                uv[4] = new Vector2(0, 0);
                uv[5] = new Vector2(0.25f, 0);
                uv[6] = new Vector2(0.5f, 0);
                uv[7] = new Vector2(0.75f, 0);
            }

            ApplyToMesh();
        }
    }
}
