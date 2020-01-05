using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subStage : MonoBehaviour
{
    [SerializeField] Camera ARCamera;
    [SerializeField] Transform TargetTransform;

    [SerializeField] Camera mainCamera;
    [SerializeField] Transform mainCameraTransform;
    [SerializeField] Transform CubeTransform;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.fieldOfView = ARCamera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        mainCameraTransform.localPosition = Vector3.zero;
        mainCameraTransform.localRotation = Quaternion.identity;

        CubeTransform.localPosition = TargetTransform.position;
        CubeTransform.localRotation = TargetTransform.rotation;

        mainCameraTransform.parent = CubeTransform;

        CubeTransform.localPosition = Vector3.zero;
        CubeTransform.localRotation = Quaternion.identity;

        mainCameraTransform.parent = transform;
    }
}
