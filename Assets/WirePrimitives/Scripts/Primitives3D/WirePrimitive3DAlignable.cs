using UnityEngine;
using System.Collections;
namespace WirePrimitives
{
    public abstract class WirePrimitive3DAlignable : WirePrimitiveBase
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

        public enum ZAlign
        {
            front,
            center,
            back
        }

        [SerializeField]
        protected XAlign xAlign;
        [SerializeField]
        protected YAlign yAlign;
        [SerializeField]
        protected ZAlign zAlign;

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
        public ZAlign AlignZ
        {
            get { return zAlign; }
            set
            {
                if (zAlign != value)
                {
                    zAlign = value;
                    RecalculateAligment();
                }
            }
        }

        protected float startX = -0.5f, startY = -0.5f, startZ = -0.5f;

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

            switch (zAlign)
            {
                case ZAlign.front:
                    startZ = 0f;
                    break;
                case ZAlign.center:
                    startZ = -0.5f;
                    break;
                case ZAlign.back:
                    startZ = -1f;
                    break;
                default:
                    break;
            }

            Build();
        }
    }
}
