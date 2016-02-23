using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    Vector3 lastFramePosition;
    Vector3 dragStartPosition;
    Vector3 currFramePosition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Remember where the mouse started
        currFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currFramePosition.z = 0;

        //UpdateCursor();
        //UpdateDragging();
        UpdateCameraMovement();

        // Remember where the mouse is now
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateCameraMovement()
    {
        // Handle middle or right click screen moving... check where the mouse moved
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }

    public void moveCameraTo(float x, float y)
    {
        Camera.main.transform.position = new Vector3(x, y, -10);
        Camera.main.orthographicSize = 20f;
    }

    public Vector3 getCurFramePosition() {
        return currFramePosition;
    }
}
