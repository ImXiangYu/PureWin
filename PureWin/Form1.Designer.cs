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
            btnSearch.Location = new Point(48, 134);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(207, 74);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "开始搜索";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(48, 255);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(207, 74);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "确认删除";
            btnDelete.UseVisualStyleBackColor = true;
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
            btnSelectAll.Size = new Size(75, 23);
            btnSelectAll.TabIndex = 5;
            btnSelectAll.Text = "全选";
            btnSelectAll.UseVisualStyleBackColor = true;
            btnSelectAll.Click += btnSelectAll_Click;
            // 
            // btnInvert
            // 
            btnInvert.Location = new Point(713, 185);
            btnInvert.Name = "btnInvert";
            btnInvert.Size = new Size(75, 23);
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
            checkBoxShortcut.Location = new Point(48, 107);
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
            checkBoxFolder.Location = new Point(180, 107);
            checkBoxFolder.Name = "checkBoxFolder";
            checkBoxFolder.Size = new Size(75, 21);
            checkBoxFolder.TabIndex = 8;
            checkBoxFolder.Text = "空文件夹";
            checkBoxFolder.UseVisualStyleBackColor = true;
            checkBoxFolder.CheckedChanged += checkBoxFolder_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(checkBoxFolder);
            Controls.Add(checkBoxShortcut);
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
    }
}
