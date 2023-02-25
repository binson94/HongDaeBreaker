using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Init();

            return _instance;
        }
    }

    InputManager _input = new InputManager();
    public InputManager Input { get { return _input; } }
    SpriteHandler _spriteHandle = new SpriteHandler();
    public SpriteHandler SpriteHandle { get { return _spriteHandle; } }

    static void Init()
    {
        if (_instance == null)
        {
            GameObject container = new GameObject() { name = "@GameManager" };
            _instance = container.AddComponent<GameManager>();
            DontDestroyOnLoad(container);
        }
    }

    private void FixedUpdate()
    {
        _input.OnFixedUpdate();
    }

    private void Update()
    {
        _input.OnUpdate();
    }
}
