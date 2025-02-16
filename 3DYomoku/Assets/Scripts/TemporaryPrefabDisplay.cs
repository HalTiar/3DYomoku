using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TemporaryPrefabDisplay : MonoBehaviour
{
    public GameObject TemporaryPrefabWhite;
    public GameObject TemporaryPrefabBlack;

    public GameManager GameManager;

    // カーソルが乗った時の処理
    void OnMouseDown(){
        Debug.Log("test");
        if (gameObject.CompareTag("BoardParts")){
            Debug.Log("Mouse Enter");
            int x = (int)transform.position.x;
            int y = (int)transform.position.z;
            if (GameManager.Instance != null){
                if (GameManager.Instance.currentPlayer == 0){
                    Vector3 PrefabPosition = new Vector3(x, 0, y);
                    Instantiate(TemporaryPrefabBlack, PrefabPosition, Quaternion.identity, transform);
                }
                if(GameManager.Instance.currentPlayer == 1){
                    Vector3 PrefabPosition = new Vector3(x, 0, y);
                    Instantiate(TemporaryPrefabWhite, PrefabPosition, Quaternion.identity, transform);
                }
            }
        }
    }

    // カーソルが離れたときの処理
    void OnMouseExit(){

    }
}
