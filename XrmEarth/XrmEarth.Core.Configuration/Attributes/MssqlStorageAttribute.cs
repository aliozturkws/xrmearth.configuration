using XrmEarth.Core.Configuration.Target;

namespace XrmEarth.Core.Configuration.Attributes
{
    public class MssqlStorageAttribute : StorageTargetAttribute
    {
        public MssqlStorageAttribute() : base(typeof(MssqlStorageTarget))
        {
        }
    }
}
