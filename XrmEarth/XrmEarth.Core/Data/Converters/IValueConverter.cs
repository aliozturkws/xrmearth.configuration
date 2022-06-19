namespace XrmEarth.Core.Data.Converters
{
    public interface IValueConverter
    {
        object Convert(object value);
        object ConvertBack(object value);
    }
}
