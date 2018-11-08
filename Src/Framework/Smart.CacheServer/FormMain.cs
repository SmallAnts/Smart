using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Smart.Core.Extensions;

namespace Smart.CacheServer
{
    public partial class FormMain : Form
    {
        WcfHost wcfhost;
        ConcurrentQueue<string> messages;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.Icon = this.Icon;
            wcfhost = new WcfHost();
            messages = new ConcurrentQueue<string>();

            this.FormClosing += FormMain_FormClosing;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                e.Cancel = true;
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            if (btnSwitch.Text == "启动")
            {
                this.btnSwitch.Text = "停止";
                this.btnWcfConfig.Enabled = false;
                this.wcfhost.Start();
                this.AddMessage($"缓存服务启动成功！{wcfhost.Host}:{wcfhost.Port}");
            }
            else
            {
                this.btnSwitch.Text = "启动";
                this.wcfhost.Stop();
                this.AddMessage("缓存服务已停止！");
                this.btnWcfConfig.Enabled = true;
            }
        }

        private void btnWcfConfig_Click(object sender, EventArgs e)
        {
            var config = new FormWcfConfig();
            config.propertyGrid.SelectedObject = wcfhost.Copy();
            if (config.ShowDialog() == DialogResult.OK)
            {
                wcfhost = config.propertyGrid.SelectedObject as WcfHost;
            }
        }

        public void AddMessage(string message)
        {
            messages.Enqueue(message);
            if (!timer1.Enabled)
            {
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var len = Math.Min(messages.Count, 100);
            string message = string.Empty;
            var msgs = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                messages.TryDequeue(out message);
                msgs.AppendLine(message);
            }
            this.textBox1.AppendText(msgs.ToString());
            this.textBox1.ScrollToCaret();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要退出缓存服务吗？", "确认提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
