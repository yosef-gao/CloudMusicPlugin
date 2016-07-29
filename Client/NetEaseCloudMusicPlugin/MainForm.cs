using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetEaseCloudMusicPlugin
{
    public partial class MainForm : Form
    {
        List<string> songsList = new List<string>();
        Random random = new Random(Environment.TickCount);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            
        }

        private Task<int> SomeFunction()
        {
            return Task.Run(() => 
            {
                IntPtr test = SystemCallUtils.FindWindow("OrpheusBrowserHost", null);
                if (test != IntPtr.Zero)
                {
                    test = SystemCallUtils.FindWindowEx(test, IntPtr.Zero, "CefBrowserWindow", null);
                    test = SystemCallUtils.FindWindowEx(test, IntPtr.Zero, "Chrome_WidgetWin_0", null);
                }

                // 鼠标移动到搜索框
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_MOUSEMOVE, 0x00, 0x001B014E);
                Thread.Sleep(10);

                // 让网易云客户端获得焦点
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONDOWN, 0x01, 0x001B014E);
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONUP, 0x00, 0x001B014E);
                Thread.Sleep(10);

                // 鼠标点击搜索框
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONDOWN, 0x01, 0x001B014E);
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONUP, 0x00, 0x001B014E);
                Thread.Sleep(10);

                // 发送ctrl+v，粘贴
                SystemCallUtils.keybd_event(SystemCallUtils.VK_CONTROL, 0, 0, 0); // 用keybord event 发送ctrl
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_KEYDOWN, SystemCallUtils.VK_V, 0x002F0001);
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_CHAR, 0x016, 0x002F0001);
                Thread.Sleep(10);
                SystemCallUtils.keybd_event(SystemCallUtils.VK_CONTROL, 0, 0x02, 0);
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_KEYUP, SystemCallUtils.VK_V, 0xC02F0001);
                Thread.Sleep(10);

                // 回车
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_KEYDOWN, SystemCallUtils.VK_RETURN, 0x001C001);
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_KEYUP, SystemCallUtils.VK_RETURN, 0xC01C001);

                // 点击单曲
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_MOUSEMOVE, 0x00, 0x00870119);
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONDOWN, 0x01, 0x00870119);
                Thread.Sleep(10);
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONUP, 0x00, 0x00870119);
                Thread.Sleep(10);

                // 等待网页刷新
                Thread.Sleep(1000);

                // 移动到第一个
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_MOUSEMOVE, 0x00, 0x00C30151);
                Thread.Sleep(10);
                // 按下鼠标左键
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONDOWN, 0x01, 0x00C30151);
                Thread.Sleep(10);
                // 双击
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONDBLCLK, 0x01, 0x00C30151);
                Thread.Sleep(10);
                // 松开
                SystemCallUtils.PostMessage(test, SystemCallUtils.WM_LBUTTONUP, 0x00, 0x00C30151);
                Thread.Sleep(10);

                return 1;
            });
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(GetNextSongID());
            int i = await SomeFunction();
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Multiselect = false;
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                bool result = await ReadSongsFile(fDialog.FileName);
            }
        }

        private string GetNextSongID()
        {
            int index = random.Next(songsList.Count);
            string ret = songsList[index];
            songsList.RemoveAt(index);
            return ret;
        }

        private Task<bool> ReadSongsFile(string songsFile)
        {
            return Task.Run(() => 
            {
                using (StreamReader reader = new StreamReader(songsFile))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                        songsList.Add(line);
                }
                return true;
            });
        }
    }
}
