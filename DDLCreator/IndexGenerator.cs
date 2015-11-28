using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDLCreator
{
    public partial class IndexGenerator : Form
    {
        private Trait currentTrait;

        public IndexGenerator(Trait trait)
        {
            InitializeComponent();
            this.currentTrait = trait;
            updateList();
        }

        private void updateList()
        {
            valueList.Items.Clear();
            for (int i = 0; i < (int)quantity.Value; i++)
            {
                int value = (int)startValue.Value + ((int)seperation.Value * i);
                valueList.Items.Add(value.ToString());
            }
        }

        private bool outOfRange()
        {
            return (((((int)quantity.Value - 1) * (int)seperation.Value) + (int)startValue.Value) > 255);
        }


        //controls
        private void valueChanged(object sender, EventArgs e)
        {
            if (outOfRange())
            {
                ((NumericUpDown)sender).Value--;
            }
            updateList();
        }

        private void generate_Click(object sender, EventArgs e)
        {
            Data.genIndex(currentTrait,
                          (int)quantity.Value,
                          (int)startValue.Value,
                          (int)seperation.Value,
                          prefix.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}