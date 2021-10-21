using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraManager : MonoBehaviour
{
    WebCamTexture camTexture;

    // public RawImage cameraViewImage;
    // public GameObject targetObject;         // ī�޶� �������� ȭ��

    public void Start()
    {
        // ī�޶� ���� ȹ��
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        // ī�޶� ���� ���
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("no camera");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        int selectedCamIndex = -1;

        // ���� ī�޶� ã��
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

            camTexture.requestedFPS = 30;   // ī�޶� ������ ����
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
            camTexture.Stop();                      // ī�޶� ����
            WebCamTexture.Destroy(camTexture);      // ī�޶� ��ü ����
            camTexture = null;                      // ī�޶� ���� �ʱ�ȭ
        }
    }
}
