namespace XrmEarth.Core
{
    public sealed class Globals
    {
        #region - SingleTon -
        private class Nested
        {
            internal static readonly Globals Instance;

            static Nested()
            {
                Instance = new Globals();
            }
        }

        private static Globals Instance
        {
            get { return Nested.Instance; }
        }
        #endregion

        private const string _companyName = "CrmAkademi";
        public static string CompanyName 
        {
            get
            {
                //Yeni Alan Eklendi
                return Globals._companyName; 
            }
        }
    }
}
