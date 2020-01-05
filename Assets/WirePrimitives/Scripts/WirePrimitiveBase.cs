using UnityEngine;
using System.Collections.Generic;

namespace WirePrimitives
{
    /// <summary>
    /// Base class for all WirePrimitives
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    public abstract class WirePrimitiveBase : MonoBehaviour
    {
        /// <summary>
        /// Does we need UV coordinates 
        /// UV coordinate used to add dashed line type to primitives by texturing them with texture with alpha
        /// </summary>
        [SerializeField]
        protected bool generateUV;

        /// <summary>
        /// Force primitive rebuilding in Update every 
        /// Set this to "true" if you need to animate primitives with Unity animation
        /// Set this to "false" if you not animating you primitives or animate them from code using properties
        /// </summary>
        [SerializeField]
        protected bool rebuildEveryFrame;

        /// <summary>
        /// Vertices for mesh
        /// </summary>
        protected Vector3[] vertices;

        /// <summary>
        /// Lines indices for mesh
        /// </summary>
        protected int[] indices;

        /// <summary>
        /// UV coordinates (only "X" component is used)
        /// </summary>
        protected Vector2[] uv;

        /// <summary>
        /// Scale UV coordinates to [0..1] domain
        /// </summary>
        protected bool normalizeUV = false;

        /// <summary>
        /// Mesh of the primitive
        /// </summary>
        protected Mesh mesh;

        /// <summary>
        /// Does we need UV coordinates 
        /// </summary>
        public bool GenerateUV
        {
            get { return generateUV; }
            set
            {
                if (generateUV != value)
                {
                    generateUV = value;
                    Build();
                }
            }
        }

        protected abstract void Build();

        /// <summary>
        /// Subslasses override this method for subclass specific initialisation
        /// </summary>
        protected virtual void CustomStart() { }

        void Awake()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = mesh;
            CustomStart();

            Build();
        }

        /// <summary>
        /// Apply computed arrays to mesh
        /// </summary>
        protected void ApplyToMesh()
        {
            mesh.vertices = vertices;
            if (generateUV)
            {
                mesh.uv = uv;
            }
            else
            {
                mesh.uv = null;
            }

            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            mesh.RecalculateBounds();
        }


        /// <summary>
        /// Update is used to see changes when you edit primitives in editor inspector
        /// for build Update method can be removed if you don't use "rebuildEveryFrame" flag (used to animate primitives at runtime by Unity amination) 
        /// </summary>
        void Update()
        {
#if UNITY_EDITOR
            if (mesh != null)
            {
                CustomStart();
                Build();
            }
#else
            if (rebuildEveryFrame)
            {
                CustomStart();
                Build();
            }
#endif
        }
    }
}
