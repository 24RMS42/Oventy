using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace oventy
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Extensions method that verifies if a string is a valid email
        /// </summary>
        /// <param name="email">The string to be checked</param>
        /// <returns>bool</returns>
        public static bool IsValidEmail(
            this string email)
        {
            if(email == null)
                return false;
            var regex =
                new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@"
                          + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            return regex.IsMatch(email);
        }

        /// <summary>
        ///     Creates a KeyValuePair from an initial key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static KeyValuePair<string, string> ToKeyValuePair(
            this string key,
            string value)
        {
            return new KeyValuePair<string, string>(key,
                                                    value);
        }
    }
}
