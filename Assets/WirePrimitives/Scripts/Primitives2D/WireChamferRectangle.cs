using UnityEngine;
using System.Collections;

namespace WirePrimitives
{
    /// <summary>
    /// Rectangle with cutted corners
    /// </summary>
    [AddComponentMenu("WirePrimitives/Rectangle with chamfers")]
    public class WireChamferRectangle : WirePrimitive2DAlignable
    {
        /// <summary>
        /// Width of rectangle (if it lay in XY plane)
        /// </summary>
        [SerializeField]
        private float sizeX;

        /// <summary>
        /// Height of rectangle (if it lay in XY plane)
        /// </summary>
        [SerializeField]
        private float sizeY;

        /// <summary>
        /// Size of cutting along rectangle sides at lower left corner
        /// </summary>
        [SerializeField]
        private float sizeChamferLL;

        /// <summary>
        /// Size of cutting along rectangle sides at upper left corner
        /// </summary>
        [SerializeField]
        private float sizeChamferUL;

        /// <summary>
        /// Size of cutting along rectangle sides at upper right corner
        /// </summary>
        [SerializeField]
        private float sizeChamferUR;

        /// <summary>
        /// Size of cutting along rectangle sides at lower right corner
        /// </summary>
        [SerializeField]
        private float sizeChamferLR;

        private float P;///Perimeter
        private float sqrtOfTwo = Mathf.Sqrt(2f);

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

        public float SizeChamfersLL
        {
            get { return sizeChamferLL; }
            set
            {
                sizeChamferLL = value;
                Build();
            }
        }

        public float SizeChamfersUL
        {
            get { return sizeChamferUL; }
            set
            {
                sizeChamferUL = value;
                Build();
            }

        }

        public float SizeChamfersUR
        {
            get { return sizeChamferUR; }
            set
            {
                sizeChamferUR = value;
                Build();
            }
        }

        public float SizeChamfersLR
        {
            get { return sizeChamferLR; }
            set
            {
                sizeChamferLR = value;
                Build();
            }
        }

        public void SetAllChamfersSize(float size)
        {
            sizeChamferLR = sizeChamferUR = sizeChamferUL = sizeChamferLL = size;
            Build();
        }

        protected override void CustomStart()
        {
            vertices = new Vector3[9];
            indices = new int[16];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 1;
            indices[3] = 2;
            indices[4] = 2;
            indices[5] = 3;
            indices[6] = 3;
            indices[7] = 4;
            indices[8] = 4;
            indices[9] = 5;
            indices[10] = 5;
            indices[11] = 6;
            indices[12] = 6;
            indices[13] = 7;
            indices[14] = 7;
            indices[15] = 8;

            if (generateUV)
            {
                uv = new Vector2[9];
            }

            RecalculateAligment();
        }

        [ContextMenu("Buld")]
        protected override void Build()
        {
            switch (axes)
            {
                case PlaneAxes.XY:
                    vertices[0] = new Vector3(startX * sizeX + sizeChamferLL, startY * sizeY, 0);
                    vertices[1] = new Vector3(startX * sizeX, startY * sizeY + sizeChamferLL, 0);

                    vertices[2] = new Vector3(startX * sizeX, startY * sizeY + sizeY - sizeChamferUL, 0);
                    vertices[3] = new Vector3(startX * sizeX + sizeChamferUL, startY * sizeY + sizeY, 0);

                    vertices[4] = new Vector3(startX * sizeX + sizeX - sizeChamferUR, startY * sizeY + sizeY, 0);
                    vertices[5] = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeY - sizeChamferUR, 0);

                    vertices[6] = new Vector3(startX * sizeX + sizeX, startY * sizeY + sizeChamferLR, 0);
                    vertices[7] = new Vector3(startX * sizeX + sizeX - sizeChamferLR, startY * sizeY, 0);

                    vertices[8] = vertices[0];
                    break;
                case PlaneAxes.XZ:
                    vertices[0] = new Vector3(startX * sizeX + sizeChamferLL, 0, startY * sizeY);
                    vertices[1] = new Vector3(startX * sizeX, 0, startY * sizeY + sizeChamferLL);

                    vertices[2] = new Vector3(startX * sizeX, 0, startY * sizeY + sizeY - sizeChamferUL);
                    vertices[3] = new Vector3(startX * sizeX + sizeChamferUL, 0, startY * sizeY + sizeY);

                    vertices[4] = new Vector3(startX * sizeX + sizeX - sizeChamferUR, 0, startY * sizeY + sizeY);
                    vertices[5] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY + sizeY - sizeChamferUR);

