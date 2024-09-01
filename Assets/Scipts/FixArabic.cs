using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;

public class FixArabic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = ArabicFixer.Fix(GetComponent<Text>().text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
