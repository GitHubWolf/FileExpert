﻿using FileExpert.ASN;
using Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FileExpert.ASN
{
    public partial class FormASN : FileExpert.FormParser
    {
        //Parse to show the data.
        ParserASN parserASN = new ParserASN();

        public FormASN()
        {
            InitializeComponent();
        }

        protected override void DoWork()
        {
            PushMessageIntoQueue(MessageId.MESSAGE_PARSING_START, null);
            try
            {
                ParseASNFile();
            }
            catch (Exception e)
            {
                WriteLog("Unknown error happened!");
                WriteLog(e.ToString());
            }
            PushMessageIntoQueue(MessageId.MESSAGE_PARSING_DONE, null);
        }
        protected void ParseASNFile()
        {
            Result result = new Result();

            int fileSize = (int)fileStream.Length;

            //ASN file is small. We will read in all the data in once.

            Int64 currentProgress = 0;
            Int64 latestProgress = 0;
            while (result.Fine && canRun)
            {
                byte[] ASNData = new byte[fileSize];

                latestProgress = (100 * fileStream.Position / fileStream.Length);
                if (currentProgress  != latestProgress)
                {
                    PushMessageIntoQueue(MessageId.MESSAGE_PROGRESS, latestProgress);
                    currentProgress = latestProgress;
                }

                //All in;
                if (fileSize != fileStream.Read(ASNData, 0, fileSize))
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }


                if(result.Fine)
                {
                    //Save the info related to data source and file offset.
                    List<DataSourceMetadata> dataSourceMetadataList = new List<DataSourceMetadata>();
                    DataSourceMetadata dataSourceMetadata = new DataSourceMetadata();
                    dataSourceMetadata.FileOffset = 0;
                    dataSourceMetadata.PacketNumber = 1;
                    dataSourceMetadata.PacketSource = DataSource.SOURCE_FILE;

                    dataSourceMetadataList.Add(dataSourceMetadata);

                    if(result.Fine)
                    {
                        DataStore dataStore = new DataStore(ASNData);
                        dataStore.SetMetadataList(dataSourceMetadataList);


                        //Post  a message into the queue.
                        PushMessageIntoQueue(MessageId.MESSAGE_DATA, dataStore);
                    }
                }
            }
        }


        protected override void ProcessMessage(MessageId messageType, object message)
        {
            switch (messageType)
            {
                case MessageId.MESSAGE_PARSING_START:
                    OnStreamStart();
                    break;
                case MessageId.MESSAGE_DATA:
                    ShowData((DataStore)message);
                    break;
                case MessageId.MESSAGE_PROGRESS:
                    DisplayProgress((Int64)message);
                    break;
                case MessageId.MESSAGE_PARSING_DONE:
                    OnStreamStop();
                    break;
                default:
                    break;

            }
        }

        private void FormASN_Load(object sender, EventArgs e)
        {
            //Create the root node.
            CreateDefaultNodes();
        }

        protected void ShowData(DataStore dataStore)
        {
            parserASN.ParseASN(this.treeViewParser, this.rootNode, dataStore);
        }
    }
}
