using System;

namespace XrmEarth.Core.Configuration.Data.Core
{
    public class EnumConverter<T> : IValueConverter
    {
        public object Convert(object val)
        {
            return (T)(object)System.Convert.ToInt32(val);
        }

        public object ConvertBack(object val)
        {
            throw new NotImplementedException();
        }
    }
}
