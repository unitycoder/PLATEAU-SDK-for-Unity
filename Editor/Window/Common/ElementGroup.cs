namespace PLATEAU.Editor.Window.Common
{
    /// <summary>
    /// 複数の<see cref="Element"/>をまとめます。
    /// また、まとめた要素から1つを返すための検索メソッドを提供します。
    /// </summary>
    internal class ElementGroup : Element
    {
        protected Element[] Elements { get; set; }
        
        public ElementGroup(string name, params Element[] elements) : base(name)
        {
            Elements = elements;
        }
        public override void DrawContent()
        {
            foreach (var d in Elements)
            {
                d.Draw();
            }
        }

        /// <summary>
        /// <see cref="Element"/>を型で検索して最初に合致したものを返します。
        /// <see cref="ElementGroup"/>の入れ子になっている場合は子も検索します。
        /// </summary>
        public T Get<T>() where T : Element
        {
            foreach (var d in Elements)
            {
                if (d is T ret) return ret;
            }

            foreach (var d in Elements)
            {
                if (d is ElementGroup child)
                {
                    var t = child.Get<T>();
                    if (t != null) return t;
                }
            }
            return null;
        }

        /// <summary>
        /// <see cref="Element"/>を名前で検索して最初に合致した者を返します。
        /// <see cref="ElementGroup"/>の入れ子になっている場合は子も検索します。
        /// </summary>
        public Element Get(string name)
        {
            foreach (var d in Elements)
            {
                if (d.Name == name) return d;
            }

            foreach (var d in Elements)
            {
                if (d is ElementGroup child)
                {
                    var n = child.Get(name);
                    if (n != null) return n;
                }
            }

            return null;
        }

        /// <summary>
        /// 型と名前の両方一致検索です。
        /// </summary>
        public T Get<T>(string name) where T : Element
        {
            foreach (var d in Elements)
            {
                if (d.Name == name && d is T ret) return ret;
            }

            foreach (var d in Elements)
            {
                if (d is ElementGroup child)
                {
                    var hit = child.Get<T>(name);
                    if (hit != null) return hit;
                }
            }

            return null;
        }

        public override void Dispose()
        {
            foreach (var d in Elements)
            {
                d.Dispose();
            }
        }
    }
}