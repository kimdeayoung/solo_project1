using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TRUnit : TRFoundation
{
    private UnitType unitType;

    public override bool ReadRawData(XmlReader reader)
    {
        bool result = base.ReadRawData(reader);


        return result;
    }

    public override void Read(BinaryReader reader)
    {
        base.Read(reader);
        unitType = (UnitType)reader.ReadInt32();
    }

    public override void Write(BinaryWriter writer)
    {
        base.Write(writer);
        writer.Write((int)unitType);
    }
}
