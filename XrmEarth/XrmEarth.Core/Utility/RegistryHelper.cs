using Microsoft.Win32;

namespace XrmEarth.Core.Utility
{
    public class RegistryHelper
    {
        private RegistryKey baseKey = null;

        private RegistryHelper(string subKeyPath)
        {
            baseKey = Registry.LocalMachine.OpenSubKey(subKeyPath, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.ReadKey);
        }

        public string Value(string keyName)
        {
            return baseKey.GetValue(keyName, string.Empty).ToString();
        }

        ~RegistryHelper()
        {
            baseKey.Close();
        }

        private static RegistryHelper registryHelperInstance = null;
        private static readonly object lockthread = new object();

        public static RegistryHelper Get
        {
            get
            {
                lock (lockthread)
                {
                    if (registryHelperInstance == null)
                    {
                        registryHelperInstance = new RegistryHelper("SOFTWARE\\Wow6432Node\\" + Globals.CompanyName);
                    }
                    return registryHelperInstance;
                }
            }
        }
    }
}
