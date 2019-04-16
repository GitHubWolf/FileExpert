using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public class OIDParser
    {
        private static StringDictionary oidDictionary = null;

        public static String OID2String(Stream bt)
        {
            String retval = "";
            byte b;
            ulong v = 0;
            b = (byte)bt.ReadByte();
            retval += Convert.ToString(b / 40);
            retval += "." + Convert.ToString(b % 40);
            while (bt.Position < bt.Length)
            {
                try
                {
                    DecodeValue(bt, ref v);
                    retval += "." + v.ToString();
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to decode OID value: " + e.Message);
                }
            }
            return retval;
        }

        protected static int DecodeValue(Stream bt, ref ulong v)
        {
            byte b;
            int i = 0;
            v = 0;
            while (true)
            {
                b = (byte)bt.ReadByte();
                i++;
                v <<= 7;
                v += (ulong)(b & 0x7f);
                if ((b & 0x80) == 0)
                    return i;
            }
        }

        public static String GetOIDName(String oidString)
        {
            if (oidDictionary == null) //Initialize oidDictionary:
            {
                oidDictionary = new StringDictionary();
                string path = Application.ExecutablePath;
                string oidFile = System.IO.Path.GetDirectoryName(path) + "\\OID.txt";
                string oidBackupFile = System.IO.Path.GetDirectoryName(path) + "\\OID.Backup.txt";
                string oidStr = "";
                string oidDesc = "";
                bool loadOidError = false;
                int dbCounter = 0;
                try
                {
                    using (StreamReader sr = new StreamReader(oidFile))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] strs = line.Split(',');
                            if (strs.Length < 2) continue;
                            oidStr = strs[0].Trim();
                            oidDesc = strs[1].Trim();
                            try
                            {
                                oidDictionary.Add(oidStr, oidDesc);
                            }
                            catch (Exception ex)
                            {
                                loadOidError = true;
                                string msg = ex.Message;
                                dbCounter++;
                            }
                        }
                    }
                    if (loadOidError)
                    {
                        using (StreamWriter sw = new StreamWriter(oidBackupFile))
                        {

                            using (StreamReader sr = new StreamReader(oidFile))
                            {
                                string line;
                                while ((line = sr.ReadLine()) != null)
                                {
                                    sw.Write(line + "\r\n");
                                }
                            }
                        }

                        System.Collections.SortedList sList = new System.Collections.SortedList();
                        using (StreamWriter sw = new StreamWriter(oidFile))
                        {
                            string val = "";
                            foreach (System.Collections.DictionaryEntry de in oidDictionary)
                            {
                                if (!sList.ContainsKey(de.Key))
                                    sList.Add(de.Key, de.Value);
                            }
                            for (int i = 0; i < sList.Count; i++)
                            {
                                val = String.Format("{0}, {1}\r\n", sList.GetKey(i), sList.GetByIndex(i));
                                sw.Write(val);
                            }
                        }
                        MessageBox.Show(String.Format("Duplicated OIDs were found in the OID table: {0}.\r\n" +
                            "The duplicate has been removed; the table is sorted.\r\n" +
                            "The original OID file is copied as: {1}\r\n", oidFile, oidBackupFile));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to read OID values from file." + ex.Message);
                }
            }
            return oidDictionary[oidString];
        }
    }
}
