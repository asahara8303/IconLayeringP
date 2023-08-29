using System;
using System.Collections;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
// これを適当なGameObjectに貼り付けることで、Shift+Sで通常撮影(透明度ナシ)、Shift+Gで透明度あり撮影を行うことができます。
// https://gist.github.com/negipoyoc/5bf5db8e0187c167a823cb05ff1b2182
namespace Negipoyoc.SS
{
    public class ScreenShot : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
            {
                ScreenCapture.CaptureScreenshot(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
            }
            if (Input.GetKeyDown(KeyCode.G) && Input.GetKey(KeyCode.LeftShift))
            {
                StartCoroutine(CaptureWithAlpha(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
            }
        }

        IEnumerator CaptureWithAlpha(string name)
        {
            yield return new WaitForEndOfFrame();

            var tex = ScreenCapture.CaptureScreenshotAsTexture();

            var width = tex.width;
            var height = tex.height;
            var texAlpha = new Texture2D(width, height, TextureFormat.ARGB32, false);
            // Read screen contents into the texture
            texAlpha.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texAlpha.Apply();

            // Encode texture into PNG
            var bytes = texAlpha.EncodeToPNG();
            Destroy(tex);
            var dirPath = Application.dataPath + "/../output/";
            //ディレクトリがあるか
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var path = dirPath + name + ".png";
            File.WriteAllBytes(path, bytes);
            Debug.Log(path);
        }

        public IEnumerator CaptureTask(string name)
        {
           return CaptureWithAlpha(name);
        }

        [Button("実行")]
        private void OnClick()
        {
            Debug.Log("押した！");
            StartCoroutine(CaptureWithAlpha(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
        }
    }
}