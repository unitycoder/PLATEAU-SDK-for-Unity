﻿using System;

namespace PLATEAU.Interop
{
    /// <summary>
    /// C++側の std::string を扱います。
    /// </summary>
    
    // TODO 今までは、DLL側から string を受け取るためにかなり面倒な手続きを踏んでいました。
    //      例えば、stringを返す関数ごとに、P/Invoke で文字アドレスと文字列長の両方を受け渡す P/Invoke関数をいちいち作っていました。
    //      そのような面倒な部分を NativeString に置き換えればコード簡略化できそうです。ただし寿命に要注意です。
    //      加えて string の配列となるとさらに複雑になっていますが、こちらも NativeVectorString に置き換えることでシンプルになりそうです。
    
    public class NativeString
    {
        public IntPtr Handle { get; }

        public NativeString(IntPtr handle)
        {
            Handle = handle;
        }
        
        public override string ToString()
        {
            int strSize = DLLUtil.GetNativeValue<int>(Handle, NativeMethods.plateau_string_get_size);
            var charPtr = DLLUtil.GetNativeValue<IntPtr>(Handle, NativeMethods.plateau_string_get_char_ptr);
            string str = DLLUtil.ReadUtf8Str(charPtr, strSize);
            return str;
        }
    }
}
