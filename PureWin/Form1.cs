using System;
using System.IO;
using System.Windows.Forms;
using IWshRuntimeLibrary; // 必须引用

namespace PureWin
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
                MessageBox.Show("路径不存在！");
                return;
            }

            if (!checkBoxShortcut.Checked && !checkBoxFolder.Checked)
            {
                MessageBox.Show("请至少勾选一个搜索选项（快捷方式或空文件夹）");
                return;
            }

            // --- 1. 搜索失效快捷方式 ---
            if (checkBoxShortcut.Checked)
            {
                try
                {
                    string[] files = Directory.GetFiles(path, "*.lnk");
                    WshShell shell = new WshShell();

                    foreach (var file in files)
                    {
                        try
                        {
                            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(file);
                            string target = shortcut.TargetPath;

                            if (string.IsNullOrEmpty(target) || (!System.IO.File.Exists(target) && !Directory.Exists(target)))
                            {
                                clbResults.Items.Add(file, true);
                            }
                        }
                        catch { /* 跳过无法解析的快捷方式 */ }
                    }
                }
                catch (Exception ex) { MessageBox.Show("搜索快捷方式时出错: " + ex.Message); }
            }

            // --- 2. 搜索空文件夹 ---
            if (checkBoxFolder.Checked)
            {
                try
                {
                    // 获取当前路径下的所有一级子文件夹
                    string[] dirs = Directory.GetDirectories(path);

                    foreach (var dir in dirs)
                    {
                        // 判断文件夹内是否既没有文件也没有子文件夹
                        bool isEmpty = Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0;

                        if (isEmpty)
                        {
                            clbResults.Items.Add(dir, true);
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("搜索文件夹时出错: " + ex.Message); }
            }

            if (clbResults.Items.Count == 0)
            {
                MessageBox.Show("未发现匹配的项目");
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

            var result = MessageBox.Show($"确定要删除这 {clbResults.CheckedItems.Count} 个项目吗？", "确认", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                var itemsToDelete = clbResults.CheckedItems.Cast<object>()
                                    .Select(x => x?.ToString())
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .ToList();

                foreach (var item in itemsToDelete)
                {
                    if (item == null) continue; // 显式排除 null

                    try
                    {
                        if (System.IO.File.Exists(item))
                        {
                            System.IO.File.Delete(item); // 删除文件
                        }
                        else if (Directory.Exists(item))
                        {
                            Directory.Delete(item); // 删除文件夹
                        }
                        clbResults.Items.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"删除失败: {item}\n错误: {ex.Message}");
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

        private void checkBoxFolder_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxShortcut_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
