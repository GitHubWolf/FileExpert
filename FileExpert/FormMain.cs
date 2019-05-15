using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;
using FileExpert.MP4;
using FileExpert.XMR;
using FileExpert.PRE;
using FileExpert.PRH;
using FileExpert.ASN;
using FileExpert.DER;
using FileExpert.JavaClass;

namespace FileExpert
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Open a file.
            OpenFileDialog openStreamFileDialog = new OpenFileDialog();
            openStreamFileDialog.Filter = "files|*.*";
            openStreamFileDialog.Title = "Select a file";
            openStreamFileDialog.CheckFileExists = true;
            openStreamFileDialog.CheckPathExists = true;
            if (DialogResult.OK == openStreamFileDialog.ShowDialog())
            {
                OpenLocalFile(openStreamFileDialog.FileName);
            }
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            List<string> recentFileList = ManagerMRU.GetMruList();

            toolStripMenuItemRecentFiles.DropDownItems.Clear();

            foreach (string file in recentFileList)
            {
                ToolStripMenuItem fileMenu = new ToolStripMenuItem(file);
                toolStripMenuItemRecentFiles.DropDownItems.Add(fileMenu);
                fileMenu.Click += new System.EventHandler(this.OpenRecentFile_Click); ;
            }
        }

        private void OpenRecentFile_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            OpenLocalFile(menuItem.Text);
        }

        private void OpenLocalFile(string fileName)
        {
            FormSelectParser formSelectParser = new FormSelectParser();
            formSelectParser.ShowDialog();
            //formSelectParser.SelectedIndex = 6;

            if (formSelectParser.DialogResult == DialogResult.OK)
            {
                switch (formSelectParser.SelectedIndex)
                {
                    case 0:
                        {
                            FormMp4 newForm = new FormMp4();
                            newForm.MdiParent = this;
                            newForm.WindowState = FormWindowState.Maximized;
                            //Set the stream file to the parser.
                            newForm.SetStreamFile(fileName);
                            newForm.Show();

                            //Save into the MRU.
                            ManagerMRU.AddMruItem(fileName);
                            break;
                        }

                    case 1:
                        {
                            FormXMR newForm = new FormXMR();
                            newForm.MdiParent = this;
                            newForm.WindowState = FormWindowState.Maximized;
                            //Set the stream file to the parser.
                            newForm.SetStreamFile(fileName);
                            newForm.Show();

                            //Save into the MRU.
                            ManagerMRU.AddMruItem(fileName);
                            break;
                        }
                    case 2:
                        {
                            FormPRE newForm = new FormPRE();
                            newForm.MdiParent = this;
                            newForm.WindowState = FormWindowState.Maximized;
                            //Set the stream file to the parser.
                            newForm.SetStreamFile(fileName);
                            newForm.Show();

                            //Save into the MRU.
                            ManagerMRU.AddMruItem(fileName);
                            break;
                        }

                    case 3:
                        {
                            FormPRH newForm = new FormPRH();
                            newForm.MdiParent = this;
                            newForm.WindowState = FormWindowState.Maximized;
                            //Set the stream file to the parser.
                            newForm.SetStreamFile(fileName);
                            newForm.Show();

                            //Save into the MRU.
                            ManagerMRU.AddMruItem(fileName);
                            break;
                        }
                    case 4:
                        {
                            FormASN newForm = new FormASN();
                            newForm.MdiParent = this;
                            newForm.WindowState = FormWindowState.Maximized;
                            //Set the stream file to the parser.
                            newForm.SetStreamFile(fileName);
                            newForm.Show();

                            //Save into the MRU.
                            ManagerMRU.AddMruItem(fileName);
                            break;
                        }
                    case 5:
                        {
                            FormDER newForm = new FormDER();
                            newForm.MdiParent = this;
                            newForm.WindowState = FormWindowState.Maximized;
                            //Set the stream file to the parser.
                            newForm.SetStreamFile(fileName);
                            newForm.Show();

                            //Save into the MRU.
                            ManagerMRU.AddMruItem(fileName);
                            break;
                        }
                    case 6:
                        {
                            FormJavaClass newForm = new FormJavaClass();
                            newForm.MdiParent = this;
                            newForm.WindowState = FormWindowState.Maximized;
                            //Set the stream file to the parser.
                            newForm.SetStreamFile(fileName);
                            newForm.Show();

                            //Save into the MRU.
                            ManagerMRU.AddMruItem(fileName);
                            break;
                        }
                    default:
                        break;
                }
            }
            

        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string fileToParse in fileList)
            {
                OpenLocalFile(fileToParse);
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

    }
}
