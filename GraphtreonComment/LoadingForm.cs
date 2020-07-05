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
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            this.TransparencyKey = Color.White;
            label1.BackColor = Color.White;
        }
    }
}
