using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCameraController : MonoBehaviour
{
    public LogViewer Viewer;
    public int CamNum = 0;
    public int FPS = 30;
    public bool DEBUG = false;

    private Quaternion baseRotate; // 最初の回転
    private DeviceOrientation currentOrientation; // デバイスの向き

    // Use this for initialization
    private void Start()
    {
        baseRotate = this.GetComponent<RectTransform>().rotation;
        ApplyDeviceRotate();

        // ウェブカメラデバイスの列挙
        var devices = WebCamTexture.devices;
        if (DEBUG)
        {
            for (var i = 0; i < devices.Length; i++)
            {
                Viewer.GetComponent<LogViewer>().AddLine("Cam[" + i + "] : " + devices[i].name);
            }
        }

        // デバイスが存在したらRawImageのテクスチャをウェブカメラにする
        if (devices.Length > 0)
        {
            if (CamNum > devices.Length) CamNum = 0;

            var wcam = new WebCamTexture(devices[CamNum].name);
            wcam.Play();
            var webcamTexture = new WebCamTexture(devices[CamNum].name, wcam.width, wcam.height, FPS);
            wcam.Stop();

            this.GetComponent<RawImage>().material.mainTexture = webcamTexture;
            webcamTexture.Play();
        }
        else
        {
            Debug.LogError("Webカメラを検出できません！");
            if (DEBUG) Viewer.GetComponent<LogViewer>().AddLine("Webカメラを検出できません！");
        }

    }

    // Update is called once per frame
    private void Update()
    {
        // canvasのサイズが変更されたタイミングを知ることができないので、
        // 毎フレーム実行することで適応させる(ゴリ押し)
        ApplyDeviceRotate();

    }

    private void ApplyDeviceRotate()
    {
        // Androidでなければ回転しない
        if (!(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)) return;

        var canvas = this.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        var changeState = true;

        // 回転とサイズを変更する
        switch (Input.deviceOrientation)
        {
            // 左に倒した時
            case DeviceOrientation.LandscapeLeft:
                this.GetComponent<RectTransform>().rotation = baseRotate;
                this.GetComponent<RectTransform>().sizeDelta = canvas.sizeDelta;
                break;

            // 右に倒した時
            case DeviceOrientation.LandscapeRight:
                this.GetComponent<RectTransform>().rotation = baseRotate * Quaternion.Euler(0, 0, 180);
                this.GetComponent<RectTransform>().sizeDelta = canvas.sizeDelta;
                break;

            // 縦上向き
            case DeviceOrientation.Portrait:
                this.GetComponent<RectTransform>().rotation = baseRotate * Quaternion.Euler(0, 0, -90);
                this.GetComponent<RectTransform>().sizeDelta = new Vector2(canvas.sizeDelta.y, canvas.sizeDelta.x);
                break;

            // 縦下向き
            case DeviceOrientation.PortraitUpsideDown:
                this.GetComponent<RectTransform>().rotation = baseRotate * Quaternion.Euler(0, 0, 90);
                this.GetComponent<RectTransform>().sizeDelta = new Vector2(canvas.sizeDelta.y, canvas.sizeDelta.x);
                break;

            // 画面が上/下向きの時と向きがわからない時は回転しない
            case DeviceOrientation.FaceUp:
            case DeviceOrientation.FaceDown:
            case DeviceOrientation.Unknown:
                changeState = false;
                break;

            default:
                break;
        }

        if (changeState) currentOrientation = Input.deviceOrientation;

    }

}
