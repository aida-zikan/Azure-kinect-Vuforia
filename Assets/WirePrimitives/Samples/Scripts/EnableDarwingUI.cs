using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WirePrimitives;

/// <summary>
/// UI for startin/stopping drawing
/// </summary>
public class EnableDarwingUI : MonoBehaviour
{
    [SerializeField]
    private Toggle EnableDrawingToggle;
    [SerializeField]
    private WireFreeLineDrawing lineDrawing;

    void Start()
    {
        EnableDrawingToggle.onValueChanged.AddListener(OnEnableDrawing);
    }

    private void OnEnableDrawing(bool isOn)
    {
        if (isOn)
        {
            lineDrawing.EnableDrawing = true;
        }
        else
        {
            lineDrawing.EnableDrawing = false;
        }
    }
}
