namespace XrmEarth.Configuration.Data.Core
{
    public interface IValueConverter
    {
        object Convert(object val);

        object ConvertBack(object val);
    }
}
