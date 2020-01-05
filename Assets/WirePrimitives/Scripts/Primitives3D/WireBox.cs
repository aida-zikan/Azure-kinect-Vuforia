using UnityEngine;
using System.Collections;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// Box (cuboid)
    /// </summary>
    [AddComponentMenu("WirePrimitives/Box")]
    public class WireBox : WirePrimitive3DAlignable
    {
        [SerializeField]
        private float sizeX;
        [SerializeField]
        private float sizeY;
        [SerializeField]
        private float sizeZ;

        private float L;

        /// <summary>
        /// Length along X axis
        /// </summary>
        public float SizeX
        {
            get { return sizeX; }
            set
            {
                if (sizeX != value)
                {
                    sizeX = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Length along Y axis
        /// </summary>
        public float SizeY
        {
            get { return sizeY; }
            set
            {
                if (sizeY != value)
                {
                    sizeY = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Length along Z axis
        /// </summary>
        public float SizeZ
        {
            get { return sizeZ; }
            set
            {
                if (sizeZ != value)
                {
                    sizeZ = value;
                    Build();
                }
            }
        }

        internal void SetSize(Vector3 size)
        {
            SetSize(size.x, size.y, size.z);
        }

        public void SetSize(float sizeX, float sizeY, float sizeZ)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.sizeZ = sizeZ;
            Build();
        }

        protected override void CustomStart()
        {
            vertices = new Vector3[8];
            indices = new int[24];

            ///-----------
            ///Bottom
            indices[0] = 0;
            indices[1] = 1;

            indices[2] = 1;
            indices[3] = 2;

            indices[4] = 2;
            indices[5] = 3;

            indices[6] = 3;
            indices[7] = 0;

            ///-----------
            ///Top
            indices[8] = 4;
            indices[9] = 5;

            indices[10] = 5;
            indices[11] = 6;

            indices[12] = 6;
            indices[13] = 7;

            indices[14] = 7;
            indices[15] = 4;

            ///-----------
            ///Vertical edges
            indices[16] = 0;
            indices[17] = 4;

            indices[18] = 1;
            indices[19] = 5;

            indices[20] = 2;
            indices[21] = 6;

            indices[22] = 3;
            indices[23] = 7;

            if (generateUV)
            {
                uv = new Vector2[8];
            }

            RecalculateAligment();
        }


        [ContextMenu("Buld")]
        protected override void Build()
        {
            //bottom vertices
            vertices[0] = new Vector3(startX * sizeX, startY * sizeY, startZ * sizeZ);
            vertices[1] = new Vector3(startX * sizeX, startY * sizeY, startZ * sizeZ + sizeZ);
            vertices[2] = new Vector3(startX * sizeX + sizeX, startY * sizeY, startZ * sizeZ + sizeZ);
            vertices[3] = new Vector3(startX * sizeX + sizeX, startY * sizeY, startZ * sizeZ);

            //top vertices
            vertices[4] = new Vector3(startX * sizeX, startY * sizeY + sizeY, startZ * sizeZ);
            vertices[5] = new Vector3(startX * sizeX, startY * sizeY + sizeY, startZ * sizeZ + sizeZ);
            vertices[6] = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY, startZ * sizeZ + sizeZ);
            vertices[7] = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY, startZ * sizeZ);

            if (generateUV)
            {
                if (normalizeUV)
                {
                    L = sizeX + sizeY + sizeZ;
                    //bottom vertices
                    uv[0] = new Vector2(0, 0);
                    uv[1] = new Vector2(sizeZ / L, 0);
                    uv[2] = new Vector2((sizeX + sizeZ) / L, 0);
                    uv[3] = new Vector2(sizeX / L, 0);

                    //top vertices
                    uv[4] = new Vector2(sizeY/L, 0);
                    uv[5] = new Vector2((sizeY + sizeZ) /L, 0);
                    uv[6] = new Vector2((sizeY + sizeZ + sizeX) / L, 0);
                    uv[7] = new Vector2((sizeY + sizeX) / L, 0);
                }
                else
                {
                    //bottom vertices
                    uv[0] = new Vector2(0, 0);
                    uv[1] = new Vector2(sizeZ, 0);
                    uv[2] = new Vector2(sizeX + sizeZ, 0);
                    uv[3] = new Vector2(sizeX, 0);

                    //top vertices
                    uv[4] = new Vector2(sizeY, 0);
                    uv[5] = new Vector2(sizeY + sizeZ, 0);
                    uv[6] = new Vector2(sizeY + sizeZ + sizeX, 0);
                    uv[7] = new Vector2(sizeY + sizeX, 0);
                }
            }

            ApplyToMesh();
        }
    }
}
