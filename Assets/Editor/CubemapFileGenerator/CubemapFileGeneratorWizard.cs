using UnityEngine;
using UnityEditor;
using System.IO;

namespace CubemapFileGenerator
{
    public class CubemapFileGeneratorWizard : ScriptableWizard
    {
        [Tooltip("テクスチャの縦および横のサイズ.")]
        public int width = 256;

        [Tooltip("出力するファイルの形式.")]
        public FileType fileType = FileType.VerticalPng;

        [Tooltip("Cubemapのレンダリングの中心地点.\n\"CubemapRenderingPoint\"という名前のオブジェクトがシーンにある場合は自動でセットされる.")]
        public Transform renderFromPosition;

        // レンダリングカメラの位置を表すGameObjectの名前
        const string DefaultTransformPositionName = "CubemapRenderingPoint";

        [MenuItem("CubemapFileGenerator/Generate cubemap file")]
        static void RenderCubemap()
        {
            ScriptableWizard.DisplayWizard<CubemapFileGeneratorWizard>(
                "Generate cubemap file", "Generate!");
        }

        void OnWizardUpdate()
        {
            helpString = "Select transform to render from and cubemap to render into";

            if (renderFromPosition == null) {
                var renderingPointGO = GameObject.Find(DefaultTransformPositionName);
                renderFromPosition = renderingPointGO?.transform;
            }

            isValid = (renderFromPosition != null);//  && (renderCubemap != null);
        }

        void OnWizardCreate()
        {
            string path = EditorUtility.SaveFilePanel(
                    title: "Save png",
                    directory: "",
                    defaultName: "GeneratedCubemap.png",
                    extension: "png");

            if (path == null || path == "") {
                Debug.LogError(string.Format("Invalid path: \"{0}\"", path));
                return;
            }

            Cubemap cubemap = new Cubemap(width, TextureFormat.RGB24, false);

            RenderToCubemap(cubemap);
            EncodeToPng(cubemap, path);

            AssetDatabase.Refresh();
        }

        private void RenderToCubemap(Cubemap cubemap)
        {
            GameObject go = new GameObject("CubemapCamera");

            go.AddComponent<Camera>();
            go.transform.position = renderFromPosition.position;
            go.transform.rotation = Quaternion.identity;
            go.GetComponent<Camera>().RenderToCubemap(cubemap);

            DestroyImmediate(go);
        }

        void EncodeToPng(Cubemap cubemap, string path)
        {
            var converter = GetConverter(fileType);
            byte[] pngBytes = converter.Convert(cubemap);
            File.WriteAllBytes(path, pngBytes);
        }

        private static ICubemapConverter GetConverter(FileType fileType)
        {
            switch (fileType) {
                case FileType.HorizontalPng:
                    return new Cubemap2HorizontalPngConverter();
                default:
                    return new Cubemap2VerticalPngConverter();
            }
        }

        public enum FileType
        {
            VerticalPng,
            HorizontalPng,
        }
    }
}