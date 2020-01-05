using UnityEngine;
using WirePrimitives;

namespace WirePrimitives
{
    /// <summary>
    /// Sample script that sets size of rectCorners and ruler from bounding box of meshRenderer doing so every frame
    /// 
    /// </summary>
    public class FollowBounds : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;
        [SerializeField]
        private WireRectangleCorners rectCorners;
        [SerializeField]
        private WireRuler ruler;
        [SerializeField]
        private float margin = 10;

        private Bounds bb;

        void Update()
        {
            bb = meshRenderer.bounds;
            if (bb.size != Vector3.zero)
            {
                rectCorners.SizeX = bb.size.x + 2 * margin;
                rectCorners.SizeY = bb.size.z + 2 * margin;
                rectCorners.transform.position = bb.center;

                Vector3 p = ruler.transform.position;
                p.x = bb.center.x;
                ruler.transform.position = p;

                ruler.RulerLength = rectCorners.SizeX;
            }

        }
    }
}
