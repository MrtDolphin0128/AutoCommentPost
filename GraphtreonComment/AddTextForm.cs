using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphtreonComment
{
    public partial class AddTextForm : Form
    {
        private string text;
        public AddTextForm()
        {
            InitializeComponent();
        }

        public string GetNewText()
        {
            return text;
        }

        public void SetCaption(string caption)
        {
            this.Text = caption;
        }

        public void SetDescription(string desc)
        {
            labelDesc.Text = desc;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            text = txtString.Text;
            this.Close();
        }

        private void txtString_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                this.DialogResult = DialogResult.OK;
                text = txtString.Text;
                this.Close();
            }
        }
    }
}
