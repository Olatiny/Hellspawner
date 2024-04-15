using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextButtonColorUpdater : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        text = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.color = button.targetGraphic.canvasRenderer.GetColor();
    }
}
