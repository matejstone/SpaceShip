using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Text mainText;
    public Text XYCoord;
    public Text objectText;
    public Text viewText;
    public CanvasRenderer selectedItemPanel;
    public CanvasRenderer galaxyMap;

    public static UIController Instance { get; protected set; }

    GameObject map;

    // Use this for initialization
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There are two UI controllers present!");
        }
        Instance = this;

        updateViewText("Ship");
    }

    void OnGUI()
    {
        // if the galaxy map is open
        if (galaxyMap.gameObject.activeSelf) {

            if (!map)
            {
                int universeWidth = UniverseController.Instance.universe.width;
                int universeHeight = UniverseController.Instance.universe.height;
                float width = galaxyMap.GetComponent<RectTransform>().rect.width;
                float height = galaxyMap.GetComponent<RectTransform>().rect.height;
                
                float border = 5.0f;

                float mapWidth = width - (border * 2);
                float mapHeight = height - (border * 2);

                Texture2D tex2d = new Texture2D(universeWidth, (int)universeHeight);
               // Texture2D tex2d = new Texture2D(2, 1);
                //tex2d.SetPixels(new Color[2] { Color.red, Color.blue });
                tex2d.SetPixels(GetMapPixels());
                tex2d.Apply();

                GUI.DrawTexture(new Rect(   galaxyMap.transform.position.x - (width / 2) + border,
                                            galaxyMap.transform.position.y - (height / 2) + border,
                                            mapWidth, mapHeight), tex2d);

                //gui_tex.texture = tex2d;
            }
            else {
                GUITexture gui_tex = map.GetComponent<GUITexture>();
                //gui_tex.texture.SetPixels();
            }
        }
    }


    Color[] GetMapPixels() {
        //UniverseController.Instance.universe.universeSpaceLocationArray;
        SpaceLocation[,] spaceLocations = UniverseController.Instance.universe.universeSpaceLocationArray;
        int universeWidth = UniverseController.Instance.universe.width;
        int universeHeight = UniverseController.Instance.universe.height;

        Color[] mapPixels = new Color[universeWidth * universeHeight];

        for (int i = 0; i < universeWidth; i++)
        {
            for (int j = 0; j < universeHeight; j++)
            {
                Color color;
                if (spaceLocations[i, j] != null)
                {
                    Debug.Log("got a pixel");
                    color = Color.white;
                }
                else
                {
                    color = Color.black;
                }

                mapPixels[j * universeWidth + i] = color;
            }
        }

        return mapPixels;
    }

    public void toggleSelectPanel(bool active = true, PlacedObject selectedObject = null)
    {
        selectedItemPanel.gameObject.SetActive(active);

        if (selectedObject != null && active == true)
        {
            Text selectedItemPanelMainText = selectedItemPanel.transform.FindChild("SelectedItemMainText").GetComponent<Text>();

            selectedItemPanelMainText.text = selectedObject.getType();
            //Debug.Log(selectedObject.getType() + " was selected!");
        }

    }

    public void toggleGalaxyMap() {
        galaxyMap.gameObject.SetActive(!galaxyMap.gameObject.activeSelf);
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
        if (oldText != newText)
        {
            XYCoord.text = newText;
        }
    }

    public void updateObjectText(string objectName)
    {
        if (objectName != objectText.text)
        {
            objectText.text = objectName;
        }
    }

    public void updateViewText(string viewText)
    {
        if (viewText != this.viewText.text) {
            this.viewText.text = viewText;
        }
    }

    public void RotateLeftButton()
    {
        MouseController.Instance.RotateSelectedObject(MouseController.RotDirection.Left);
    }

    public void RotateRightButton()
    {
        MouseController.Instance.RotateSelectedObject(MouseController.RotDirection.Right);
    }
}
