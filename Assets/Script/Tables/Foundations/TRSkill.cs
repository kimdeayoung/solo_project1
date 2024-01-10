using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TRSkill : TRFoundation
{
    private SkillType skillType;
    private uint[] skillEventIndexes;
    private float activeCoolTime;

    private int[][] eventIntegerValues;
    private float[][] eventFloatValues;
    private string[][] eventStringValues;

    public SkillType SkillType { get => skillType; }
    public uint[] SkillEventIndexes { get => skillEventIndexes; }
    public float ActiveCoolTime { get => activeCoolTime; }

    public int[][] EventIntegerValues { get => eventIntegerValues; }
    public float[][] EventFloatValues { get => eventFloatValues; }
    public string[][] EventStringValues { get => eventStringValues; }

    public override bool ReadRawData(XmlReader reader)
    {
        bool result = base.ReadRawData(reader);
        result &= XmlHelper.ReadEnumInt(reader, "SkillType", ref skillType);
        result &= XmlHelper.ReadElementArray(reader, "SkillEventIndexes", ref skillEventIndexes);
        result &= XmlHelper.ReadElement(reader, "ActiveCoolTime", ref activeCoolTime);

        result &= XmlHelper.ReadElementJaggedArray(reader, "EventIntegerValues", ref eventIntegerValues);
        result &= XmlHelper.ReadElementJaggedArray(reader, "EventFloatValues", ref eventFloatValues);
        result &= XmlHelper.ReadElementJaggedArray(reader, "EventStringValues", ref eventStringValues);

        return result;
    }

    public override void Read(BinaryReader reader)
    {
        base.Read(reader);
        skillType = (SkillType)reader.ReadInt32();
        skillEventIndexes = reader.ReadUInt32Array();
        activeCoolTime = reader.ReadSingle();

        eventIntegerValues = reader.ReadInt32JaggedArray();
        eventFloatValues = reader.ReadSingleJaggedArray();
        eventStringValues = reader.ReadStringJaggedArray();
    }

    public override void Write(BinaryWriter writer)
    {
        base.Write(writer);
        writer.Write((int)skillType);
        writer.Write(skillEventIndexes);
        writer.Write(activeCoolTime);

        writer.Write(eventIntegerValues);
        writer.Write(eventFloatValues);
        writer.Write(eventStringValues);
    }
}
