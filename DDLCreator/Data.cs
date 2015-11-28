/*
 * class used for storing DDL data for editing/opening/saving
 * 
 * public properties
 *     List<Trait> traits
 *     String fixture
 *     String manufacturer
 *     String creator
 *     String notes
 * 
 * public functions:
 *     Data.addTrait()
 *     Data.addIndex(Trait trait)
 *     Data.genIndex(Trait trait, int count, int start, int seperation, String prefix)
 *     Data.addUnion(Trait trait)
 *     Data.genUnion(Trait trait, bool cont, int count, int start, int span, int seperation, String prefix)
 *     Data.check()
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDLCreator
{
    //Data structure types/////////////////////////////////////////////////

    public enum ControlType {Continuous, Indexed, Union, EndUnion};
    public enum ControlSize {Bit8, Bit16};

    public class Trait
    {
        public string name;
        public ControlType type;
        public int channel;
        public int defaultValue;
        public bool black;
        public int blackValue;

        public int min;
        public int max;
        public ControlSize size;
        public bool x;
        public bool y;
        public bool invert;
        public bool grand;

        public readonly List<Index> index;
        public readonly List<Union> union;
        public int indexSelect;
        public int unionSelect;

        public Trait()
        {
            index = new List<Index>();
            union = new List<Union>();
        }

        public Trait clone()
        {
            Trait trait = new Trait();
            trait.name = this.name;
            trait.type = this.type;
            trait.channel = this.channel;
            trait.defaultValue = this.defaultValue;
            trait.black = this.black;
            trait.blackValue = this.blackValue;
            trait.min = this.min;
            trait.max = this.max;
            trait.size = this.size;
            trait.x = this.x;
            trait.y = this.y;
            trait.invert = this.invert;
            trait.grand = this.grand;
            trait.indexSelect = this.indexSelect;
            trait.unionSelect = this.unionSelect;
            
            for (int i = 0; i < this.index.Count; i++)
            {
                trait.index.Add(this.index[i].clone());
            }
            for (int i = 0; i < this.union.Count; i++)
            {
                trait.union.Add(this.union[i].clone());
            }

            return trait;
        }
    }

    public class Index
    {
        public String name;
        public int value;

        public Index clone()
        {
            Index index = new Index();
            index.name = this.name;
            index.value = this.value;
            return index;
        }
    }

    public class Union
    {
        public String name;
        public ControlType type;
        public int min; //value of index
        public int max;

        public Union clone()
        {
            Union union = new Union();
            union.name = this.name;
            union.type = this.type;
            union.min = this.min;
            union.max = this.max;
            return union;
        }
    }

    //Data handler/////////////////////////////////////////////////

    public static class Data
    {
        public static readonly List<Trait> traits; //main storage
        public static String fixture;
        public static String manufacturer;
        public static String creator;
        public static String notes;


        static Data()
        {
            traits = new List<Trait>();
            fixture = "";
            manufacturer = "";
            creator = "";
            notes = "";
        }

        public static void addTrait()
        {
            Trait trait = new Trait();
            traits.Add(trait);

            trait.name = "Trait " + traits.Count.ToString();
            trait.type = ControlType.Continuous;
            trait.channel = traits.Count; //TODO make channel account for 16Bit traits and holes in the channel layout
            trait.defaultValue = 0;
            trait.black = false;
            trait.blackValue = 0;

            trait.min = 0;
            trait.max = 255;
            trait.size = ControlSize.Bit8;
            trait.x = false;
            trait.y = false;
            trait.invert = false;
            trait.grand = false;

            trait.indexSelect = -1;
            trait.unionSelect = -1;
        }

        public static void deleteTrait(Trait trait)
        {
            traits.Remove(trait);
        }

        public static void duplicateTrait(int trait)
        {
            traits.Insert(trait, traits[trait].clone());
        }

        public static void addIndex(Trait trait)
        {
            Index index = new Index();
            trait.index.Add(index);

            index.name = "Index " + trait.index.Count;
            index.value = 0;
        }

        public static void deleteIndex(Trait trait)
        {
            trait.index.Remove(trait.index[trait.indexSelect]);
            if (trait.indexSelect >= trait.index.Count)
            {
                trait.indexSelect--;
            }
        }

        public static void duplicateIndex(Trait trait)
        {
            trait.index.Insert(trait.indexSelect, trait.index[trait.indexSelect].clone());
        }

        //called by form IndexGenerator.cs
        public static void genIndex(Trait trait, int count, int start, int seperation, String prefix)
        {
            for (int i = 0; i < count; i++)
            {
                Index index = new Index();
                index.name = prefix + " " + (i + 1).ToString();
                index.value = start + (seperation * i);
                trait.index.Add(index);
            }
        }


        public static void addUnion(Trait trait)
        {

            Union union = new Union();

            if (trait.union.Count > 0)
            {
                //if there are things in the list, use the last type (better workflow)
                union.type = trait.union[trait.union.Count - 1].type;
            }
            else
            {
                union.type = ControlType.Continuous;
            }

            trait.union.Add(union);
            union.name = "Union " + trait.union.Count;
            union.min = 0;
            union.max = 255;
        }

        public static void deleteUnion(Trait trait)
        {
            trait.union.Remove(trait.union[trait.unionSelect]);
            if (trait.unionSelect >= trait.union.Count)
            {
                trait.unionSelect--;
            }
        }

        public static void duplicateUnion(Trait trait)
        {
            trait.union.Insert(trait.unionSelect, trait.union[trait.unionSelect].clone());
        }

        //called by form UnionGenerator.cs
        public static void genUnion(Trait trait, bool cont, int count, int start, int span, int seperation, String prefix)
        {
            if (cont)
            {
                span--;
                for (int i = 0; i < count; i++)
                {
                    Union union = new Union();
                    union.name = prefix + " " + (i + 1).ToString();
                    union.type = ControlType.Continuous;
                    union.min = start + ((span + seperation) * i);
                    union.max = union.min + span;
                    trait.union.Add(union);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Union union = new Union();
                    union.name = prefix + " " + (i + 1).ToString();
                    union.type = ControlType.Indexed;
                    union.min = start + (seperation * i);
                    trait.union.Add(union);
                }
            }
        }

        //careful now...
        public static void deleteAll()
        {
            traits.Clear();
            fixture = "";
            manufacturer = "";
            creator = "";
            notes = "";
        }

        //checks the integrity of the data and shows a modal dialog with any errors
        public static bool check()
        {
            List<String> report = new List<String>();

            if (fixture == "") { report.Add("Missing Fixture Name"); }
            if (traits.Count == 0) { report.Add("There are no traits to save"); }

            for (int t = 0; t < traits.Count; t++)
            {
                Trait trait = traits[t];
                String traitName = trait.name;
                if (trait.name == "")
                {
                    report.Add("Trait " + (t + 1) + " has no name");
                    traitName = "Trait " + (t + 1);
                }

                //trait cross check
                for (int tt = 0; tt < traits.Count; tt++)
                {
                    if (tt != t) //all other traits
                    {
                        Trait otherTrait = traits[tt];
                        String otherName = otherTrait.name;
                        if (otherTrait.name == "") { otherName = "Trait " + (tt + 1); }

                        if ((trait.type == ControlType.Continuous) &&
                            (trait.size == ControlSize.Bit16) &&
                            (trait.channel + 1 == otherTrait.channel)) { report.Add(otherName + " collides with the upper (16 Bit) channel of " + traitName); }

                        if (tt > t) //complete graph traits
                        {
                            if ((trait.name != "") && (trait.name == otherTrait.name)) { report.Add("Traits " + (t + 1) + " and " + (tt + 1) + " are both named " + traitName); }
                            if (trait.channel == otherTrait.channel) { report.Add(traitName + " and " + otherName + " occupy the same channel"); }
                        }
                    }
                }


                switch (trait.type)
                {
                    case ControlType.Indexed: /////////////////
                        for (int i = 0; i < trait.index.Count; i++)
                        {
                            Index index = trait.index[i];
                            if (index.name == "") { report.Add("Index " + (i + 1) + " [" + traitName + "] has no name"); }
                        }
                        break;

                    case ControlType.Union: /////////////////
                        for (int u = 0; u < trait.union.Count; u++)
                        {
                            Union union = trait.union[u];
                            if (union.name == "") { report.Add("Union " + (u + 1) + " [" + traitName + "] has no name"); }
                        }
                        break;
                }
            }

            if (report.Count == 0)
            {
                return true;
            }
            else
            {
                ErrorReport dialog = new ErrorReport(report);
                return (dialog.ShowDialog() == DialogResult.OK);
            }
        }
    }
}