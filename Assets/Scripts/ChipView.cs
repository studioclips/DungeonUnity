using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipView : MonoBehaviour
{
    //  マップチップイメージ
    [SerializeField]
    private Image _image = null;


    /// <summary>
    /// マップスプライトを設置する
    /// </summary>
    /// <param name="sprite">表示するイメージ</param>
    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
