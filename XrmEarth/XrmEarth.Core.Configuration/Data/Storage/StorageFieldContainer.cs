using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using XrmEarth.Core.Configuration.Data.Core;

namespace XrmEarth.Core.Configuration.Data.Storage
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerDisplay("{Property.Name} - {Property.PropertyType}")]
    public class StorageFieldContainer
    {
        public StorageFieldContainer(StorageObjectContainer objectContainer)
        {
            ObjectContainer = objectContainer;
        }

        public StorageObjectContainer ObjectContainer { get; private set; }
        public object DefaultValue { get; set; }
        public PropertyInfo Property { get; set; }
        public string PropertyKey { get; set; }
        public IValueConverter Converter { get; set; }
        public StorageFieldContainer Parent { get; set; }
        public List<StorageFieldContainer> Childs { get; set; }

        public bool IsClass
        {
            get { return Childs != null; }
        }

        private string _key;
        public string Key
        {
            get
            {
                if (string.IsNullOrEmpty(_key))
                    InitializeKey();

                return _key;
            }
        }

        private void InitializeKey()
        {
            var t = ObjectContainer.OwnerType;

            var parents = new List<PropertyInfo>();
            var currField = Parent;
            while (currField != null)
            {
                parents.Add(currField.Property);
                currField = currField.Parent;
            }

            parents.Reverse();
            _key = CreateKey(t, Property, parents);
        }

        protected virtual string CreateKey(Type type, PropertyInfo property, List<PropertyInfo> parents = null)
        {
            return string.IsNullOrEmpty(PropertyKey) ? Utils.NameUniquefier(type, property, parents) : Utils.NameUniquefier(type, PropertyKey, parents);
        }
    }
}
