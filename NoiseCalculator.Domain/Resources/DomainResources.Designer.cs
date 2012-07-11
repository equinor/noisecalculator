﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoiseCalculator.Domain.Resources {
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
    internal class DomainResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DomainResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NoiseCalculator.Domain.Resources.DomainResources", typeof(DomainResources).Assembly);
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
        ///   Looks up a localized string similar to Calculated noise exposure is not akseptable and work can not be performed as planned..
        /// </summary>
        internal static string NoiseLevelStatusTextCritical {
            get {
                return ResourceManager.GetString("NoiseLevelStatusTextCritical", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Calculated noise exposure is not akseptable and work can not be performed as planned. If the work has allready been performed it must be reported as &quot;Danger of Work Related Injury&quot;.
        /// </summary>
        internal static string NoiseLevelStatusTextDangerOfWorkRelatedInjury {
            get {
                return ResourceManager.GetString("NoiseLevelStatusTextDangerOfWorkRelatedInjury", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Calculated noise exposure is acceptable, but the employee can not be exposed for more noise this work day..
        /// </summary>
        internal static string NoiseLevelStatusTextMaximumAllowedDosage {
            get {
                return ResourceManager.GetString("NoiseLevelStatusTextMaximumAllowedDosage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Calculated noise exposure is acceptable..
        /// </summary>
        internal static string NoiseLevelStatusTextNormal {
            get {
                return ResourceManager.GetString("NoiseLevelStatusTextNormal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Calculated noise exposure is acceptable, but is approaching maximum allowed exposure..
        /// </summary>
        internal static string NoiseLevelStatusTextWarning {
            get {
                return ResourceManager.GetString("NoiseLevelStatusTextWarning", resourceCulture);
            }
        }
    }
}
