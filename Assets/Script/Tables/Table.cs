using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class Table
{
    private TableType type;
    private TRFoundation[] trFoundations;

    public Table(TableType type)
    {
        this.type = type;
    }

    public void LoadTable(TableLoadType loadType, in string loadTablePath)
    {
        if (File.Exists(loadTablePath) == false)
        {
            Debug.LogError($"can't find Table, path: {loadTablePath}");
            return;
        }

        switch (loadType)
        {
            case TableLoadType.Local:
                LoadTableXml(loadTablePath);
                break;
            case TableLoadType.Binary:
                LoadTableBinary(loadTablePath);
                break;
        }
    }

    private void LoadTableXml(in string loadTablePath)
    {
        XmlReaderSettings settings = XmlHelper.GetDefaultReadSetting();
        XmlReader reader = XmlReader.Create(loadTablePath, settings);

        string typeString = type.ToString();
        List<TRFoundation> foundations = new List<TRFoundation>();

        while (reader.EOF == false)
        {
            if (reader.IsStartElement(typeString))
            {
                reader.ReadStartElement(typeString);
                TRFoundation trFoundation = TableFactory.CreateTRFoundation(type);
                trFoundation.ReadRawData(reader);
                reader.ReadEndElement();

                foundations.Add(trFoundation);
            }
            else
            {
                reader.Read();
            }
        }

        reader.Close();
        trFoundations = foundations.ToArray();
    }

    private void LoadTableBinary(in string loadTablePath)
    {
        if (File.Exists(loadTablePath) == false)
        {
            Debug.LogError($"can't find Table, path: {loadTablePath}");
            return;
        }

        FileStream stream = File.Open(loadTablePath, FileMode.Open);
        Assert.IsNotNull(stream);

        BinaryReader reader = new BinaryReader(stream);
        Assert.IsNotNull(reader);

        int count = reader.ReadInt32();
        trFoundations = new TRFoundation[count];
        for (int i = 0; i < count; ++i)
        {
            TRFoundation trFoundation = TableFactory.CreateTRFoundation(type);
            trFoundation.Read(reader);
            trFoundations[i] = trFoundation;
        }
    }

    public void WriteDataToBinaryFile(in string writeBinaryPath)
    {
        Assert.IsNotNull(trFoundations);
        if (File.Exists(writeBinaryPath))
        {
            File.Delete(writeBinaryPath);
        }

        MemoryStream memory = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(memory);
        {
            writer.Write(trFoundations.Length);

            for (int i = 0; i < trFoundations.Length; ++i)
            {
                TRFoundation trFoundation = trFoundations[i];

                trFoundation.Write(writer);
            }
            writer.Close();

            var bytes = memory.ToArray();
            File.WriteAllBytes(writeBinaryPath, bytes);
        }
    }

    public T GetRecordOrNull<T>(uint index) where T : TRFoundation
    {
        for (int i = 0; i < trFoundations.Length; ++i)
        {
            if (trFoundations[i].Index == index)
            {
                return (T)trFoundations[i];
            }
        }

        return null;
    }

    public void Clear()
    {

    }
}
