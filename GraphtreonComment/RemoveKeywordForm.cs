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
    public partial class RemoveKeywordForm : Form
    {
        public List<int> selectedIndices = new List<int>();

        public RemoveKeywordForm(List<string> keywords)
        {
            InitializeComponent();

            foreach (string keyword in keywords)
                keywordList.Items.Add(keyword);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            ListBox.SelectedIndexCollection indices = keywordList.SelectedIndices;
            foreach (int index in indices)
                selectedIndices.Add(index);

            this.Close();
        }
    }
}
