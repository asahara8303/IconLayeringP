using System;
using System.Collections;
using System.Collections.Generic;
using Negipoyoc.SS;
using Sirenix.OdinInspector;
using Unity.EditorCoroutines.Editor;
using UnityEngine;

public class IconSetting : MonoBehaviour
{
    [Serializable]
    public class InList
    {
        public List<GameObject> List;
    }

    [SerializeField] private List<InList> _list;


    [SerializeField] private List<Camera> _cameraList;

    [SerializeField] private ScreenShot _screenShot;

    [SerializeField] private string _nameSuffix = "icon_";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button("実行")]
    private void OnClick()
    {
        Debug.Log("押した！");
        StartCoroutine(Capture());
    }

    IEnumerator Test()
    {
        _screenShot.CaptureTask("test");
        yield return null;
    }

    IEnumerator Capture()
    {
        foreach (var il in _list)
        {
            foreach (var l in il.List)
            {
                if (l.gameObject != null)
                {
                    l.gameObject.SetActive(false);
                }

            }
        }

        foreach (var c in _cameraList)
        {
            c.gameObject.SetActive(false);
        }

        for (int i_c = 0; i_c < _cameraList.Count; i_c++)
        {
            _cameraList[i_c].gameObject.SetActive(true);
            yield return Calc(0, new List<int>(), _nameSuffix + i_c.ToString("00"));
            _cameraList[i_c].gameObject.SetActive(false);
        }


    }

    IEnumerator Calc(int index, List<int> list, string suffix)
    {
        if (index == _list.Count)
        {
            var t = "";


            for (int i = 0; i < list.Count; i++)
            {
                if (_list[i].List[list[i]] != null)
                {
                    _list[i].List[list[i]].SetActive(true);
                }

            }



            //yield return null;


            foreach (var l in list)
            {
                t += l.ToString("00");

            }

            Debug.Log(suffix + t);
            yield return _screenShot.CaptureTask(suffix + t);
            for (int i = 0; i < list.Count; i++)
            {
                if (_list[i].List[list[i]] != null)
                {
                    _list[i].List[list[i]].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < _list[index].List.Count; i++)
            {
                var l = new List<int>(list);
                l.Add(i);
                yield return Calc(index + 1, l, suffix);
            }
        }

    }
}
