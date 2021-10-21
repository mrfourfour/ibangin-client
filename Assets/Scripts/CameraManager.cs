using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManager : MonoBehaviour
{
    WebCamTexture camTexture;

    // public RawImage cameraViewImage;
    // public GameObject targetObject;         // 카메라가 보여지는 화면

    public void Start()
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
        int selectedCamIndex = -1;

        // 전면 카메라 찾기
        for (int i = 0; i < devices.Length; ++i)
        {
            if (devices[i].isFrontFacing == true)
            {
                selectedCamIndex = i;
                break;
            }
        }

        if (selectedCamIndex >= 0)
        {
            camTexture = new WebCamTexture(devices[selectedCamIndex].name);

            camTexture.requestedFPS = 30;   // 카메라 프레임 설정
        }

        if(camTexture != null)
        {
            Renderer render = GetComponent<Renderer>();
            render.material.mainTexture = camTexture;

            camTexture.Play();
        }
    }

    public void CameraOff()
    {
        if (camTexture != null)
        {
            camTexture.Stop();                      // 카메라 정지
            WebCamTexture.Destroy(camTexture);      // 카메라 객체 삭제
            camTexture = null;                      // 카메라 변수 초기화
        }
    }
}
