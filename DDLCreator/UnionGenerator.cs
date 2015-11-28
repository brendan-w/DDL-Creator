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
    public partial class UnionGenerator : Form
    {
        private Trait currentTrait;

        public UnionGenerator(Trait trait)
        {
            InitializeComponent();
            this.currentTrait = trait;
            updateList();
        }

        private void updateList()
        {
            valueList.Items.Clear();
            if (continuous.Checked)
            {
                for (int i = 0; i < (int)quantity.Value; i++)
                {
                    int width = (int)span.Value - 1;
                    int min = (int)startValue.Value + ((width + (int)seperation.Value) * i);
                    int max = min + width;
                    valueList.Items.Add(min.ToString() + " - " + max.ToString());
                }
            }
            else
            {
                for (int i = 0; i < (int)quantity.Value; i++)
                {
                    int value = (int)startValue.Value + ((int)seperation.Value * i);
                    valueList.Items.Add(value.ToString());
                }
            }
        }

        private bool outOfRange()
        {
            int width = (int)span.Value - 1;
            int value = (int)startValue.Value + ((width + (int)seperation.Value) * ((int)quantity.Value - 1)) + width;

            return (value > 255);
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

        //type radios
        private void continuous_CheckedChanged(object sender, EventArgs e)
        {
            if (continuous.Checked)
            {
                spanGroup.Enabled = true;
                updateList();
            }
        }
        private void index_CheckedChanged(object sender, EventArgs e)
        {
            if (index.Checked)
            {
                spanGroup.Enabled = false;
                updateList();
            }
        }

        private void generate_Click(object sender, EventArgs e)
        {
            
            Data.genUnion(currentTrait,
                          continuous.Checked,
                          (int)quantity.Value,
                          (int)startValue.Value,
                          (int)span.Value,
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