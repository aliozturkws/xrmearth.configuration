using XrmEarth.Core.Configuration.Target;

namespace XrmEarth.Core.Configuration.Attributes
{
    public class FileStorageAttribute : StorageTargetAttribute
    {
        public FileStorageAttribute() : base(typeof(FileStorageTarget))
        {
        }
    }
}
