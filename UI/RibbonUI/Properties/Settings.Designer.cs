﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RibbonUI.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool FirstRun {
            get {
                return ((bool)(this["FirstRun"]));
            }
            set {
                this["FirstRun"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection SearchFolders {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["SearchFolders"]));
            }
            set {
                this["SearchFolders"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("(Default)")]
        public global::System.Globalization.CultureInfo UICulture {
            get {
                return ((global::System.Globalization.CultureInfo)(this["UICulture"]));
            }
            set {
                this["UICulture"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection KnownSubtitleFormats {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["KnownSubtitleFormats"]));
            }
            set {
                this["KnownSubtitleFormats"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection KnownSubtitleExtensions {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["KnownSubtitleExtensions"]));
            }
            set {
                this["KnownSubtitleExtensions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringDictionary AudioCodecIdBindings {
            get {
                return ((global::System.Collections.Specialized.StringDictionary)(this["AudioCodecIdBindings"]));
            }
            set {
                this["AudioCodecIdBindings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringDictionary VideoCodecIdBindings {
            get {
                return ((global::System.Collections.Specialized.StringDictionary)(this["VideoCodecIdBindings"]));
            }
            set {
                this["VideoCodecIdBindings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringDictionary KnownSegments {
            get {
                return ((global::System.Collections.Specialized.StringDictionary)(this["KnownSegments"]));
            }
            set {
                this["KnownSegments"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringDictionary CustomLanguageMappings {
            get {
                return ((global::System.Collections.Specialized.StringDictionary)(this["CustomLanguageMappings"]));
            }
            set {
                this["CustomLanguageMappings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection ExcludedSegments {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["ExcludedSegments"]));
            }
            set {
                this["ExcludedSegments"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection ReleaseGroups {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["ReleaseGroups"]));
            }
            set {
                this["ReleaseGroups"] = value;
            }
        }
    }
}