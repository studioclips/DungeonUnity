using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
#region データ関連

    //  アニメーションに関連あるプレイヤーの状態
    public enum PlayerMode
    {
        Idle      = 0,
        RightWalk = 1,
        FrontWalk = 2,
        LeftWalk  = 3,
        BackWalk  = 4,
        RIdle     = 5,
        FIdle     = 6,
        LIdle     = 7,
        BIdle     = 8,
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

    private static readonly int     _frontAction          = Animator.StringToHash("FrontAction");
    private static readonly int     _rightAction          = Animator.StringToHash("RightAction");
    private static readonly int     _leftAction           = Animator.StringToHash("LeftAction");
    private static readonly int     _backAction           = Animator.StringToHash("BackAction");
    private static readonly int     _idleAction           = Animator.StringToHash("IdleAction");
    private static readonly int     _frontIdle          = Animator.StringToHash("FIdle");
    private static readonly int     _rightIdle          = Animator.StringToHash("RIdle");
    private static readonly int     _leftIdle           = Animator.StringToHash("LIdle");
    private static readonly int     _backIdle           = Animator.StringToHash("BIdle");
    private static readonly Vector3 PlayerInitialPosition = new Vector3(-272f, 272f, 0f);
    private static readonly Vector3 MapLeftTopPosition    = new Vector3(-304, 304, 0);

    //  移動終了時呼び出すコールバック関数
    private Action _walkEndCallback = null;
    //  現在のプレイヤーの向き
    private PlayerDirection _playerDirection = PlayerDirection.None;
    //  外部からのアクセスは read only として書き換えられないようにする
    public PlayerDirection PlayerDir => _playerDirection;
    // public PlayerDirection PlayerDir {get{return _playerDirection;}}

#endregion
    

    private void Awake()
    {
        //  プレイヤーの初期座標を指定する
        transform.localPosition = PlayerInitialPosition;
    }

    /// <summary>
    /// 移動終了時に呼び出す callback 関数の登録
    /// </summary>
    /// <param name="walkEndCallback">登録するコールバック関数</param>
    public void SetupWalkEndCallback(Action walkEndCallback)
    {
        _walkEndCallback = walkEndCallback;
    }

    /// <summary>
    /// 外部よりアニメーションのモードを切り替える
    /// </summary>
    /// <param name="playerMode">アニメーションの状態</param>
    public void SetAnimationState(PlayerMode playerMode)
    {
        //  同じアニメーションをリクエストしていたら再呼び出ししない
        if(_playerMode == playerMode) return;
        _playerMode = playerMode;
        switch (playerMode)
        {
            case PlayerMode.FrontWalk: _animator.SetTrigger(_frontAction); break;
            case PlayerMode.RightWalk: _animator.SetTrigger(_rightAction); break;
            case PlayerMode.LeftWalk:  _animator.SetTrigger(_leftAction); break;
            case PlayerMode.BackWalk:  _animator.SetTrigger(_backAction); break;
            case PlayerMode.FIdle: _animator.SetTrigger(_frontIdle); break;
            case PlayerMode.RIdle: _animator.SetTrigger(_rightIdle); break;
            case PlayerMode.LIdle:  _animator.SetTrigger(_leftIdle); break;
            case PlayerMode.BIdle:  _animator.SetTrigger(_backIdle); break;
            default:                   _animator.SetTrigger(_idleAction); break;
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="playerDirection">移動方向</param>
    public void WalkAction(PlayerDirection playerDirection, bool isWalkEnable)
    {
        //  すでに移動中ならば何もしない
        if (_isWalking) return;
        _isWalking = true;
        SetAnimationState((PlayerMode)playerDirection);
        if (false == isWalkEnable)
        {
            _isWalking = false;
            return;
        }
        //  移動開始
        StartCoroutine(Walking(playerDirection));
    }

    /// <summary>
    /// プレイヤーの位置を設定する
    /// </summary>
    /// <param name="pos">表示する位置</param>
    public void SetPlayerPosition(Vector3Int pos)
    {
        //  プレイヤー管理座標を更新
        PlayerPos = pos;
        //  プレイヤー位置座標を更新
        transform.localPosition =
            MapLeftTopPosition + (Vector3)(Vector3Int.right * pos.x * 32 + Vector3Int.down * pos.y * 32);
    }
    
    /// <summary>
    /// プレイヤーの次の目的地（座標）を取得する
    /// ** 実際には移動はしない。移動確認用に使用する。
    /// </summary>
    /// <param name="playerDirection">移動方向</param>
    /// <returns>移動予定の場所</returns>
    public Vector3Int GetNextPosition(PlayerDirection playerDirection)
    {
        //  向いている方向を保存する
        _playerDirection = playerDirection;
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
        if (null != _walkEndCallback)
            _walkEndCallback();
    }

    /// <summary>
    /// キーを入力した際のプレイヤーの向き設定
    /// </summary>
    /// <param name="playerDirection">プレイヤーの向き</param>
    public void PlayerDirectionSet(PlayerDirection playerDirection)
    {
        switch (playerDirection)
        {
            case PlayerDirection.Back: _animator.SetTrigger(_backIdle); break;
            case PlayerDirection.Front: _animator.SetTrigger(_frontIdle); break;
            case PlayerDirection.Left: _animator.SetTrigger(_leftIdle); break;
            case PlayerDirection.Right: _animator.SetTrigger(_rightIdle); break;
        }
    }
}
