using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TRSkillEvent : TRFoundation
{
    private EventType eventType;

    public EventType EventType { get => eventType; }

    public override bool ReadRawData(XmlReader reader)
    {
        bool result = base.ReadRawData(reader);
        result &= XmlHelper.ReadEnumInt(reader, "EventType", ref eventType);

        return result;
    }

    public override void Read(BinaryReader reader)
    {
        base.Read(reader);

        eventType = (EventType)reader.ReadInt32();
    }

    public override void Write(BinaryWriter writer)
    {
        base.Write(writer);

        writer.Write((int)eventType);
    }
}
