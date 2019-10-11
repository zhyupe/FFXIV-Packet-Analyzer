using NReco.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    class DB
    {
        static Dictionary<string, DB> db = new Dictionary<string, DB>();
        static Dictionary<string, string> emptyRow = new Dictionary<string, string>();
        public static string Root = "";

        public static DB Get(string name)
        {
            if (db.ContainsKey(name))
            {
                return db[name];
            }

            var newDB = new DB(name);
            db.Add(name, newDB);
            return newDB;
        }

        bool isEmpty = false;
        Dictionary<uint, Dictionary<string, string>> rows = new Dictionary<uint, Dictionary<string, string>>();

        public DB(string name, string language = "Chinese")
        {
            if (Root == "")
            {
                string libPath = Assembly.GetExecutingAssembly().Location;
                if (libPath == "")
                {
                    throw new Exception("Failed to locate the library.");
                }

                Root = Path.GetDirectoryName(libPath);
            }
            
            string filePath = Path.Combine(Root, "DB", language, name + ".csv");
            try
            {
                using (var fs = File.OpenRead(filePath))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        int lineNum = -1;
                        var csvReader = new CsvReader(sr, ",");
                        string[] columnNames = null;
                        while (csvReader.Read())
                        {
                            ++lineNum;
                            switch (lineNum)
                            {
                                case 0: // index
                                    continue;
                                case 1: // column names
                                    columnNames = new string[csvReader.FieldsCount];
                                    for (int i = 0; i < csvReader.FieldsCount; i++)
                                    {
                                        columnNames[i] = csvReader[i];
                                    }
                                    continue;
                                case 2: // types
                                    continue;
                            }

                            var row = new Dictionary<string, string>();
                            for (int i = 1; i < csvReader.FieldsCount; i++)
                            {
                                if (string.IsNullOrEmpty(columnNames[i])) continue;

                                row.Add(columnNames[i], csvReader[i]);
                            }

                            rows.Add(uint.Parse(csvReader[0]), row);
                        }
                    }
                }
            }
            catch
            {
                isEmpty = true;
            }
        }

        public Dictionary<string, string> FindById(uint id)
        {
            if (!isEmpty && rows.ContainsKey(id))
            {
                return rows[id];
            }

            return emptyRow;
        }

        public string FindById(uint id, string column)
        {
            var row = FindById(id);
            if (row.TryGetValue(column, out var ret))
            {
                return ret;
            }
            else
            {
                return "(unknown)";
            }
        }
    }
}
