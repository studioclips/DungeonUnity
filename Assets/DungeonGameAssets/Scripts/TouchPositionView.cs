using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TouchPositionView : MonoBehaviour
{
    //  Tilemap 情報をアタッチする
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Tilemap _eventTileMap;
    //  情報の表示テキストをアタッチする
    [SerializeField] private TextMeshProUGUI _tileMapDataText;
    //  input system
    private PlayerGameInput _playerGameInput;

    // Start is called before the first frame update
    void Start()
    {
        //  Input System の取得
        _playerGameInput = new PlayerGameInput();
        _playerGameInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //  左クリック押し
        if (_playerGameInput.Player.LeftClick.triggered)
        {
            Debug.Log("tap");
            // デバイスを取得
            var pointer = Pointer.current;
            Debug.Log($"pointer:{pointer}");
            if (pointer == null)
                return;

            // クリック位置を取得
            var pos = pointer.position.ReadValue();
            var position = Camera.main.ScreenToWorldPoint(pos);
            position.z = 0;
            var tileBasePos = _tilemap.WorldToCell(position);
            _tileMapDataText.text = $"({tileBasePos.x},{tileBasePos.y},{tileBasePos.z})";
        }
    }
}
