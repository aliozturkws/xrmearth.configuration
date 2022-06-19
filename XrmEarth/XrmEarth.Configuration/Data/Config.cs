using System;
using System.Collections.Generic;
using System.Reflection;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Core.Configuration.Common;

namespace XrmEarth.Configuration.Data
{
    public class Config
    {
        public static readonly HashSet<Assembly> RegisteredAssemblies = new HashSet<Assembly>();
        
        public static bool Sandbox { get; set; }

        private readonly Dictionary<string, Tuple<ConfigSourceType, LazyValueLoader>> _values = new Dictionary<string, Tuple<ConfigSourceType, LazyValueLoader>>();
    }
}