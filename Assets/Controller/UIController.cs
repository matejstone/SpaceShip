using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text mainText;
    public Text XYCoord;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateMainText(string text)
    {
        if (mainText.text == text)
        {

        }
        else {
            mainText.text = text;
        }
    }

    public void updateXYCoord(int x, int y)
    {
        string oldText = XYCoord.text;
        string newText = "";

        if (x < 0 || y < 0)
        {
            XYCoord.text = "";
        }
        else {
            newText = x + ", " + y;
        }
        if (oldText != newText) {
            XYCoord.text = newText;
        }
    }
}