                    vertices[6] = new Vector3(startX * sizeX + sizeX, 0, startY * sizeY + sizeChamferLR);
                    vertices[7] = new Vector3(startX * sizeX + sizeX - sizeChamferLR, 0, startY * sizeY);

                    vertices[8] = vertices[0];
                    break;
                case PlaneAxes.ZY:
                    vertices[0] = new Vector3(0, startY * sizeY, startX * sizeX + sizeChamferLL);
                    vertices[1] = new Vector3(0, startY * sizeY + sizeChamferLL, startX * sizeX);

                    vertices[2] = new Vector3(0, startY * sizeY + sizeY - sizeChamferUL, startX * sizeX);
                    vertices[3] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX + sizeChamferUL);

                    vertices[4] = new Vector3(0, startY * sizeY + sizeY, startX * sizeX + sizeX - sizeChamferUR);
                    vertices[5] = new Vector3(0, startY * sizeY + sizeY - sizeChamferUR, startX * sizeX + sizeX);

                    vertices[6] = new Vector3(0, startY * sizeY + sizeChamferLR, startX * sizeX + sizeX);
                    vertices[7] = new Vector3(0, startY * sizeY, startX * sizeX + sizeX - sizeChamferLR);

                    vertices[8] = vertices[0];
                    break;
            }

            if (generateUV)
            {
                P = sizeX * 2 + sizeY * 2 - (sizeChamferLL * 2 + sizeChamferUL * 2 + sizeChamferUR * 2 + sizeChamferLR * 2) + (sizeChamferLL + sizeChamferUL + sizeChamferUR + sizeChamferLR) * sqrtOfTwo;
                if (normalizeUV)
                {
                    uv[0] = new Vector2(0, 0);
                    uv[1] = new Vector2(sizeChamferLL * sqrtOfTwo / P, 0);
                    uv[2] = uv[1] + new Vector2((sizeY - (sizeChamferLL + sizeChamferUL)) / P, 0);
                    uv[3] = uv[2] + new Vector2(sizeChamferUL * sqrtOfTwo / P, 0);
                    uv[4] = uv[3] + new Vector2((sizeX - (sizeChamferUL + sizeChamferUR)) / P, 0);
                    uv[5] = uv[4] + new Vector2(sizeChamferUR * sqrtOfTwo / P, 0);
                    uv[6] = uv[5] + new Vector2((sizeY - (sizeChamferUR + sizeChamferLR)) / P, 0);
                    uv[7] = uv[6] + new Vector2(sizeChamferLR * sqrtOfTwo / P, 0);
                    uv[8] = new Vector2(1, 0);
                }
                else
                {
                    uv[0] = new Vector2(0, 0);
                    uv[1] = new Vector2(sizeChamferLL * sqrtOfTwo, 0);
                    uv[2] = uv[1] + new Vector2((sizeY - (sizeChamferLL + sizeChamferUL)), 0);
                    uv[3] = uv[2] + new Vector2(sizeChamferUL * sqrtOfTwo, 0);
                    uv[4] = uv[3] + new Vector2((sizeX - (sizeChamferUL + sizeChamferUR)), 0);
                    uv[5] = uv[4] + new Vector2(sizeChamferUR * sqrtOfTwo, 0);
                    uv[6] = uv[5] + new Vector2((sizeY - (sizeChamferUR + sizeChamferLR)), 0);
                    uv[7] = uv[6] + new Vector2(sizeChamferLR * sqrtOfTwo, 0);
                    uv[8] = new Vector2(P, 0);
                }
            }

            ApplyToMesh();
        }


    }
}