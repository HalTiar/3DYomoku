using System.Collections.Generic;
using UnityEngine;

public class PositionDisplay : MonoBehaviour
{
    public CameraBasedSpawner cameraBasedSpawner; // CameraBasedSpawner スクリプトへの参照

    void Update()
    {
        // 座標を表示（例: Pキーで実行）
        if (Input.GetKeyDown(KeyCode.P))
        {
            DisplayPositions();
        }
    }

    void DisplayPositions()
    {
        List<Vector3> positions = cameraBasedSpawner.GetObjectPositions();

        if (positions.Count == 0)
        {
            Debug.Log("生成されたオブジェクトがありません。");
            return;
        }

        Debug.Log("生成されたオブジェクトの座標:");

        for (int i = 0; i < positions.Count; i++)
        {
            Debug.Log($"オブジェクト {i + 1}: X={positions[i].x:F2}, Y={positions[i].y:F2}, Z={positions[i].z:F2}");
        }
    }
}