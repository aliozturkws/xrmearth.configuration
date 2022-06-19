using XrmEarth.Core.Configuration.Target;

namespace XrmEarth.Core.Configuration.Attributes
{
    public class RegeditStorageAttribute : StorageTargetAttribute
    {
        public RegeditStorageAttribute() : base(typeof(RegeditStorageTarget))
        {
        }
    }
}
