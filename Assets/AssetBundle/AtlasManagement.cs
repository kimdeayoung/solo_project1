using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class AtlasInfo
{
    private string _atlasName;
    private Dictionary<string, Sprite> _spriteInfo;

    public string AtlasName { get => _atlasName; }

    public AtlasInfo(string atlasName, SpriteAtlas spriteAtlas)
    {
        _atlasName = atlasName;
        Sprite[] sprites = new Sprite[spriteAtlas.spriteCount];
        spriteAtlas.GetSprites(sprites);

        _spriteInfo = new Dictionary<string, Sprite>(sprites.Length);
        for (int i = 0; i < sprites.Length; ++i)
        {
            Sprite sprite = sprites[i];
            string spriteName = sprite.name;
            spriteName = spriteName.Split('(')[0];//just remove spriteName + (Clone)
            _spriteInfo.Add(spriteName, sprite);
        }
    }

    public List<string> GetSpriteNames()
    {
        return new List<string>(_spriteInfo.Keys);
    }

    public bool IsExistSpriteName(string spriteName)
    {
        return _spriteInfo.ContainsKey(spriteName);
    }

    public Sprite GetSpriteOrNull(string spriteName)
    {
        if (IsExistSpriteName(spriteName))
        {
            return _spriteInfo[spriteName];
        }

        return null;
    }

    public void Release()
    {
        foreach(Sprite sprite in _spriteInfo.Values)
        {
            Sprite.Destroy(sprite);
        }
        _spriteInfo.Clear();
    }
}

public class AtlasManagement
{
    private List<AtlasInfo> _atalsInfos;

    public AtlasManagement()
    {
        _atalsInfos = new List<AtlasInfo>();
    }

    public void LoadSpriteAtlas(string atlasName)
    {
        if (AddressableBundleLoader.Instance.IsCachedAsset(atlasName) == false)
        {
            _atalsInfos.Add(new AtlasInfo(atlasName, AddressableBundleLoader.Instance.LoadAsset<SpriteAtlas>(atlasName)));
        }
    }

    public void LoadSpriteAtlasAsync(string atlasName, Action onFinished = null)
    {
        if (AddressableBundleLoader.Instance.IsCachedAsset(atlasName) == false)
        {
            AddressableBundleLoader.Instance.LoadAssetAsync(atlasName, (SpriteAtlas atlas) =>
            {
                _atalsInfos.Add(new AtlasInfo(atlasName, atlas));
                onFinished?.Invoke();
            });
        }
    }

#if UNITY_EDITOR
    public void LoadSpriteAtlasForEditor(string atlasName)
    {
        if (AddressableBundleLoader.Instance.IsCachedAsset(atlasName) == false)
        {
            AddressableBundleLoader.Instance.LoadAssetAsync(atlasName, (SpriteAtlas atlas) =>
            {
                _atalsInfos.Add(new AtlasInfo(atlasName, atlas));
                Debug.Log($"{atlasName} : Load Success!");
            },
            () =>
            {
                Debug.LogError($"{atlasName} : Load Failed!");
            });
        }
        else
        {
            Debug.LogError($"{atlasName} : Load Already Finished!");
        }
    }
#endif

    public string GetAtlasName(string spriteName)
    {
        for (int i = 0; i < _atalsInfos.Count; ++i)
        {
            AtlasInfo atlasInfo = _atalsInfos[i];
            if (atlasInfo.IsExistSpriteName(spriteName))
            {
                return atlasInfo.AtlasName;
            }
        }

        return string.Empty;
    }

    public List<string> GetSpriteNamesOrNull(string atlasName)
    {
        for (int i = 0; i < _atalsInfos.Count; ++i)
        {
            AtlasInfo atlasInfo = _atalsInfos[i];
            if (atlasName == atlasInfo.AtlasName)
            {
                return atlasInfo.GetSpriteNames();
            }
        }

        return null;
    }

    public Sprite GetSpriteOrNull(string spriteName)
    {
        for (int i = 0; i < _atalsInfos.Count; ++i)
        {
            AtlasInfo atlasInfo = _atalsInfos[i];
            Sprite sprite = atlasInfo.GetSpriteOrNull(spriteName);
            if (sprite != null)
            {
                return sprite;
            }
        }

        return null;
    }

    public void ClearData()
    {
        for (int i = 0; i < _atalsInfos.Count; ++i)
        {
            _atalsInfos[i].Release();
        }
        _atalsInfos.Clear();
    }
}
