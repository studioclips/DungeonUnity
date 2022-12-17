using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindowManager : MonoBehaviour
{
    public enum MessageType
    {
        UpMessage = 0,
        DownMessage = 1,
        HoleMessage = 2,
        TresureMessage = 3,
        DoorMessage = 4,
        NoKeyMessage = 5,
    }

    //  表示のための操作
    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    [SerializeField]
    private Text _messageText = null;

    [SerializeField]
    private float _messageSpeed = 0.02f;

    [SerializeField]
    private float _fadeSpeed = 0.3f;

    //  ウィンドウの表示状態
    private bool _isDisp = false;
    public bool IsDisp => _isDisp;

    private string _dispMessage = "";

    public Dictionary<MessageType, string> _mapMessages =
        new Dictionary<MessageType, string>()
        {
            {MessageType.UpMessage, "{0}階に上がりました" },
            {MessageType.DownMessage, "{0}階に降りました" },
            {MessageType.HoleMessage, "落とし穴！！！\n下の階に落下！！！" },
            {MessageType.TresureMessage, "{0}の鍵を手に入れました" },
            {MessageType.DoorMessage, "扉を開きました" },
            {MessageType.NoKeyMessage, "{0}の鍵を持っていません" },
        };

    /// <summary>
    /// 鍵の名前
    /// </summary>
    private List<string> _keyNames = new List<string>()
    {
        "普通の","銀の","金の","最後の"
    };
    /// <summary>
    /// メッセージの表示
    /// </summary>
    /// <param name="message">表示するメッセージ</param>
    public void DispMessage(string message)
    {
        _dispMessage = message;
        _messageText.text = "";
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        _canvasGroup.alpha = 0f;
        while(_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime;
            if(1f < _canvasGroup.alpha) _canvasGroup.alpha = 1f;
            yield return null;
        }
        yield return DispCo();
    }

    private IEnumerator DispCo()
    {
        int position = 0;
        //  文字の最後まで表示する
        while(position < _dispMessage.Length)
        {
            _messageText.text = _dispMessage.Substring(0, position);
            position++;
            //  後でスピード調整用の変数を用意してインスペクターで調整する
            yield return new WaitForSeconds(_messageSpeed);
        }
        _messageText.text = _dispMessage;
    }

    /// <summary>
    /// メッセージの表示
    /// </summary>
    /// <param name="messageType">メッセージの種類</param>
    /// <param name="number">数値</param>
    public void DispMessage(MessageType messageType, int number)
    {
        //  メッセージが存在するかどうかの確認
        if(_mapMessages.ContainsKey(messageType))
        {
            if (MessageType.NoKeyMessage == messageType ||
                MessageType.TresureMessage == messageType)
            {
                var msg = string.Format(_mapMessages[messageType], _keyNames[number]);
                DispMessage(msg);
            }
            else
            {
                var msg = string.Format(_mapMessages[messageType], number);
                DispMessage(msg);
            }
        }
    }

    /// <summary>
    /// メッセージウィンドウを閉じる
    /// </summary>
    public void CloseMessage()
    {
        _canvasGroup.alpha = 0;
    }
}
