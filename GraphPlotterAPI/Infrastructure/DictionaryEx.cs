

// ReSharper disable once CheckNamespace

using System.Text;

using MathCore.CSV;
using MathCore.Values;

using Microsoft.Extensions.Primitives;

namespace System.Collections.Generic;

public static class DictionaryEx
{
    private static readonly string __NewLine = Environment.NewLine;
    private const int __IdentSize = 4;

    public static string ToStringJson(this IDictionary values, bool Ident = false) => values.ToStringBuilderJson(Ident ? 1 : 0).ToString();

    private static StringBuilder ToStringBuilderJson(this IDictionary values, int IdentLevel)
    {
        if (values.Count == 0) return new StringBuilder("{ }");

        var result = new StringBuilder();
        var ident = IdentLevel > 1 ? new string(' ', (IdentLevel + 1) * __IdentSize) : null;

        if (IdentLevel > 0)
            result.AppendLine("{");
        else
            result.Append("{");

        foreach (var key in values.Keys)
            switch (values[key])
            {
                case Array array:
                    if (IdentLevel > 0)
                        result.AppendFormat("{0}\"{1}\" : {2}{3}", ident, key, array.ToStringBuilderJson(IdentLevel), __NewLine);
                    else
                        result.AppendFormat("\"{0}\" : {1}, ", key, array.ToStringBuilderJson(IdentLevel));
                    break;

                case IDictionary dict:
                    if (IdentLevel > 0)
                    {
                        result.Append(ident);
                        result.AppendFormat("\"{0}\" : \"{1}\",{2}", key, dict.ToStringBuilderJson(IdentLevel + 1), __NewLine);
                    }
                    else
                        result.AppendFormat("\"{0}\" : \"{1}\", ", key, dict.ToStringBuilderJson(IdentLevel));

                    break;

                case { } value:
                    if (IdentLevel > 0)
                    {
                        result.Append(ident);
                        result.AppendFormat("\"{0}\" : \"{1}\",{2}", key, value, __NewLine);
                    }
                    else
                        result.AppendFormat("\"{0}\" : \"{1}\", ", key, value);
                    break;
            }

        result.Length -= 2;

        if (IdentLevel > 0)
            result.Append(new string(' ', IdentLevel * __IdentSize));
        result.Append("}");

        return result;
    }

    private static StringBuilder ToStringBuilderJson(this Array array, int IdentLevel)
    {
        if (array.Length == 0) return new StringBuilder("[ ]");

        var result = new StringBuilder("[");
        var ident = IdentLevel > 1 ? new string(' ', (IdentLevel + 1) * __IdentSize) : null;

        foreach (var element in array)
            switch (element)
            {
                case Array array_element:
                    if (IdentLevel > 0)
                        result.AppendFormat("{0}\"{1}\",{2}", ident, array_element.ToStringBuilderJson(IdentLevel + 1), __NewLine);
                    else
                        result.AppendFormat("\"{0}\", ", array_element.ToStringBuilderJson(0));
                    break;

                case IDictionary dictionary_element:
                    if (IdentLevel > 0)
                        result.AppendFormat("{0}\"{1}\",{2}", ident, dictionary_element.ToStringBuilderJson(IdentLevel + 1), __NewLine);
                    else
                        result.AppendFormat("\"{0}\", ", dictionary_element.ToStringBuilderJson(0));
                    break;

                case string:
                    if (IdentLevel > 0)
                        result.AppendFormat("{0}\"{1}\",{2}", ident, element, __NewLine);
                    else
                        result.AppendFormat("\"{0}\", ", element);
                    break;

                default:
                    if (IdentLevel > 0)
                        result.AppendFormat("{0}{1},{2}", ident, element, __NewLine);
                    else
                        result.AppendFormat("{0}, ", element);
                    break;
            }


        result.Length -= 2;

        if (IdentLevel > 0)
            result.Append(new string(' ', IdentLevel * __IdentSize));
        result.Append("]");

        return result;
    }
}