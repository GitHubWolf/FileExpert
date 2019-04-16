using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Library
{
    public partial class HexEdit: RichTextBox
    {
        private int EACH_CHARS_PER_LINE = 16;
        public static int MAX_TEXT_SIZE = 10 * 1024 * 1024;

        public HexEdit()
        {
            InitializeComponent();

            InitContextMenuEventHandler();
        }

        private void InitContextMenuEventHandler()
        {
            toolStripMenuItemUndo.Click += toolStripMenuItemUndo_Click;

            toolStripMenuItemCopy.Click += toolStripMenuItemCopy_Click;
            toolStripMenuItemCut.Click += toolStripMenuItemCut_Click;
            toolStripMenuItemPaste.Click += toolStripMenuItemPaste_Click;

            toolStripMenuItemSelectAll.Click += toolStripMenuItemSelectAll_Click;
            toolStripMenuItemClearAll.Click += toolStripMenuItemClearAll_Click;
            
            toolStripMenuItemLoadBinary.Click += toolStripMenuItemLoadBinary_Click;
            toolStripMenuItemLoadText.Click += toolStripMenuItemLoadText_Click;
            toolStripMenuItemSaveBinary.Click += toolStripMenuItemSaveBinary_Click;
            toolStripMenuItemSaveText.Click += toolStripMenuItemSaveText_Click;

        }

        void toolStripMenuItemUndo_Click(object sender, EventArgs e)
        {
            try
            {
                this.Undo();
            }
            catch (Exception err)
            {
                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void toolStripMenuItemSaveText_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Text";
            saveFileDialog.RestoreDirectory = false;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter streamWriter = new StreamWriter(new FileStream(saveFileDialog.FileName, FileMode.Create));
                    streamWriter.Write(this.Text);
                    streamWriter.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void toolStripMenuItemSaveBinary_Click(object sender, EventArgs e)
        {
            //Check whether all the chars are valid.
            byte[] byteArray = null;
            int validLength = 0;
            bool isValid = GetByteArray(out byteArray, ref validLength);

            if(isValid)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save Binary";
                saveFileDialog.RestoreDirectory = false;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        BinaryWriter binaryWriter = new BinaryWriter(new FileStream(saveFileDialog.FileName, FileMode.Create));
                        binaryWriter.Write(byteArray,0, validLength);//Write data according to validLength!
                        binaryWriter.Close();
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

        void toolStripMenuItemLoadText_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load Text";
            openFileDialog.RestoreDirectory = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader textStreamReader = new StreamReader(openFileDialog.FileName);

                    string readInLine = "";
                    while (null != readInLine)
                    {
                        readInLine = textStreamReader.ReadLine();
                        if (null != readInLine)
                        {
                            this.AppendText(readInLine);
                        }
                    }

                    textStreamReader.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void toolStripMenuItemLoadBinary_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load Binary";
            openFileDialog.RestoreDirectory = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BinaryReader binaryReader = new BinaryReader(new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read));

                    StringBuilder stringBuilder = new StringBuilder();
                    byte[] readInData = null;
                    do
                    {
                        readInData = binaryReader.ReadBytes(EACH_CHARS_PER_LINE);

                        if ((null == readInData) || (readInData.Length == 0))
                        {
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < readInData.Length; ++i)
                            {
                                /*Only one parameter, 2 byte align and in hex format with ZERO padding, with one SPACE after.*/
                                stringBuilder.AppendFormat("{0,2:X2} ", readInData[i]);
                            }

                            /*New line.*/
                            stringBuilder.Append(Environment.NewLine);

                            if (stringBuilder.Length > 1024 * 1024 * 2)
                            {
                                //Append it.
                                this.AppendText(stringBuilder.ToString());

                                stringBuilder.Clear();
                            }
                        }
                    } while (null != readInData);

                    //Append the left.
                    this.AppendText(stringBuilder.ToString());
                    binaryReader.Close();
                }
                catch (Exception err)
                {
                    MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        void toolStripMenuItemClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.Clear();
            }
            catch (Exception err)
            {
                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void toolStripMenuItemSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectAll();
            }
            catch (Exception err)
            {
                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void toolStripMenuItemPaste_Click(object sender, EventArgs e)
        {
            try
            {
                this.Paste();
            }
            catch (Exception err)
            {
                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void toolStripMenuItemCut_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cut();
            }
            catch (Exception err)
            {
                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            try
            {
                this.Copy();
            }
            catch (Exception err)
            {
                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /**
         *Check whether all the chars are valid heximal chars. 
         *If yes, convert the hex chars into byte array.
         *
         */
        public bool GetByteArray(out byte[] byteArray, ref int length)
        {
            bool result = true;
            byteArray = null;
            length = 0;

            try
            {
                //Allocate buffer to save the binary data.
                byteArray = new byte[this.Text.Length / 2 + 1];
                int counter = 0;
                byte currentByte = 0;
                byte fourBitValue = 0;
                bool isEven = true;

                string currentText = this.Text;
                char currentChar = '0';
                for (int i = 0; i < currentText.Length; ++i)
                {
                    currentChar = Char.ToUpper(currentText[i]);
                    if (((currentChar >= '0') && (currentChar <= '9'))
                        || ((currentChar >= 'A') && (currentChar <= 'F')))
                    {
                        fourBitValue = HexToByte(currentChar);
                        if (isEven)
                        {
                            currentByte = (byte)(fourBitValue << 4);

                            isEven = false;
                        }
                        else
                        {
                            currentByte = (byte)(currentByte | fourBitValue);

                            isEven = true;
                        }

                        if (isEven)
                        {
                            byteArray[counter] = currentByte;
                            counter++;
                        }
                    }//Valid char.
                    else if ((currentChar == '\t') || (currentChar == '\n') || (currentChar == '\r') || (currentChar == ' '))
                    {
                        //Space, tab or CRLF, ignore them.
                    }//Ingored char.
                    else
                    {
                        //Invalid char.
                        result = false;
                        this.Focus();
                        this.Select(i, 1);//Select the invalid char.

                        MessageBox.Show(null, "Invalid HEX char. All chars must be within '0'-'9' or 'A'-'F'", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        break;
                    }//Invalid char.
                }

                //If an odd char is left,like "F"
                if (!isEven)
                {
                    MessageBox.Show(null, "Incorrec HEX format. No single HEX char like 'F' is allowed. It must be '0F'", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                }
                else
                {
                    //Save the length now.
                    length = counter;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(null, err.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
                byteArray = null;
            }

            return result;
        }

        private static byte HexToByte(char currentChar)
        {
            byte byteValue = 0;
            if((currentChar >= '0') && (currentChar <= '9'))
            {
                byteValue = (byte)(Convert.ToByte(currentChar) - Convert.ToByte('0'));
            }
            else if ((currentChar >= 'A') && (currentChar <= 'F'))
            {
                byteValue = (byte)(10 + (Convert.ToByte(currentChar) - Convert.ToByte('A')));
            }

            return byteValue;
        }

        public string ToolTip
        {
            get
            {
                return this.toolTipForHexEdit.GetToolTip(this);
            }
            set
            {
                this.toolTipForHexEdit.SetToolTip(this, value);
            }
        }
    }
}
