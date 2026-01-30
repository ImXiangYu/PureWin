namespace PureWin
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnBrowse = new Button();
            txtPath = new TextBox();
            btnSearch = new Button();
            btnDelete = new Button();
            clbResults = new CheckedListBox();
            btnSelectAll = new Button();
            btnInvert = new Button();
            checkBoxShortcut = new CheckBox();
            checkBoxFolder = new CheckBox();
            groupBox1 = new GroupBox();
            cbRecursive = new CheckBox();
            pbScan = new ProgressBar();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(713, 44);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 23);
            btnBrowse.TabIndex = 0;
            btnBrowse.Text = "选择目录";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // txtPath
            // 
            txtPath.Location = new Point(48, 44);
            txtPath.Name = "txtPath";
            txtPath.Size = new Size(643, 23);
            txtPath.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(55, 314);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(255, 40);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "开始搜索";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.White;
            btnDelete.Location = new Point(713, 314);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 40);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "确认删除";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // clbResults
            // 
            clbResults.CheckOnClick = true;
            clbResults.FormattingEnabled = true;
            clbResults.Location = new Point(369, 134);
            clbResults.Name = "clbResults";
            clbResults.Size = new Size(322, 220);
            clbResults.TabIndex = 4;
            clbResults.SelectedIndexChanged += clbResults_SelectedIndexChanged;
            // 
            // btnSelectAll
            // 
            btnSelectAll.Location = new Point(713, 134);
            btnSelectAll.Name = "btnSelectAll";
            btnSelectAll.Size = new Size(75, 36);
            btnSelectAll.TabIndex = 5;
            btnSelectAll.Text = "全选";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // btnInvert
            // 
            btnInvert.Location = new Point(713, 195);
            btnInvert.Name = "btnInvert";
            btnInvert.Size = new Size(75, 36);
            btnInvert.TabIndex = 6;
            btnInvert.Text = "反选";
            btnInvert.UseVisualStyleBackColor = true;
            btnInvert.Click += btnInvert_Click;
            // 
            // checkBoxShortcut
            // 
            checkBoxShortcut.AutoSize = true;
            checkBoxShortcut.Checked = true;
            checkBoxShortcut.CheckState = CheckState.Checked;
            checkBoxShortcut.Location = new Point(23, 34);
            checkBoxShortcut.Name = "checkBoxShortcut";
            checkBoxShortcut.Size = new Size(75, 21);
            checkBoxShortcut.TabIndex = 7;
            checkBoxShortcut.Text = "快捷方式";
            checkBoxShortcut.UseVisualStyleBackColor = true;
            checkBoxShortcut.CheckedChanged += checkBoxShortcut_CheckedChanged;
            // 
            // checkBoxFolder
            // 
            checkBoxFolder.AutoSize = true;
            checkBoxFolder.Location = new Point(23, 61);
            checkBoxFolder.Name = "checkBoxFolder";
            checkBoxFolder.Size = new Size(75, 21);
            checkBoxFolder.TabIndex = 8;
            checkBoxFolder.Text = "空文件夹";
            checkBoxFolder.UseVisualStyleBackColor = true;
            checkBoxFolder.CheckedChanged += checkBoxFolder_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbRecursive);
            groupBox1.Controls.Add(checkBoxShortcut);
            groupBox1.Controls.Add(checkBoxFolder);
            groupBox1.Location = new Point(55, 134);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(255, 133);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "选择要清理的项目";
            // 
            // cbRecursive
            // 
            cbRecursive.AutoSize = true;
            cbRecursive.Location = new Point(23, 88);
            cbRecursive.Name = "cbRecursive";
            cbRecursive.Size = new Size(99, 21);
            cbRecursive.TabIndex = 9;
            cbRecursive.Text = "包含子文件夹";
            cbRecursive.UseVisualStyleBackColor = true;
            // 
            // pbScan
            // 
            pbScan.Location = new Point(55, 391);
            pbScan.Name = "pbScan";
            pbScan.Size = new Size(636, 23);
            pbScan.TabIndex = 10;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pbScan);
            Controls.Add(groupBox1);
            Controls.Add(btnInvert);
            Controls.Add(btnSelectAll);
            Controls.Add(clbResults);
            Controls.Add(btnDelete);
            Controls.Add(btnSearch);
            Controls.Add(txtPath);
            Controls.Add(btnBrowse);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PureWin";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBrowse;
        private TextBox txtPath;
        private Button btnSearch;
        private Button btnDelete;
        private CheckedListBox clbResults;
        private Button btnSelectAll;
        private Button btnInvert;
        private CheckBox checkBoxShortcut;
        private CheckBox checkBoxFolder;
        private GroupBox groupBox1;
        private CheckBox cbRecursive;
        private ProgressBar pbScan;
    }
}
