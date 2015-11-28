/*
 * DDL Creator Version 6.0
 * Designed and Implemented by: Brendan Whitfield
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace DDLCreator
{
    public partial class MainForm : Form
    {
        bool refreshing = false; //disables event handlers when program is changing control values
        //tried using keydown and keypress events, but they are dispatched before the control's fields are updated

        Trait currentTrait = null;

        //used for switching groups (.visible had strange issues)
        Point showLoc = new System.Drawing.Point(324, 159);
        Point hideLoc = new System.Drawing.Point(324, 559);


        public MainForm()
        {
            InitializeComponent();
            refreshProperties(); //ensures that everything is disabled or hidden
        }

        //MenuStrip Items/////////////////////////////////////////////////////

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogResult.Yes;

            if (!IO.saved)
            {
                dialogResult = MessageBox.Show("You have unsaved data. Are you sure you want to erase the current traits?", "New DDL?", MessageBoxButtons.YesNo);
            }

            if (dialogResult == DialogResult.Yes)
            {
                Data.deleteAll();
                refreshTraits();
                refreshProperties();
                updateInfo();
                IO.saved = true;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IO.open();
            refreshTraits();
            setTrait(0);
            refreshProperties();
            updateInfo();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveCheckMenuItem.Checked)
            {
                if (Data.check())
                {
                    IO.save(false);
                }
            }
            else
            {
                IO.save(false);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveCheckMenuItem.Checked)
            {
                if (Data.check())
                {
                    IO.save(true);
                }
            }
            else
            {
                IO.save(true);
            }
        }

        private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Preview preview = new Preview();
            preview.ShowDialog();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO undo functionality
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO redo functionality
        }


        //Context Menu Items/////////////////////////////////////////////////////

        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //don't need refreshProperties() because all values are the same
            if (listContextMenu.SourceControl == traitList)
            {
                Data.duplicateTrait(traitList.SelectedIndex);
                refreshTraits();
                traitList.SelectedIndex++;
            }
            else if (listContextMenu.SourceControl == indexList)
            {
                Data.duplicateIndex(currentTrait);
                refreshIndex(false);
                indexList.SelectedIndex++;
            }
            else if (listContextMenu.SourceControl == unionList)
            {
                Data.duplicateUnion(currentTrait);
                refreshUnion(false);
                unionList.SelectedIndex++;
            }
            IO.saved = false;
            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogResult.Yes;

            if (confirmDeletionsToolStripMenuItem.Checked)
            {
                dialogResult = MessageBox.Show("Are you sure you want to delete this?", "Delete?", MessageBoxButtons.YesNo);
            }
            
            if (dialogResult == DialogResult.Yes)
            {
                if (listContextMenu.SourceControl == traitList)
                {
                    Data.deleteTrait(currentTrait);
                    
                    if (traitList.SelectedIndex >= traitList.Items.Count)
                    {
                        traitList.SelectedIndex--;
                    }

                    refreshTraits();

                    //update currentTrait
                    if (traitList.SelectedIndex != -1)
                    {
                        currentTrait = Data.traits[traitList.SelectedIndex];
                    }
                    
                    refreshProperties();
                }
                else if (listContextMenu.SourceControl == indexList)
                {
                    Data.deleteIndex(currentTrait);
                    refreshIndex(false);
                    refreshIndexProperties(false);
                }
                else if (listContextMenu.SourceControl == unionList)
                {
                    Data.deleteUnion(currentTrait);
                    refreshUnion(false);
                    refreshUnionProperties(false);
                }
                IO.saved = false;
                
            }
        }  


        //Form Controls/////////////////////////////////////////////////////

        private void fixtureName_TextChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                Data.fixture = fixtureName.Text;
                IO.saved = false;
                
            }
        }

        private void manufacturer_TextChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                Data.manufacturer = manufacturer.Text;
                IO.saved = false;
                
            }
        }  

        private void fileCreator_TextChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                Data.creator = fileCreator.Text;
                IO.saved = false;
                
            }
        }

        private void notes_TextChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                Data.notes = notes.Text;
                IO.saved = false;
                
            }
        }




        //Trait list
        private void traitList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                traitList.SelectedIndex = traitList.IndexFromPoint(e.Location);
                if (traitList.SelectedIndex != -1)
                {
                    listContextMenu.Show(traitList, e.Location);
                }
            }

            if (traitList.SelectedIndex != -1)
            {
                int index = traitList.SelectedIndex;
                currentTrait = Data.traits[index];
                if (e.Button == MouseButtons.Left) { traitList.DoDragDrop(index, DragDropEffects.Move); }
                refreshProperties();
            }
        }

        private void traitList_DragDrop(object sender, DragEventArgs e)
        {
            int oldIndex = (int)e.Data.GetData(typeof(int));
            int newIndex = getIndex(traitList, e.X, e.Y);

            //swap the elements in Data
            Trait moving = Data.traits[oldIndex];
            Data.traits[oldIndex] = Data.traits[newIndex];
            Data.traits[newIndex] = moving;

            //update UI
            setTrait(newIndex);
            refreshTraits();
            IO.saved = false;
            
        }

        private void traitList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        //add trait
        private void addTrait_Click(object sender, EventArgs e)
        {
            Data.addTrait();
            refreshTraits();
            int index = Data.traits.Count - 1;
            setTrait(index);
            refreshProperties();
            IO.saved = false;
            
        }

        //trait name
        private void traitName_TextChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.name = traitName.Text;
                refreshTraits();
                IO.saved = false;
                
            }
        }


        private void traitName_KeyPress(object sender, KeyPressEventArgs e)
        {
            currentTrait.name = traitName.Text;
            refreshTraits();
            IO.saved = false;
            
        }

        //type radios
        private void traitTypeC_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing && traitTypeC.Checked)
            {
                currentTrait.type = ControlType.Continuous;
                refreshProperties();
                IO.saved = false;
            }
        }
        private void traitTypeI_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing && traitTypeI.Checked)
            {
                currentTrait.type = ControlType.Indexed;
                refreshProperties();
                IO.saved = false;
            }
        }
        private void traitTypeU_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing && traitTypeU.Checked)
            {
                currentTrait.type = ControlType.Union;
                refreshProperties();
                IO.saved = false;
            }
        }

        //channel
        private void traitChannel_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.channel = (int)traitChannel.Value;
                IO.saved = false;
            }
        }

        //default
        private void traitDefault_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.defaultValue = (int)traitDefault.Value;
                IO.saved = false;
            }
        }

        //blackout enable
        private void traitBlack_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.black = traitBlack.Checked;
                IO.saved = false;
            }

            traitBlackValue.Enabled = traitBlack.Checked;
        }

        //blackout value
        private void traitBlackValue_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.blackValue = (int)traitBlackValue.Value;
                IO.saved = false;
            }
        }


        //Continuous/////////////////////////////////////////////////

        //min
        private void continuousMin_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.min = (int)continuousMin.Value;
                IO.saved = false;
            }
        }

        //max
        private void continuousMax_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.max = (int)continuousMax.Value;
                IO.saved = false;
            }
        }

        //8Bit
        private void continuous8Bit_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing && continuous8Bit.Checked)
            {
                currentTrait.size = ControlSize.Bit8;
                if (continuousMin.Value > 255) { continuousMin.Value = 255; }
                if (continuousMax.Value > 255) { continuousMax.Value = 255; }
                continuousMin.Maximum = 255;
                continuousMax.Maximum = 255;
                IO.saved = false;
            }
        }

        //16bit
        private void continuous16Bit_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing && continuous16Bit.Checked)
            {
                currentTrait.size = ControlSize.Bit16;
                continuousMin.Maximum = 65535;
                continuousMax.Maximum = 65535;
                if ((int)continuousMax.Value == 255) { continuousMax.Value = 65535; } //odds are, they want the max to be the new max
                IO.saved = false;

                if (bitWarningToolStripMenuItem.Checked)
                {
                    MessageBox.Show("Warning: 16 Bit traits consume 2 DMX channels.", "Warning", MessageBoxButtons.OK);
                }
            }
        }

        //X
        private void continuousX_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.x = continuousX.Checked;
                IO.saved = false;
            }
        }

        //Y
        private void continuousY_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.y = continuousY.Checked;
                IO.saved = false;
            }
        }

        //Invert
        private void continuousInvert_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.invert = continuousInvert.Checked;
                IO.saved = false;
            }
        }

        //Grand
        private void continuousGrand_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentTrait.grand = continuousGrand.Checked;
                IO.saved = false;
            }
        }


        //Index/////////////////////////////////////////////////


        //Index List
        private void indexList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                indexList.SelectedIndex = indexList.IndexFromPoint(e.Location);
                if (indexList.SelectedIndex != -1) { listContextMenu.Show(indexList, e.Location); }
            }

            if (traitList.SelectedIndex != -1)
            {
                int index = indexList.SelectedIndex;
                currentTrait.indexSelect = index;
                if (e.Button == MouseButtons.Left) { indexList.DoDragDrop(index, DragDropEffects.Move); }
                refreshIndexProperties(false);
            }
        }

        private void indexList_DragDrop(object sender, DragEventArgs e)
        {
            int oldIndex = (int)e.Data.GetData(typeof(int));
            int newIndex = getIndex(indexList, e.X, e.Y);

            //swap the elements in Data
            Index moving = currentTrait.index[oldIndex];
            currentTrait.index[oldIndex] = currentTrait.index[newIndex];
            currentTrait.index[newIndex] = moving;

            //update UI
            currentTrait.indexSelect = newIndex;
            indexList.SelectedIndex = newIndex;
            refreshIndex(false);
            IO.saved = false;
        }

        private void indexList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }


        //add
        private void addIndex_Click(object sender, EventArgs e)
        {
            Data.addIndex(currentTrait);
            refreshIndex(false);
            currentTrait.indexSelect = currentTrait.index.Count - 1;
            indexList.SelectedIndex = currentTrait.indexSelect;
            refreshIndexProperties(false);
            IO.saved = false;
        }

        //generate
        private void generateIndex_Click(object sender, EventArgs e)
        {
            IndexGenerator dialog = new IndexGenerator(currentTrait);
            dialog.ShowDialog();
            refreshIndex(false);
            currentTrait.indexSelect = currentTrait.index.Count - 1;
            indexList.SelectedIndex = currentTrait.indexSelect;
            refreshIndexProperties(false);
            IO.saved = false;
        }

        //name
        private void indexName_TextChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentIndex().name = indexName.Text;
                refreshIndex(false);
                IO.saved = false;
            }
        }

        //value
        private void indexValue_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentIndex().value = (int)indexValue.Value;
                IO.saved = false;
            }
        }


        //Union/////////////////////////////////////////////////

        //Union list
        private void unionList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                unionList.SelectedIndex = unionList.IndexFromPoint(e.Location);
                if (unionList.SelectedIndex != -1) { listContextMenu.Show(unionList, e.Location); }
            }

            if (traitList.SelectedIndex != -1)
            {
                int index = unionList.SelectedIndex;
                currentTrait.unionSelect = index;
                if (e.Button == MouseButtons.Left) { unionList.DoDragDrop(index, DragDropEffects.Move); }
                refreshUnionProperties(false);
            }
        }

        private void unionList_DragDrop(object sender, DragEventArgs e)
        {
            int oldIndex = (int)e.Data.GetData(typeof(int));
            int newIndex = getIndex(unionList, e.X, e.Y);

            //swap the elements in Data
            Union moving = currentTrait.union[oldIndex];
            currentTrait.union[oldIndex] = currentTrait.union[newIndex];
            currentTrait.union[newIndex] = moving;

            //update UI
            currentTrait.unionSelect = newIndex;
            unionList.SelectedIndex = newIndex;
            refreshUnion(false);
            IO.saved = false;
        }

        private void unionList_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        //add
        private void addUnion_Click(object sender, EventArgs e)
        {
            Data.addUnion(currentTrait);
            refreshUnion(false);
            currentTrait.unionSelect = currentTrait.union.Count - 1;
            unionList.SelectedIndex = currentTrait.unionSelect;
            refreshUnionProperties(false);
            IO.saved = false;
        }

        //generate
        private void generateUnion_Click(object sender, EventArgs e)
        {
            UnionGenerator dialog = new UnionGenerator(currentTrait);
            dialog.ShowDialog();
            refreshUnion(false);
            currentTrait.unionSelect = currentTrait.union.Count - 1;
            unionList.SelectedIndex = currentTrait.unionSelect;
            refreshUnionProperties(false);
            IO.saved = false;
        }

        //name
        private void unionName_TextChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentUnion().name = unionName.Text;
                refreshUnion(false);
                IO.saved = false;
            }
        }

        //type
        private void unionTypeC_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing && unionTypeC.Checked)
            {
                currentUnion().type = ControlType.Continuous;
                refreshUnionProperties(false);
                IO.saved = false;
            }
        }
        private void unionTypeI_CheckedChanged(object sender, EventArgs e)
        {
            if (!refreshing && unionTypeI.Checked)
            {
                currentUnion().type = ControlType.Indexed;
                refreshUnionProperties(false);
                IO.saved = false;
            }
        }

        //min
        private void unionMin_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentUnion().min = (int)unionMin.Value;
                IO.saved = false;
            }
        }

        //max
        private void unionMax_ValueChanged(object sender, EventArgs e)
        {
            if (!refreshing)
            {
                currentUnion().max = (int)unionMax.Value;
                IO.saved = false;
            }
        }

        //General Functions/////////////////////////////////////////////////

        private void updateInfo()
        {
            refreshing = true;
            fixtureName.Text = Data.fixture;
            manufacturer.Text = Data.manufacturer;
            fileCreator.Text = Data.creator;
            notes.Text = Data.notes;
            refreshing = false;
        }

        private void setTrait(int t)
        {
            if (t < Data.traits.Count)
            {
                traitList.SelectedIndex = t;
                currentTrait = Data.traits[t];
            }
            else
            {
                traitList.SelectedIndex = Data.traits.Count - 1;
                if (traitList.SelectedIndex != -1)
                {
                    currentTrait = Data.traits[t];
                }
            }
        }

        private void refreshTraits()
        {
            refreshing = true;

            //listbox
            int selection = traitList.SelectedIndex;
            traitList.Items.Clear();
            for (int i = 0; i < Data.traits.Count; i++)
            {
                traitList.Items.Add(Data.traits[i].name);
            }

            //protect against indexes out of range
            if (selection >= Data.traits.Count)
            {
                selection = Data.traits.Count - 1;
            }
            traitList.SelectedIndex = selection;

            //traitCount label
            traitCount.Text = Data.traits.Count.ToString();
            if (Data.traits.Count == 1) { traitCount.Text += " Trait"; }
            else { traitCount.Text += " Traits"; }

            refreshing = false;
        }

        private void refreshProperties()
        {
            refreshing = true;

            traitContinuous.Location = hideLoc;
            traitIndex.Location = hideLoc;
            traitUnion.Location = hideLoc;

            if ((Data.traits.Count > 0) && (currentTrait != null))
            {
                traitProperties.Enabled = true;

                //load values into controls
                switch (currentTrait.type)
                {
                    case ControlType.Continuous:
                        traitTypeC.Checked = true;
                        traitContinuous.Location = showLoc;
                        refreshContinuous();
                        break;
                    case ControlType.Indexed:
                        traitTypeI.Checked = true;
                        traitIndex.Location = showLoc;
                        refreshIndex(true);
                        refreshIndexProperties(true);
                        break;
                    case ControlType.Union:
                        traitTypeU.Checked = true;
                        traitUnion.Location = showLoc;
                        refreshUnion(true);
                        refreshUnionProperties(true);
                        break;
                }
                traitName.Text = currentTrait.name;
                traitChannel.Value = currentTrait.channel;
                traitDefault.Value = currentTrait.defaultValue;
                traitBlack.Checked = currentTrait.black;
                traitBlackValue.Value = currentTrait.blackValue;

            }
            else
            {
                traitProperties.Enabled = false;
            }

            refreshing = false;
        }

        private void refreshContinuous()
        {
            //update controls
            switch (currentTrait.size)
            {
                case ControlSize.Bit8:
                    continuous8Bit.Checked = true;
                    continuousMin.Maximum = 255;
                    continuousMax.Maximum = 255;
                    break;
                case ControlSize.Bit16:
                    continuous16Bit.Checked = true;
                    continuousMin.Maximum = 65535;
                    continuousMax.Maximum = 65535;
                    break;
            }
            continuousMin.Value = currentTrait.min;
            continuousMax.Value = currentTrait.max;
            continuousX.Checked = currentTrait.x;
            continuousY.Checked = currentTrait.y;
            continuousInvert.Checked = currentTrait.invert;
            continuousGrand.Checked = currentTrait.grand;
        }

        private Index currentIndex() //more readable than the alternative
        {
            return currentTrait.index[currentTrait.indexSelect];
        }

        private void refreshIndex(bool refresh)
        {
            //update list
            refreshing = true;

            indexList.Items.Clear();
            for (int i = 0; i < currentTrait.index.Count; i++)
            {
                indexList.Items.Add(currentTrait.index[i].name);
            }

            if (currentTrait.indexSelect >= currentTrait.index.Count)
            {
                currentTrait.indexSelect = currentTrait.index.Count - 1;
            }
            indexList.SelectedIndex = currentTrait.indexSelect;

            //indexCount label
            indexCount.Text = currentTrait.index.Count.ToString();
            if (currentTrait.index.Count == 1) { indexCount.Text += " Index"; }
            else { indexCount.Text += " Indices"; }

            refreshing = refresh;
        }

        private void refreshIndexProperties(bool refresh)
        {
            //update controls
            refreshing = true;

            if (currentTrait.index.Count > 0)
            {
                indexNameGroup.Enabled = true;
                indexValueGroup.Enabled = true;

                Index index = currentTrait.index[currentTrait.indexSelect];
                indexName.Text = index.name;
                indexValue.Value = index.value;
            }
            else
            {
                indexNameGroup.Enabled = false;
                indexValueGroup.Enabled = false;
            }

            refreshing = refresh;
        }



        private Union currentUnion() //more readable than the alternative
        {
            return currentTrait.union[currentTrait.unionSelect];
        }

        private void refreshUnion(bool refresh)
        {
            //update list
            refreshing = true;

            unionList.Items.Clear();
            for (int i = 0; i < currentTrait.union.Count; i++)
            {
                unionList.Items.Add(currentTrait.union[i].name);
            }

            if (currentTrait.unionSelect >= currentTrait.union.Count)
            {
                currentTrait.unionSelect = unionList.Items.Count - 1;
            }
            unionList.SelectedIndex = currentTrait.unionSelect;

            //unionCount label
            unionCount.Text = currentTrait.union.Count.ToString();
            if (currentTrait.union.Count == 1) { unionCount.Text += " Union"; }
            else { unionCount.Text += " Unions"; }

            refreshing = refresh;
        }

        private void refreshUnionProperties(bool refresh)
        {
            //update controls
            refreshing = true;

            if (currentTrait.union.Count > 0)
            {
                unionNameGroup.Enabled = true;
                unionTypeGroup.Enabled = true;
                unionRangeGroup.Enabled = true;

                Union union = currentTrait.union[currentTrait.unionSelect];

                switch (union.type)
                {
                    case ControlType.Continuous:
                        unionTypeC.Checked = true;
                        unionMax.Enabled = true;
                        unionRangeLabel.Enabled = true;
                        unionRangeGroup.Text = "Range";
                        break;
                    case ControlType.Indexed:
                        unionTypeI.Checked = true;
                        unionMax.Enabled = false;
                        unionRangeLabel.Enabled = false;
                        unionRangeGroup.Text = "Value";
                        break;
                }
                unionName.Text = union.name;
                unionMin.Value = union.min;
                unionMax.Value = union.max;
            }
            else
            {
                unionNameGroup.Enabled = false;
                unionTypeGroup.Enabled = false;
                unionRangeGroup.Enabled = false;
            }

            refreshing = refresh;
        }


        private int getIndex(ListBox list, int x, int y)
        {
            Point point = list.PointToClient(new Point(x, y));
            int index = list.IndexFromPoint(point);
            if (index < 0) { index = list.Items.Count - 1; } //off the end of the list
            return index;
        }
    }
}