using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using System.Text;

public class SqliteMgr : MonoSingleton<SqliteMgr>
{
    //Table in : AresStatus, Contents
    string dbPath;
    //string dbFileName = "/NodeData.db"; //@"/NodeData.db";


    public string Init(string dbFIleName)
    {
        dbPath = Application.persistentDataPath + '/' + dbFIleName; //after path
        Common.Instance.PrintLog('w', "SqliteMgr", dbPath, "");

        //check DB file exist
        if (CONST_VALUE.ISDEV && File.Exists(dbPath)) //remove db file on dev mode
            File.Delete(dbPath);
        
        if (!File.Exists(dbPath))
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                byte[] readByte = File.ReadAllBytes(Application.streamingAssetsPath + '/' + dbFIleName);
                File.WriteAllBytes(dbPath, readByte);
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                byte[] readByte = File.ReadAllBytes(Application.streamingAssetsPath + '/' + dbFIleName);
                File.WriteAllBytes(dbPath, readByte);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                BetterStreamingAssets.Initialize();
                byte[] readByte = BetterStreamingAssets.ReadAllBytes(dbFIleName);
                File.WriteAllBytes(dbPath, readByte);
            }
        }
        return dbPath;
    }

    /// <summary>
    /// clear DB table
    /// </summary>
    public void ClearTable(string tableName)
    {
        using (SqliteConnection conn = new SqliteConnection("URI=file:" + dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "delete from " + tableName;
                Common.Instance.PrintLog('w', "ClearTable", cmd.CommandText, null);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();
        }
    }

    /// <summary>
    /// insert data
    /// </summary>
    public void WriteDicData(Dictionary<string, string> data, string tableName)
    {
        if (data == null) return;

        using (SqliteConnection conn = new SqliteConnection("URI=file:" + dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                foreach(KeyValuePair<string, string> items in data)
                {
                    cmd.CommandText = "insert into " + tableName + " values('" + items.Key + "', '" + items.Value + "');";
                    Common.Instance.PrintLog('w', "WriteDicData", cmd.CommandText, null);
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();
        }
    }

    /// <summary>
    /// access query direct
    /// </summary>
    public void SQLDirect(string query)
    {
        if (string.IsNullOrEmpty(query)) return;

        using (SqliteConnection conn = new SqliteConnection("URI=file:" + dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();
        }
    }

    public void WriteStrData(string data, string tableName)
    {
        using (SqliteConnection conn = new SqliteConnection("URI=file:" + dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                //foreach (KeyValuePair<string, string> items in data)
                {
                    cmd.CommandText = "insert into " + tableName + " values('" + data + "');";
                    cmd.ExecuteNonQuery();
                }
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();
        }
    }

    /// <summary>
    /// get DB data single line
    /// </summary>
    /// <returns></returns>
    public string ReadData(string query)
    {
        if (string.IsNullOrEmpty(query)) return null;
        Common.Instance.PrintLog('d', "ReadData", query, "");

        string rtnData = "";

        //idb connection
        using (SqliteConnection conn = new SqliteConnection("URI=file:" + dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;

                SqliteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    rtnData = reader.GetString(0);

                reader.Close();
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();
        }
        return rtnData;
    }

    /// <summary>
    /// get DB data multi line
    /// </summary>
    /// <returns></returns>
    public List<string> ReadData(string query, int cntRtnRow)
    {
        if (string.IsNullOrEmpty(query)) return null;
        Common.Instance.PrintLog('d', "ReadData", query, "");

        List<string> rtnData = new List<string>();

        //idb connection
        using (SqliteConnection conn = new SqliteConnection("URI=file:" + dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;

                SqliteDataReader reader = cmd.ExecuteReader();
                int cnt = 0;

                while (reader.Read())
                {
                    cnt++;
                    StringBuilder tmpStr = new StringBuilder();

                    for (int i = 0; i < cntRtnRow; i++)
                        if (reader.IsDBNull(i))
                            tmpStr.Append("" + '\\');
                        else
                        {
                            if (i == cntRtnRow - 1)
                                tmpStr.Append(reader.GetString(i));
                            else
                                tmpStr.Append(reader.GetString(i) + '\\');
                        }
                    //Debug.Log("dbData : " + tmpStr.ToString());
                    rtnData.Add(tmpStr.ToString());
                }

                if (cnt == 0)
                    rtnData.Clear();
                reader.Close();
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();
        }
        return rtnData;
    }

    public Dictionary<string, string> ReadDicData(string query)
    {
        if (string.IsNullOrEmpty(query)) return null;
        Common.Instance.PrintLog('d', "ReadDicData", query, "");
        
        Dictionary<string, string> rtnDic = new Dictionary<string, string>();

        //idb connection
        using (SqliteConnection conn = new SqliteConnection("URI=file:" + dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;

                SqliteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    rtnDic.Add(reader.GetString(0), reader.GetString(1));

                reader.Close();
                cmd.Dispose();
            }
            conn.Close();
            conn.Dispose();
        }
        return rtnDic;
    }
}