using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class BinaryHelper
{
    #region BinaryReader
    public static int[] ReadInt32Array(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length == 0)
        {
            return null;
        }

        int[] values = new int[length];
        for (int i = 0; i < length; ++i)
        {
            values[i] = reader.ReadInt32();
        }
        return values;
    }

    public static uint[] ReadUInt32Array(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length == 0)
        {
            return null;
        }

        uint[] values = new uint[length];
        for (int i = 0; i < length; ++i)
        {
            values[i] = reader.ReadUInt32();
        }
        return values;
    }

    public static long[] ReadInt64Array(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length == 0)
        {
            return null;
        }

        long[] values = new long[length];
        for (int i = 0; i < length; ++i)
        {
            values[i] = reader.ReadInt64();
        }
        return values;
    }

    public static float[] ReadSingleArray(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length == 0)
        {
            return null;
        }

        float[] values = new float[length];
        for (int i = 0; i < length; ++i)
        {
            values[i] = reader.ReadSingle();
        }
        return values;
    }

    public static double[] ReadDoubleArray(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length == 0)
        {
            return null;
        }

        double[] values = new double[length];
        for (int i = 0; i < length; ++i)
        {
            values[i] = reader.ReadDouble();
        }
        return values;
    }

    public static string[] ReadStringArray(this BinaryReader reader)
    {
        int length = reader.ReadInt32();
        if (length == 0)
        {
            return null;
        }

        string[] values = new string[length];
        for (int i = 0; i < length; ++i)
        {
            values[i] = reader.ReadString();
        }
        return values;
    }

    public static int[][] ReadInt32JaggedArray(this BinaryReader reader)
    {
        int arrLength = reader.ReadInt32();
        if (arrLength == 0)
        {
            return null;
        }

        int[][] values = new int[arrLength][];
        for (int i = 0; i < values.Length; ++i)
        {
            int length = reader.ReadInt32();
            values[i] = new int[length];
            for (int j = 0; j < length; ++j)
            {
                values[i][j] = reader.ReadInt32();
            }
        }
        return values;
    }

    public static float[][] ReadSingleJaggedArray(this BinaryReader reader)
    {
        int arrLength = reader.ReadInt32();
        if (arrLength == 0)
        {
            return null;
        }

        float[][] values = new float[arrLength][];
        for (int i = 0; i < values.Length; ++i)
        {
            int length = reader.ReadInt32();
            values[i] = new float[length];
            for (int j = 0; j < length; ++j)
            {
                values[i][j] = reader.ReadSingle();
            }
        }
        return values;
    }

    public static double[][] ReadDoubleJaggedArray(this BinaryReader reader)
    {
        int arrLength = reader.ReadInt32();
        if (arrLength == 0)
        {
            return null;
        }

        double[][] values = new double[arrLength][];
        for (int i = 0; i < values.Length; ++i)
        {
            int length = reader.ReadInt32();
            values[i] = new double[length];
            for (int j = 0; j < length; ++j)
            {
                values[i][j] = reader.ReadDouble();
            }
        }
        return values;

    }

    public static string[][] ReadStringJaggedArray(this BinaryReader reader)
    {
        int arrLength = reader.ReadInt32();
        if (arrLength == 0)
        {
            return null;
        }

        string[][] values = new string[arrLength][];
        for (int i = 0; i < values.Length; ++i)
        {
            int length = reader.ReadInt32();
            values[i] = new string[length];
            for (int j = 0; j < length; ++j)
            {
                values[i][j] = reader.ReadString();
            }
        }
        return values;
    }

    #endregion

    #region BinaryWriter
    public static void Write(this BinaryWriter writer, int[] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i]);
            }
        }
    }

    public static void Write(this BinaryWriter writer, uint[] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i]);
            }
        }
    }

    public static void Write(this BinaryWriter writer, long[] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i]);
            }
        }
    }

    public static void Write(this BinaryWriter writer, float[] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i]);
            }
        }
    }

    public static void Write(this BinaryWriter writer, double[] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i]);
            }
        }
    }

    public static void Write(this BinaryWriter writer, string[] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i]);
            }
        }
    }

    public static void Write(this BinaryWriter writer, int[][] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i].Length);
                for (int j = 0; j < values[i].Length; ++j)
                {
                    writer.Write(values[i][j]);
                }
            }
        }
    }

    public static void Write(this BinaryWriter writer, float[][] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i].Length);
                for (int j = 0; j < values[i].Length; ++j)
                {
                    writer.Write(values[i][j]);
                }
            }
        }
    }

    public static void Write(this BinaryWriter writer, double[][] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i].Length);
                for (int j = 0; j < values[i].Length; ++j)
                {
                    writer.Write(values[i][j]);
                }
            }
        }
    }

    public static void Write(this BinaryWriter writer, string[][] values)
    {
        if (values == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                writer.Write(values[i].Length);
                for (int j = 0; j < values[i].Length; ++j)
                {
                    writer.Write(values[i][j]);
                }
            }
        }
    }
    #endregion
}
