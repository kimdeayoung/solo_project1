using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Scripting;

public static class XmlHelper
{
    private static XmlReaderSettings defaultReadsettings;
    private static XmlWriterSettings defaultWritesettings;

    public static XmlReaderSettings GetDefaultReadSetting()
    {
        if (defaultReadsettings == null)
        {
            defaultReadsettings = new XmlReaderSettings();
            defaultReadsettings.IgnoreComments = true;
            defaultReadsettings.IgnoreWhitespace = true;
            defaultReadsettings.IgnoreProcessingInstructions = true;
        }
        return defaultReadsettings;
    }

    public static XmlWriterSettings GetDefaultSetting()
    {
        if (defaultWritesettings == null)
        {
            defaultWritesettings = new XmlWriterSettings();
            defaultWritesettings.Indent = true;
            defaultWritesettings.OmitXmlDeclaration = false;
            defaultWritesettings.IndentChars = ("\t");
            defaultWritesettings.NewLineChars = "\r\n";
        }
        return defaultWritesettings;
    }

    public static bool ReadElement<T>(XmlReader reader, string elementName, ref T result)
    {
        try
        {
            if (reader.IsEmptyElement)
            {
                Debug.LogError($"[Failed] empty element, element name: {reader.Name}");
                return false;
            }

            reader.ReadStartElement(elementName);
            string content = reader.ReadContentAsString();
            result = (T)Convert.ChangeType(content, typeof(T));
            reader.ReadEndElement();
        }
        catch (XmlException e)
        {
            Debug.LogError("[Failed] invalid xml node name: " + elementName + ", " + e.Message);
        }

        return true;
    }

    public static bool ReadElement(XmlReader reader, string elementName, ref Vector3 result)
    {
        try
        {
            if (reader.IsEmptyElement)
            {
                Debug.LogError($"[Failed] empty element, element name: {reader.Name}");
                return false;
            }

            reader.ReadStartElement(elementName);
            string content = reader.ReadContentAsString();
            int splitStartIndex = 0;
            splitStartIndex = content.IndexOf('(', splitStartIndex) + 1;
            int splitEndIndex = content.IndexOf(")", splitStartIndex);

            content = content.Substring(splitStartIndex, splitEndIndex - splitStartIndex);
            string[] splitResult = content.Split(",", StringSplitOptions.RemoveEmptyEntries);
            result = new Vector3(Convert.ToSingle(splitResult[0]), Convert.ToSingle(splitResult[1]), Convert.ToSingle(splitResult[2]));

            reader.ReadEndElement();
        }
        catch (XmlException e)
        {
            Debug.LogError("[Failed] invalid xml node name: " + elementName + ", " + e.Message);
        }

        return true;
    }

    public static bool ReadElementArray<T>(XmlReader reader, string elementName, ref T[] result)
    {
        try
        {
            if (reader.IsEmptyElement)
            {
                Debug.LogError($"[Failed] empty element, element name: {reader.Name}");
                return false;
            }

            reader.ReadStartElement(elementName);
            string content = reader.ReadContentAsString();
            string[] splits = content.Split(",");
            result = new T[splits.Length];
            for (int i = 0; i < splits.Length; ++i)
            {
                result[i] = (T)Convert.ChangeType(splits[i], typeof(T));
            }

            reader.ReadEndElement();
        }
        catch (XmlException e)
        {
            Debug.LogError("[Failed] invalid xml node name: " + elementName + ", " + e.Message);
        }

        return true;
    }

    public static bool ReadElementJaggedArray<T>(XmlReader reader, string elementName, ref T[][] result)
    {
        try
        {
            if (reader.IsEmptyElement)
            {
                Debug.LogError($"[Failed] empty element, element name: {reader.Name}");
                return false;
            }

            reader.ReadStartElement(elementName);
            string content = reader.ReadContentAsString();
            if (content == "empty")
            {
                result = null;
            }
            else
            {
                List<string> splitResult1 = new List<string>(16);
                int splitStartIndex = 0;
                while (splitStartIndex + 1 < content.Length)
                {
                    splitStartIndex = content.IndexOf('(', splitStartIndex) + 1;
                    Assert.IsTrue(splitStartIndex > 0);

                    int splitEndIndex = content.IndexOf(")", splitStartIndex);
                    Assert.IsTrue(splitEndIndex > 0);

                    splitResult1.Add(content.Substring(splitStartIndex, splitEndIndex - splitStartIndex));
                    splitStartIndex = splitEndIndex;
                }

                result = new T[splitResult1.Count][];
                for (int i = 0; i < splitResult1.Count; ++i)
                {
                    string[] splitResult2 = splitResult1[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                    result[i] = new T[splitResult2.Length];

                    for (int j = 0; j < splitResult2.Length; ++j)
                    {
                        result[i][j] = (T)Convert.ChangeType(splitResult2[j], typeof(T));
                    }
                }
            }

            reader.ReadEndElement();
        }
        catch (XmlException e)
        {
            Debug.LogError("[Failed] invalid xml node name: " + elementName + ", " + e.Message);
        }

        return true;
    }

    public static bool ReadEnumStringInt<T>(XmlReader reader, string elementName, ref int result) where T : IConvertible
    {
        string text = string.Empty;
        bool readResult = ReadElement(reader, elementName, ref text);
        T enumValue = (T)Enum.Parse(typeof(T), text);
        result = (int)Convert.ChangeType(enumValue, typeof(int));

        return readResult;
    }

    public static bool ReadEnumInt<T>(XmlReader reader, string elementName, ref T result) where T : IConvertible
    {
        int value = 0;
        bool readResult = ReadElement(reader, elementName, ref value);
        result = (T)(Enum.ToObject(typeof(T), value));

        return readResult;
    }

    public static bool ReadEnumIntArray<T>(XmlReader reader, string elementName, ref T[] result) where T : IConvertible
    {
        int[] value = new int[0];
        bool readResult = ReadElementArray(reader, elementName, ref value);
        result = new T[value.Length];
        for (int i = 0; i < value.Length; ++i)
        {
            result[i] = (T)(Enum.ToObject(typeof(T), value[i]));
        }

        return readResult;
    }
}
