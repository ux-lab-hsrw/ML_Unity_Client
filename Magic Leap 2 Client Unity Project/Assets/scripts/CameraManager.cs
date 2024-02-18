using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    private bool camAvailable;
    public WebCamTexture frontCam;
    private Texture defaultBackground;
    public RawImage background;
    public AspectRatioFitter fit;

    private int activeCam = 0;

    private WebCamDevice[] devices;

    void Start()
    {
        defaultBackground = background.texture;
        devices = WebCamTexture.devices;

        if (devices.Length == 0) {
            Debug.Log("No Camera found");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++) {
            //if (devices[0] != null) {
            //    frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            //}
            

            Debug.Log("Camera Nr." + i + ": " + devices[i]);
            
        }
        StartCam();
    }


    void Update()
    {
        Debug.Log(camAvailable + "__using camera:" + frontCam);
        if (!camAvailable) {
            return;
        }

        float ratio = (float)frontCam.width / (float)frontCam.height;
        fit.aspectRatio = ratio;

        float scaleY = frontCam.videoVerticallyMirrored ? -1f: 1f;

        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f) * 6;
        
        int orient = -frontCam.videoRotationAngle;

        background.rectTransform.localEulerAngles = new Vector3(0,0,orient);
    }

    public void StartCam() {
        
        if (frontCam != null) {
            frontCam.Stop();
        }

        frontCam = new WebCamTexture(devices[activeCam].name, Screen.width, Screen.height);

        if (frontCam == null) {
            Debug.Log("Unable to find front camera");
            return;
        }

        frontCam.Play();
        background.texture = frontCam;

        camAvailable = true;
    }

    public void IncreaseCameraIndex() {
        if (activeCam < devices.Length-1) {
            activeCam += 1;
            StartCam();
            Debug.Log("++increased CameraIndex; now using: " + activeCam);
        }
        
    }
    public void DecreaseCameraIndex() {
        if (activeCam != 0) {
            activeCam -= 1;
            StartCam();
            Debug.Log("--decreased CameraIndex; now using: " + activeCam);
        } 
        
    }

    public string Hello(){
        return "hello";
    }
}
