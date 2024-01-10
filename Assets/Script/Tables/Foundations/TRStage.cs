using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class TRStage : TRFoundation
{
    private uint stageNumber;
    private StageDifficultyType difficultyType;
    private string sceneName;
    private uint[] waveIndexes;
    private Vector3 playerStartPos;

    public uint StageNumber { get => stageNumber; }
    public StageDifficultyType DifficultyType { get => difficultyType; }
    public string SceneName { get => sceneName; }
    public uint[] WaveIndexes { get => waveIndexes; }
    public Vector3 PlayerStartPos { get => playerStartPos; }

    public override bool ReadRawData(XmlReader reader)
    {
        bool result = base.ReadRawData(reader);
        result &= XmlHelper.ReadElement(reader, "StageNumber", ref stageNumber);
        result &= XmlHelper.ReadEnumInt(reader, "DifficultyType", ref difficultyType);
        result &= XmlHelper.ReadElement(reader, "Scene", ref sceneName);
        result &= XmlHelper.ReadElementArray(reader, "Wave", ref waveIndexes);
        result &= XmlHelper.ReadElement(reader, "PlayerStartPos", ref playerStartPos);

        return result;
    }

    public override void Read(BinaryReader reader)
    {
        base.Read(reader);
        stageNumber = reader.ReadUInt32();
        difficultyType = (StageDifficultyType)reader.ReadInt32();
        sceneName = reader.ReadString();
        waveIndexes = reader.ReadUInt32Array();
    }

    public override void Write(BinaryWriter writer)
    {
        base.Write(writer);
        writer.Write(stageNumber);
        writer.Write((int)difficultyType);
        writer.Write(sceneName);
        writer.Write(waveIndexes);
    }
}
