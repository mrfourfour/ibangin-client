using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class WebCamManager : MonoBehaviour
{
    // Start is called before the first frame update
    static WebCamTexture camTexture;

    void Start()
    {
        // 카메라 권한 획득
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        // 카메라가 없는 경우
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("no camera");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices;

        int frontCameraIndex = -1;
        for (int i = 0; i < devices.Length; ++i)
        {
            if (devices[i].isFrontFacing)
            {
                frontCameraIndex = i;
                break;
            }
        }

        if (frontCameraIndex >= 0)
        {
            camTexture = new WebCamTexture(devices[frontCameraIndex].name);

            if (camTexture != null)
                camTexture.requestedFPS = 60;
        }

        GetComponent<Renderer>().material.mainTexture = camTexture;

        if (!camTexture.isPlaying)
            camTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
