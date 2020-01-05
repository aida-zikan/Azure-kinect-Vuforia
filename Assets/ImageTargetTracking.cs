using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetTracking : MonoBehaviour
{
    [SerializeField] Transform centerObjTransform;
    [SerializeField] Transform ImageTargetTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        /*
        centerObjTransform.localRotation = ImageTargetTransform.rotation;
        centerObjTransform.localPosition = ImageTargetTransform.position;

        transform.localPosition = centerObjTransform.localPosition;
        transform.localRotation = Quaternion.Euler(
            0 - centerObjTransform.localRotation.eulerAngles.x,
            0 - centerObjTransform.localRotation.eulerAngles.y,
            0 - centerObjTransform.localRotation.eulerAngles.z);

        centerObjTransform.localPosition = Vector3.zero;
        centerObjTransform.localRotation = Quaternion.identity;

        //transform.LookAt(centerObjTransform.transform);


        //transform.RotateAround(centerObjTransform.position, Vector3.up, 0.1f);
        */
    }
}
