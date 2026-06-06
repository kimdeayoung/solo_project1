using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public abstract class TRFoundation
{
    protected uint index;

    public uint Index { get => index; }

    public virtual bool ReadRawData(XmlReader reader)
    {
        bool result = true;
        result &= XmlHelper.ReadElement(reader, "Index", ref index);

        return result;
    }

    public virtual void Read(BinaryReader reader)
    {
        index = reader.ReadUInt32();
    }

    public virtual void Write(BinaryWriter writer)
    {
        writer.Write(index);
    }

}
