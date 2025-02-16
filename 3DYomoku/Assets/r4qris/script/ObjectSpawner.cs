using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBasedSpawner : MonoBehaviour
{
    public GameObject prefab; // 生成するPrefab
    public Camera[] cameras; // 複数のカメラを格納する配列
    private int activeCameraIndex = 0; // 現在アクティブなカメラのインデックス
    public string clickableTag = "Clickable"; // クリック可能なオブジェクトのタグ
    public List<GameObject> spawnedObjects = new List<GameObject>(); // 生成されたオブジェクトのリスト

    void Start()
    {
        if (cameras.Length > 0)
        {
            SwitchCamera(activeCameraIndex);
        }
        else
        {
            Debug.LogError("カメラが設定されていません！");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnObjectAtClick();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCamera(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchCamera(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchCamera(4);
    }

    void SpawnObjectAtClick()
    {
        if (cameras.Length == 0)
        {
            Debug.LogError("カメラが設定されていません！");
            return;
        }

        if (activeCameraIndex < 0 || activeCameraIndex >= cameras.Length)
        {
            Debug.LogError($"activeCameraIndex ({activeCameraIndex}) が無効です。");
            return;
        }

        Camera activeCamera = cameras[activeCameraIndex];
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            if (!clickedObject.CompareTag(clickableTag))
            {
                Debug.Log("クリック可能なオブジェクトではありません。");
                return;
            }

            Collider clickedCollider = hit.collider;
            Collider prefabCollider = prefab.GetComponent<Collider>();

            if (clickedCollider != null && prefabCollider != null)
            {
                Vector3 rayOrigin = clickedCollider.bounds.center;
                rayOrigin.y = clickedCollider.bounds.max.y + 0.1f;

                RaycastHit downHit;
                bool originalState = clickedCollider.enabled;
                clickedCollider.enabled = false;

                if (Physics.Raycast(rayOrigin, Vector3.down, out downHit, Mathf.Infinity))
                {
                    GameObject belowObject = downHit.collider.gameObject;

                    Vector3 spawnPosition = new Vector3(
                        clickedCollider.bounds.center.x,
                        downHit.collider.bounds.max.y + prefabCollider.bounds.extents.y,
                        clickedCollider.bounds.center.z
                    );

                    GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

                    spawnedObjects.Add(spawnedObject); // リストに追加

                    Collider spawnedCollider = spawnedObject.GetComponent<Collider>();
                    if (spawnedCollider != null)
                    {
                        spawnedCollider.enabled = false;
                        StartCoroutine(EnableColliderAfterFrame(spawnedCollider));
                    }

                    spawnedObject.transform.SetParent(belowObject.transform);

                    Debug.Log($"Prefab を生成しました。位置: {spawnPosition} 親オブジェクト: {belowObject.name}");
                }
                else
                {
                    Debug.Log("下方向にオブジェクトが見つかりませんでした。");
                }

                clickedCollider.enabled = originalState;
            }
            else
            {
                Debug.LogError("クリックされたオブジェクトまたはPrefabにコライダーがありません！");
            }
        }
        else
        {
            Debug.Log("クリックした場所にヒットするオブジェクトがありませんでした。");
        }
    }

    void SwitchCamera(int cameraIndex)
    {
        if (cameraIndex < 0 || cameraIndex >= cameras.Length)
        {
            Debug.LogError($"指定されたカメラインデックス {cameraIndex} は無効です。");
            return;
        }

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == cameraIndex);
        }

        activeCameraIndex = cameraIndex;
    }

    private System.Collections.IEnumerator EnableColliderAfterFrame(Collider collider)
    {
        yield return null;
        collider.enabled = true;
    }

    // 座標リストを取得するメソッド
    public List<Vector3> GetObjectPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        foreach (GameObject obj in spawnedObjects)
        {
            positions.Add(obj.transform.position); // 各オブジェクトの座標をリストに追加
        }

        return positions;
    }
}