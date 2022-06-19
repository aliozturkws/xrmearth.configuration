namespace XrmEarth.Core.Configuration.Common
{
    /// <summary>
    /// Kaynak tipleri
    /// </summary>
    public enum ConfigSourceType
    {
        /// <summary>
        /// Varsay�lan
        /// </summary>
        Default,
        /// <summary>
        /// S�n�f ile atama
        /// </summary>
        Override,
        /// <summary>
        /// Ayar dosyas�
        /// </summary>
        Config,
        /// <summary>
        /// Komut sat�r�
        /// </summary>
        Startup
    }
}