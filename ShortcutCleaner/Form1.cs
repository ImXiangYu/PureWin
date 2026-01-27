using System;
using System.IO;
using System.Windows.Forms;
using IWshRuntimeLibrary; // 必须引用

namespace ShortcutCleaner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // 默认路径设为当前程序运行目录
            txtPath.Text = AppDomain.CurrentDomain.BaseDirectory;
        }

        // “浏览”按钮逻辑
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                    txtPath.Text = fbd.SelectedPath;
            }
        }

        // “开始搜索”按钮逻辑
        private void btnSearch_Click(object sender, EventArgs e)
        {
            clbResults.Items.Clear();
            string path = txtPath.Text;

            if (!Directory.Exists(path))
            {
                MessageBox.Show("路径不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 获取所有快捷方式
                string[] files = Directory.GetFiles(path, "*.lnk", SearchOption.TopDirectoryOnly);
                WshShell shell = new WshShell();

                foreach (var file in files)
                {
                    try
                    {
                        // 解析快捷方式
                        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(file);
                        string target = shortcut.TargetPath;

                        // 核心判断逻辑：
                        // 1. 目标路径为空（有些特殊快捷方式）
                        // 2. 目标文件不存在 且 目标文件夹也不存在
                        if (string.IsNullOrWhiteSpace(target) || (!System.IO.File.Exists(target) && !Directory.Exists(target)))
                        {
                            clbResults.Items.Add(file, true);
                        }
                    }
                    catch { /* 遇到系统保护文件或损坏的快捷方式，跳过 */ }
                }

                if (clbResults.Items.Count == 0)
                    MessageBox.Show("未发现失效的快捷方式！", "完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"搜索过程中发生错误: {ex.Message}");
            }
        }

        // “确认删除”按钮逻辑
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (clbResults.CheckedItems.Count == 0)
            {
                MessageBox.Show("请先勾选要删除的项目");
                return;
            }

            var result = MessageBox.Show($"确定要从磁盘删除这 {clbResults.CheckedItems.Count} 个失效文件吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 先把要删除的项拷贝出来，避免在遍历时修改集合引发异常
                List<string> filesToDelete = new List<string>();
                foreach (var item in clbResults.CheckedItems)
                {
                    string? path = item?.ToString(); // 使用 string? 表示可能为空
                    if (path != null)
                    {
                        filesToDelete.Add(path);
                    }
                }

                foreach (string file in filesToDelete)
                {
                    try
                    {
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                        clbResults.Items.Remove(file); // 从界面移除
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"删除失败: {file}\n错误: {ex.Message}");
                    }
                }

                MessageBox.Show("清理完毕！", "成功");
            }
        }

        // 全选按钮
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbResults.Items.Count; i++)
            {
                clbResults.SetItemChecked(i, true);
            }
        }

        // 反选按钮
        private void btnInvert_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbResults.Items.Count; i++)
            {
                // 如果当前是勾选的，就设为不勾选，反之亦然
                bool isChecked = clbResults.GetItemChecked(i);
                clbResults.SetItemChecked(i, !isChecked);
            }
        }


        private void clbResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
