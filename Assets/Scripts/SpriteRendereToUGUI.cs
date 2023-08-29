using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class SpriteRendereToUGUIClass
{
    /// <summary>
    /// SpriteRendererをUGUIのImageに変換
    /// </summary>
    [MenuItem("GameObject/SpriteRendereToUGUI", false, 20)]
    public static void SpriteRendereToUGUI()
    {
        var canvasRect = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        var gameObject = Selection.activeGameObject;
        if (gameObject != null)
        {
            var createObject = CopyObject(gameObject);
            createObject.transform.SetParent(canvasRect, false);
            GetAllChildlen(createObject, SetNativeSize);
        }
    }


    [MenuItem("GameObject/AllSetNativeSize", false, 30)]
    /// <summary>
    /// 選択中のオブジェクトのImageを一括でサイズ初期化を行う。
    /// </summary>
    private static void SetAllNativeSize()
    {
        var gameObject = Selection.activeGameObject;
        GetAllChildlen(gameObject, SetNativeSize);
    }

    /// <summary>
    /// オブジェクトにImageがついている場合サイズを初期化する
    /// </summary>
    /// <param name="obj"></param>
    private static void SetNativeSize(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        var image = obj.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.SetNativeSize();
        }
    }

    /// <summary>
    /// 子オブジェクトを検索して見つかったらコールバックで返す。
    /// </summary>
    /// <param name="parentObj"></param>
    /// <param name="callback"></param>
    private static void GetAllChildlen(GameObject parentObj, UnityAction<GameObject> callback)
    {
        var image = parentObj.GetComponent<UnityEngine.UI.Image>();

        foreach (Transform child in parentObj.GetComponentInChildren<Transform>())
        {
            GetAllChildlen(child.gameObject, callback);
        }

        callback(parentObj);
    }

    private static GameObject CopyObject(GameObject targetObj)
    {

        GameObject createObject = CreateCopyGameObject(targetObj);
        foreach (Transform child in targetObj.GetComponentInChildren<Transform>())
        {
            var childObj = CopyObject(child.gameObject);
            if (childObj != null)
            {
                childObj.transform.SetParent(createObject.transform, false);
            }
        }

        return createObject;
    }

    /// <summary>
    /// 元オブジェクトをコピーしてrecttransformにして生成
    /// </summary>
    /// <param name="targetObjct"></param>
    /// <returns></returns>
    private static GameObject CreateCopyGameObject(GameObject targetObjct)
    {
        if (targetObjct.activeInHierarchy == false)
        {
            return null;
        }

        GameObject copyObject = new GameObject(targetObjct.name);
        RectTransform copyTransform = copyObject.AddComponent<RectTransform>();
        Transform targetTransform = targetObjct.transform;
        SpriteRenderer targetSpriteRenderer = targetObjct.GetComponent<SpriteRenderer>();
        var targetPos = targetTransform.localPosition;

        copyTransform.localScale = Vector2.one;
        copyTransform.localPosition = new Vector2(targetPos.x, targetPos.y);
        copyTransform.localRotation = targetTransform.localRotation;
        AddImageSprite(targetSpriteRenderer, copyObject);
        return copyObject;
    }

    /// <summary>
    /// SpriteRendererをImageに変換
    /// </summary>
    /// <param name="target"></param>
    /// <param name="copy"></param>
    private static void AddImageSprite(SpriteRenderer target, GameObject copy)
    {
        if (target != null)
        {
            var image = copy.AddComponent<UnityEngine.UI.Image>();
            image.sprite = target.sprite;
            //image.SetNativeSize();
        }
    }
}