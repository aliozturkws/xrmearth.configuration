namespace XrmEarth.Core.Configuration.Common
{
    /// <summary>
    /// Kaynak tipleri
    /// </summary>
    public enum ConfigSourceType
    {
        /// <summary>
        /// Varsayýlan
        /// </summary>
        Default,
        /// <summary>
        /// Sýnýf ile atama
        /// </summary>
        Override,
        /// <summary>
        /// Ayar dosyasý
        /// </summary>
        Config,
        /// <summary>
        /// Komut satýrý
        /// </summary>
        Startup
    }
}