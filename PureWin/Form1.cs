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
        // 修改为 async 异步方法
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("路径不存在！");
                return;
            }

            if (!checkBoxShortcut.Checked && !checkBoxFolder.Checked)
            {
                MessageBox.Show("请至少勾选一个搜索选项");
                return;
            }

            // UI 初始化
            clbResults.Items.Clear();
            pbScan.Value = 0;
            pbScan.Style = ProgressBarStyle.Marquee; // 搜索初期不知道总数，先用跑马灯效果
            btnSearch.Enabled = false; // 禁用按钮防止重复点击

            // 使用 Task.Run 在后台线程执行搜索
            await Task.Run(() =>
            {
                SearchLogic(path, cbRecursive.Checked);
            });

            // 搜索完成后恢复 UI
            pbScan.Style = ProgressBarStyle.Blocks;
            pbScan.Value = pbScan.Maximum;
            btnSearch.Enabled = true;

            if (clbResults.Items.Count == 0)
                MessageBox.Show("未发现匹配的项目");
        }

        // 递归搜索逻辑：解决权限拒绝和 UI 卡死
        private void SearchLogic(string rootPath, bool recursive)
        {
            WshShell shell = new WshShell();

            // 定义一个内部递归函数
            void Traverse(string currentPath)
            {
                try
                {
                    // 1. 处理当前文件夹下的快捷方式
                    if (checkBoxShortcut.Checked)
                    {
                        string[] files = Directory.GetFiles(currentPath, "*.lnk");
                        foreach (var file in files)
                        {
                            CheckShortcut(file, shell);
                        }
                    }

                    // 2. 处理当前路径下的子文件夹
                    string[] subDirs = Directory.GetDirectories(currentPath);

                    foreach (var dir in subDirs)
                    {
                        // 如果勾选了搜索空文件夹
                        if (checkBoxFolder.Checked)
                        {
                            CheckEmptyFolder(dir);
                        }

                        // 如果开启了递归，则继续深入
                        if (recursive)
                        {
                            Traverse(dir);
                        }
                    }
                }
                catch (UnauthorizedAccessException) { /* 自动跳过无权访问的文件夹 */ }
                catch (Exception ex) { Console.WriteLine("错误: " + ex.Message); }
            }

            Traverse(rootPath);
        }

        // 检查快捷方式是否失效
        private void CheckShortcut(string file, WshShell shell)
        {
            try
            {
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(file);
                string target = shortcut.TargetPath;
                if (string.IsNullOrEmpty(target) || (!System.IO.File.Exists(target) && !Directory.Exists(target)))
                {
                    // 异步跨线程更新 UI
                    this.Invoke(new Action(() => clbResults.Items.Add(file, true)));
                }
            }
            catch { }
        }

        // 检查是否为空文件夹
        private void CheckEmptyFolder(string dir)
        {
            try
            {
                // 如果该目录下既没有文件也没有文件夹
                if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
                {
                    this.Invoke(new Action(() => clbResults.Items.Add(dir, true)));
                }
            }
            catch { }
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

        private void clbResults_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 获取双击时鼠标所在位置的索引
            int index = clbResults.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string itemPath = clbResults.Items[index].ToString();

                try
                {
                    // 如果是文件，打开并定位到该文件；如果是文件夹，直接打开文件夹
                    if (System.IO.File.Exists(itemPath) || System.IO.Directory.Exists(itemPath))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{itemPath}\"");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法打开路径: {ex.Message}");
                }
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
