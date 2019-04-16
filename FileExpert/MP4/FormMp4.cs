using FileExpert.MP4;
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

namespace FileExpert.MP4
{
    public partial class FormMp4 : FileExpert.FormParser
    {
        //Parse to show the data.
        ParseMp4 parserMp4 = new ParseMp4();

        Int64 mediaDataBoxCounter = 0;
        public FormMp4()
        {
            InitializeComponent();
        }

        protected override void DoWork()
        {
            PushMessageIntoQueue(MessageId.MESSAGE_PARSING_START, null);
            try
            {
                ParseMp4File();
            }
            catch (Exception e)
            {
                WriteLog("Unknown error happened!");
                WriteLog(e.ToString());
            }
            PushMessageIntoQueue(MessageId.MESSAGE_PARSING_DONE, null);
        }
        protected void ParseMp4File()
        {
            Result result = new Result();

            Int64 fileSize = fileStream.Length;

            byte[] boxSizeBytes = new byte[4];
            byte[] boxTypeBytes = new byte[4];

            string boxTypeName = null;
            Int64 boxSize = 0;
            byte[] boxLargeSizeBytes = new byte[8];
            Int64 boxLargeSize = 0;
            byte[] userTypeBytes = new byte[16];
            Int64 realBoxSize = 0;
            Int64 readInSize = 0;

            Int64 currentProgress = 0;
            Int64 latestProgress = 0;
            while (result.Fine && canRun)
            {
                //Read the box one by one.

                //Reset the size each time.
                realBoxSize = 0;

                latestProgress = (100 * fileStream.Position / fileStream.Length);
                if (currentProgress  != latestProgress)
                {
                    PushMessageIntoQueue(MessageId.MESSAGE_PROGRESS, latestProgress);
                    currentProgress = latestProgress;
                }

                //unsigned int(32) size;
                if (4 != fileStream.Read(boxSizeBytes, 0, 4))
                {
                    result.SetResult(ResultCode.INSUFFICIENT_DATA);
                }
                else
                {
                    boxSize = ((boxSizeBytes[0] << 24) | (boxSizeBytes[1] << 16) | (boxSizeBytes[2] << 8) | boxSizeBytes[3]);

                    realBoxSize = boxSize;
                }

                if (result.Fine)
                {
                    //unsigned int(32) type = boxtype; 
                    if (4 != fileStream.Read(boxTypeBytes, 0, 4))
                    {
                        result.SetResult(ResultCode.INSUFFICIENT_DATA);
                    }
                    else
                    {
                        boxTypeName = System.Text.Encoding.Default.GetString(boxTypeBytes, 0, 4);
                    }
                }

                if (result.Fine)
                {
                    //unsigned int(64) largesize;
                    if (1 == boxSize)
                    {
                        if (8 != fileStream.Read(boxLargeSizeBytes, 0, 8))
                        {
                            result.SetResult(ResultCode.INSUFFICIENT_DATA);
                        }
                        else
                        {
                            Int64 boxSize1 = ((boxLargeSizeBytes[0] << 24) | (boxLargeSizeBytes[1] << 16) | (boxLargeSizeBytes[2] << 8) | boxLargeSizeBytes[3]);
                            Int64 boxSize2 = ((boxLargeSizeBytes[4] << 24) | (boxLargeSizeBytes[5] << 16) | (boxLargeSizeBytes[6] << 8) | boxLargeSizeBytes[7]);
                            boxLargeSize = (boxSize1 << 32) | (boxSize2);

                            realBoxSize = boxLargeSize;

                            DumpMediaData(boxTypeName, fileStream, realBoxSize - 16);

                            //Seek back to the beginning of this box so that we can read in the whole box in one call.
                            fileStream.Seek(-16, SeekOrigin.Current);
                        }
                    }
                    else if (0 == boxSize)
                    {
                        // box extends to end of file
                        realBoxSize = fileStream.Length - fileStream.Position + 8;

                        DumpMediaData(boxTypeName, fileStream, realBoxSize - 8);

                        //Seek back to the beginning of this box so that we can read in the whole box in one call.
                        fileStream.Seek(-8, SeekOrigin.Current);
                    }
                    else
                    {
                        DumpMediaData(boxTypeName, fileStream, realBoxSize - 8);

                        //Seek back to the beginning of this box so that we can read in the whole box in one call.
                        fileStream.Seek(-8, SeekOrigin.Current);
                    }
                }

                if(result.Fine)
                {
                    //Save the info related to data source and file offset.
                    List<DataSourceMetadata> dataSourceMetadataList = new List<DataSourceMetadata>();
                    DataSourceMetadata dataSourceMetadata = new DataSourceMetadata();
                    dataSourceMetadata.FileOffset = fileStream.Position;
                    dataSourceMetadata.PacketNumber = 1;
                    dataSourceMetadata.PacketSource = DataSource.SOURCE_FILE;

                    dataSourceMetadataList.Add(dataSourceMetadata);

                    //Some boxes are very large, we will only read in part on the box in such case.
                    byte[] aBox = null;

                    if (realBoxSize >= Library.HexEdit.MAX_TEXT_SIZE)
                    {
                        //Read in part of the box.
                        aBox = new byte[5000000];

                        readInSize = fileStream.Read(aBox, 0, aBox.Length);

                        if (readInSize != aBox.Length)
                        {
                            result.SetResult(ResultCode.INSUFFICIENT_DATA);
                        }

                        if (result.Fine)
                        {
                            try
                            {
                                fileStream.Seek(realBoxSize - 5000000, SeekOrigin.Current);
                            }
                            catch (Exception)
                            {
                                result.SetResult(ResultCode.INSUFFICIENT_DATA);
                            }
                            
                        }
                    }
                    else
                    {
                        //Read in the whole box now.
                        aBox = new byte[realBoxSize];

                        readInSize = fileStream.Read(aBox, 0, aBox.Length);

                        if (readInSize != aBox.Length)
                        {
                            result.SetResult(ResultCode.INSUFFICIENT_DATA);
                        }
                    }                    

                    if(result.Fine)
                    {
                        DataStore dataStore = new DataStore(aBox);
                        dataStore.SetMetadataList(dataSourceMetadataList);


                        //Post  a message into the queue.
                        PushMessageIntoQueue(MessageId.MESSAGE_DATA, dataStore);
                    }
                }
            }
        }

