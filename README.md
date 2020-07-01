# CubemapFileGenerator-Unity
シーンからCubemap形式のpngファイルを生成するUnity向けEditorツール

## インストール
1. [Releases](https://github.com/tsgcpp/CubemapFileGenerator-Unity/releases)より"CubemapFileGenerator.unitypackage"をダウンロード
2. "CubemapFileGenerator.unitypackage"をインストール

## 使用方法
1. Cubemap化したいSceneを作成しエディター上にロード
2. Cubemapの中心位置にGameObjectを配置
  - 名前を`CubemapRenderingPoint`とすると、自動でツールに設定されます
3. タブのCubemapFileGenerator -> Generate cubemap file
4. Width(縦横兼用), File Type, Render From Positionを指定
5. "Generate!"ボタンをクリックし、出力先を指定して保存

### 出力された画像の補足
現時点では出力された画像のInspector上のTextureShape, Mappingは自動で設定されません。  
以下のように設定することを推奨します。

- TextureShape: Cube
- Mapping: 6 Frames Layout (Cubic Environment)
- 設定をApply

## FileTypeについて
出力される画像の形式を指定。  
[Cubemaps](https://docs.unity3d.com/Manual/class-Cubemap.html)に記載されている形式に準拠します。

- VerticalPng
  - テクスチャを縦に並べた形式
  - pngファイルとして出力
- HorizontalPng
  - テクスチャを横に並べた形式
  - pngファイルとして出力

## ライセンス
- [LICENSE](./LICENSE)を参照
