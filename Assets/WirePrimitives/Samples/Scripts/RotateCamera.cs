using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour
{
    [SerializeField]
    private float angleLimit = 10;
    private Vector3 euler;
    void Start()
    {
        euler = transform.eulerAngles;
    }

    void Update()
    {
        float amount = Input.mousePosition.x / Screen.width;
        transform.eulerAngles = euler + new Vector3(0, amount * angleLimit - 0.5f, 0);
    }
}
