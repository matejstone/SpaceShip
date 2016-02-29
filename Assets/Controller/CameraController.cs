using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour
{

    Vector3 lastFramePosition;
    Vector3 dragStartPosition;
    Vector3 currFramePosition;
    public GameObject background;

    public static CameraController Instance { get; protected set; }
    public enum ViewMode { Ship, SpaceObject, SolarSystem, Galaxy }

    private float waitBeforeZoomSwitch = 2.0f;
    private float timeSinceLastViewSwitch = 0f;
    private float timeSinceLastSwitchTrigger = 0f;
    private bool canSwitchView = false;
    private bool cameraIsMoving = false;
    private float cameraMoveSpeed;
    private Vector3 destination;
    private float destOrthoSize;
    private ViewMode currViewMode = ViewMode.Ship;

    private Vector3 lastShipViewLocation;

    // Use this for initialization
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There are two Camera controllers present!");
        }
        Instance = this;
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
        doBackgroundResize();

        // Remember where the mouse is now
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    void UpdateCameraMovement()
    {

        //timeSinceLastSwitchTrigger += Time.deltaTime;
        //if (canSwitchView == false && timeSinceLastSwitchTrigger > waitBeforeZoomSwitch) {
            canSwitchView = true;
        //}

        //if (canSwitchView == true && Camera.main.orthographicSize == 25.0f && timeSinceLastSwitchTrigger > 10.0f) {
            //timeSinceLastSwitchTrigger = 0.0f;
            //canSwitchView = false;
        //}


        if (cameraIsMoving) {
            moveCameraTowards();
            return;
        }

        // Handle middle or right click screen moving... check where the mouse moved
        if (currViewMode != ViewMode.SpaceObject && (Input.GetMouseButton(1) || Input.GetMouseButton(2)))
        {
            Vector3 diff = lastFramePosition - currFramePosition;
            Camera.main.transform.Translate(diff);
        }

        if (currViewMode == ViewMode.Ship)
        {
            

            if (Camera.main.orthographicSize < 25.0f)
            {
                ZoomInOut();
            }
            else {


                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    ZoomInOut();
                }
                else {

                    if (canSwitchView)
                    {
                        lastShipViewLocation = Camera.main.transform.position;
                        SwitchViewMode(ViewMode.SpaceObject, 21.0f);
                    }
                    else {
                        // do something if you cant switch view yet
                    }
                }
            }
        }
        else if (currViewMode == ViewMode.SpaceObject)
        {
            if (Camera.main.orthographicSize < 20.0f)
            {
                SwitchViewMode(ViewMode.Ship, 24.0f);
            }
            else if (Camera.main.orthographicSize < 25.0f)
            {
                ZoomInOut();
            }
            else {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    ZoomInOut();
                }
                else {
                    if (canSwitchView)
                    {
                        SwitchViewMode(ViewMode.SolarSystem, 11.0f);
                    }
                }
            }
        }
        else if (currViewMode == ViewMode.SolarSystem)
        {
            if (Camera.main.orthographicSize < 10.0f)
            {

                SwitchViewMode(ViewMode.SpaceObject, 24.0f);
            }
            else if (Camera.main.orthographicSize < 25.0f)
            {
                Debug.Log("Attempting to zoom in out");
                ZoomInOut();
            }
            else {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    ZoomInOut();
                }
                else {
                    if (canSwitchView)
                    {
                        SwitchViewMode(ViewMode.Galaxy, 11.0f);
                    }
                }
            }
        }
        else if (currViewMode == ViewMode.Galaxy) {
            if (Camera.main.orthographicSize < 10.0f)
            {
                SwitchViewMode(ViewMode.SolarSystem, 24.0f);
            }
            else
            {
                ZoomInOut();
            }
        }
    }

    void ZoomInOut() {
        Debug.Log("zoom in out");
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 25f);
    }

    void SwitchViewMode(ViewMode viewMode, float orthoSize) {
        currViewMode = viewMode;

        if (viewMode == ViewMode.Ship)
        {
            moveCameraTo(lastShipViewLocation.x, lastShipViewLocation.y, 20.0f, 24.0f);
            UIController.Instance.updateViewText("Ship");

            resizeBackground(1.0f, 1f);
            ShipController.Instance.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (viewMode == ViewMode.SpaceObject)
        {
            moveCameraTo(-10, 0, 20.0f, orthoSize);
            UIController.Instance.updateViewText("Space Object");
            ShipController.Instance.transform.localScale = new Vector3(0.4f, 0.4f, 1f);
            resizeBackground(0.4f, 1f);
        }
        else if (viewMode == ViewMode.SolarSystem)
        {
            UIController.Instance.updateViewText("Solar System");
            moveCameraTo(Camera.main.transform.position.x, Camera.main.transform.position.y, 20.0f, orthoSize);
            ShipController.Instance.transform.localScale = new Vector3(0f, 0f, 0f);
            resizeBackground(0.00f, 0.5f);
        }
        else if (viewMode == ViewMode.Galaxy) {
            UIController.Instance.updateViewText("Galaxy");
            moveCameraTo(40, 40, 200.0f, orthoSize);
        }
    }

    float backgroundResizeTime;
    float backgroundSizeDest;
    float backgroundSizeStep;
    bool isBackgroundResizing = false;

    void resizeBackground(float size, float speed = 1.0f) {
        backgroundSizeDest = size;
        isBackgroundResizing = true;
        backgroundSizeStep = speed / 100.0f;
    }

    void doBackgroundResize() {
        if (isBackgroundResizing) {
            if (backgroundSizeDest != background.transform.localScale.x) {
                float currSize = background.transform.localScale.x;
                float currDiff = Math.Abs(background.transform.localScale.x - backgroundSizeDest); ;

                if (currDiff > 0.01f)
                {
                    backgroundResizeTime -= backgroundResizeTime;

                    float resize = 0.0f;

                    if (currSize > backgroundSizeDest)
                    {
                        resize = currSize - backgroundSizeStep;
                    }
                    else {
                        resize = currSize + backgroundSizeStep;
                    }

                    background.transform.localScale = new Vector3(resize, resize * 2, 1.0f);
                }
                else {
                    background.transform.localScale = new Vector3(backgroundSizeDest, backgroundSizeDest * 2, 1.0f);
                    isBackgroundResizing = false;
                }
            }
            
        }
        
    }

    public void positionCameraTo(float x, float y)
    {
        Camera.main.transform.position = new Vector3(x, y, -10);
        Camera.main.orthographicSize = 20f;
    }

    public void moveCameraTo(float x, float y, float speed = 5.0f, float orthoSize = 20.0f) {
        cameraIsMoving = true;
        cameraMoveSpeed = speed;
        destOrthoSize = orthoSize;
        destination = new Vector3(x, y, -10);
    }

    public void moveCameraTowards() {
        float step = cameraMoveSpeed * Time.deltaTime;

        if (Camera.main.transform.position == destination && Camera.main.orthographicSize == destOrthoSize)
        {
            Debug.Log("camera is at destination");
            cameraIsMoving = false;
            destination = new Vector3();
        }
        else {
            
            if(Camera.main.transform.position != destination)
            {
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, destination, step);
            }

            if (Math.Abs(destOrthoSize - Camera.main.orthographicSize) < 0.25f) {
                Camera.main.orthographicSize = destOrthoSize;
            }

            if (Camera.main.orthographicSize < destOrthoSize) {
                Camera.main.orthographicSize += step / 2;
            } else if (Camera.main.orthographicSize > destOrthoSize) {
                Camera.main.orthographicSize -= step / 2;
            }
        }
    }

    public Vector3 getCurFramePosition() {
        return currFramePosition;
    }
}
