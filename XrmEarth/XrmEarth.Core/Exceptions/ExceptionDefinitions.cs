namespace XrmEarth.Core.Exceptions
{
    public static class ExceptionDefinitions
    {
        #region | Constants |

        public const string PARAMETER_CANNOT_BE_NULL = "Parametre NULL olamaz";
        public const string PARAMETER_CANNOT_BE_GUIDEMPTY = "Parametre boş yada Guid.Empty olamaz";
        public const string PARAMETER_CANNOT_BE_NULL_OR_GUIDEMPTY = "Parametre boş yada Guid.Empty olamaz";
        public const string PARAMETER_CANNOT_BE_NULL_OR_UNDEFINED = "Parametre boş yada Undefined olamaz";
        public const string PARAMETER_CANNOT_BE_NEGATIVE = "Parametre değeri 0 yada 0 'dan büyük olmalıdır.";
        public const string PARAMETER_CANNOT_BE_POSITIVE = "Parametre değeri 0 'dan büyük olmalıdır.";
        public const string PARAMETER_MUST_BE_NULL = "Parametre boş yada NULL değerli olmalı.";
        public const string PARAMETER_CANNOT_BE_GREATER_THAN = "Parametrenin alabileceği değer en fazla {0} olmalıdır.";
        public const string PARAMETER_CANNOT_BE_LESS_THAN = "Parametrenin alabileceği değer en az {0} olmalıdır.";
        public const string PARAMETER_EXPECTED = "Parametre '{0}' değerini içermelidir.";

        #endregion
    }
}
