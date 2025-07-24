using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerDirection
    {
        PlayerDown,
        PlayerUp,
        PlayerLeft,
        PlayerRight
    }
    private PlayerLogic _playerLogic;
    //  PlayerView をアタッチする
    [SerializeField] private PlayerView _playerView;

    // Start is called before the first frame update
    void Start()
    {
        _playerLogic = new PlayerLogic();
    }

    // Update is called once per frame
    void Update()
    {
        _playerLogic.Update();
    }
}
