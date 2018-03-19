using System;
using System.Windows.Forms;

namespace Smart.CacheServer
{
    internal partial class FormWcfConfig : Form
    {
        public FormWcfConfig()
        {
            InitializeComponent();
        }

        internal WcfHost WcfHost { get; set; }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

    }
}
