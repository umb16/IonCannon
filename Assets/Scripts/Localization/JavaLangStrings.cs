using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class JavaLangStrings
{
    public static Dictionary<string, string> ConverJavaLocalToDict(string text)
    {
        text = Regex.Replace(text, @"\r\n?|\n", "\n");
        string[] temp = text.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, string> result = new Dictionary<string, string>();
        for (int i = 0; i < temp.Length; i++)
        {
            var item = temp[i];
            if (item.Length > 0 && item[0] == '#')
                continue;
            item = item.Replace("\\n", System.Environment.NewLine).Replace("\\\"", "\"");
            int index = item.IndexOf("=");
            if (index > 0&&item.Length>index+1)
            {
                string[] pair = new string[2];
                pair[0] = item.Substring(0, index);
                pair[1] = item.Substring(index+1, item.Length - index-1);

                if (!string.IsNullOrEmpty(pair[0]))
                {
                    if (!result.ContainsKey(pair[0]))
                    {
                        result.Add(pair[0], pair[1]);
                    //    print(pair[0] + "###" + pair[1]);
                    }
                }
                else
                {
                    MonoBehaviour.print("pair[0] is empty" + " " + item);
                }
            }
            else
            {
               MonoBehaviour.print("= no entry" + " " + item);
                return null;
            }
        }
        return result;
    }

    public static string ConverDictToJavaLocal(Dictionary<string, string> text)
    {
        string result = "";
        foreach (var item in text)
        {
            string temp = item.Key + "=" + item.Value;
            
            result += Regex.Replace(temp, @"\r\n?|\n", "\\n") + "\n";
        }
        return result;
    }
    public static string ConverDictToTextOnly(Dictionary<string, string> text)
    {
        string result = "";
        foreach (var item in text)
        {
            string temp = item.Value;

            result += temp+ "\n";
        }
        return result;
    }
}
