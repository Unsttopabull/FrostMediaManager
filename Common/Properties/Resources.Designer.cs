﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Frost.Common.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Frost.Common.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- ----------------------------
        ///-- Table structure for &quot;EdmMetadata&quot;
        ///-- ----------------------------
        ///CREATE TABLE IF NOT EXISTS &quot;EdmMetadata&quot; (
        ///	&quot;Id&quot;  TEXT,
        ///	&quot;ModelHash&quot;  TEXT
        ///);
        ///
        ///-- ----------------------------
        ///-- Table structure for &quot;__MigrationHistory&quot;
        ///-- ----------------------------
        ///CREATE TABLE IF NOT EXISTS &quot;__MigrationHistory&quot; (
        ///	&quot;MigrationId&quot;  TEXT NOT NULL,
        ///	&quot;Model&quot;  BLOB NOT NULL,
        ///	&quot;ProductVersion&quot;  TEXT NOT NULL
        ///);
        ///
        ///
        ///UPDATE movies
        ///SET year=NULL
        ///WHERE id NOT IN(SELECT id FROM m [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FixXjbSQL {
            get {
                return ResourceManager.GetString("FixXjbSQL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*
        ///Target Server Type    : SQLite
        ///Target Server Version : 30706
        ///File Encoding         : 65001
        ///
        ///Date: 2014-01-08 15:45:38
        ///*/
        ///
        ///-- ----------------------------
        ///-- Table structure for &quot;EdmMetadata&quot;
        ///-- ----------------------------
        ///CREATE TABLE IF NOT EXISTS &quot;EdmMetadata&quot; (
        ///	&quot;Id&quot;  TEXT,
        ///	&quot;ModelHash&quot;  TEXT
        ///);
        ///
        ///-- ----------------------------
        ///-- Table structure for &quot;__MigrationHistory&quot;
        ///-- ----------------------------
        ///CREATE TABLE IF NOT EXISTS &quot;__MigrationHistory&quot; (
        ///	&quot;MigrationId&quot;  TEXT NOT NULL [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MovieVoSQL {
            get {
                return ResourceManager.GetString("MovieVoSQL", resourceCulture);
            }
        }
    }
}
