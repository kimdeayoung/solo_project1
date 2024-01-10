using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class Tables : Singleton<Tables>
{
    private Table[] tables;
    private readonly string tableRoot;

    public Tables()
    {
        tables = new Table[(int)TableType.Length];
        tableRoot = Application.dataPath + "/Resources/Tables/";
    }

#if UNITY_EDITOR
    public void ConvertLocalTableToBinary()
    {
        foreach (TableType type in Enum.GetValues(typeof(TableType)))
        {
            if (type == TableType.Length)
            {
                continue;
            }

            Table table = new Table(type);
            table.LoadTable(TableLoadType.Local, Application.dataPath + "/Tables/" + type + "Table.xml");

            table.WriteDataToBinaryFile(tableRoot + type + ".bytes");
        }
    }
#endif

    public void LoadTables_Binary()
    {
        StringBuilder builder = new StringBuilder();
        foreach (TableType type in Enum.GetValues(typeof(TableType)))
        {
            if (type == TableType.Length)
            {
                continue;
            }

            Table table = new Table(type);

            builder.Append(tableRoot);
            builder.Append(type);
            builder.Append(".bytes");

            table.LoadTable(TableLoadType.Binary, builder.ToString());
            tables[(int)type] = table;
            builder.Clear();
        }
    }

    public Table GetTable(TableType type)
    {
        Assert.IsNotNull(tables);
        Assert.IsNotNull(tables[(int)type]);
        return tables[(int)type];
    }

    public T GetRecordOrNull<T>(TableType type, uint index) where T : TRFoundation
    {
        Table table = GetTable(type);
        Assert.IsNotNull(table);

        return table.GetRecordOrNull<T>(index);
    }

    public void Clear()
    {
        foreach (TableType type in Enum.GetValues(typeof(TableType)))
        {
            Table table = tables[(int)type];
            table.Clear();
            tables[(int)type] = null;
        }

        tables = null;
    }
}
