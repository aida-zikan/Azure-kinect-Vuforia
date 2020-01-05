using UnityEngine;
using System.Collections;
namespace WirePrimitives
{
    /// <summary>
    /// Base class for 2D primitives with changable pivot 
    /// </summary>
    public abstract class WirePrimitive2DAlignable : WirePrimitive2D
    {
        public enum XAlign
        {
            left,
            center,
            right
        }

        public enum YAlign
        {
            bottom,
            center,
            top
        }

        /// <summary>
        /// Align along X axis 
        /// </summary>
        [SerializeField]
        protected XAlign xAlign;

        /// <summary>
        /// Align along Y axis for XY and ZY planes and along Z axis for XZ plane primitives
        /// </summary>
        [SerializeField]
        protected YAlign yAlign;

        protected float startX, startY;

        public XAlign AlignX
        {
            get { return xAlign; }
            set
            {
                if (xAlign != value)
                {
                    xAlign = value;
                    RecalculateAligment();
                }
            }
        }

        public YAlign AlignY
        {
            get { return yAlign; }
            set
            {
                if (yAlign != value)
                {
                    yAlign = value;
                    RecalculateAligment();
                }
            }
        }

        protected void RecalculateAligment()
        {
            switch (xAlign)
            {
                case XAlign.left:
                    startX = 0f;
                    break;
                case XAlign.center:
                    startX = -0.5f;
                    break;
                case XAlign.right:
                    startX = -1f;
                    break;
                default:
                    break;
            }

            switch (yAlign)
            {
                case YAlign.bottom:
                    startY = 0f;
                    break;
                case YAlign.center:
                    startY = -0.5f;
                    break;
                case YAlign.top:
                    startY = -1f;
                    break;
                default:
                    break;
            }

            Build();
        }

#if UNITY_EDITOR
        void Update()
        {
            CustomStart();
            RecalculateAligment();
            Build();
        }
#endif
    }
}
