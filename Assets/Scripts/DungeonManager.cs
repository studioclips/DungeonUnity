using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    //  マップチップのプレハブ
    [SerializeField]
    private GameObject _mapParts = null;

    //  親オブジェクト
    [SerializeField]
    private Transform _parent = null;

    //  マップチップスプライト
    [SerializeField]
    private List<Sprite> _mapChipSprites = new List<Sprite>();

    private int[,,] _mapDataList = new int[,,]
    {
        {
        //   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,0,1,1,1,1,0x0107,0,0,1,1,1,1,1,1,1,1},
            {1,0,1,1,0,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1},
            {1,0,1,1,0,0,0,0,0x0204,0,0,0x2009,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,2,0,0,0,0,0,0,0,0x0109,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0x0104,0,0,0,0,2,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,0x0209,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        },
        {
        //   0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,3,0,0,0,0,0,0,0,6,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,1,1,1,1,1,3,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,0x0207,0,0,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        },
    };

    [SerializeField, Header("プレイヤーをアタッチする")]
    private PlayerView _playerView = null;

    //  生成したマップオブジェクトを格納する場所
    private List<ChipView> _chipViews = new List<ChipView>();

    //  現在の階数（0:１階, 1:２階...）
    private int _mapFloor = 0;

    //  当たり判定のマップキャラクター番号
    private List<int> _mapHitTable = new List<int>() { 1, 4, 7, 8 };

    // Start is called before the first frame update
    void Start()
    {
        //  移動終了のコールバックを登録
        _playerView.SetupWalkEndCallback(EventWalkEndCallback);
        MapMake();
    }

    /// <summary>
    /// マップデータの取得
    /// </summary>
    /// <param name="pos">移動先座標</param>
    /// <returns>マップデータ</returns>
    private int GetMapData(Vector3Int pos)
    {
        return _mapDataList[_mapFloor, pos.y, pos.x] & 0xff;
    }

    /// <summary>
    /// マップステータス
    /// </summary>
    /// <param name="pos">移動先座標</param>
    /// <returns>マップ属性</returns>
    private int GetMapStat(Vector3Int pos)
    {
        return _mapDataList[_mapFloor, pos.y, pos.x] >> 8;
    }
    /// <summary>
    /// 移動可能な場所かどうかのチェック
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool IsWalkEnable(Vector3Int pos)
    {
        int mapData = GetMapData(pos);      //  マップデータの取得
        int mapStat = GetMapStat(pos);      //  マップ情報の取得
        //  マップが壁であり、通り抜け可能なら true を返す
        if (1 == mapStat && 1 == mapData)
            return true;
        //  当たり判定のチップデータを順番にマップデータと比較してヒットしたら移動不可なので false を返す
        foreach (int mapHitChip in _mapHitTable)
        {
            if (mapHitChip == mapData)
                return false;
        }
        //  どのチップとも一致しなければ通り抜け可能なので true を返す
        return true;
    }

    private void MapMake()
    {
        //  y を０から１９まで変化させる
        foreach (int y in Enumerable.Range(0, 20))
        {
            //  x を０から１９まで変化させる
            foreach (int x in Enumerable.Range(0, 20))
            {
                //  プレハブの実態をヒエラルキーに生成する
                GameObject gobj = Instantiate(_mapParts, _parent);
                //  表示座標を設定する
                gobj.transform.localPosition = new Vector3(-304 + x * 32, 304 - y * 32, 0);
                //  マップチップデータの取得
                int mData = GetMapData(new Vector3Int(x, y, 0));
                //  マップスプライトの設定
                gobj.GetComponent<ChipView>().SetImage(_mapChipSprites[mData]);
                //  マップチップオブジェクトの格納
                _chipViews.Add(gobj.GetComponent<ChipView>());
            }
        }
    }

    /// <summary>
    /// マップの再描画
    /// </summary>
    private void RedrawMap()
    {
        //  y を０から１９まで変化させる
        foreach (int y in Enumerable.Range(0, 20))
        {
            //  x を０から１９まで変化させる
            foreach (int x in Enumerable.Range(0, 20))
            {
                int index = y * 20 + x;
                int mData = GetMapData(new Vector3Int(x, y, 0));
                _chipViews[index].SetImage(_mapChipSprites[mData]);
            }
        }
    }

    private void Update()
    {
        if (false == _playerView.IsWalking)
        {
            //  右の矢印を押した？
            if (Input.GetKey(KeyCode.RightArrow))
            {
                bool isWalkEnable = IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Right));
                _playerView.WalkAction(PlayerView.PlayerDirection.Right, isWalkEnable);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                bool isWalkEnable = IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Left));
                _playerView.WalkAction(PlayerView.PlayerDirection.Left, isWalkEnable);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                bool isWalkEnable = IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Back));
                _playerView.WalkAction(PlayerView.PlayerDirection.Back, isWalkEnable);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                bool isWalkEnable = IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Front));
                _playerView.WalkAction(PlayerView.PlayerDirection.Front, isWalkEnable);
            }
            else if (false == _playerView.IsWalking)
            {
                _playerView.SetAnimationState(PlayerView.PlayerMode.Idle);
                //  移動終了していてスペースキーを押した場合
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //  マップのイベントをチェックする
                    MapEventCheck();
                }
            }
        }
    }

    #region イベントチェック

    //  鍵の保存場所
    private List<int> _doorKeys = new List<int>() { 0, 0, 0, 0 };

    private enum AroundDirection
    {
        Up,
        Left,
        Down,
        Right
    }

    //  辞書テーブルの用意（向きのenumとその方向のベクトルをセットにして登録）
    private Dictionary<PlayerView.PlayerDirection, Vector3Int> _directionVecLists =
        new Dictionary<PlayerView.PlayerDirection, Vector3Int>()
        {
            {PlayerView.PlayerDirection.None, Vector3Int.zero},
            {PlayerView.PlayerDirection.Back, Vector3Int.down},
            {PlayerView.PlayerDirection.Left, Vector3Int.left},
            {PlayerView.PlayerDirection.Right, Vector3Int.right},
            {PlayerView.PlayerDirection.Front, Vector3Int.up}
        };

    /// <summary>
    /// 移動終了で呼び出されるコールバック関数
    /// </summary>
    private void EventWalkEndCallback()
    {
        //  落とし穴チェック
        HoleCheck();
        //  ワープチェック
        WarpCheck();
    }

    /// <summary>
    /// 座標から index を取得する
    /// </summary>
    /// <param name="pos">対象の座標</param>
    /// <returns>index</returns>
    private int GetObjectIndex(Vector3Int pos)
    {
        return pos.y * 20 + pos.x;
    }

    /// <summary>
    /// プレイヤー周囲座標検出
    /// </summary>
    /// <param name="aroundDirection">方向</param>
    /// <returns>指定方向の座標</returns>
    private Vector3Int GetAroundPos(AroundDirection aroundDirection)
    {
        switch (aroundDirection)
        {
            case AroundDirection.Left:
                return _playerView.PlayerPos + Vector3Int.left;
            case AroundDirection.Right:
                return _playerView.PlayerPos + Vector3Int.right;
            case AroundDirection.Up:
                return _playerView.PlayerPos + Vector3Int.up;
            case AroundDirection.Down:
                return _playerView.PlayerPos + Vector3Int.down;
        }
        return _playerView.PlayerPos;
    }


    /// <summary>
    /// マップのイベントチェック
    /// </summary>
    private void MapEventCheck()
    {
        //  階段上り下りのチェック
        if (UpFloorCheck())
            DownFloorCheck();
        //  宝箱のチェック
        TreasureCheck();
        //  ドアチェック
        DoorCheck();
    }

    /// <summary>
    /// 登り階段チェック
    /// </summary>
    /// <returns>登れたら false を返す</returns>
    private bool UpFloorCheck()
    {
        int mdata = GetMapData(_playerView.PlayerPos);
        if (2 == mdata)
        {
            _mapFloor++;
            RedrawMap();
            return false;
        }
        return true;
    }

    /// <summary>
    /// 下り階段チェック
    /// </summary>
    private void DownFloorCheck()
    {
        int mdata = GetMapData(_playerView.PlayerPos);
        if (3 == mdata)
        {
            _mapFloor--;
            RedrawMap();
        }
    }

    /// <summary>
    /// 落とし穴チェック
    /// </summary>
    private void HoleCheck()
    {
        int mdata = GetMapData(_playerView.PlayerPos);
        if (6 == mdata)
        {
            _mapFloor--;
            RedrawMap();
        }
    }

    /// <summary>
    /// ワープチェック
    /// </summary>
    private void WarpCheck()
    {
        //  最初のマップデータとステータスを取得する
        int mData = GetMapData(_playerView.PlayerPos);
        int mStat = GetMapStat(_playerView.PlayerPos) >> 4;
        //  ワープポイントかどうかのチェック
        if (9 == mData)
        {
            //  y を０から１９まで変化させる
            foreach (int y in Enumerable.Range(0, 20))
            {
                //  x を０から１９まで変化させる
                foreach (int x in Enumerable.Range(0, 20))
                {
                    //  マップ座標を取得
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    int md = GetMapData(pos);
                    int sd = GetMapStat(pos) & 0x0f;
                    //  マップがワープポイントで移動先のインデックスと移動予定のインデックスが一致すればそこがワープ先
                    if (9 == md && mStat == sd)
                    {
                        _playerView.SetPlayerPosition(pos);
                        return;
                    }
                }
            }
        }
    }

    private void TreasureCheck()
    {
        //  向いている方向の座標を取得
        var checkPos = _playerView.PlayerPos + _directionVecLists[_playerView.PlayerDir];
        //  マップチップデータを取得
        var mData = GetMapData(checkPos);
        //  マップステータスを取得
        var sData = GetMapStat(checkPos);
        //  マップチップデータが閉じた宝箱ならば処理する
        if (7 == mData)
        {
            //  宝箱の鍵の番号を調整（１〜なので０〜に直すために１マイナスする）
            sData--;
            //  持っている鍵が追加される
            _doorKeys[sData]++;
            //  閉じた宝箱を開いた宝箱に変更
            _mapDataList[_mapFloor, checkPos.y, checkPos.x] = 8;
            //  マップチップの配置場所を取得
            var index = checkPos.y * 20 + checkPos.x;
            //  対象のスプライトを入れ替える
            _chipViews[index].SetImage(_mapChipSprites[8]);
        }
    }

    private void DoorCheck()
    {
        //  向いている方向の座標を取得
        var checkPos = _playerView.PlayerPos + _directionVecLists[_playerView.PlayerDir];
        //  マップチップデータを取得
        var mData = GetMapData(checkPos);
        //  マップステータスを取得
        var sData = GetMapStat(checkPos);
        //  マップチップデータが扉ならば処理する
        if (4 == mData)
        {
            //  宝箱の鍵の番号を調整（１〜なので０〜に直すために１マイナスする）
            sData--;
            if (0 < _doorKeys[sData])
            {
                //  持っている鍵が消費される
                _doorKeys[sData]--;
                //  扉を地面に変更
                _mapDataList[_mapFloor, checkPos.y, checkPos.x] = 0;
                //  マップチップの配置場所を取得
                var index = checkPos.y * 20 + checkPos.x;
                //  対象のスプライトを入れ替える
                _chipViews[index].SetImage(_mapChipSprites[0]);
            }
        }
    }

    #endregion

}
