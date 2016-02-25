using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text mainText;
    public Text XYCoord;
    public Text objectText;
    public CanvasRenderer selectedItemPanel;

    public static UIController Instance { get; protected set; }

    // Use this for initialization
    void Start () {
        if (Instance != null)
        {
            Debug.LogError("There are two UI controllers present!");
        }
        Instance = this;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleSelectPanel(bool active = true, PlacedObject selectedObject = null) {
        selectedItemPanel.gameObject.SetActive(active);

        if (selectedObject != null && active == true)
        {
            Text selectedItemPanelMainText = selectedItemPanel.transform.FindChild("SelectedItemMainText").GetComponent<Text>();

            selectedItemPanelMainText.text = selectedObject.getType();
            //Debug.Log(selectedObject.getType() + " was selected!");
        }

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

    public void updateObjectText(string objectName) {
        string oldName = objectText.text;

        if (objectName != oldName) {
            objectText.text = objectName;
        }
    }

    public void RotateLeftButton() {
        MouseController.Instance.RotateSelectedObject(MouseController.RotDirection.Left);
    }

    public void RotateRightButton() {
        MouseController.Instance.RotateSelectedObject(MouseController.RotDirection.Right);
    }
}
