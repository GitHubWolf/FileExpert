namespace FileExpert
{
    partial class FormParser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParser));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelTopRight = new System.Windows.Forms.TableLayoutPanel();
            this.richTextBoxHexString = new Library.HexEdit();
            this.richTextBoxProperty = new Library.HexEdit();
            this.treeViewParser = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.richTextBoxLogger = new Library.HexEdit();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerToUpdateData = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dumpBinaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            this.tableLayoutPanelTopRight.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStripTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelTop, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.richTextBoxLogger, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(717, 498);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.ColumnCount = 2;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelTop.Controls.Add(this.tableLayoutPanelTopRight, 1, 0);
            this.tableLayoutPanelTop.Controls.Add(this.treeViewParser, 0, 0);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 1;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(711, 392);
            this.tableLayoutPanelTop.TabIndex = 0;
            // 
            // tableLayoutPanelTopRight
            // 
            this.tableLayoutPanelTopRight.ColumnCount = 1;
            this.tableLayoutPanelTopRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTopRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTopRight.Controls.Add(this.richTextBoxHexString, 0, 0);
            this.tableLayoutPanelTopRight.Controls.Add(this.richTextBoxProperty, 0, 1);
            this.tableLayoutPanelTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTopRight.Location = new System.Drawing.Point(500, 3);
            this.tableLayoutPanelTopRight.Name = "tableLayoutPanelTopRight";
            this.tableLayoutPanelTopRight.RowCount = 2;
            this.tableLayoutPanelTopRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelTopRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelTopRight.Size = new System.Drawing.Size(208, 386);
            this.tableLayoutPanelTopRight.TabIndex = 0;
            // 
            // richTextBoxHexString
            // 
            this.richTextBoxHexString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxHexString.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxHexString.HideSelection = false;
            this.richTextBoxHexString.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxHexString.Name = "richTextBoxHexString";
            this.richTextBoxHexString.Size = new System.Drawing.Size(202, 264);
            this.richTextBoxHexString.TabIndex = 0;
            this.richTextBoxHexString.Text = "";
            this.richTextBoxHexString.ToolTip = "";
            // 
            // richTextBoxProperty
            // 
            this.richTextBoxProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxProperty.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxProperty.HideSelection = false;
            this.richTextBoxProperty.Location = new System.Drawing.Point(3, 273);
            this.richTextBoxProperty.Name = "richTextBoxProperty";
            this.richTextBoxProperty.Size = new System.Drawing.Size(202, 110);
            this.richTextBoxProperty.TabIndex = 1;
            this.richTextBoxProperty.Text = "";
            this.richTextBoxProperty.ToolTip = "";
            // 
            // treeViewParser
            // 
            this.treeViewParser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewParser.ImageIndex = 0;
            this.treeViewParser.ImageList = this.imageListTreeView;
            this.treeViewParser.Location = new System.Drawing.Point(3, 3);
            this.treeViewParser.Name = "treeViewParser";
            this.treeViewParser.SelectedImageIndex = 0;
            this.treeViewParser.Size = new System.Drawing.Size(491, 386);
            this.treeViewParser.TabIndex = 1;
            this.treeViewParser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewParser_AfterSelect);
            this.treeViewParser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewParser_MouseDown);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "Error.png");
            this.imageListTreeView.Images.SetKeyName(1, "Field.png");
            this.imageListTreeView.Images.SetKeyName(2, "Item.png");
            this.imageListTreeView.Images.SetKeyName(3, "Loop.png");
            this.imageListTreeView.Images.SetKeyName(4, "Root.png");
            this.imageListTreeView.Images.SetKeyName(5, "SearchRequest.png");
            this.imageListTreeView.Images.SetKeyName(6, "Section1.png");
            this.imageListTreeView.Images.SetKeyName(7, "Section2.png");
            this.imageListTreeView.Images.SetKeyName(8, "Statistic.png");
            this.imageListTreeView.Images.SetKeyName(9, "TopGroup1.png");
            this.imageListTreeView.Images.SetKeyName(10, "TopGroup2.png");
            this.imageListTreeView.Images.SetKeyName(11, "Warning.png");
            this.imageListTreeView.Images.SetKeyName(12, "Data.png");
            // 
            // richTextBoxLogger
            // 
            this.richTextBoxLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLogger.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxLogger.HideSelection = false;
            this.richTextBoxLogger.Location = new System.Drawing.Point(3, 401);
            this.richTextBoxLogger.Name = "richTextBoxLogger";
            this.richTextBoxLogger.Size = new System.Drawing.Size(711, 94);
            this.richTextBoxLogger.TabIndex = 1;
            this.richTextBoxLogger.Text = "";
            this.richTextBoxLogger.ToolTip = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(717, 32);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.AutoSize = false;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(600, 26);
            this.toolStripProgressBar1.Step = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(100, 27);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "0%";
            // 
            // timerToUpdateData
            // 
            this.timerToUpdateData.Enabled = true;
            this.timerToUpdateData.Interval = 500;
            this.timerToUpdateData.Tick += new System.EventHandler(this.timerToUpdateData_Tick);
            // 
            // contextMenuStripTreeView
            // 
            this.contextMenuStripTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dumpBinaryToolStripMenuItem});
            this.contextMenuStripTreeView.Name = "contextMenuStripTreeView";
            this.contextMenuStripTreeView.Size = new System.Drawing.Size(153, 48);
            // 
            // dumpBinaryToolStripMenuItem
            // 
            this.dumpBinaryToolStripMenuItem.Image = global::FileExpert.Properties.Resources.save;
            this.dumpBinaryToolStripMenuItem.Name = "dumpBinaryToolStripMenuItem";
            this.dumpBinaryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dumpBinaryToolStripMenuItem.Text = "Dump Binary";
            this.dumpBinaryToolStripMenuItem.Click += new System.EventHandler(this.dumpBinaryToolStripMenuItem_Click);
            // 
            // FormParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 498);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormParser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parser";
            this.Load += new System.EventHandler(this.FormParser_Load);
            this.Resize += new System.EventHandler(this.FormParser_Resize);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelTopRight.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStripTreeView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTopRight;
        protected System.Windows.Forms.TreeView treeViewParser;
        protected Library.HexEdit richTextBoxHexString;
        protected Library.HexEdit richTextBoxProperty;
        protected Library.HexEdit richTextBoxLogger;
        private System.Windows.Forms.ImageList imageListTreeView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        protected System.Windows.Forms.Timer timerToUpdateData;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeView;
        private System.Windows.Forms.ToolStripMenuItem dumpBinaryToolStripMenuItem;
    }
}