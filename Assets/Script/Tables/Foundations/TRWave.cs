using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TRWave : TRFoundation
{


    public override bool ReadRawData(XmlReader reader)
    {
        return base.ReadRawData(reader);
    }

    public override void Read(BinaryReader reader)
    {
        base.Read(reader);
    }

    public override void Write(BinaryWriter writer)
    {
        base.Write(writer);
    }
}
