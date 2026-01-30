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

            // 确定搜索范围
            SearchOption opt = cbRecursive.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            // 初始化进度条
            pbScan.Value = 0;
            pbScan.Maximum = 100;

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
                    string[] files = Directory.GetFiles(path, "*.lnk", opt);
                    if (files.Length > 0)
                    {
                        pbScan.Maximum = files.Length; // 临时设为文件总数
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
                            catch { /* 忽略损坏的快捷方式文件 */ }
                            pbScan.PerformStep(); // 进度条走一格
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show("快捷方式搜索失败: " + ex.Message); }
            }

            // --- 2. 搜索空文件夹 ---
            if (checkBoxFolder.Checked)
            {
                try
                {
                    // 获取所有目录（根据是否递归）
                    string[] dirs = Directory.GetDirectories(path, "*", opt);
                    foreach (var dir in dirs)
                    {
                        // 安全检查：由于递归可能搜到系统保护目录，加个 try
                        try
                        {
                            if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
                            {
                                clbResults.Items.Add(dir, true);
                            }
                        }
                        catch { continue; }
                    }
                }
                catch (Exception ex) { MessageBox.Show("文件夹搜索失败: " + ex.Message); }
            }

            pbScan.Value = pbScan.Maximum; // 扫完直接拉满

            if (clbResults.Items.Count == 0)
            {
                MessageBox.Show("未发现匹配的项目");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (clbResults.CheckedItems.Count == 0)
            {
                MessageBox.Show("请先勾选要删除的项目");
                return;
            }

            var result = MessageBox.Show($"确定要删除这 {clbResults.CheckedItems.Count} 个项目吗？\n警告：删除后无法撤销。", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var itemsToDelete = clbResults.CheckedItems.Cast<object>()
                                    .Select(x => x?.ToString())
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .ToList();

                // --- 新增：定义一个计数器 ---
                int successCount = 0;

                // 重置进度条显示删除进度
                pbScan.Value = 0;
                pbScan.Maximum = itemsToDelete.Count;

                foreach (var item in itemsToDelete)
                {
                    if (item == null) continue;

                    try
                    {
                        if (System.IO.File.Exists(item))
                        {
                            System.IO.File.Delete(item);
                            successCount++; // 成功后计数加 1
                        }
                        else if (System.IO.Directory.Exists(item))
                        {
                            // 再次检查确认文件夹确实为空，防止搜索后又有新文件存入
                            if (System.IO.Directory.GetFiles(item).Length == 0 &&
                                System.IO.Directory.GetDirectories(item).Length == 0)
                            {
                                System.IO.Directory.Delete(item);
                                successCount++; // 成功后计数加 1
                            }
                        }

                        // 从 UI 列表中移除（注意：这里必须通过这种方式从后往前或安全移除）
                        clbResults.Items.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        // 删除失败时不弹窗（防止循环中弹出几百个窗），改为在控制台输出或继续
                        Console.WriteLine($"删除失败: {item}, 错误: {ex.Message}");
                    }

                    pbScan.PerformStep();
                    Application.DoEvents();
                }

                pbScan.Value = 0; // 清理完重置
                // --- 修改：弹窗提示具体的清理数量 ---
                MessageBox.Show($"清理完毕！已成功清理 {successCount} 个项目。", "成功");
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
