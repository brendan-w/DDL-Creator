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
    public partial class Preview : Form
    {
        private int page = 0;
        private int maxPage = 0;

        private List<Trait> displayTraits;

        public Preview()
        {
            InitializeComponent();

            displayTraits = new List<Trait>();
            for (int i = 0; i < Data.traits.Count; i++)
            {
                if (!Data.traits[i].x && !Data.traits[i].y)
                {
                    displayTraits.Add(Data.traits[i]);
                }
            }

            maxPage = (int) Math.Ceiling((double) displayTraits.Count / 4) - 1;
            update();
        }

        private void update()
        {
            trait1.Text = "";
            trait2.Text = "";
            trait3.Text = "";
            trait4.Text = "";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();

            if (displayTraits.Count > 0)
            {
                int start = page * 4;
                int stop = start + 4;
                if (stop > displayTraits.Count) { stop = displayTraits.Count; }
                int range = stop - start;

                if (range > 0)
                {
                    printTrait(displayTraits[start], trait1, listBox1);
                }
                if (range > 1)
                {
                    printTrait(displayTraits[start + 1], trait2, listBox2);
                }
                if (range > 2)
                {
                    printTrait(displayTraits[start + 2], trait3, listBox3);
                }
                if (range > 3)
                {
                    printTrait(displayTraits[start + 3], trait4, listBox4);
                }
            }
            pageLabel.Text = "Page " + (page + 1).ToString();
        }

        private void printTrait(Trait trait, Label label, ListBox list)
        {
            label.Text = trait.name;
            if (trait.type == ControlType.Indexed)
            {
                for (int i = 0; i < trait.index.Count; i++)
                {
                    list.Items.Add(trait.index[i].name);
                }
            }
            else if (trait.type == ControlType.Union)
            {
                for (int i = 0; i < trait.union.Count; i++)
                {
                    list.Items.Add(trait.union[i].name);
                }
            }
            else if (trait.type == ControlType.Continuous)
            {
                list.Items.Add("Min: " + trait.min.ToString());
                list.Items.Add("Max: " + trait.max.ToString());
            }
        }

        private void done_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void prev_Click(object sender, EventArgs e)
        {
            page--;
            if (page < 0) { page = maxPage; }
            update();
        }

        private void next_Click(object sender, EventArgs e)
        {
            page++;
            if (page > maxPage) { page = 0; }
            update();
        }
    }
}