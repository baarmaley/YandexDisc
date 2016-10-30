using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YandexDisc
{
    public partial class AddDirectory : Form
    {
        public String NameDirectory { get; protected set; }
        public AddDirectory()
        {
            InitializeComponent();
            btn_OK.DialogResult = DialogResult.OK;
            btn_cancel.DialogResult = DialogResult.Cancel;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            NameDirectory = tb_name.Text;
            this.Close();
        }
    }
}
