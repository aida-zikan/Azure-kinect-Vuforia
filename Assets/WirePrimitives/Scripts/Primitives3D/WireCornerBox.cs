using UnityEngine;
using System.Collections;

namespace WirePrimitives
{
    /// <summary>
    /// Corner box 
    /// </summary>
    [AddComponentMenu("WirePrimitives/Corner box")]
    public class WireCornerBox : WirePrimitive3DAlignable
    {
        [SerializeField]
        private float sizeX;
        [SerializeField]
        private float sizeY;
        [SerializeField]
        private float sizeZ;

        /// <summary>
        /// Length of corner segment
        /// </summary>
        [SerializeField]
        private float sizeCorner;

        private float sizeCornerX, sizeCornerY, sizeCornerZ;
        public float SizeX
        {
            get { return sizeX; }
            set
            {
                if (sizeX != value)
                {
                    sizeX = value;
                    ClumpCornerX();
                    Build();
                }
            }
        }

        public float SizeY
        {
            get { return sizeY; }
            set
            {
                if (sizeY != value)
                {
                    sizeY = value;
                    ClumpCornerY();
                    Build();
                }
            }
        }

        public float SizeZ
        {
            get { return sizeZ; }
            set
            {
                if (sizeZ != value)
                {
                    sizeZ = value;
                    ClumpCornerZ();
                    Build();
                }
            }
        }

        /// <summary>
        /// Length of corner segment
        /// </summary>
        public float SizeCorner
        {
            get { return sizeCorner; }
            set
            {
                if (sizeCorner != value)
                {
                    sizeCorner = value;
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
            ClumpCornerSizes();
            Build();
        }

        private void ClumpCornerSizes()
        {
            ClumpCornerX();
            ClumpCornerY();
            ClumpCornerZ();
        }

        /// <summary>
        /// Clump corner segment length to half box side to avoid opposite corners segments overlap
        /// </summary>
        private void ClumpCornerX()
        {
            if (sizeX / 2f < sizeCorner)
            {
                sizeCornerX = sizeX / 2f;
            }
            else
            {
                ///Restore corner length when there is room for it
                sizeCornerX = sizeCorner;
            }
        }

        /// <summary>
        /// Clump corner segment length to half box side to avoid opposite corners segments overlap
        /// </summary>
        private void ClumpCornerY()
        {
            if (sizeY / 2f < sizeCorner)
            {
                sizeCornerY = sizeY / 2f;
            }
            else
            {
                ///Restore corner length when there is room for it
                sizeCornerY = sizeCorner;
            }
        }

        /// <summary>
        /// Clump corner segment length to half box side to avoid opposite corners segments overlap
        /// </summary>
        private void ClumpCornerZ()
        {
            if (sizeZ / 2f < sizeCorner)
            {
                sizeCornerZ = sizeZ / 2f;
            }
            else
            {
                ///Restore corner length when there is room for it
                sizeCornerZ = sizeCorner;
            }
        }

        protected override void CustomStart()
        {
            int cornersCount = 8;
            int vertexCount = cornersCount * 4;
            int edgesCount = cornersCount * 3;
            vertices = new Vector3[vertexCount];
            indices = new int[edgesCount * 2];

            int vertexIndex;
            int edgeIndex = 0;
            for (int i = 0; i < cornersCount; i++)
            {
                vertexIndex = i * 4;
                indices[edgeIndex + 0] = vertexIndex + 0;
                indices[edgeIndex + 1] = vertexIndex + 1;

                indices[edgeIndex + 2] = vertexIndex + 0;
                indices[edgeIndex + 3] = vertexIndex + 2;

                indices[edgeIndex + 4] = vertexIndex + 0;
                indices[edgeIndex + 5] = vertexIndex + 3;
                edgeIndex += 3 * 2;
            }

            if (generateUV)
            {
                uv = new Vector2[vertexCount];

                Vector2 sizeCornerUV;
                if (normalizeUV)
                {
                    sizeCornerUV = Vector2.right;
                }
                else
                {
                    sizeCornerUV = new Vector2(sizeCorner, 0);
                }

                for (int i = 0; i < cornersCount; i++)
                {
                    vertexIndex = i * 4;
                    uv[vertexIndex + 0] = Vector2.zero;
                    uv[vertexIndex + 3] = uv[vertexIndex + 2] = uv[vertexIndex + 1] = sizeCornerUV;
                }
            }
            ClumpCornerSizes();
            RecalculateAligment();
        }

        private void FillCorner(Vector3 corner, Vector3 cornerDirections, float sizeCorner, ref int vertexIndex)
        {
            vertices[vertexIndex + 0] = corner;
            vertices[vertexIndex + 1] = corner + Vector3.Scale(Vector3.right * sizeCornerX, cornerDirections);
            vertices[vertexIndex + 2] = corner + Vector3.Scale(Vector3.up * sizeCornerY, cornerDirections);
            vertices[vertexIndex + 3] = corner + Vector3.Scale(Vector3.forward * sizeCornerZ, cornerDirections);

            vertexIndex += 4;
        }

        [ContextMenu("Buld")]
        protected override void Build()
        {
            //Debug.Log("Corners, Build" + sizeX + " " + sizeY + " " + sizeZ + " " + sizeCorner);
            //Debug.Log("Corners, vertices " + vertices.Length);
            //Debug.Log("mesh  " + mesh);//
            //Debug.Log("go  " + gameObject.activeSelf);
#if UNITY_EDITOR
            //ClumpCornerSizes();
#endif
            ///bottom corners            
            int vertexIndex = 0;
            Vector3 corner = new Vector3(startX * sizeX, startY * sizeY, startZ * sizeZ);
            FillCorner(corner, Vector3.one, sizeCorner, ref vertexIndex);

            corner = new Vector3(startX * sizeX, startY * sizeY, startZ * sizeZ + sizeZ);
            FillCorner(corner, new Vector3(1, 1, -1), sizeCorner, ref vertexIndex);

            corner = new Vector3(startX * sizeX + sizeX, startY * sizeY, startZ * sizeZ + sizeZ);
            FillCorner(corner, new Vector3(-1, 1, -1), sizeCorner, ref vertexIndex);

            corner = new Vector3(startX * sizeX + sizeX, startY * sizeY, startZ * sizeZ);
            FillCorner(corner, new Vector3(-1, 1, 1), sizeCorner, ref vertexIndex);

            ///top corners
            corner = new Vector3(startX * sizeX, startY * sizeY + sizeY, startZ * sizeZ);
            FillCorner(corner, new Vector3(1, -1, 1), sizeCorner, ref vertexIndex);

            corner = new Vector3(startX * sizeX, startY * sizeY + sizeY, startZ * sizeZ + sizeZ);
            FillCorner(corner, new Vector3(1, -1, -1), sizeCorner, ref vertexIndex);

            corner = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY, startZ * sizeZ + sizeZ);
            FillCorner(corner, -Vector3.one, sizeCorner, ref vertexIndex);

            corner = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY, startZ * sizeZ);
            FillCorner(corner, new Vector3(-1, -1, 1), sizeCorner, ref vertexIndex);

            ApplyToMesh();
        }
    }
}
