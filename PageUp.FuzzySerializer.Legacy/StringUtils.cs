using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageUp.FuzzySerializer.Legacy
{
    public class StringUtils
    {
        // StringUtils taken from Newtonsoft.JSON's own implementation so that it matches the behavior in the up-to-date PageUp.FuzzySerializer project
        // https://github.com/JamesNK/Newtonsoft.Json/blob/122afba9908832bd5ac207164ee6c303bfd65cf1/Src/Newtonsoft.Json/Utilities/StringUtils.cs
        public static string ToCamelCase(string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            char[] chars = s.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    if (char.IsSeparator(chars[i + 1]))
                    {
                        chars[i] = Char.ToLowerInvariant(chars[i]);
                    }

                    break;
                }

                chars[i] = Char.ToLowerInvariant(chars[i]);
            }

            return new string(chars);
        }
    }
}
