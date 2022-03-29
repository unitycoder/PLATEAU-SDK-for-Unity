using System.IO;
using LibPLATEAU.NET;
using PlateauUnitySDK.Runtime;
using UnityEditor;
using UnityEngine;

namespace PlateauUnitySDK.Editor {
    
    /// <summary>
    /// gmlファイルを読んでobjファイルに変換して出力する機能を持ったウィンドウです。
    /// </summary>
    public class GMLConvertWindow : EditorWindow {
        private string gmlFilePath = "";
        private string destinationFilePath = "";
        private const float spaceWidth = 20f;
        private int optimizeLevel = 1;
        private bool mergeMeshFlg = true;
        private AxesConversion axesConversion = AxesConversion.RUF;
        
        /// <summary> ウィンドウを表示します。 </summary>
        [MenuItem("Plateau/GML Converter Window")]
        private static void Init() {
            var window = GetWindow<GMLConvertWindow>("GML Convert Window");
            window.Show();
        }

        /// <summary> 初期化処理のうち、Unityの仕様で Init に書けない部分をここに書きます。 </summary>
        private void OnEnable() {
            DefaultPathIfEmpty(ref this.gmlFilePath);
            DefaultPathIfEmpty(ref this.destinationFilePath);
        }

        /// <summary>
        /// パスのデフォルト値は、ファイル選択画面で最初に表示されるディレクトリに影響します。
        /// Assetsフォルダを起点にしたほうが操作性が良さそうなので、そのようにデフォルト値を設定します。
        /// </summary>
        private static void DefaultPathIfEmpty(ref string path) {
            if (string.IsNullOrEmpty(path)) {
                path = Application.dataPath; // Assetsフォルダ
            }
        }

        /// <summary> GUI表示のメインメソッドです。 </summary>
        private void OnGUI() {
            EditorUtil.Heading1("1. Select GML File");
            if (GUILayout.Button("Select File")) {
                ButtonSelectGMLFilePushed();
            }
            GUILayout.Label("GML file path:");
            GUILayout.TextArea($"{this.gmlFilePath}");
            
            Space();
            EditorUtil.Heading1("2. Select Obj File Destination");
            
            if (GUILayout.Button("Select Destination")) {
                ButtonSelectDestination();
            }
            GUILayout.Label("Destination obj file path:");
            GUILayout.TextArea($"{this.destinationFilePath}");

            Space();
            EditorUtil.Heading1("3. Configure");
            this.optimizeLevel = EditorGUILayout.IntField("Optimize level", this.optimizeLevel);
            this.mergeMeshFlg = EditorGUILayout.Toggle("Merge Mesh", this.mergeMeshFlg);
            this.axesConversion = (AxesConversion)EditorGUILayout.EnumPopup("Axes Conversion", this.axesConversion);


            Space();
            EditorUtil.Heading1("4. Convert");
            
            if (GUILayout.Button("Convert")) {
                ButtonConvert();
            }
        }
        

        /// <summary> ボタン押下時に呼ばれます。gmlファイルを選択するウィンドウを出します。 </summary>
        private void ButtonSelectGMLFilePushed() {
            string path = EditorUtility.OpenFilePanel("Select GML File", this.gmlFilePath, "gml");
            if (string.IsNullOrEmpty(path)) return;
            this.gmlFilePath = path;
        }
        
        /// <summary> ボタン押下時に呼ばれます。objファイルの出力先を選択するウィンドウを出します。 </summary>
        private void ButtonSelectDestination() {
            string path = EditorUtility.SaveFilePanel(
                "Select Destination",
                Path.GetDirectoryName(this.gmlFilePath),
                "exported", "obj"
                );
            if (string.IsNullOrEmpty(path)) return;
            this.destinationFilePath = path;
        }

        /// <summary> ボタン押下時に呼ばれます。gmlからobjに変換し、結果を表示します。 </summary>
        private void ButtonConvert() {
            var gmlToObjConverter = new GmlToObjConverter(this.optimizeLevel, this.mergeMeshFlg, this.axesConversion);
            bool result = gmlToObjConverter.Convert(this.gmlFilePath, this.destinationFilePath);
            EditorUtility.DisplayDialog(
                "Convert Result",
                result ? "Convert Complete!" : "Convert Failed...\nSee console log for detail.",
                "OK");
            if (result) {
                AssetDatabase.Refresh();
            }
            
        }

        /// <summary> 空白を表示します。 </summary>
        private static void Space() {
            EditorGUILayout.Space(spaceWidth);
        }
    }
}
