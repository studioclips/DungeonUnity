using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindowManager : MonoBehaviour
{
    //  表示のための操作
    [SerializeField]
    private CanvasGroup _canvasGroup = null;

    [SerializeField]
    private Text _messageText = null;

    //  ウィンドウの表示状態
    private bool _isDisp = false;
    public bool IsDisp => _isDisp;

    private string _dispMessage = "";

    /// <summary>
    /// メッセージの表示
    /// </summary>
    /// <param name="message">表示するメッセージ</param>
    public void DispMessage(string message)
    {
        _dispMessage = message;
    }

    private IEnumerator FedeIn()
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
            yield return new WaitForSeconds(0.01f);
        }
    }
}
