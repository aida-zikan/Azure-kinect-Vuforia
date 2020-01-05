using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WirePrimitives
{
    /// <summary>
    /// Rectangular grid with 3 submeshes (materials) for different line types
    /// </summary>
    [AddComponentMenu("WirePrimitives/Rectangular grid")]
    public class WireRectangleGrid : WirePrimitive2DAlignable
    {
        /// <summary>
        /// Cell size along first axis (usually X)
        /// </summary>
        [SerializeField]
        public float cellSizeX;

        /// <summary>
        /// Cell size along second axis (usually Y)
        /// </summary>
        [SerializeField]
        public float cellSizeY;

        /// <summary>
        /// Number of cells in first direction 
        /// </summary>
        [SerializeField]
        public int cellCountX;

        /// <summary>
        /// Number of cells in second direction 
        /// </summary>
        [SerializeField]
        public int cellCountY;

        /// <summary>
        /// Number of cells between major lines in first direction 
        /// Major lines are drawn by second material from MeshRenderer materials array
        /// </summary>
        [SerializeField]
        private int bigCellPerCellsX;

        /// <summary>
        /// Number of cells between major lines in second direction 
        /// Major lines are drawn by second material from MeshRenderer materials array
        /// </summary>
        [SerializeField]
        private int bigCellPerCellsY;

        /// <summary>
        /// If you have even number of cells then grid center line can be drawn by dedicated material by switching this to "true"
        /// </summary>
        [SerializeField]
        private bool drawCenterLines;

        /// <summary>
        /// Delegate for creating vertices for selected plane
        /// </summary>
        private Func<float, float, Vector3> CreateVertex;

        /// <summary>
        /// Cell size along first axis (usually X)
        /// </summary>
        public float CellSizeX
        {
            get { return cellSizeX; }
            set
            {
                if (cellSizeX != value)
                {
                    cellSizeX = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Cell size along second axis (usually Y)
        /// </summary>        
        public float CellSizeY
        {
            get { return cellSizeY; }
            set
            {
                if (cellSizeY != value)
                {
                    cellSizeY = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Number of cells in first direction 
        /// </summary>        
        public int CellCountX
        {
            get { return cellCountX; }
            set
            {
                if (0 <= value && cellSizeX != value)
                {
                    cellSizeX = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Number of cells in second direction 
        /// </summary>
        public int CellCountY
        {
            get { return cellCountY; }
            set
            {
                if (0 <= value && cellSizeY != value)
                {
                    cellSizeY = value;
                    Build();
                }
            }
        }        

        /// <summary>
        /// Number of cells between major lines in first direction 
        /// Major lines are drawn by second material from MeshRenderer materials array
        /// </summary>
        public int BigCellPerCellsX
        {
            get { return bigCellPerCellsX; }
            set
            {
                if (bigCellPerCellsX != value)
                {
                    bigCellPerCellsX = value;
                    Build();
                }
            }
        }

        /// <summary>
        /// Number of cells between major lines in second direction 
        /// Major lines are drawn by second material from MeshRenderer materials array
        /// </summary>        
        public int BigCellPerCellsY
        {
            get { return bigCellPerCellsY; }
            set
            {
                if (bigCellPerCellsY != value)
                {
                    bigCellPerCellsY = value;
                    Build();
                }
            }
        }


        /// <summary>
        /// Draw grid center line in third material
        /// </summary>
        public bool DrawCenterLines
        {
            get { return drawCenterLines; }
            set
            {
                if (drawCenterLines != value)
                {
                    drawCenterLines = value;
                    Build();
                }
            }
        }

        protected override void CustomStart()
        {
            //if (bigCellPerCellsX == 0) bigCellPerCellsX = 10;
            //if (bigCellPerCellsY == 0) bigCellPerCellsY = 10;
            if (cellCountX < 0) cellCountX = 0;
            if (cellCountY < 0) cellCountY = 0;

            int vertexCount = (cellCountX + 1) * 2 + (cellCountY + 1) * 2;
            vertices = new Vector3[vertexCount];
            indices = new int[vertexCount];
            for (int i = 0; i < vertexCount; i += 2)
            {
                indices[i] = i;
                indices[i + 1] = i + 1;
            }

            if (generateUV)
            {
                uv = new Vector2[vertexCount];
            }

            RecalculateAligment();
        }

        public void Build2()
        {
            Build();
        }

        [ContextMenu("Buld")]
        protected override void Build()
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

            List<int> lines = new List<int>();
            List<int> linesBigCells = new List<int>();
            List<int> linesCenter = new List<int>();

            int vertexCountX = (cellCountX + 1) * 2;
            int vertexCountY = (cellCountY + 1) * 2;
            float x, y;
            float gridSizeX = cellSizeX * cellCountX;
            float gridSizeY = cellSizeY * cellCountY;

            int lineIndex;

            //lines along y
            for (int i = 0; i < vertexCountX; i += 2)
            {
                lineIndex = i / 2;
                x = startX * gridSizeX + cellSizeX * lineIndex;
                y = startY * gridSizeY;
                vertices[i] = CreateVertex(x, y);
                y = startY * gridSizeY + gridSizeY;
                vertices[i + 1] = CreateVertex(x, y);

                bool middle = drawCenterLines && (cellCountX + 1) % 2 == 1 && lineIndex == (cellCountX + 1) / 2;
                if (middle)///Grid center line
                {
                    linesCenter.Add(i);
                    linesCenter.Add(i + 1);
                }
                else if (bigCellPerCellsX != 0 && lineIndex % bigCellPerCellsX == 0)///Major lines
                {
                    linesBigCells.Add(i);
                    linesBigCells.Add(i + 1);
                }
                else///Ordinary lines
                {
                    lines.Add(i);
                    lines.Add(i + 1);
                }
            }

            //lines along x
            for (int i = 0; i < vertexCountY; i += 2)
            {
                lineIndex = i / 2;
                int vertexIndex = i + vertexCountX;
                y = startY * gridSizeY + cellSizeY * lineIndex;
                x = startX * gridSizeX;
                vertices[vertexIndex] = CreateVertex(x, y);
                x = startX * gridSizeX + gridSizeX;
                vertices[vertexIndex + 1] = CreateVertex(x, y);

                bool middle = drawCenterLines && (cellCountY + 1) % 2 == 1 && lineIndex == (cellCountY + 1) / 2;
                if (middle)///Grid center line
                {
                    linesCenter.Add(vertexIndex);
                    linesCenter.Add(vertexIndex + 1);
                }
                else if (bigCellPerCellsY != 0 && lineIndex % bigCellPerCellsY == 0)///Major lines
                {
                    linesBigCells.Add(vertexIndex);
                    linesBigCells.Add(vertexIndex + 1);
                }
                else///Ordinary lines
                {
                    lines.Add(vertexIndex);
                    lines.Add(vertexIndex + 1);
                }
            }

            if (generateUV)
            {
                Vector2 uvEndY = new Vector2(gridSizeY, 0);
                //lines along y
                for (int i = 0; i < vertexCountX; i += 2)
                {
                    uv[i] = Vector2.zero;
                    uv[i + 1] = uvEndY;
                }

                Vector2 uvEndX = new Vector2(gridSizeX, 0);
                //lines along x
                for (int i = 0; i < vertexCountY; i += 2)
                {
                    int vertexIndex = i + vertexCountX;
                    uv[vertexIndex] = Vector2.zero;
                    uv[vertexIndex + 1] = uvEndX;
                }
            }

            mesh.Clear();
            mesh.subMeshCount = 3;
            mesh.vertices = vertices;
            if (generateUV)
            {
                mesh.uv = uv;
            }
            else
            {
                mesh.uv = null;
            }

            mesh.SetIndices(lines.ToArray(), MeshTopology.Lines, 0);
            mesh.SetIndices(linesBigCells.ToArray(), MeshTopology.Lines, 1);
            mesh.SetIndices(linesCenter.ToArray(), MeshTopology.Lines, 2);
            mesh.RecalculateBounds();
        }

        private Vector3 CreateXYVertex(float x, float y)
        {
            return new Vector3(x, y, 0);
        }

        private Vector3 CreateXZVertex(float x, float z)
        {
            return new Vector3(x, 0, z);
        }

        private Vector3 CreateZYVertex(float z, float y)
        {
            return new Vector3(0, y, z);
        }

    }
}