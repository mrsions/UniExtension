// Copyright (c) 2016 Sions
// 
// UniExtension version 1.0.0
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System
{
public static class stringExtension
{
    public static bool EqualsIgnoreCase(this string self, string target)
    {
        return string.Equals(self, target, StringComparison.OrdinalIgnoreCase);
    }

    public static string ToFormat(this string format, object arg0)
    {
        return string.Format(format, arg0);
    }
    public static string ToFormat(this string format, params object[] args)
    {
        return string.Format(format, args);
    }
    public static string ToFormat(this string format, object arg0, object arg1)
    {
        return string.Format(format, arg0, arg1);
    }
    public static string ToFormat(this string format, object arg0, object arg1, object arg2)
    {
        return string.Format(format, arg0, arg1, arg2);
    }
    public static bool IsEmpty([NotNullWhen(false)] this string str)
    {
        return str == null || str.Length == 0;
    }
    public static bool IsWhitespace(this string str)
    {
        for (int i = 0, len = str.Length; i < len; i++)
        {
            switch (str[i])
            {
                case ' ':
                case '\r':
                case '\n':
                case '\t':
                    break;
                default:
                    return false;
            }
        }
        return true;
    }

    public static string? ToExistOrNull(this string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return null;
        }
        return str;
    }

    public static string? ToNotEmptyOrNull(this string? str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return null;
        }
        return str;
    }

    public static string? TryTrim(this string? str)
    {
        return str != null ? str.Trim() : null;
    }

    public static bool IsNotEmpty([NotNullWhen(true)] this string? str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static int CountChars(this string str, char c)
    {
        var result = 0;
        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] == c)
            {
                result++;
            }
        }
        return result;
    }

    public static bool ContainsChar(this string str, char c)
    {
        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] == c)
            {
                return true;
            }
        }
        return false;
    }

    public static string Substring(this string str, string find, bool removeFind)
    {
        var index = str.IndexOf(find);
        if (index == -1) return str;

        if (removeFind)
        {
            return str.Substring(index + find.Length);
        }
        else
        {
            return str.Substring(index);
        }
    }

    public static string? TrySubstring(this string? str, int startIndex, int length)
    {
        if (str == null) return null;
        if (str.Length >= startIndex + length)
        {
            return str.Substring(startIndex, length);
        }
        else if (str.Length <= startIndex)
        {
            return "";
        }
        else
        {
            return str.Substring(startIndex, str.Length - startIndex);
        }
    }
    public static string? TrySubstring(this string? str, int length)
    {
        if (str == null) return null;
        if (str.Length < length)
        {
            return str;
        }
        else
        {
            return str.Substring(0, length);
        }
    }

    public static string? TryLastSubstring(this string? str, int length)
    {
        if (str == null) return null;
        if (str.Length < length)
        {
            return str;
        }
        else
        {
            return str.Substring(str.Length - length, length);
        }
    }

    public static string RemoveEnd(this string str, int end)
    {
        return str.Substring(0, str.Length - end);
    }

    //public static string[] FindSplit(this string str, string find, int startIndex = 0, StringSplitOptions option = StringSplitOptions.None)
    //{
    //    int indexOf = str.IndexOf(find, startIndex);
    //    if (indexOf == -1)
    //    {
    //        if (option == StringSplitOptions.RemoveEmptyEntries && str.IsEmpty())
    //        {
    //            return new string[0];
    //        }
    //        else
    //        {
    //            return new string[] { str };
    //        }
    //    }
    //    else
    //    {
    //        string first = str.Substring(0, indexOf);
    //        string second = str.Substring(indexOf + find.Length);

    //        if (option == StringSplitOptions.None)
    //        {
    //            return new string[] { first, second };
    //        }
    //        else if (first.IsEmpty() && second.IsEmpty())
    //        {
    //            return new string[0];
    //        }
    //        else if (first.IsEmpty())
    //        {
    //            return new string[] { second };
    //        }
    //        else if (second.IsEmpty())
    //        {
    //            return new string[] { first };
    //        }
    //        else
    //        {
    //            return new string[] { first, second };
    //        }
    //    }
    //}

    public static (string, string?) FindSplit(this string str, string find, int startIndex = 0)
    {
        var indexOf = str.IndexOf(find, startIndex);
        if (indexOf == -1)
        {
            return (str, null);
        }
        else
        {
            var first = str.Substring(startIndex, indexOf);
            var second = str.Substring(indexOf + find.Length);

            return (first, second);
        }
    }

    public static (string, string?) LastFindSplit(this string str, string find, int startIndex = 0)
    {
        var indexOf = str.LastIndexOf(find, str.Length - startIndex);
        if (indexOf == -1)
        {
            return (str, null);
        }
        else
        {
            var first = str.Substring(startIndex, indexOf);
            var second = str.Substring(indexOf + find.Length);

            return (first, second);
        }
    }

    public static int ParseInt32(this string str, int offset, int length)
    {
        char c;
        var result = 0;
        for (var i = 0; i < length; i++)
        {
            c = str[offset + i];
            if ('0' <= c && c <= '9')
            {
                result += c - '0';
            }
            else
            {
                throw new System.FormatException("'{0}' is not convert int32".ToFormat(str.Substring(offset, length)));
            }

            if (i + 1 < length)
            {
                result *= 10;
            }
        }
        return result;
    }

    public static string FixEnd(this string str, char c)
    {
        if (str == null || str.Length == 0)
            return string.Empty;

        if (str[str.Length - 1] != c)
            str += c;

        return str;
    }

    public static string FixStart(this string str, char c)
    {
        if (str == null || str.Length == 0)
            return string.Empty;

        if (str[0] != c)
            str = c + str;

        return str;
    }

    static System.Text.StringBuilder? sb;
    public static string JoinToString<T>(this IList<T> array, string separator) => array.JoinToString(separator, 0, array.Count);
    public static string JoinToString<T>(this IList<T> array, string separator, int offset, int length)
    {
        if (sb == null) sb = new System.Text.StringBuilder();

        for (var i = 0; i < length; i++)
        {
            if (i != 0 && separator.Length > 0) sb.Append(separator);
            sb.Append(array[i + offset]);
        }
        return sb.ToStringAndClear();
    }

    //public static string[] SplitRange(this string str, int range)
    //{
    //    //using var list = PList.Take<string>();

    //    //int offset = 0;
    //    //while (true)
    //    //{
    //    //    if (offset + range < str.Length)
    //    //    {
    //    //        list.Add(str.Substring(offset, range));
    //    //        offset += range;
    //    //    }
    //    //    else
    //    //    {
    //    //        list.Add(str.Substring(offset));
    //    //        return list.ToArray();
    //    //    }
    //    //}
    //    return null;
    //}

    public static string[] Split(this string str, System.StringSplitOptions option, params char[] split)
    {
        return str.Split(split, option);
    }

    public static string[] SplitRemoved(this string str, params char[] split)
    {
        return str.Split(split, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// Dictionary를 이용하여 format 시킨다.
    /// </summary>
    /// <example>
    /// <code>
    ///     string format = "v:{Name} / {{ HP : {Hp}}}";
    ///     string message = format.ToFormat(new Dictionary<string, string> { ["Name"] = "Sions", ["Hp"] = "52" })
    ///     // v:Sions / { HP : 52}
    /// </code>
    /// </example>
    /// <typeparam name="T"></typeparam>
    /// <param name="format"></param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static string ToFormat<T>(this string format, IDictionary<string, T> dictionary)
    {
        var sb = new StringBuilder(format.Length * 2);
        var sb2 = new StringBuilder(format.Length);

        var openChar = (char)0;

        for (int i = 0, len = format.Length; i < len; i++)
        {
            var c = format[i];
            if (c == '{')
            {
                if (openChar == 0)
                {
                    openChar = c;
                }
                else if (openChar == '{')
                {
                    sb.Append('{');
                    openChar = (char)0;
                }
                else
                {
                    throw new FormatException($"Input string was not in a correct format. ({i}/{format.Length})");
                }
            }
            else if (c == '}')
            {
                if (openChar == 0)
                {
                    openChar = c;
                }
                else if (openChar == '}')
                {
                    sb.Append('}');
                    openChar = (char)0;
                }
                else if (openChar == '{')
                {
                    if (sb2.Length > 0)
                    {
                        var ssb2 = sb2.ToString();
                        if (dictionary.TryGetValue(ssb2, out var rst))
                        {
                            sb.Append(rst);
                        }
                        else
                        {
                            sb.Append('{').Append(ssb2).Append('}');
                        }
                        sb2.Clear();
                        openChar = (char)0;
                    }
                    else
                    {
                        throw new FormatException($"Input string was not in a correct format. ({i - 1}/{format.Length})");
                    }
                }
                else
                {
                    throw new FormatException($"Input string was not in a correct format. ({i}/{format.Length})");
                }
            }
            else
            {
                if (openChar != 0)
                {
                    sb2.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
        }

        return sb.ToString();
    }

    static string SpaceString = " \t\r\n";
    static char[] SpaceChars = SpaceString.ToCharArray();
    public static string UnwrapQuotes(this string str)
    {
        const string QuoteString = "'\"`";

        if (str == null)
        {
            return string.Empty;
        }
        else if (string.IsNullOrWhiteSpace(str))
        {
            return str.Trim(SpaceChars);
        }

        // 공백이 아닌 첫글자를 찾는다.
        var startIndex = 0;
        for (; startIndex < str.Length; startIndex++)
        {
            if (SpaceString.ContainsChar(str[startIndex]))
            {
                continue;
            }
            break;
        }

        // quotes에 속하지않는 문자가 시작문자라면 제거한다.
        if (!QuoteString.ContainsChar(str[startIndex]))
        {
            return str.Trim(SpaceChars);
        }

        var endIndex = str.Length - 1;
        for (; endIndex > startIndex; endIndex--)
        {
            if (SpaceString.ContainsChar(str[endIndex]))
            {
                continue;
            }
            if (str[startIndex] == str[endIndex])
            {
                return str.Substring(startIndex + 1, endIndex - startIndex - 1);
            }

            break;
        }

        return str.Trim(SpaceChars);
    }

    public static char LastChar(this string str)
    {
        return str == null || str.Length == 0 ? '\0' : str[str.Length - 1];
    }

    public static char FirstChar(this string str)
    {
        return str == null || str.Length == 0 ? '\0' : str[0];
    }

    /// <summary>
    /// ToLower는 무조건 새로운 string을 만들기때문에
    /// 그 전에 A-Z 의 값이 있을경우에만 ToLower를 수행한다.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToLowerWithCheck(this string str)
    {
        char c;
        for (var i = 0; i < str.Length; i++)
        {
            c = str[i];
            if ('A' <= c && c <= 'Z')
            {
                return str.ToLower();
            }
        }
        return str;
    }

    /// <summary>
    /// ToUpper는 무조건 새로운 string을 만들기때문에
    /// 그 전에 a-z 의 값이 있을경우에만 ToUpper를 수행한다.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToUpperWithCheck(this string str)
    {
        char c;
        for (var i = 0; i < str.Length; i++)
        {
            c = str[i];
            if ('a' <= c && c <= 'z')
            {
                return str.ToUpper();
            }
        }
        return str;
    }

    public static byte[] GetBytes(this string str, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetBytes(str);
    }

    public static string GetString(this byte[] bytes, int offset = 0, int length = -1, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        if (length == -1) length = bytes.Length - offset;
        return encoding.GetString(bytes, offset, length);
    }

    /// <summary>
    /// 좌우 여백을 입력된 msg 내용을 사용하여 채운다.
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static string PadRandom(this string msg, int left, int right)
    {
        var sb = new StringBuilder(msg.Length + left + right);

        for (var i = 0; i < left; i++)
        {
            sb.Append(msg[UnityEngine.Random.Range(0, msg.Length)]);
        }

        sb.Append(msg);

        for (var i = 0; i < right; i++)
        {
            sb.Append(msg[UnityEngine.Random.Range(0, msg.Length)]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 좌우의 여백을 지운 나머지를 반환한다.
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static string SubstringPad(this string msg, int left, int right)
    {
        return msg.Substring(left, msg.Length - left - right);
    }
}
}
