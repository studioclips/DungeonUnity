using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TilePositionView : MonoBehaviour
{
    //  横のテキスト並び
    [System.Serializable]
    public class HorizontalPosTexts
    {
        public List<TextMeshProUGUI> TextMeshes = new List<TextMeshProUGUI>();
    }
    //  縦のテキスト並び
    [SerializeField]
    private List<HorizontalPosTexts> _posTexts = new List<HorizontalPosTexts>();
    
    [Button]
    public void SetTilePosTexts()
    {
        if (_posTexts.Count is 0) return;
        //  縦に２０回分チェックする
        for (int y = 0; y < _posTexts.Count; y++)
        {
            if (_posTexts[y].TextMeshes.Count == 0) return;
            //  横に２０回分チェックする
            for (int x = 0; x < _posTexts[y].TextMeshes.Count; x++)
            {
                //  左と上からのポジションを、左上の原点が(-18, 9, 0)なのでこれを基準に
                //  作成する
                var pos = new Vector3Int(-18 + x, 9 - y, 0);
                _posTexts[y].TextMeshes[x].text = pos.ToString();
            }
        }
    }
}
