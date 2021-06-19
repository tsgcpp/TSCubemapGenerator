using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace TSCubemapGenerator.Window
{
    public class CubemapGeneratorEditorWindow : EditorWindow
    {
        [MenuItem("TSCubemapGenerator/Open Tool Window")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(
                t: typeof(CubemapGeneratorEditorWindow),
                utility: false,
                title: "TSCubemapGenerator");
        }

        private CubemapConverterSelector _cubemapConverterSelector;
        private RounderSelector _rounderSelector;
        private CycleCubemapRenderer _cycleCubemapRenderer = null;

        private void OnEnable()
        {
            _cubemapConverterSelector = new CubemapConverterSelector();
            _rounderSelector = new RounderSelector();
            _skyboxMaterial = new Material(Shader.Find(DefaultCubemapShaderPath));

            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        }

        private void OnPlayModeChanged(PlayModeStateChange _)
        {
            Reset();
        }

        private void Reset()
        {
            DisableCycleRendering();

            _inPreview = false;
            _cycleCubemapRenderer = null;
            _previousSkyboxMaterial = null;
        }

        private Camera _renderCamera = null;

        private Material _cubemapPreviewMaterial = null;
        private string _cubemapShaderTextureName = DefaultCubemapShaderTextureName;

        private OutputImageType _imageType = OutputImageType.VerticalPng;

        private bool _inPreview = false;
        private bool _realtimeRendering = false;

        private Material _previousSkyboxMaterial = null;
        private Material _skyboxMaterial = null;
        private Cubemap _renderedCubemap = null;
        private SizeType _sizeType = SizeType.None;
        private int _size = 512;

        private void OnGUI()
        {
            _renderCamera = EditorGUILayout.ObjectField("Render Camera", _renderCamera, typeof(Camera), true) as Camera;

            if (EditorApplication.isPlaying)
            {
                SetStopButton();
            }
            else
            {
                SetStartButton();
            }

            GUI.enabled = true;

            SetSizeLayout();
            SetPreviewLayout();
            SetRenderingLayout();
            SetExportLayout();

            GUI.enabled = true;
        }

        private void SetStandardSpace()
        {
            EditorGUILayout.Space(8);
        }

        private void SetStartButton()
        {
            if (!GUILayout.Button("Start"))
            {
                return;
            }

            EditorApplication.isPlaying = true;
        }

        private void SetStopButton()
        {
            if (!GUILayout.Button("Stop"))
            {
                return;
            }

            EditorApplication.isPlaying = false;
        }

        private void SetSizeLayout()
        {
            SetStandardSpace();
            GUILayout.Label("Size", EditorStyles.boldLabel);
            _sizeType = (SizeType)EditorGUILayout.EnumPopup("Size Type", _sizeType);
            var rounder = _rounderSelector.Select(_sizeType);

            _size = EditorGUILayout.IntSlider("Size", _size, SizeRange.x, SizeRange.y);
            _size = rounder.Round(_size);
            GUILayout.Label($"{_size} x {_size} x 6 Rects", EditorStyles.label);
            SetStandardSpace();
        }

        private void SetPreviewLayout()
        {
            GUILayout.Label("Preview", EditorStyles.boldLabel);
            _cubemapPreviewMaterial = EditorGUILayout.ObjectField("Preview Material", _cubemapPreviewMaterial, typeof(Material), true) as Material;
            _cubemapShaderTextureName = EditorGUILayout.TextField("Shader Texture Property", _cubemapShaderTextureName);

            bool lastEnabled = GUI.enabled;
            GUI.enabled = CheckRenderable();

            if (GUILayout.Button(_inPreview ? "Revert Skybox" : "Set Material Preview as Skybox"))
            {
                TogglePreview();
            }

            GUI.enabled = lastEnabled;
        }

        private void SetRenderingLayout()
        {
            SetStandardSpace();
            GUILayout.Label("Rendering", EditorStyles.boldLabel);

            bool lastEnabled = GUI.enabled;
            GUI.enabled = CheckRenderable();

            if (GUILayout.Button("Render Cubemap"))
            {
                RenderCubemap();
            }

            SetStandardSpace();
            if (GUILayout.Button(_realtimeRendering ? "Disable Realtime Rendering" : "Enbable Realtime Rendering"))
            {
                ToggleCycleRendering();
            }

            GUI.enabled = lastEnabled;
        }

        private void SetExportLayout()
        {
            SetStandardSpace();
            GUILayout.Label("Export", EditorStyles.boldLabel);

            _imageType = (OutputImageType)EditorGUILayout.EnumPopup("Image Type", _imageType);

            bool lastEnabled = GUI.enabled;
            GUI.enabled = CheckExportable();
            if (GUILayout.Button("Export Cubemap to File"))
            {
                ExportCubemap();
            }

            GUILayout.Label(text: "Export requires \"Render Cubemap\"", style: EditorStyles.miniBoldLabel);

            GUI.enabled = lastEnabled;
        }

        private bool CheckRenderable()
        {
            return EditorApplication.isPlaying && _cubemapPreviewMaterial;
        }

        private bool CheckHasRendered()
        {
            return _renderedCubemap != null;
        }

        private bool CheckExportable()
        {
            return CheckRenderable()
                && CheckHasRendered();
        }

        private void TogglePreview()
        {
            _inPreview = !_inPreview;

            if (_inPreview)
            {
                _previousSkyboxMaterial = RenderSettings.skybox;
                RenderSettings.skybox = _skyboxMaterial;
            }
            else
            {
                RenderSettings.skybox = _previousSkyboxMaterial;
                _previousSkyboxMaterial = null;
            }
        }

        private void ToggleCycleRendering()
        {
            bool next = !_realtimeRendering;
            if (next)
            {
                EnableCycleRendering();
            }
            else
            {
                DisableCycleRendering();
            }
        }

        private void RenderCubemap()
        {
            if (!_renderCamera)
            {
                Debug.LogWarning("Camera isn't set. Specify a camera to render a cubemap.");
                return;
            }

            if (!_renderedCubemap || (_renderedCubemap.width != _size))
            {
                RefreshCubemap();
            }

            _renderCamera.RenderToCubemap(_renderedCubemap);
        }

        private void RefreshCubemap()
        {
            if (_renderedCubemap)
            {
                GameObject.DestroyImmediate(_renderedCubemap);
            }

            _renderedCubemap = new Cubemap(_size, TextureFormat.RGB24, false);
            _skyboxMaterial.SetTexture(DefaultCubemapShaderTextureName, _renderedCubemap);
        }

        private void ExportCubemap()
        {
            if (!_renderedCubemap)
            {
                Debug.LogWarning("Cubemap isn't rendered.");
                return;
            }

            string imagePath = EditorUtility.SaveFilePanel(
                    title: "Save Image",
                    directory: "",
                    defaultName: "Cubemap.png",
                    extension: "png");

            if (imagePath == null || imagePath == "")
            {
                return;
            }

            var converter = _cubemapConverterSelector.Select(_imageType);
            byte[] pngBytes = converter.ConvertFrom(_renderedCubemap);
            File.WriteAllBytes(imagePath, pngBytes);

            if (imagePath.StartsWith(Application.dataPath))
            {
                ReimportTextureAsCubemap(imagePath);
            }
            AssetDatabase.Refresh();
        }

        private static void ReimportTextureAsCubemap(string imageAbsolutePath)
        {
            AssetDatabase.Refresh();

            var relativeImagePath = "Assets" + imageAbsolutePath.Substring(Application.dataPath.Length);
            var importer = AssetImporter.GetAtPath(relativeImagePath) as TextureImporter;

            importer.textureType = TextureImporterType.Default;
            importer.textureShape = TextureImporterShape.TextureCube;
            importer.generateCubemap = TextureImporterGenerateCubemap.FullCubemap;

            importer.SaveAndReimport();
        }

        private void EnableCycleRendering()
        {
            _realtimeRendering = true;
            if (_cycleCubemapRenderer)
            {
                return;
            }

            var go = new GameObject("CycleCubemapRenderer")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            _cycleCubemapRenderer = go.AddComponent<CycleCubemapRenderer>();
            _cycleCubemapRenderer.Render = RenderCubemap;
        }

        private void DisableCycleRendering()
        {
            _realtimeRendering = false;
            if (_cycleCubemapRenderer)
            {
                GameObject.DestroyImmediate(_cycleCubemapRenderer.gameObject);
                _cycleCubemapRenderer = null;
            }
        }

        private class CycleCubemapRenderer : MonoBehaviour
        {
            public Action Render { get; internal set; } = null;

            private void LateUpdate() => Render?.Invoke();
        }

        private const string DefaultCubemapShaderPath = "Skybox/Cubemap";
        private const string DefaultCubemapShaderTextureName = "_Tex";
        private readonly Vector2Int SizeRange = new Vector2Int(16, 1024 * 8);
    }
}