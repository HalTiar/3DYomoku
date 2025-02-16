using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras; // 4つのカメラをインスペクターで設定
    private int currentCameraIndex = 0;

    void Start()
    {
        // 最初のカメラ以外を無効にする
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].enabled = (i == currentCameraIndex);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchToNextCamera();
        }
    }

    void SwitchToNextCamera()
    {
        cameras[currentCameraIndex].enabled = false; // 現在のカメラを無効化
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length; // 次のカメラのインデックスを取得
        cameras[currentCameraIndex].enabled = true; // 次のカメラを有効化
    }
}
