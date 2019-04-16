using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExpert
{
    public partial class FormSelectParser : Form
    {
        public int SelectedIndex = 0;
        public FormSelectParser()
        {
            InitializeComponent();

            listBoxParsers.SelectedIndex = 0;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SelectedIndex = listBoxParsers.SelectedIndex;
            this.DialogResult = DialogResult.OK;
        }
    }
}
