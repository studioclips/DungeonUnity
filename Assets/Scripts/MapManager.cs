using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // マップチッププレハブ
    [SerializeField]
    private GameObject _mapChip = null;
    
    // マップチップ配置エリア
    [SerializeField]
    private Transform _mapArea = null;

    //  マップチップイメージ
    [SerializeField]
    private List<Sprite> _mapSprites = new List<Sprite>();

    //  生成したマップチップリスト
    private List<MapChipView> _mapChipViews = new List<MapChipView>();

    private List<List<int>> _mapData  = new List<List<int>>()
    {
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        new List<int>(){1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    };
    private int             _startX   = -456; //マップチップ開始X座標
    private int             _startY   = 456;  //マップチップ開始Y座標う
    private int             _chipSize = 48;   // マップチップの大きさ
    
    // Start is called before the first frame update
    void Start()
    {
        MakeMap();
    }

    /// <summary>
    /// マップ生成
    /// </summary>
    private void MakeMap()
    {
        //  縦のチップを２０並べる
        for (int y = 0; y < 20; y++)
        {
            //  横のチップを２０並べる
            for (int x = 0; x < 20; x++) 
            {
                //  プレハブから実体を生成してマップエリアに貼り付ける
                GameObject gobj = Instantiate(_mapChip, _mapArea);
                Transform trans = gobj.transform;
                //  表示位置を左上を基準に縦、横の場所を基準に算出して貼り付ける
                trans.localPosition = new Vector3(_startX + _chipSize * x,
                                                  _startY - _chipSize * y,
                                                  0);
                //  マップチップViewコンポーネントを取得する
                MapChipView mpView   = gobj.GetComponent<MapChipView>();
                //  マップ上の選択場所にあるマップチップ番号取得
                int         mapIndex = _mapData[y][x];
                //  対応するスプライトを設定
                mpView.SetMapImage(_mapSprites[mapIndex]);
                //  マップチップデータを保存する（再描画で使用するため）
                _mapChipViews.Add(mpView);
            }
        }
    }
}
