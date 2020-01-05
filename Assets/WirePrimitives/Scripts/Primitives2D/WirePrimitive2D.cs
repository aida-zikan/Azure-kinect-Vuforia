using UnityEngine;
using System.Collections;
namespace WirePrimitives
{
    public abstract class WirePrimitive2D : WirePrimitiveBase
    {
        /// <summary>
        /// Axes which defines a plane
        /// </summary>
        public enum PlaneAxes
        {
            XY,
            XZ,
            ZY
        }

        /// <summary>
        /// Plane in which lies primitive
        /// </summary>
        [SerializeField]
        protected PlaneAxes axes;

        /// <summary>
        /// Plane in which lies primitive
        /// </summary>
        protected PlaneAxes Axes
        {
            get { return axes; }
            set
            {
                axes = value;
                Build();
            }
        }
    }
}
