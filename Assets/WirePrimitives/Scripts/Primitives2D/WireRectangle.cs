using UnityEngine;
using System.Collections;

namespace WirePrimitives
{
    /// <summary>
    /// Rectangle
    /// </summary>
    [AddComponentMenu("WirePrimitives/Rectangle 2D")]
    public class WireRectangle : WirePrimitive2DAlignable
    {
        /// <summary>
        /// Width of rectangle
        /// </summary>
        [SerializeField]
        private float sizeX;

        /// <summary>
        /// Height of rectangle
        /// </summary>
        [SerializeField]
        private float sizeY;
        private float P;

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

        protected override void CustomStart()
        {
            vertices = new Vector3[5];
            indices = new int[8];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 1;
            indices[3] = 2;
            indices[4] = 2;
            indices[5] = 3;
            indices[6] = 3;
            indices[7] = 4;

            if (generateUV)
            {
                uv = new Vector2[5];
            }

            RecalculateAligment();
        }


        [ContextMenu("Buld")]
        protected override void Build()
        {
            switch (axes)
            {
                case PlaneAxes.XY:
                    vertices[0] = new Vector3(startX * sizeX, startY * sizeY, 0);
                    vertices[1] = new Vector3(startX * sizeX, startY * sizeY + sizeY, 0);
                    vertices[2] = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY, 0);
                    vertices[3] = new Vector3(startX * sizeX + sizeX, startY * sizeY, 0);
                    vertices[4] = vertices[0];
                    break;
                case PlaneAxes.XZ:
                    vertices[0] = new Vector3(startX * sizeX, 0, startY * sizeY);
                    vertices[1] = new Vector3(startX * sizeX, 0, startY * sizeY + sizeY);
                    vertices[2] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY + sizeY);
                    vertices[3] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY);
                    vertices[4] = vertices[0];
                    break;
                case PlaneAxes.ZY:
                    vertices[0] = new Vector3(0, startY * sizeY, startX * sizeX);
                    vertices[1] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX);
                    vertices[2] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX + sizeX);
                    vertices[3] = new Vector3(0, startY * sizeY, startX * sizeX + sizeX);
                    vertices[4] = vertices[0];
                    break;
            }

            if (generateUV)
            {
                P = sizeX * 2 + sizeY * 2;
                if (normalizeUV)
                {
                    uv[0] = new Vector2(0, 0);
                    uv[1] = new Vector2(sizeY / P, 0);
                    uv[2] = new Vector2((sizeX + sizeY) / P, 0);
                    uv[3] = new Vector2((sizeX + sizeY * 2) / P, 0);
                    uv[4] = new Vector2(1, 0);
                }
                else
                {                    
                    uv[0] = new Vector2(0, 0);
                    uv[1] = new Vector2(sizeY, 0);
                    uv[2] = new Vector2(sizeX + sizeY, 0);
                    uv[3] = new Vector2(sizeX + sizeY * 2, 0);
                    uv[4] = new Vector2(P, 0);
                }
            }

            ApplyToMesh();
        }

        
    }
}