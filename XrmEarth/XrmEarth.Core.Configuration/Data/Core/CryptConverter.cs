using XrmEarth.Core.Utility;

namespace XrmEarth.Core.Configuration.Data.Core
{
    public class CryptConverter : IValueConverter
    {
        public object Convert(object val)
        {
            if (val == null)
                return null;

            return CryptHelper.Crypt(val.ToString());
        }

        public object ConvertBack(object val)
        {
            if (val == null)
                return null;

            return CryptHelper.Decrypt(val.ToString());
        }
    }
}
