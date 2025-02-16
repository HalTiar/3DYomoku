using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopToBottomRaycast : MonoBehaviour
{
    public GameObject previewPrefab; // プレビュー用のプレハブ
    private GameObject currentPreview; // 現在表示中のプレビュー
    private bool hasHit = false; // 衝突済みかどうかを判定するフラグ
    public float transparency = 0.5f; // 透明度（0.0〜1.0）
    public Color previewColor = new Color(1f, 1f, 1f, 0.5f); // 薄い色（RGBA）

    void Update()
    {
        // カーソルが乗っているオブジェクトを検出
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Clickable"))
            {
                hasHit = true; // カーソルが「Clickable」に乗った場合にフラグを立てる

                Vector3 topPoint = hitObject.GetComponent<Renderer>().bounds.center +
                                   Vector3.up * (hitObject.GetComponent<Renderer>().bounds.size.y / 2);

                Collider objectCollider = hitObject.GetComponent<Collider>();
                objectCollider.enabled = false;

                if (Physics.Raycast(topPoint, Vector3.down, out hit))
                {
                    GameObject targetObject = hit.collider.gameObject;

                    if (targetObject.CompareTag("Target"))
                    {
                        ShowPreview(hit.point, targetObject, hitObject);
                        DisablePreviewCollider();
                    }
                }

                objectCollider.enabled = true;
                return;
            }
        }

        // カーソルが「Clickable」以外の場所に移動した場合、プレビューを削除
        if (hasHit)
        {
            RemovePreview();
            hasHit = false; // フラグリセット
        }
    }

    private void ShowPreview(Vector3 position, GameObject targetObject, GameObject hitObject)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        currentPreview = Instantiate(previewPrefab, position, Quaternion.identity);

        Collider previewCollider = currentPreview.GetComponent<Collider>();
        Collider targetCollider = targetObject.GetComponent<Collider>();

        if (previewCollider != null && targetCollider != null)
        {
            Vector3 targetTopPoint = targetCollider.bounds.center + Vector3.up * (targetCollider.bounds.size.y / 2);
            Vector3 previewBottomPoint = previewCollider.bounds.center - Vector3.up * (previewCollider.bounds.size.y / 2);
            Vector3 offset = targetTopPoint - previewBottomPoint;

            currentPreview.transform.position += offset;
            Vector3 previewPosition = currentPreview.transform.position;
            previewPosition.x = hitObject.transform.position.x;
            previewPosition.z = hitObject.transform.position.z;
            currentPreview.transform.position = previewPosition;

            // プレハブの色と透明度を変更
            SetPreviewTransparency(previewColor);
        }
    }

    private void DisablePreviewCollider()
    {
        if (currentPreview != null)
        {
            Collider previewCollider = currentPreview.GetComponent<Collider>();
            if (previewCollider != null)
            {
                previewCollider.enabled = false;
            }
        }
    }

    private void RemovePreview()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
    }

    private void SetPreviewTransparency(Color color)
    {
        if (currentPreview != null)
        {
            Renderer renderer = currentPreview.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;

                // マテリアルのレンダリングモードを確認・設定
                material.SetFloat("_Mode", 2); // Transparent モード
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;

                // 色と透明度を設定
                material.color = color;

                renderer.material = material; // マテリアルを再設定
            }
        }
    }
}