﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TouchGame.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Vision : global::System.Configuration.ApplicationSettingsBase {
        
        private static Vision defaultInstance = ((Vision)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Vision())));
        
        public static Vision Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Resources\\Vision\\texture")]
        public string basePath {
            get {
                return ((string)(this["basePath"]));
            }
            set {
                this["basePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4;5;6")]
        public string elementNumber {
            get {
                return ((string)(this["elementNumber"]));
            }
            set {
                this["elementNumber"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int totalLevel {
            get {
                return ((int)(this["totalLevel"]));
            }
            set {
                this["totalLevel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("80;82;84;86;88;90;92;94;96;98")]
        public string textureScale {
            get {
                return ((string)(this["textureScale"]));
            }
            set {
                this["textureScale"] = value;
            }
        }
    }
}
