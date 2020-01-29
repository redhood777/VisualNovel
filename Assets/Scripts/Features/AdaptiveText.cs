using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdaptiveText : MonoBehaviour
{
    TextMeshProUGUI txt;
    public bool continualUpdate = true;

    public int defaultFontSize = 24;
    public static float defaultResolution = 1789f;

    // Start is called before the first frame update
    void Start()
    {
        print(Screen.height + Screen.width);

        txt = GetComponent<TextMeshProUGUI>();

        if (continualUpdate)
        {
            InvokeRepeating("Adjust", 0f, Random.Range(0.5f, 2f));
        }
        else
        {
            Adjust();
            enabled = false;
        }
    }

    void Adjust()
    {
        if (!enabled || !gameObject.activeInHierarchy)
            return;

        float totalCurrentRes = Screen.height + Screen.width;
        float perc = totalCurrentRes / defaultResolution;
        int fontsize = Mathf.RoundToInt((float)defaultFontSize * perc);

        txt.fontSize = fontsize;
    }
}
