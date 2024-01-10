using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TRUnit : TRFoundation
{
    private UnitType unitType;

    private uint hp;
    private uint def;
    private float moveSpeed;

    private string prefabName;

    private uint[] skillIndexes;

    public UnitType UnitType { get => unitType; }
    public uint Hp { get => hp; }
    public uint Def { get => def; }
    public float MoveSpeed { get => moveSpeed; }
    public string PrefabName { get => prefabName; }
    public uint[] SkillIndexes { get => skillIndexes; }

    public override bool ReadRawData(XmlReader reader)
    {
        bool result = base.ReadRawData(reader);
        result &= XmlHelper.ReadEnumInt(reader, "UnitType", ref unitType);
        result &= XmlHelper.ReadElement(reader, "Hp", ref hp);
        result &= XmlHelper.ReadElement(reader, "Def", ref def);
        result &= XmlHelper.ReadElement(reader, "MoveSpeed", ref moveSpeed);
        result &= XmlHelper.ReadElement(reader, "PrefabName", ref prefabName);
        result &= XmlHelper.ReadElementArray(reader, "SkillIndexes", ref skillIndexes);

        return result;
    }

    public override void Read(BinaryReader reader)
    {
        base.Read(reader);
        unitType = (UnitType)reader.ReadInt32();
        hp = reader.ReadUInt32();
        def = reader.ReadUInt32();
        moveSpeed = reader.ReadSingle();
        prefabName = reader.ReadString();
        skillIndexes = reader.ReadUInt32Array();
    }

    public override void Write(BinaryWriter writer)
    {
        base.Write(writer);
        writer.Write((int)unitType);
        writer.Write(hp);
        writer.Write(def);
        writer.Write(moveSpeed);
        writer.Write(prefabName);
        writer.Write(skillIndexes);
    }
}
