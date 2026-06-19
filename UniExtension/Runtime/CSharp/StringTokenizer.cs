#nullable enable

using System.Collections;
using System.Collections.Generic;

namespace UniExtension
{
public sealed class StringTokenizer : IEnumerable<string>
{
    private readonly string _source;
    private readonly string _delimiters;
    private readonly bool _skipEmpty;
    private readonly bool _returnDelims;

    public StringTokenizer(string source, string delimiters, bool skipEmpty, bool returnDelims)
    {
        _source = source ?? string.Empty;
        _delimiters = delimiters ?? string.Empty;
        _skipEmpty = skipEmpty;
        _returnDelims = returnDelims;
    }

    public IEnumerator<string> GetEnumerator()
    {
        if (_source.Length == 0)
        {
            if (!_skipEmpty)
            {
                yield return string.Empty;
            }
            yield break;
        }

        int tokenStart = 0;
        for (int i = 0; i < _source.Length; i++)
        {
            var ch = _source[i];
            bool isDelim = _delimiters.IndexOf(ch) >= 0;

            if (isDelim)
            {
                int tokenLen = i - tokenStart;
                if (!_skipEmpty || tokenLen > 0)
                {
                    yield return _source.Substring(tokenStart, tokenLen);
                }

                if (_returnDelims)
                {
                    yield return ch.ToString();
                }

                tokenStart = i + 1;
            }
        }

        int tailLen = _source.Length - tokenStart;
        if (!_skipEmpty || tailLen > 0)
        {
            yield return _source.Substring(tokenStart, tailLen);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
}
