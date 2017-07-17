using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ShinraCo.Settings.Forms.Design
{
    public partial class UserNumeric : UserControl
    {
        [Category("Data")]
        public decimal Value { get { return customNumeric1.Value; } set { customNumeric1.Value = value; } }

        public event EventHandler ValueChanged
        {
            add { customNumeric1.ValueChanged += value; }
            remove { customNumeric1.ValueChanged -= value; }
        }

        public UserNumeric()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Focus();
            if (customNumeric1.Value < customNumeric1.Maximum)
            {
                customNumeric1.Value += 5;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Focus();
            if (customNumeric1.Value > customNumeric1.Minimum)
            {
                customNumeric1.Value -= 5;
            }
        }

        private void customNumeric1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                label1.Focus();
            }
        }
    }
}