        private void DumpMediaData(string boxTypeName, FileStream fileStream, long length)
        {

            if (0 == boxTypeName.CompareTo("mdat"))
            {
                //Save the position.
                long currentFilePositon = fileStream.Position;
                
                //To give a new file name.
                mediaDataBoxCounter++;

                FileStream outputFileStream = new FileStream(streamFile+mediaDataBoxCounter+".mdat", FileMode.Create, FileAccess.Write);
                if (null != outputFileStream)
                {
                    int sizeForEachRead = 10 * 1024 * 1024;
                    byte[] tempBuffer = new byte[sizeForEachRead];
                    int sizeReadIn = 0;


                    while (length >= sizeForEachRead)
                    {
                        sizeReadIn = fileStream.Read(tempBuffer, 0, sizeForEachRead);

                        outputFileStream.Write(tempBuffer, 0, sizeReadIn);

                        if (sizeReadIn != sizeForEachRead)
                        {                            
                            break;
                        }

                        length -= sizeForEachRead;
                    }

                    //Write the left if any.
                    if (length != 0)
                    {
                        sizeReadIn = fileStream.Read(tempBuffer, 0, (int)length);

                        outputFileStream.Write(tempBuffer, 0, sizeReadIn);
                    }

                    outputFileStream.Close();

                }

                //Recover the position.
                fileStream.Position = currentFilePositon;
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

        private void FormMp4_Load(object sender, EventArgs e)
        {
            //Create the root node.
            CreateDefaultNodes();
        }

        protected void ShowData(DataStore dataStore)
        {
            parserMp4.ParseFirstLevelBox(this.treeViewParser, this.rootNode, dataStore);
        }
    }
}
