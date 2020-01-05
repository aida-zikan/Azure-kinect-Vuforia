using UnityEngine;
using System.Collections;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// Four "L" shapes that form a rectangle 
    /// </summary>
    [AddComponentMenu("WirePrimitives/Rectangle corners 2D")]
    public class WireRectangleCorners : WirePrimitive2DAlignable
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

        /// <summary>
        /// Size of "L" segments
        /// </summary>
        [SerializeField]
        private float sizeCorner;
        private float P;

        private float SizeCornerX
        {
            get
            {
                /// Clamp corners size so they not exceed width of rectangle
                if (sizeX / 2f < sizeCorner)
                {
                    return sizeX / 2f;
                }
                return sizeCorner;
            }
        }

        private float SizeCornerY
        {
            get
            {
                ///Clamp corners size so they not exceed height of rectangle
                if (sizeY / 2f < sizeCorner)
                {
                    return sizeY / 2f;
                }
                return sizeCorner;
            }
        }

        /// <summary>
        /// Width of rectangle
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
        /// Height of rectangle
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
        /// Size of "L" segments
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

        protected override void CustomStart()
        {
            vertices = new Vector3[12];
            indices = new int[16];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 1;
            indices[3] = 2;

            indices[4] = 3;
            indices[5] = 4;
            indices[6] = 4;
            indices[7] = 5;

            indices[8] = 6;
            indices[9] = 7;
            indices[10] = 7;
            indices[11] = 8;

            indices[12] = 9;
            indices[13] = 10;
            indices[14] = 10;
            indices[15] = 11;

            if (generateUV)
            {
                uv = new Vector2[12];
            }

            RecalculateAligment();
        }


        [ContextMenu("Buld")]
        protected override void Build()
        {
            switch (axes)
            {
                case PlaneAxes.XY:
                    vertices[0] = new Vector3(startX * sizeX + SizeCornerX, startY * sizeY, 0);
                    vertices[1] = new Vector3(startX * sizeX, startY * sizeY, 0);
                    vertices[2] = new Vector3(startX * sizeX, startY * sizeY + SizeCornerY, 0);

                    vertices[3] = new Vector3(startX * sizeX, startY * sizeY + sizeY - SizeCornerY, 0);
                    vertices[4] = new Vector3(startX * sizeX, startY * sizeY + sizeY, 0);
                    vertices[5] = new Vector3(startX * sizeX + SizeCornerX, startY * sizeY + sizeY, 0);

                    vertices[6] = new Vector3(startX * sizeX + sizeX - SizeCornerX, startY * sizeY + sizeY, 0);
                    vertices[7] = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY, 0);
                    vertices[8] = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY - SizeCornerY, 0);

                    vertices[9] = new Vector3(startX * sizeX + sizeX, startY * sizeY + SizeCornerY, 0);
                    vertices[10] = new Vector3(startX * sizeX + sizeX, startY * sizeY, 0);
                    vertices[11] = new Vector3(startX * sizeX + sizeX - SizeCornerX, startY * sizeY, 0);
                    break;

                case PlaneAxes.XZ:
                    vertices[0] = new Vector3(startX * sizeX + SizeCornerX, 0, startY * sizeY);
                    vertices[1] = new Vector3(startX * sizeX, 0, startY * sizeY);
                    vertices[2] = new Vector3(startX * sizeX, 0, startY * sizeY + SizeCornerY);

                    vertices[3] = new Vector3(startX * sizeX, 0, startY * sizeY + sizeY - SizeCornerY);
                    vertices[4] = new Vector3(startX * sizeX, 0, startY * sizeY + sizeY);
                    vertices[5] = new Vector3(startX * sizeX + SizeCornerX, 0, startY * sizeY + sizeY);

                    vertices[6] = new Vector3(startX * sizeX + sizeX - SizeCornerX, 0, startY * sizeY + sizeY);
                    vertices[7] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY + sizeY);
                    vertices[8] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY + sizeY - SizeCornerY);

                    vertices[9] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY + SizeCornerY);
                    vertices[10] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY);
                    vertices[11] = new Vector3(startX * sizeX + sizeX - SizeCornerX, 0, startY * sizeY);
                    break;

                case PlaneAxes.ZY:
                    vertices[0] = new Vector3(0, startY * sizeY, startX * sizeX + SizeCornerX);
                    vertices[1] = new Vector3(0, startY * sizeY, startX * sizeX);
                    vertices[2] = new Vector3(0, startY * sizeY + SizeCornerY, startX * sizeX);

                    vertices[3] = new Vector3(0, startY * sizeY + sizeY - SizeCornerY, startX * sizeX);
                    vertices[4] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX);
                    vertices[5] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX + SizeCornerX);

                    vertices[6] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX + sizeX - SizeCornerX);
                    vertices[7] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX + sizeX);
                    vertices[8] = new Vector3(0, startY * sizeY + sizeY - SizeCornerY, startX * sizeX + sizeX);

                    vertices[9] = new Vector3(0, startY * sizeY + SizeCornerY, startX * sizeX + sizeX);
                    vertices[10] = new Vector3(0, startY * sizeY, startX * sizeX + sizeX);
                    vertices[11] = new Vector3(0, startY * sizeY, startX * sizeX + sizeX - SizeCornerX);
                    break;
            }

            if (generateUV)
            {
                P = sizeX * 2 + sizeY * 2;
                if (normalizeUV)
                {
                    uv[0] = new Vector2(sizeCorner / P, 0);
                    uv[1] = new Vector2(0, 0);
                    uv[2] = new Vector2(sizeCorner / P, 0);

                    uv[3] = new Vector2(sizeCorner / P, 0);
                    uv[4] = new Vector2(0, 0);
                    uv[5] = new Vector2(sizeCorner / P, 0);

                    uv[6] = new Vector2(sizeCorner / P, 0);
                    uv[7] = new Vector2(0, 0);
                    uv[8] = new Vector2(sizeCorner / P, 0);

                    uv[9] = new Vector2(sizeCorner / P, 0);
                    uv[10] = new Vector2(0, 0);
                    uv[11] = new Vector2(sizeCorner / P, 0);
                }
                else
                {
                    uv[0] = new Vector2(sizeCorner, 0);
                    uv[1] = new Vector2(0, 0);
                    uv[2] = new Vector2(sizeCorner, 0);

                    uv[3] = new Vector2(sizeCorner, 0);
                    uv[4] = new Vector2(0, 0);
                    uv[5] = new Vector2(sizeCorner, 0);

                    uv[6] = new Vector2(sizeCorner, 0);
                    uv[7] = new Vector2(0, 0);
                    uv[8] = new Vector2(sizeCorner, 0);

                    uv[9] = new Vector2(sizeCorner, 0);
                    uv[10] = new Vector2(0, 0);
                    uv[11] = new Vector2(sizeCorner, 0);
                }
            }

            ApplyToMesh();
        }
    }
}