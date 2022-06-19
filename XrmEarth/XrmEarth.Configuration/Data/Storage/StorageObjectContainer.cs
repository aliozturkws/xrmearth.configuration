using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XrmEarth.Configuration.Attributes;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Utility;

namespace XrmEarth.Configuration.Data.Storage
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [DebuggerDisplay("{OwnerType} - Fields Count : {Fields.Count}", Name = "{OwnerType}")]
    public class StorageObjectContainer
    {
        public Type TargetType { get; set; }
        public HashSet<StorageFieldContainer> Fields { get; set; }

        private StorageObjectPolicy _policy;
        public StorageObjectPolicy Policy
        {
            get { return _policy ?? (_policy = new StorageObjectPolicy()); }
            set
            {
                _policy = value;
            }
        }

        private Type _ownerType;
        public Type OwnerType
        {
            get { return _ownerType; }
            set
            {
                if (_ownerType == value)
                    return;

                _ownerType = value;
                ValidateOwnerType();
            }
        }

        private Dictionary<string, Type> _keys;


        private void ValidateOwnerType()
        {
            _keys = null;
            Initialize();
        }


        public void Initialize()
        {
            if (Fields == null)
            {
                Fields = new HashSet<StorageFieldContainer>();
            }
            else
            {
                Fields.Clear();
            }

            if (OwnerType == null)
                return;

            var fields = new List<StorageFieldContainer>();
            LoadField(OwnerType, ref fields);
            foreach (var field in fields)
            {
                Fields.Add(field);
            }
        }

        private void LoadField(Type type, ref List<StorageFieldContainer> fields, StorageFieldContainer parentField = null)
        {
            var propInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.CanRead && pi.CanWrite);
            foreach (var propInfo in propInfos)
            {
                var propAttr = propInfo.GetCustomAttributes(typeof(StorageAttribute), false).FirstOrDefault() as StorageAttribute;

                var propType = propInfo.PropertyType;
                if (propType != typeof(decimal) &&
                    propType != typeof(string) &&
                    propType != typeof(DateTime) &&
                    !Utils.IsArray(propType) &&
                    !Utils.IsCollection(propType) &&
                    propType.IsClass)
                {
                    if (Policy.IgnoreRecursiveProperties && propInfo.PropertyType == type)
                        continue;

                    var stFieldCont = new StorageFieldContainer(this)
                    {
                        Property = propInfo,
                        Parent = parentField,
                    };

                    if (propAttr != null)
                    {
                        if (propAttr.Exclude)
                            continue;

                        if (propAttr.Converter != null)
                        {
                            stFieldCont.Converter = Activator.CreateInstance(propAttr.Converter) as IValueConverter;
                        }

                        stFieldCont.PropertyKey = propAttr.Key;
                        stFieldCont.DefaultValue = propAttr.DefaultValue;
                    }

                    fields.Add(stFieldCont);

                    var childs = new List<StorageFieldContainer>();
                    LoadField(propInfo.PropertyType, ref childs, stFieldCont);
                    stFieldCont.Childs = childs;
                }
                else
                {
                    var stFieldCont = new StorageFieldContainer(this)
                    {
                        Property = propInfo,
                        Parent = parentField
                    };

                    if (propAttr != null)
                    {
                        if (propAttr.Exclude)
                            continue;

                        if (propAttr.Converter != null)
                        {
                            stFieldCont.Converter = Activator.CreateInstance(propAttr.Converter) as IValueConverter;
                        }

                        stFieldCont.PropertyKey = propAttr.Key;
                        stFieldCont.DefaultValue = propAttr.DefaultValue;
                    }

                    fields.Add(stFieldCont);
                }
            }
        }


        public Dictionary<string, Type> GetKeys()
        {
            if (_keys != null)
                return _keys;

            _keys = new Dictionary<string, Type>();

            foreach (var fieldContainer in Fields)
            {
                GetKeys(fieldContainer, ref _keys);
            }

            return _keys;
        }

        public void GetKeys(StorageFieldContainer field, ref Dictionary<string, Type> keys)
        {
            if (field.Childs != null)
            {
                foreach (var childField in field.Childs)
                {
                    GetKeys(childField, ref keys);
                }
            }
            else
            {
                _keys[field.Key] = field.Property.PropertyType;
            }
        }

        public Dictionary<string, ValueContainer> GetKeyAndValues(object instance, bool ignoreBindInstance = true)
        {
            var keyAndValues = new Dictionary<string, ValueContainer>();
            foreach (var field in Fields)
            {
                GetValue(field, instance, ref keyAndValues, ignoreBindInstance);
            }
            return keyAndValues;
        }

        private void GetValue(StorageFieldContainer field, object instance, ref Dictionary<string, ValueContainer> keyAndValues, bool ignoreBindInstance = true)
        {
            var value = field.Property.GetValue(instance);

            if (!field.IsClass || !ignoreBindInstance)
            {
                value = ValueConvert(field, value);
                value = ValueConvertWritable(field, value);

                keyAndValues[field.Key] = new ValueContainer {Value = value, Type = field.Property.PropertyType};
            }

            if (field.IsClass)
            {
                foreach (var childField in field.Childs)
                {
                    GetValue(childField, value, ref keyAndValues, ignoreBindInstance);
                }
            }
        }

        public void SetValues(object instance, Dictionary<string, ValueContainer> keyAndValues)
        {
            foreach (var field in Fields)
            {
                SetValues(instance, keyAndValues, field);
            }
        }

        public void SetValues(object instance, Dictionary<string, ValueContainer> keyAndValues, StorageFieldContainer field)
        {
            if (field.IsClass)
            {
                var ins = Activator.CreateInstance(field.Property.PropertyType);
                foreach (var childField in field.Childs)
                {
                    SetValues(ins, keyAndValues, childField);
                }
                ValueSet(field, instance, ins);
            }
            else
            {
                SetValue(instance, keyAndValues, field);
            }
        }

        public void SetValue(object instance, Dictionary<string, ValueContainer> keyAndValues, StorageFieldContainer field)
        {
            if (!keyAndValues.ContainsKey(field.Key))
                return;


            var value = keyAndValues[field.Key].Value;

            value = ValueConvertReadable(field, value);
            value = ValueConvertBack(field, value);
            value = ValueTypeConvert(field, value);

            ValueSet(field, instance, value);
        }

        #region - Virtuals -

        protected virtual object ValueConvert(StorageFieldContainer field, object value)
        {
            if (field.Converter != null)
            {
                value = field.Converter.Convert(value);
            }
            return value;
        }

        protected virtual object ValueConvertBack(StorageFieldContainer field, object value)
        {
            if (field.Converter != null)
            {
                value = field.Converter.ConvertBack(value);
            }
            return value;
        }

        protected virtual object ValueTypeConvert(StorageFieldContainer field, object value)
        {
            if (value == null)
                return null;

            var valueType = value.GetType();
            var propType = field.Property.PropertyType;

            if (valueType == propType)
                return value;

            if (Utils.IsNumeric(valueType) && Utils.IsNumeric(propType))
            {
                return Convert.ChangeType(value, Type.GetTypeCode(propType));
            }

            return TypeDescriptor.GetConverter(field.Property.PropertyType).ConvertFrom(value);
        }

        protected virtual void ValueSet(StorageFieldContainer field, object instance, object value)
        {
            field.Property.SetValue(instance, value);
        }

        protected virtual object ValueConvertWritable(StorageFieldContainer field, object value)
        {
            if (Utils.IsWritable(field.Property.PropertyType))
            {
                return value;
            }

            if (Utils.IsCollection(field.Property.PropertyType) || Utils.IsArray(field.Property.PropertyType))
            {
                return JsonSerializerUtil.Serialize(value);
            }

            return value;
        }

        protected virtual object ValueConvertReadable(StorageFieldContainer field, object value)
        {
            if (value == null)
                return null;

            var propType = field.Property.PropertyType;
            if (Utils.IsWritable(propType))
            {
                return value;
            }

            if (Utils.IsCollection(propType) || Utils.IsArray(propType))
            {
                return JsonSerializerUtil.Deserialize(value.ToString(), propType);
            }

            return value;
        }

        #endregion - Virtuals -
    }
}
