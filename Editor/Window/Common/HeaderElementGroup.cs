using System;
using UnityEditor;

namespace PLATEAU.Editor.Window.Common
{
    // EditorGUILayout等のシンプルな機能をElementでラップしたクラス群です。
    
    /// <summary>
    /// EditorGUILayout.ToggleLeftをElementでラップしたクラスです。
    /// </summary>
    internal class ToggleLeftElement : Element
    {
        private string labelText;
        public bool Value { get; private set; }

        public ToggleLeftElement(string elementName, string labelText, bool defaultValue) : base(elementName)
        {
            this.labelText = labelText;
            Value = defaultValue;
        }
        
        public override void DrawContent()
        {
            Value = EditorGUILayout.ToggleLeft(labelText, Value);
        }
        
        public override void Dispose(){}
    }
    
    /// <summary>
    /// ヘッダーと、それに属するコンテンツをElementGroupでラップしたクラスです。
    /// </summary>
    internal class HeaderElementGroup : ElementGroup
    {
        private string headerText;
        private HeaderType headerType;
        
        public HeaderElementGroup(string elementName, string headerText, HeaderType headerType, params Element[] innerElements) : base(elementName, innerElements)
        {
            this.headerText = headerText;
            this.headerType = headerType;
        }

        public override void DrawContent()
        {
            // ヘッダーの表示
            switch (headerType)
            {
                case HeaderType.Header:
                    PlateauEditorStyle.Heading(headerText, null);
                    break;
                case HeaderType.Subtitle:
                    PlateauEditorStyle.SubTitle(headerText);
                    break;
                default:
                    throw new ArgumentException("unknown header type.");
            }
            using var verticalScope = PlateauEditorStyle.VerticalScopeWithPadding(16,0,8,8);
            base.DrawContent();
        }

        public override void Dispose(){}
    }

    internal enum HeaderType
    {
        Header, Subtitle
    }
    
}