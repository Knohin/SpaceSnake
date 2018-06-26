using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextLog : MonoBehaviour {
    
    public static Text textLog;

    private void Awake()
    {
        textLog = GetComponent<Text>();
    }

    public static void Print(string str)
    {
        if (textLog.text.Split('\n').Length > 12)
            textLog.text = textLog.text.Substring(textLog.text.IndexOf('\n')+1);
        textLog.text += str + '\n';
    }

}
