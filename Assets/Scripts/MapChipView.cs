using UnityEngine;
using UnityEngine.UI;

public class MapChipView : MonoBehaviour
{
    //  イメージコンポーネント
    [SerializeField]
    private Image _image = null;

    /// <summary>
    /// スプライトを設定する
    /// </summary>
    /// <param name="sprite">スプライトでーた</param>
    public void SetMapImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
