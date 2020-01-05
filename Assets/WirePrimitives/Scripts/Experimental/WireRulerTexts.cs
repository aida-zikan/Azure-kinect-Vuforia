using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace WirePrimitives
{
    /// <summary>
    /// Adding inscription (texts) to ruler
    /// Not optimised 
    /// provided as a reference
    /// </summary>
    public class WireRulerTexts : MonoBehaviour
    {
        public class TextData
        {
            public GameObject gameObject;
            public RectTransform rectTransform;
            public Text text;
        }

        /// <summary>
        /// Prototipe of texts
        /// </summary>
        [SerializeField]
        private GameObject textProto;

        /// <summary>
        /// Starting value
        /// </summary>
        [SerializeField]
        private float startValue;

        /// <summary>
        /// Step between values
        /// </summary>
        [SerializeField]
        private float stepValue;

        /// <summary>
        /// Ruler
        /// </summary>
        [SerializeField]
        private WireRuler wireRuler;

        /// <summary>
        /// Texts cache
        /// </summary>
        private List<TextData> texts = new List<TextData>();

        void Start()
        {
            textProto.gameObject.SetActive(false);

            ///Subscribe to ruler changes
            if (wireRuler != null)
            {
                wireRuler.OnRulerBuld = GenerateTexts;
            }
        }

        [ContextMenu("GenerateTexts")]
        public void GenerateTexts()
        {
            HideAll();
            var pos = wireRuler.GetBigTicksPositions();
            int flipDirectionIndex = 0;
            int sign = 1;
            for (int i = 0; i < pos.Count; i++)
            {
                if (1 < i && Vector3.Dot(pos[i] - pos[i - 1], pos[i - 1] - pos[i - 2]) < 0)
                {
                    flipDirectionIndex = i - 1;
                    sign = -1;
                }

                if (i < texts.Count)///Use alredy created text
                {
                    SetText(texts[i], pos[i], (startValue + sign * (i - flipDirectionIndex) * stepValue).ToString());
                }
                else///Create a new text
                {
                    texts.Add(GenerateText(pos[i], (startValue + sign * (i - flipDirectionIndex) * stepValue).ToString()));
                }
            }
        }

        private void SetText(TextData textData, Vector3 position, string content)
        {
            textData.rectTransform.anchoredPosition3D = position;
            textData.text.text = content;
            textData.gameObject.SetActive(true);
        }

        private void HideAll()
        {
            foreach (TextData textData in texts)
            {
                textData.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Instantiate a new text
        /// </summary>
        /// <param name="pos">position</param>
        /// <param name="content">text value</param>
        /// <returns></returns>
        public TextData GenerateText(Vector3 pos, string content)
        {
            GameObject newTextGO = Instantiate(textProto);
            newTextGO.transform.SetParent(textProto.transform.parent);
            var rTransform = newTextGO.GetComponent<RectTransform>();
            rTransform.anchoredPosition3D = pos;
            var text = newTextGO.GetComponent<Text>();
            text.text = content;

            newTextGO.SetActive(true);

            return new TextData()
            {
                gameObject = newTextGO,
                rectTransform = rTransform,
                text = text 
            };
        }
    }
}
