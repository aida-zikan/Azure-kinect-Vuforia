using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WirePrimitives;
using System;
using System.Linq;

/// <summary>
/// Retrievies name of WirePrimitive component and output it to Text
/// </summary>
public class ClassNameUI : MonoBehaviour
{
    [SerializeField]
    private Text classNameLabel;

    private Ray ray;
    private Collider prevCollider;
    private Color[] savedColors;
    private Color hoverColor = Color.red;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider != prevCollider)
            {
                if (prevCollider != null)
                {
                    SetColor(prevCollider, savedColors);
                }
                savedColors = GetColors(hit.collider);
                SetColor(hit.collider, hoverColor);

                prevCollider = hit.collider;
                var primitive = hit.collider.GetComponent<WirePrimitiveBase>();
                if (primitive != null)
                {
                    classNameLabel.text = SplitCamelCase(primitive.GetType().Name);
                }
            }
        }
        else
        {
            if (prevCollider != null)
            {
                SetColor(prevCollider, savedColors);
            }
            prevCollider = null;
        }
    }

    private static Color[] GetColors(Collider collider)
    {
        return collider.GetComponent<MeshRenderer>().materials.Select(x => x.color).ToArray();
    }

    private static void SetColor(Collider collider, Color[] colors)
    {
        MeshRenderer mr = collider.GetComponent<MeshRenderer>();
        for (int i = 0; i< mr.materials.Length; i++)// Material mat in mr.materials)
        {
            mr.materials[i].color = colors[i];
        }
    }

    private static void SetColor(Collider collider, Color color)
    {
        MeshRenderer mr = collider.GetComponent<MeshRenderer>();
        foreach (Material mat in mr.materials)
        {
            mat.color = color;
        }
    }

    private static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1");
    }
}
