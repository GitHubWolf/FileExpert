using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Library
{
    //To manage most recently used files.
    public class ManagerMRU
    {
        public static List<String> GetMruList()
        {
            List<String> mruList = new List<string>();

            string rootDir = System.AppDomain.CurrentDomain.BaseDirectory;

            //Check whether MRU config file exists.
            string mruConfigFile = rootDir + "mru.xml";

            XmlDocument mruXmlFile = new XmlDocument();
            try
            {
                mruXmlFile.Load(mruConfigFile);
                XmlNode rootNode = mruXmlFile.SelectSingleNode("MostRecentUsed");

                if (null != rootNode)
                {
                    XmlNodeList fileNodes = rootNode.SelectNodes("File");
                    foreach (XmlNode fileNode in fileNodes)
                    {
                        mruList.Add(fileNode.InnerText);
                    }
                }
            }
            catch (Exception)
            {
                //Config file doesn't exist or invalid XML file.
            }


            return mruList;
        }

        public static void AddMruItem(String fileName)
        {
            List<string> mruFileList = GetMruList();
            int count = 0;

            try
            {
                mruFileList.Insert(0, fileName);

                string rootDir = System.AppDomain.CurrentDomain.BaseDirectory;

                //Check whether MRU config file exists.
                string mruConfigFile = rootDir + "mru.xml";

                XmlDocument mruXmlFile = new XmlDocument();

                //Add declaration.
                XmlDeclaration xmldecl = mruXmlFile.CreateXmlDeclaration("1.0", null, null);
                mruXmlFile.AppendChild(xmldecl);

                //Add root item.
                XmlElement rootElement = mruXmlFile.CreateElement("MostRecentUsed");
                mruXmlFile.AppendChild(rootElement);

                //Add the first one as the new one.
                XmlElement fileElement = mruXmlFile.CreateElement("File");
                fileElement.InnerText = fileName;
                rootElement.AppendChild(fileElement);
                count++;

                foreach (string file in mruFileList)
                {
                    //Write all existing items, but skip the ones that are same with the new one.
                    if (!file.Equals(fileName))
                    {
                        fileElement = mruXmlFile.CreateElement("File");
                        fileElement.InnerText = file;
                        rootElement.AppendChild(fileElement);

                        count++;
                    }

                    //Maximum 16 items.
                    if (16 == count)
                    {
                        break;
                    }
                }

                mruXmlFile.Save(mruConfigFile);
            }
            catch (Exception )
            {
                MessageBox.Show(null, "Error", "Failed to acess MRU config file.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
