using System;

namespace XrmEarth.Core.Exceptions
{
    public static class ExceptionThrow
    {
        #region | Public Methods |

        public static void IfNull(object parameter, string name, string message = null)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NULL);
            }
        }

        public static void IfEmpty(string value, string name, string message = null)
        {
            ExceptionThrow.IfNull(value, name, message);

            if (value.Length == 0)
            {
                throw new ArgumentNullException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NULL);
            }
        }

        public static void IfNullOrEmpty(string parameter, string name, string message = null)
        {
            ExceptionThrow.IfNull(parameter, name, message);
            ExceptionThrow.IfEmpty(parameter, name, message);
        }

        public static void IfNullOrEmpty(Array parameter, string name, string message = null)
        {
            ExceptionThrow.IfNull(parameter, name, message);

            if (parameter.Length == 0)
            {
                throw new ArgumentNullException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NULL);
            }
        }

        public static void IfGuidEmpty(Guid? parameter, string name, string message = null)
        {
            ExceptionThrow.IfNull(parameter, name);

            if (parameter == Guid.Empty)
            {
                throw new ArgumentException(!string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_GUIDEMPTY, name);
            }
        }

        public static void IfGuidEmptyOrNull(Guid? parameter, string name, string message = null)
        {
            ExceptionThrow.IfNull(parameter, name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NULL);
            ExceptionThrow.IfGuidEmpty(parameter, name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_GUIDEMPTY);
        }

        /// <summary>
        /// 0 'dan küçük
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public static void IfNegative(int value, string name, string message = null)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NEGATIVE);
            }
        }

        /// <summary>
        /// 0 'dan küçük
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public static void IfNegative(TimeSpan value, string name, string message = null)
        {
            if (value < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NEGATIVE);
            }
        }

        public static void IfNotEmpty(string value, string name, string message = null)
        {
            if (value != null && value.Length != 0)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_MUST_BE_NULL);
            }
        }

        /// <summary>
        /// 0 veya 0 'dan küçük
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public static void IfNotPositive(int value, string name, string message = null)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NEGATIVE);
            }
        }

        /// <summary>
        /// 0 veya 0 'dan küçük
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public static void IfNotPositive(long value, string name, string message = null)
        {
            if (value <= (long)0)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_NEGATIVE);
            }
        }

        public static void IfGreaterThan(int value, int maxLimit, string name, string message = null)
        {
            if (value > maxLimit)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_GREATER_THAN.Replace("{0}", maxLimit.ToString()));
            }
        }

        public static void IfGreaterThan(long value, long maxLimit, string name, string message = null)
        {
            if (value > maxLimit)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_GREATER_THAN.Replace("{0}", maxLimit.ToString()));
            }
        }

        public static void IfLessThan(int value, int minLimit, string name, string message = null)
        {
            if (value > minLimit)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_LESS_THAN.Replace("{0}", minLimit.ToString()));
            }
        }

        public static void IfLessThan(long value, long minLimit, string name, string message = null)
        {
            if (value > minLimit)
            {
                throw new ArgumentOutOfRangeException(name, !string.IsNullOrEmpty(message) ? message : ExceptionDefinitions.PARAMETER_CANNOT_BE_LESS_THAN.Replace("{0}", minLimit.ToString()));
            }
        }

        public static void IfNotExpected(string value, string expectedValue, string name, string message = null)
        {
            if (!value.Equals(expectedValue))
            {
                throw new ArgumentException(!string.IsNullOrEmpty(message) ? message : string.Format(ExceptionDefinitions.PARAMETER_EXPECTED, expectedValue), name);
            }
        }

        #endregion
    }
}
