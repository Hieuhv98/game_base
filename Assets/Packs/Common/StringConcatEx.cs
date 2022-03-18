using System.Text;
using System.Threading;

namespace Lance.Common
{
    public static class StringConcatEx
    {
        private static readonly ThreadLocal<StringBuilder> StringBuilder = new ThreadLocal<StringBuilder>(() => new StringBuilder(256));

        public static string ConcatEx(this string str1, string value)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1).Append(value);

                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, int value)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1).Append(value);

                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, uint value)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1).Append(value);

                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, long value)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1).Append(value);

                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, float value)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1).Append(value);

                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, double value)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1).Append(value);

                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, string str2, string str3)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1);
                StringBuilder.Value.Append(str2);
                StringBuilder.Value.Append(str3);

                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, string str2, string str3, string str4)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1);
                StringBuilder.Value.Append(str2);
                StringBuilder.Value.Append(str3);
                StringBuilder.Value.Append(str4);


                return StringBuilder.Value.ToString();
            }
        }

        public static string ConcatEx(this string str1, string str2, string str3, string str4, string str5)
        {
            {
                StringBuilder.Value.Clear();

                StringBuilder.Value.Append(str1);
                StringBuilder.Value.Append(str2);
                StringBuilder.Value.Append(str3);
                StringBuilder.Value.Append(str4);
                StringBuilder.Value.Append(str5);

                return StringBuilder.Value.ToString();
            }
        }
    }
}