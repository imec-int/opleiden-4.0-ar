using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyDebug : MonoBehaviour
{
    public TextMeshProUGUI output;
    // Start is called before the first frame update
    void Start()
    {
        output.text = "hello world";
    }

    public void Debug(string value)
    {
        if (output.text.Length > 350) output.text = "";
        output.text = output.text + " <br>" + value;
    }


}
