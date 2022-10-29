using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,0x0107,0,0,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,0,0,0,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,0,0,0x1009,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,2,0,0,0,0,0,0,0,0x0109,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0x0104,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
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
            {1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        },
    };

    [SerializeField, Header("プレイヤーをアタッチする")]
    private PlayerView _playerView = null;

    //  当たり判定のマップキャラクター番号
    private List<int> _mapHitTable = new List<int>() { 1, 4, 7, 8 };

    // Start is called before the first frame update
    void Start()
    {
        MapMake();
    }

    /// <summary>
    /// マップデータの取得
    /// </summary>
    /// <param name="pos">移動先座標</param>
    /// <returns>マップデータ</returns>
    private int GetMapData(Vector3Int pos)
    {
        return _mapDataList[0, pos.y, pos.x] & 0xff;
    }

    /// <summary>
    /// マップステータス
    /// </summary>
    /// <param name="pos">移動先座標</param>
    /// <returns>マップ属性</returns>
    private int GetMapStat(Vector3Int pos)
    {
        return _mapDataList[0, pos.y, pos.x] >> 8;
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
        foreach (int y in Enumerable.Range(0,20))
        {
            //  x を０から１９まで変化させる
            foreach (int x in Enumerable.Range(0,20))
            {
                //  プレハブの実態をヒエラルキーに生成する
                GameObject gobj = Instantiate(_mapParts, _parent);
                //  表示座標を設定する
                gobj.transform.localPosition = new Vector3(-304 + x * 32, 304 - y * 32, 0);
                //  マップスプライトの設定
                gobj.GetComponent<ChipView>().SetImage(_mapChipSprites[_mapDataList[0,y,x]]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if(IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Right)))
                _playerView.WalkAction(PlayerView.PlayerDirection.Right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Left)))
                _playerView.WalkAction(PlayerView.PlayerDirection.Left);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if(IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Back)))
                _playerView.WalkAction(PlayerView.PlayerDirection.Back);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if(IsWalkEnable(_playerView.GetNextPosition(PlayerView.PlayerDirection.Front)))
                _playerView.WalkAction(PlayerView.PlayerDirection.Front);
        }
        else if(false == _playerView.IsWalking)
            _playerView.SetAnimationState(PlayerView.PlayerMode.Idle);
    }
}
