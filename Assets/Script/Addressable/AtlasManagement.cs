using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using System;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
#if UNITY_EDITOR
            Debug.Assert(!_spriteInfo.ContainsKey(spriteName));
#endif
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
        AddressableBundleLoader.Instance.ReleaseLoadedAsset(_atlasName);
        _spriteInfo.Clear();
    }
}

public class AtlasManagement
{
    private List<AtlasInfo> _atlasInfos;

    public AtlasManagement()
    {
        _atlasInfos = new List<AtlasInfo>();
    }

    public void LoadSpriteAtlasAsync(string atlasName, Action<string> onFinished = null)
    {
        if (AddressableBundleLoader.Instance.IsCachedAsset(atlasName) == false)
        {
            AddressableBundleLoader.Instance.LoadAssetAsync(atlasName, (SpriteAtlas atlas) =>
            {
                _atlasInfos.Add(new AtlasInfo(atlasName, atlas));
                onFinished?.Invoke(atlasName);
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
                _atlasInfos.Add(new AtlasInfo(atlasName, atlas));
                Debug.Log($"{atlasName} : Load Success!");
            },
            (string name) =>
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
        int atlasInfoCount = _atlasInfos.Count;
        for (int i = 0; i < atlasInfoCount; ++i)
        {
            AtlasInfo atlasInfo = _atlasInfos[i];
            if (atlasInfo.IsExistSpriteName(spriteName))
            {
                return atlasInfo.AtlasName;
            }
        }

        return string.Empty;
    }

    public List<string> GetSpriteNamesOrNull(string atlasName)
    {
        int atlasInfoCount = _atlasInfos.Count;
        for (int i = 0; i < atlasInfoCount; ++i)
        {
            AtlasInfo atlasInfo = _atlasInfos[i];
            if (atlasName == atlasInfo.AtlasName)
            {
                return atlasInfo.GetSpriteNames();
            }
        }

        return null;
    }

    public Sprite GetSpriteOrNull(string spriteName)
    {
        int atlasInfoCount = _atlasInfos.Count;
        for (int i = 0; i < atlasInfoCount; ++i)
        {
            AtlasInfo atlasInfo = _atlasInfos[i];
            Sprite sprite = atlasInfo.GetSpriteOrNull(spriteName);
            if (sprite != null)
            {
                return sprite;
            }
        }

        return null;
    }

    public void ReleaseSpecificAtlasInfo(string atlasName)
    {
        int atlasInfoCount = _atlasInfos.Count;
        for (int i = 0; i < atlasInfoCount; ++i)
        {
            AtlasInfo atlasInfo = _atlasInfos[i];
            if (atlasName == atlasInfo.AtlasName)
            {
                atlasInfo.Release();
                _atlasInfos.RemoveAtSwapBack(i);
                break;
            }
        }
    }

    public void ClearData()
    {
        for (int i = 0; i < _atlasInfos.Count; ++i)
        {
            _atlasInfos[i].Release();
        }
        _atlasInfos.Clear();
    }
}

public static class ImageExtensions
{
    public static void SetSprite(this Image image, string spriteName)
    {
        Debug.Assert(!string.IsNullOrEmpty(spriteName), "spriteName is Null or Empty");

        Sprite sprite = AddressableBundleLoader.Instance.TryGetLoadedAsset<Sprite>(spriteName);

        Debug.Assert(sprite != null, $"Sprite Can't Find Check SpriteName Name : {spriteName}");
        image.sprite = sprite;
    }
}

public static class RawImageExtensions
{
    public static void SetRawSpriteAsync(this RawImage image, string rawSpriteName)
    {
        Debug.Assert(!string.IsNullOrEmpty(rawSpriteName), "RawSpriteName is Null or Empty");

        Texture texture = AddressableBundleLoader.Instance.TryGetLoadedAsset<Texture>(rawSpriteName);
        Debug.Assert(texture != null, $"RawSpriteName Can't Find Check Texture Name : {rawSpriteName}");
        image.texture = texture;
    }
}
