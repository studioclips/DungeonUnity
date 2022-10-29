using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    //  アニメーションに関連あるプレイヤーの状態
    public enum PlayerMode
    {
        Idle = 0,
        RightWalk = 1,
        FrontWalk = 2,
        LeftWalk = 3,
        BackWalk = 4
    }

    //  プレイヤーの向き
    public enum PlayerDirection
    {
        None = 0,
        Right = 1,
        Front = 2,
        Left = 3,
        Back = 4
    }
    //  プレイヤーの状態
    private PlayerMode _playerMode = PlayerMode.Idle;

    //  プレイヤーアニメーションのアニメーターを設定
    [SerializeField]
    private Animator _animator = null;
    
    private Vector3 _ofp = Vector3.zero;

    private Dictionary<PlayerDirection, Vector3> _playerWalkAddLists = new Dictionary<PlayerDirection, Vector3>()
    {
        { PlayerDirection.None , Vector3.zero}, 
        { PlayerDirection.Right, new Vector3(1f, 0f, 0f) },
        { PlayerDirection.Front, new Vector3(0f, -1f, 0f) },
        { PlayerDirection.Left, new Vector3(-1f, 0f, 0f) },
        { PlayerDirection.Back, new Vector3(0f, 1f, 0f) },
    };
    private Dictionary<PlayerDirection, Vector3Int> _playerPosAddLists = new Dictionary<PlayerDirection, Vector3Int>()
    {
        { PlayerDirection.None , Vector3Int.zero}, 
        { PlayerDirection.Right, new Vector3Int(1, 0, 0) },
        { PlayerDirection.Front, new Vector3Int(0, 1, 0) },
        { PlayerDirection.Left, new Vector3Int(-1, 0, 0) },
        { PlayerDirection.Back, new Vector3Int(0, -1, 0) },
    };
    //  移動速度
    [SerializeField]
    private float _walkSpeed = 1f;
    //  移動中かどうかの判定フラグ
    private bool _isWalking = false;
    public  bool IsWalking => _isWalking;
    
    //  プレイヤー初期座標
    public Vector3Int PlayerPos { get; private set; } = new Vector3Int(1, 1, 0);

    private static readonly int     PlayerAnimStat        = Animator.StringToHash("PlayerAnimStat");
    private static readonly Vector3 PlayerInitialPosition = new Vector3(-272f, 272f, 0f);

    private void Awake()
    {
        //  プレイヤーの初期座標を指定する
        transform.localPosition = PlayerInitialPosition;
    }

    /// <summary>
    /// 外部よりアニメーションのモードを切り替える
    /// </summary>
    /// <param name="playerMode">アニメーションの状態</param>
    public void SetAnimationState(PlayerMode playerMode)
    {
        _animator.SetInteger(PlayerAnimStat, (int)playerMode);
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="playerDirection">移動方向</param>
    public void WalkAction(PlayerDirection playerDirection)
    {
        //  すでに移動中ならば何もしない
        if (_isWalking) return;
        _isWalking = true;
        SetAnimationState((PlayerMode)playerDirection);
        //  移動開始
        StartCoroutine(Walking(playerDirection));
    }

    /// <summary>
    /// プレイヤーの次の目的地（座標）を取得する
    /// ** 実際には移動はしない。移動確認用に使用する。
    /// </summary>
    /// <param name="playerDirection">移動方向</param>
    /// <returns>移動予定の場所</returns>
    public Vector3Int GetNextPosition(PlayerDirection playerDirection)
    {
        return (PlayerPos + _playerPosAddLists[playerDirection]);
    }
    
    /// <summary>
    /// 移動のコルーチン
    /// </summary>
    /// <param name="playerDirection">移動方向</param>
    /// <returns></returns>
    private IEnumerator Walking(PlayerDirection playerDirection)
    {
        //  キャラクター座標の更新
        PlayerPos += _playerPosAddLists[playerDirection];
        //  移動前の座標
        Vector3 orgPos = transform.localPosition;
        _ofp = Vector3.zero;
        while (true)
        {
            //  加算する座標を計算する
            _ofp                    += _playerWalkAddLists[playerDirection] * _walkSpeed;
            //  移動範囲が１ブロックを超えたら、１ブロック分に直す
            if (_ofp.magnitude > 32f)
            {
                _ofp                    = _playerWalkAddLists[playerDirection] * 32;
                //  座標を更新する
                transform.localPosition = orgPos + _ofp;
                //  ループを抜ける
                break;
            }
            //  座標を更新する
            transform.localPosition =  orgPos + _ofp;
            //  次の update まで待つ
            yield return null;
        }
        _isWalking = false;
    }
}
