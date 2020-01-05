using UnityEngine;
using System.Collections;
using WirePrimitives;
using UnityEngine.UI;

/// <summary>
/// UI for switchin different Selection types
/// </summary>
public class ChangeSelectionTypeUI : MonoBehaviour
{
    [SerializeField]
    private Toggle boxSelectionToggle;
    [SerializeField]
    private Toggle cornerSelectionToggle;
    [SerializeField]
    private Toggle dashedSelectionToggle;
    [SerializeField]
    private Toggle circleSelectionToggle;
    [SerializeField]
    private BoxSelection selector;

    void Start()
    {
        boxSelectionToggle.onValueChanged.AddListener(BoxSelectionOn);
        cornerSelectionToggle.onValueChanged.AddListener(CornerSelectionOn);
        dashedSelectionToggle.onValueChanged.AddListener(DashedBoxSelectionOn);

        circleSelectionToggle.onValueChanged.AddListener(CircleBoxSelectionOn);
    }

    void BoxSelectionOn(bool isOn)
    {
        if (isOn)
        {
            selector.Type = BoxSelection.SelectionType.Box;
        }
    }

    void CornerSelectionOn(bool isOn)
    {
        if (isOn)
        {
            selector.Type = BoxSelection.SelectionType.CornerBox;
        }
    }

    void DashedBoxSelectionOn(bool isOn)
    {
        if (isOn)
        {
            selector.Type = BoxSelection.SelectionType.DashedBox;
        }
    }

    void CircleBoxSelectionOn(bool isOn)
    {
        if (isOn)
        {
            selector.Type = BoxSelection.SelectionType.Circle;
        }
    }
}
