using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Vector3 lastMousePosition;
    private Vector3 newAngle = new Vector3(0, 0, 0);

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // マウスクリック開始(マウスダウン)時にカメラの角度を保持(Z軸には回転させないため).
            newAngle = transform.localEulerAngles;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            // マウスの移動量分カメラを回転させる.
            newAngle.y -= (Input.mousePosition.x - lastMousePosition.x) * -0.1f;
            newAngle.x -= (Input.mousePosition.y - lastMousePosition.y) * 0.1f;
            transform.localEulerAngles = newAngle;

            lastMousePosition = Input.mousePosition;
        }

        // ホイールクリックで移動
        if (Input.GetMouseButton(2))
        {
            transform.position += transform.right * Input.GetAxis("Mouse X") * -0.1f;
            transform.position += transform.up * Input.GetAxis("Mouse Y") * -0.1f;
        }

        // ホイールスクロールでDolly-in, Dolly-out
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // LCtrlが押されていたら、大きく動く
            transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * 15;
        } else
        {
            transform.position += transform.forward * Input.GetAxis("Mouse ScrollWheel") * 3;
        }

    }

}
