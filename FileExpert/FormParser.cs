using Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert
{
    public partial class FormParser : Form
    {
        //Delegate function prototype.
        protected delegate void AddLogDelegate(string log);

        private TimeSpan timeStart;
        private TimeSpan timeStop;

        protected String streamFile = null;
        protected bool isClosing = false;

        //For the parser worker thread.
        protected Thread parserThread = null;
        protected volatile bool canRun = true;

        protected FileStream fileStream = null;

        protected TreeNode rootNode = null;

        //Save all the message notification from stream parser.
        protected Queue<MessageNotification> messageQueue = new Queue<MessageNotification>();

        //To save current DataStore of selected FieldItem. In case it is changed, we will need to display the hex string.
        private DataStore selectedDataStore = null;

        public FormParser()
        {
            InitializeComponent();
        }

        public void SetStreamFile(String streamFileName)
        {
            streamFile = streamFileName;
        }


        private void FormParser_Load(object sender, EventArgs e)
        {
            this.Text = streamFile;

            //Create a thread to parse the file.
            StartParse(streamFile);
        }

        public void WriteLog(string log)
        {
            //In case the form is being closed, we will do nothing.
            if (!isClosing)
            {
                //if (textBoxLogger.InvokeRequired)
                richTextBoxLogger.BeginInvoke(new AddLogDelegate(AddLog), log);
            }
        }

        private void AddLog(string log)
        {
            richTextBoxLogger.AppendText(log);
        }

        public Result StartParse(String streamFile)
        {
            Result result = new Result();

            canRun = true;

            try
            {
                fileStream = new FileStream(streamFile, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                result.SetResult(ResultCode.FAILED_TO_OPEN_FILE);
            }

            if (result.Fine)
            {
                //Create a thread to parse the stream.
                parserThread = new Thread(new ParameterizedThreadStart(OnStart));

                //parserThread.Priority = ThreadPriority.Lowest;
                parserThread.Start(this);
            }

            return result;
        }

        protected void OnStart(object data)
        {
            //Call the virtual method to really do the work.
            DoWork();

            //Close the stream immediately.
            if (null != fileStream)
            {
                fileStream.Close();
                fileStream = null;
            }
            parserThread = null;
        }

        protected virtual void DoWork()
        {
            //Leave it to the child form.
        }

        protected virtual void ProcessMessage(MessageId messageType, object message)
        {
            //Leave it to the child form.
        }

        protected void PushMessageIntoQueue(MessageId messageId, object message)
        {
            lock (this)
            {
                //Put the message into the queue.
                messageQueue.Enqueue(new MessageNotification(messageId, message));
            }
        }

        private void FormParser_Resize(object sender, EventArgs e)
        {
            //Note that the autosize property of progressbar should be disabled!!!!!!!!!!!!!!
            toolStripProgressBar1.Width = (int)(this.Width * 0.9);
        }

        protected void DisplayProgress(Int64 progress)
        {
            toolStripStatusLabel1.Text = String.Format("{0}%", (int)progress);
            toolStripProgressBar1.Value = (int)progress;
        }

        protected void CreateDefaultNodes()
        {
            rootNode = Utility.AddChildNode(treeViewParser, null, "Root", streamFile, ItemType.ROOT);

            ExpandToTop(rootNode);
        }

        protected void ExpandToTop(TreeNode newNode)
        {
            while (null != newNode)
            {
                newNode.Expand();

                newNode = newNode.Parent;
            }
        }

        private void timerToUpdateData_Tick(object sender, EventArgs e)
        {
            MessageNotification notification = null;
            int messageCount = messageQueue.Count;


            if (0 != messageCount)
            {
                treeViewParser.BeginUpdate();

                while (0 != messageCount)
                {
                    lock (this)
                    {
                        notification = messageQueue.Dequeue();
                    }

                    if (null != notification)
                    {
                        ProcessMessage(notification.ID, notification.Payload);
                    }

                    messageCount--;
                }

                treeViewParser.EndUpdate();
            }
        }

        protected void OnStreamStart()
        {
            toolStripStatusLabel1.Text = "Ongoing";
            toolStripProgressBar1.Value = 0;

            timeStart = new TimeSpan(DateTime.Now.Ticks);//Record start time.

        }

        protected  void OnStreamStop()
        {
            toolStripStatusLabel1.Text = "Done";
            toolStripProgressBar1.Value = 100;

            timeStop = new TimeSpan(DateTime.Now.Ticks);//Record stop time.

            PrintDuration();

        }

        protected void PrintDuration()
        {
            TimeSpan duration = timeStop.Subtract(timeStart).Duration();
            WriteLog("Time used for parsing: " + duration.ToString() + Environment.NewLine);
        }

        private void treeViewParser_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            FieldItem fieldItem = null;

            if (null != selectedNode)
            {
                if (selectedNode.Tag is FieldItem)
                {
                    fieldItem = (FieldItem)selectedNode.Tag;
                }
            }

            int binarySelectStart = 0;
            int binarySelectLength = 0;

            if (null != fieldItem)
            {
                byte[] selectedDataBytes = null;
                if (null != selectedDataStore)
                {
                    selectedDataBytes = selectedDataStore.GetData();
                }

                byte[] fieldItemDataBytes = null;
                if (null != fieldItem.GetDataStore())
                {
                    fieldItemDataBytes = fieldItem.GetDataStore().GetData();
                }
                if(fieldItemDataBytes != selectedDataBytes)
                {
                    richTextBoxHexString.Clear();

                    //Display the hex string.
                    richTextBoxHexString.Text = fieldItem.GetDataStoreHexString();

                    //Set it as current selected.
                    selectedDataStore = fieldItem.GetDataStore();
                }



                //Select the bytes for this field.
                Int64 offsetStart = fieldItem.Offset / 8;
                Int64 offsetEnd = (fieldItem.Offset + fieldItem.Length) / 8;

                //Block start and block end.
                Int64 blockStart = fieldItem.Offset / 64; //To calculate the count of Environment.NewLine.
                Int64 blockEnd = (fieldItem.Offset + fieldItem.Length) / 64;//To calculate the count of Environment.NewLine.

                if (0 != (fieldItem.Offset + fieldItem.Length) % 8)
                {
                    offsetEnd++;
                }

                if (0 == ((fieldItem.Offset + fieldItem.Length) % 64))
                {
                    blockEnd--;
                }

                richTextBoxHexString.Select((int)(offsetStart * 3 + blockStart), (int)((offsetEnd - offsetStart) * 3 + (blockEnd - blockStart)));
                richTextBoxHexString.SelectionColor = Color.Red;

                //To avoid out of memory operation.
                /*
                try
                {
                    richTextBoxHexString.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                }
                 */
                

                //To show the detail field property as below.
                /*
                Binary  :10100010 11000011 00101001 01000001 
                Heximal :   A2C32941
                Decimal : 2730699073
                Length  :          4 byte
                Length  :         32 bit
                Position:         12 byte
                Position:         96 bit
                 */
                String binaryStr = null;
                String hexStr = null;
                String decimalStr = null;

                //We'll show the fieldValue only when the bit length is less then 64.
                if ((64 >= fieldItem.Length) && (fieldItem.Type == ItemType.FIELD))
                {
                    Int64 bytesValue = 0;
                    byte[] data = selectedDataStore.GetData();

                    for (Int64 i = offsetStart; i < offsetEnd; i++)
                    {
                        bytesValue = bytesValue << 8;
                        bytesValue = bytesValue | data[i];
                    }
                    String binary = "Binary  :";
                    binaryStr = binary + Utility.GetValueBinaryString(bytesValue, (offsetEnd - offsetStart) * 8);
                    hexStr = "Heximal :" + Utility.GetValueHexString(fieldItem.Value, fieldItem.Length);
                    decimalStr = "Decimal :" + Utility.GetValueDecimalString(fieldItem.Value, fieldItem.Length);

                    //Select those bits belonged to this field.
                    Int64 spaceCount = (fieldItem.Offset + fieldItem.Length) / 8 - (fieldItem.Offset) / 8;

                    String lengthPositionStr = String.Format("Length  :{0}-byte" + Environment.NewLine
                                                            + "Length  :{1}-bit" + Environment.NewLine
                                                            + "Offset  :{2}-byte" + Environment.NewLine
                                                            + "Offset  :{3}-bit",
                                                            offsetEnd - offsetStart,
                                                            fieldItem.Length,
                                                            fieldItem.Offset / 8,
                                                            fieldItem.Offset);

                    richTextBoxProperty.Text = binaryStr + Environment.NewLine
                                                + hexStr + Environment.NewLine
                                                + decimalStr + Environment.NewLine
                                                + lengthPositionStr + Environment.NewLine;

                    binarySelectStart = (int)(binary.Length + fieldItem.Offset % 8);
                    binarySelectLength = (int)(fieldItem.Length + spaceCount);
                }
                else
                {
                    String lengthPositionStr = String.Format("Length  :{0}-byte" + Environment.NewLine
                                                           + "Length  :{1}-bit" + Environment.NewLine
                                                           + "Offset  :{2}-byte" + Environment.NewLine
                                                           + "Offset  :{3}-bit",
                                                           offsetEnd - offsetStart,
                                                           fieldItem.Length,
                                                           fieldItem.Offset / 8,
                                                           fieldItem.Offset);

                    richTextBoxProperty.Text = binaryStr + Environment.NewLine
                                                + hexStr + Environment.NewLine
                                                + decimalStr + Environment.NewLine
                                                + lengthPositionStr + Environment.NewLine;
                }


                //Show the repeat number.
                String repeatStr = "Repeat  :" + selectedDataStore.GetRepeatTimes();
                richTextBoxProperty.AppendText(repeatStr + Environment.NewLine);

                //Show the packet metadata.
                List<DataSourceMetadata> dataSourceMetadataList = selectedDataStore.GetDataSourceMetadataList();
                richTextBoxProperty.AppendText("Packet Count: " + dataSourceMetadataList.Count + Environment.NewLine);
                String packetMetadataStr = "(Packet Number,File Offset): ";
                foreach (DataSourceMetadata packetMetadata in dataSourceMetadataList)
                {
                    packetMetadataStr += " (" + packetMetadata.PacketNumber + "," + packetMetadata.FileOffset + ")";
                }
                richTextBoxProperty.AppendText(packetMetadataStr + Environment.NewLine);

                if (binarySelectLength != 0)
                {
                    //Select the binary bits finally. We can't make the selection and then insert new sectionDetail. It will not work in such case.

                    richTextBoxProperty.Select(binarySelectStart, binarySelectLength);
                    richTextBoxProperty.SelectionColor = Color.Red;
                    /*richTextBoxProperty.ScrollToCaret();*/
                }
            }//null != fieldItem
        }

        private void treeViewParser_MouseDown(object sender, MouseEventArgs e)
        {
            bool showMenu = false;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point currentPoint = new Point(e.X, e.Y);
                TreeNode currentNode = treeViewParser.GetNodeAt(currentPoint);

                if (null != currentNode)
                {
                    treeViewParser.SelectedNode = currentNode;
                    if (currentNode.Tag is FieldItem)
                    {
                        FieldItem fieldItem = null;
                        fieldItem = (FieldItem)currentNode.Tag;
                        if (fieldItem.GetDataStore() != null)
                        {
                            showMenu = true;
                        }
                    }
                }
            }

            if (showMenu)
            {
                treeViewParser.ContextMenuStrip = contextMenuStripTreeView;
            }
            else
            {
                treeViewParser.ContextMenuStrip = null ;
            }
        }

        private void dumpBinaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = treeViewParser.SelectedNode;

            FieldItem fieldItem = null;

            if (null != selectedNode)
            {
                if (selectedNode.Tag is FieldItem)
                {
                    fieldItem = (FieldItem)selectedNode.Tag;
                    if (fieldItem.GetDataStore() != null)
                    {
                        //Select the bytes for this field.
                        Int64 offsetStart = fieldItem.Offset / 8;
                        Int64 offsetEnd = (fieldItem.Offset + fieldItem.Length) / 8;

                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Title = "Save Binary";
                        saveFileDialog.RestoreDirectory = false;

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                BinaryWriter binaryWriter = new BinaryWriter(new FileStream(saveFileDialog.FileName, FileMode.Create));
                                binaryWriter.Write(fieldItem.GetDataStore().GetData(), (int)offsetStart, (int)(offsetEnd - offsetStart));
                                binaryWriter.Close();
                            }
                            catch (Exception err)
                            {
                                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

    }
}
