using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ボードのサイズ設定
    public int boardWidth = 4;
    public int boardHeight = 4;
    public int boardDepth = 4;

    // 盤面の状態を表す３次元配列(空=-1, プレイヤー１=0, プレイヤー２=1)
    private int [,,] board;

    // 現在のターン(0 or 1)
    public int currentPlayer = 0;
    
    public void Start()
    {
        InitBoard();
        
    }

    // 盤面の初期化
    public void InitBoard()
    {
        board = new int[boardWidth, boardHeight, boardDepth];
        for (int x = 0; x < boardWidth; x++){
            for (int y = 0; y < boardHeight; y++){
                for (int z = 0; z < boardDepth; z++){
                    board[x, y, z] = -1;
                }
            }
        }
    }

    // 石を配置する処理
    public void PlacePiece(GameObject stone)
    {
        int x, y, z;
        // 座標を配列に変換する関数
        Vector3 stoneposition = WorldToBoard(stone.transform.position);
        x = (int)stoneposition.x;
        y = (int)stoneposition.z;
        
        // 石が置けるか判定
        while (!IsValidPlacement(x, y)){
            Debug.LogError("石を置けません");
        }
        // 石を置く
        for (z = 0; z < 4; z++){
            if (board[x, y, z] == -1){
                board[x, y, z] = currentPlayer;
                Debug.Log(x + "," + y + "," + z);
                break;
            }
        }
        // 勝利判定
        if (CheckWin(x, y, z)){
            EndGame(true, currentPlayer);
        }else{
            NextTurn();
        }
    }

    // 座標から配列に変換
    public Vector3Int WorldToBoard(Vector3 worldPos)
    {
        int x = (int)worldPos.x;
        int y = (int)worldPos.y;
        int z = (int)worldPos.z;
        return new Vector3Int(x, y, z);
    }

    // 石が置けるかのチェック
    public bool IsValidPlacement(int x, int y)
    {
        for (int z = 0; z < 4; z++){
            if (board[x, y, z] == -1){
                return true;
            }
        }
        return false;
    }
    

    // 勝利条件のチェック
    public bool CheckWin(int cx, int cy, int cz)
    {
        // チェックする方向のベクトルの配列
        Vector3Int[] directions = {
            new Vector3Int(1,0,0), new Vector3Int(0,1,0), new Vector3Int(0,0,1),
            new Vector3Int(1,1,0), new Vector3Int(1,-1,0), new Vector3Int(1,0,1),
            new Vector3Int(1,0,-1), new Vector3Int(0,1,1), new Vector3Int(0,1,-1),
            new Vector3Int(1,1,1), new Vector3Int(1,-1,1), new Vector3Int(1,1,-1),
            new Vector3Int(1,-1,-1)
        };

        foreach (Vector3Int dir in directions){
            int count = 1;
            count += CountInDirection(cx, cy, cz, dir.x, dir.y, dir.z);
            count += CountInDirection(cx, cy, cz, -dir.x, -dir.y, -dir.z);

            if (count >= 4) return true;
        }
        return false;
    }

    // 指定方向に連続する石の数を確認
    private int CountInDirection(int startX, int startY, int startZ, int dx, int dy, int dz)
    {
        int count = 0;
        int x = startX, y = startY, z = startZ;
        while (true){
            x += dx; y += dy; z += dz;
            if (x < 0 || x >= boardWidth || y < 0 || y >= boardHeight || z < 0 || z >= boardDepth) break;
            if (board[x, y, z] == currentPlayer) count++;
            else break;
        }
        return count;
    }

    // ターン切り替え
    public void NextTurn()
    {
        currentPlayer = (currentPlayer + 1) % 2;
        Debug.Log("次のターン");
    }

    // ゲーム終了時の処理
    public void EndGame(bool isWin, int winner)
    {
        if (isWin){
            Debug.Log(winner + "の勝利");
        }else{
            Debug.Log("引き分け");
        }
    }

    // リスタート
    public void RestartGame()
    {
        InitBoard();
        currentPlayer = 0;
    }

}
