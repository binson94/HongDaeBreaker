using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler
{
    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public Sprite LoadSprite(string path)
    {
        Sprite ret;
        if (sprites.TryGetValue(path, out ret))
            return ret;

        ret = Load<Sprite>($"Sprites/{path}");
        sprites.Add(path, ret);

        return ret;
    }
}
