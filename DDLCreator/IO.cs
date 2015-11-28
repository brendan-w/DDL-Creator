/*
 * class used for interactions between the filesystem and Data.cs
 * using DDL file spec for Firmware Rev 2.01 and above (12-15-03)
 * 
 * public properities
 *     bool saved
 *     
 * public functions:
 *     save(bool forceDialog)
 *     open()
 *     
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace DDLCreator
{

    public static class IO
    {
        public static bool saved = true;
        private static String currentFile = "";
        private static String n = "\n"; //not char because chars add numerically

        //import prereqs
        private enum lineState { DONE, NOKEY, NOVALUE };
        private enum valueType { YESNO, INDEX, NUMBER, STRING, CONTROLTYPE, SIZE, UNION };
        //(DO NOT CHANGE ORDER)//                        0         1       2         3         4       5         6        7        8         9         10         11        12          13        14     15
        private static readonly String[] keyWords = { "device", "trait", "type", "channel", "size", "invert", "xaxis", "yaxis", "black", "bovalue", "master", "default", "maximum", "minimum", "index", "end" };
        private static readonly String[] brands = { "ABSTRACT LIGHTING", "AC LIGHTING", "ACME", "ACME EFFECTS", "ALKALITE", "ALTMAN", "AMERICAN DJ", "ANTARI", "APOLLO", "ARTIC LIGHTING", "ARTICULIGHT", "AURORAE", "BIG DIPPER", "BLIZZARD LIGHTING", "CHAUVET", "CHROMA-Q", "CITY THEATRICAL", "CLAY PAKY", "COEF", "COEMAR", "COLOR KINETICS", "COLORKEY", "COLORNITE", "DELIYA", "DEVINE LIGHTING", "DISCOTECH", "DIVERSITRONICS", "DTS", "ELATION", "ELATION PRO", "ELATION PROFESSIONAL", "ELEKTRALITE", "ELIMINATOR LIGHTING", "ESP", "ETC", "EXELL", "F.A.L", "FINE ART", "FUTURELIGHT", "G-LITES", "GENERAL", "GENI ELECTRONICS", "GENIUS", "GERMAN LIGHT PRODUCTIONS", "G-LITES", "GRIVEN", "GUANG XI OLP TRADING LTD", "HIGH END SYSTEMS", "HIGH TECH LIGHTING", "HQ POWER", "HUBBELL", "IRIDEON", "IRRADIANT", "JB LIGTHING", "JEM", "JMARK", "KLS LIGHTING", "LASER WORLD", "LEMAITRE", "LIGHTWAVE RESEARCH", "LOOK SOLUTIONS", "LUMI PROFESSIONAL", "LYTE QUEST", "MAD LIGHTING", "MARTIN", "MARTIN PRO A.S", "MBT", "MDG", "MDI", "MEGA-LITE", "METEOR", "MICROH", "MOBOLAZER", "MOONLIGHT ILLUMINATION", "MORPHEUS LIGHTS", "MOVITEC", "MULTIFORM", "NESS SHOW PRO", "OMARTE", "OMNISISTEM", "OPTIMA LIGHTING", "PASSPORT LIGHTING", "PEARL RIVER", "PIXEL RANGE", "PR_LIGHTING", "PRAGMATECH", "PRISM PROJECTION", "PROEL", "Programmi and Sistemi Luce", "PULSAR", "QUASAR", "ROBE", "ROSCO", "SEACHANGER STUDIO", "SGM TECHNOLOGY", "SHINP", "SHOWCO", "SILVER STAR", "SLS", "SPACE CANNON", "SRL", "STAGE APE", "STELLAR LABS", "STUDIO DUE", "TAS", "TECHNI-LUX", "THUNDER BLAST", "TPR ENTERPRISES", "TRACOMAN", "ULTRATEC-LEMAITRE", "VARI LITE", "VENUE LIGHTING EFFECTS", "VISUAL EFFECTS (VEI)", "VRL", "WIEDAMARK", "WILDFIRE", "WYBRON", "YORKVILLE", "TMB", "SYNCROLITE", "JANDS", "ACT ONE", "TIMES SQUARE", "PRG" };

        private class KeyWord
        {
            public int index = -1; //index of keyword in keyword array
            public int pos = -1; //position index of keyword in given line
            public bool found() { return (index != -1); }
        }

        //Save/Open Functions//////////////////////////////////////////////////////////////

        public static void save(bool forceDialog)
        {
            DialogResult dialogResult = DialogResult.OK;

            if ((currentFile == "") || forceDialog)
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.InitialDirectory = currentFile;
                if (currentFile == "") { saveFile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory; }
                saveFile.Filter = "DDL Files (.ddl)|*.ddl";
                saveFile.FilterIndex = 1;

                dialogResult = saveFile.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    currentFile = saveFile.FileName;
                }
            }

            if (dialogResult == DialogResult.OK)
            {
                //write file
                String DDL = export();
                try
                {
                    File.WriteAllText(currentFile, DDL);
                    saved = true;
                }
                catch (IOException e)
                {
                    String message = "An IO Error occurred:" + n + n + e.Message + n + n + e.Data;
                    MessageBox.Show(message, "IO Error", MessageBoxButtons.OK);
                }
            }
        }

        public static void open()
        {
            DialogResult dialogResult = DialogResult.Yes;

            if (!saved)
            {
                dialogResult = MessageBox.Show("You have unsaved data. Are you sure you want to open a different file?", "Open?", MessageBoxButtons.YesNo);
            }

            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.InitialDirectory = currentFile;
                if (currentFile == "") { openFile.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory; }
                openFile.Filter = "DDL Files (.ddl)|*.ddl";
                openFile.FilterIndex = 1;

                dialogResult = openFile.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    currentFile = openFile.FileName;
                }
            }

            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    String DDL = File.ReadAllText(currentFile);
                    if (!import(DDL))
                    {
                        MessageBox.Show("Some errors were found in this DDL. Please check the data for accuracy", "DDL Error", MessageBoxButtons.OK);
                    }
                    saved = true;
                }
                catch (IOException e)
                {
                    String message = "An IO Error occurred:" + n + n + e.Message + n + n + e.Data;
                    MessageBox.Show(message, "IO Error", MessageBoxButtons.OK);
                }
            }
        }


        //Internal Functions//////////////////////////////////////////////////////////////


        /* 
         *  function for compiling information stored in Data.Traits
         *  returns string of finished DDL
         */
        private static String export()
        {
            String DDL = "";

            //Header
            DDL += ";*************************************************" + n;
            DDL += ";      Fixture: " + Data.fixture + n;
            DDL += ";      Brand: " + Data.manufacturer + n;
            DDL += ";      Channels: " + maxChannel().ToString() + n;
            DDL += ";      Notes: " + Data.notes + n;
            DDL += ";      Created by " + Data.creator + n;
            DDL += ";      Created on " + DateTime.Now.ToString("M/d/yyyy") + n;
            DDL += ";      Made using DDL Creator (TM)" + n;
            DDL += ";      (c)2008-2013 WindWorks Design" + n;
            DDL += ";      www.windworksdesign.com" + n;
            DDL += ";*************************************************" + n + n;

            //Device name
            DDL += "Device " + Data.fixture + n + n;

            //Trait definitions
            for (int t = 0; t < Data.traits.Count; t++)
            {
                Trait trait = Data.traits[t];

                switch (trait.type)
                {
                    case ControlType.Continuous: /////////////////
                        DDL += "           Trait " + trait.name + n;
                        DDL += "            Type " + trait.type.ToString() + n;
                        DDL += "         Channel " + trait.channel.ToString() + n;
                        DDL += "            Size " + sizeToString(trait.size) + n;
                        DDL += "          Invert " + boolToString(trait.invert) + n;
                        DDL += "           XAxis " + boolToString(trait.x) + n;
                        DDL += "           YAxis " + boolToString(trait.y) + n;
                        DDL += "           Black " + boolToString(trait.black) + n;
                        DDL += "         BoValue " + trait.blackValue.ToString() + n;
                        DDL += "          Master " + boolToString(trait.grand) + n;
                        DDL += "         Default " + trait.defaultValue.ToString() + n;
                        DDL += "         Maximum " + trait.max.ToString() + n;
                        DDL += "         Minimum " + trait.min.ToString() + n;
                        break;

                    case ControlType.Indexed: /////////////////
                        DDL += "           Trait " + trait.name + n;
                        DDL += "            Type " + trait.type.ToString() + n;
                        DDL += "         Channel " + trait.channel.ToString() + n;
                        DDL += "           Black " + boolToString(trait.black) + n;
                        DDL += "         BoValue " + trait.blackValue.ToString() + n;
                        DDL += "         Default " + trait.defaultValue.ToString() + n;
                        for (int i = 0; i < trait.index.Count; i++)
                        {
                            DDL += "           Index " + trait.index[i].name + "," + trait.index[i].value + n;
                        }
                        break;

                    case ControlType.Union: /////////////////
                        DDL += "           Trait " + trait.name + n;
                        DDL += "            Type " + trait.type.ToString() + n;
                        DDL += "         Channel " + trait.channel.ToString() + n;
                        DDL += "           Black " + boolToString(trait.black) + n;
                        DDL += "         BoValue " + trait.blackValue.ToString() + n;
                        DDL += "         Default " + trait.defaultValue.ToString() + n;
                        for (int u = 0; u < trait.union.Count; u++)
                        {
                            Union union = trait.union[u];
                            switch (union.type)
                            {
                                case ControlType.Continuous:
                                    DDL += n;
                                    DDL += "           Trait " + union.name + n;
                                    DDL += "            Type Continuous" + n;
                                    DDL += "         Maximum " + union.max.ToString() + n;
                                    DDL += "         Minimum " + union.min.ToString() + n;
                                    break;

                                case ControlType.Indexed:
                                    //create new sub-type header based on previous entry
                                    if ((u == 0) || (trait.union[u - 1].type == ControlType.Continuous))
                                    {
                                        DDL += n;
                                        DDL += "           Trait " + trait.name + n;
                                        DDL += "            Type Indexed" + n;
                                    }
                                    DDL += "           Index " + union.name + "," + union.min + n;
                                    break;
                            }
                        }
                        DDL += n;
                        DDL += "           Trait " + trait.name + n;
                        DDL += "            Type EndUnion" + n;

                        break;
                } //type switch
                DDL += n + n;
            } //trait loop

            DDL += "End";

            return DDL;
        }


        /*
         * ok, this is going to look a lot bigger/more involved than neccessary,
         * but I wanted this importer to withstand a wide variety of inputs and errors
         * 
         * 
         * 
         * function for parsing DDL text into Trait objects
         * WARNING! all existing information in Data.Traits will be overwritten!
         * returns true if parse was succesful, false if problems
         */
        private static bool import(String DDL)
        {
            bool problem = false;
            Data.deleteAll(); //dump the old data

            //pre-process line feeds, spaces, and extranious characters
            DDL = DDL.Replace('\r', '\n');
            trim(ref DDL, '\n');
            filterNPC(ref DDL, ' ');
            trim(ref DDL, ' ');
            DDL = DDL.Replace(" \n", "\n");
            DDL = DDL.Replace("\n ", "\n");
            trim(ref DDL, '\n');

            //split by lines to sort comments vs code
            String[] rawLines = DDL.Split('\n');
            List<String> header = new List<String>();
            List<Line> lines = new List<Line>();

            for (int i = 0; i < rawLines.Length; i++)
            {
                if (rawLines[i].Contains(";")) { header.Add(rawLines[i]); }
                else { lines.Add(new Line(rawLines[i])); }
            }

            parseHeader(header); //extract header details
            fixLines(lines, ref problem); //pre-process Line objects and try to fix NOVALUE and NOKEY errors
            buildUnions(lines); //build union objects into single lines
            buildTraits(lines); //build lines into traits
            conform(ref problem); //check finished Traits for errors (clamp values, truncate names)

            return !problem;
        }



        /*
         * class for parsing a single line of a DDL
         * attempts to find keywords first, then extracts values of the appropriate type
         * if no keyword is found, value will be extracted if possible
         */
        private class Line
        {
            //status flags
            public lineState state;
            public valueType type;

            //contents
            public String original;
            public KeyWord keyword;

            //all posible value types
            public bool boolValue;
            public int intValue;
            public String stringValue;
            public ControlType typeValue;
            public ControlSize sizeValue;
            public Index indexValue;
            public Union unionValue; //used & built externally by buildUnions();



            public Line() //empty line constructor (used by buildUnions() to construct union lines)
            {
                original = "";
                state = lineState.DONE;
                keyword = new KeyWord();
            }

            public Line(String line) //text parsing constructor
            {
                original = line;
                keyword = getKey(line, keyWords);

                if (keyword.found())
                {
                    String right = line.Remove(0, keyword.pos + keyWords[keyword.index].Length);

                    if (getValue(right, keyword.index)) //look to the right of the keyword for the value
                    {
                        state = lineState.DONE;
                    }
                    else //nothin' there
                    {
                        state = lineState.NOVALUE;
                    }
                }
                else //no keyword
                {
                    state = lineState.NOKEY;
                    findValue(line); //try to detect a value
                }

            }


            //finds and saves values according to the keyword found
            private bool getValue(String line, int k)
            {
                //search for value (in order of frequency)
                bool found = false;

                if (k == 14)
                {
                    type = valueType.INDEX;
                    found = getINDEX(line);
                }
                else if ((k == 5) || (k == 6) || (k == 7) || (k == 8) || (k == 10))
                {
                    type = valueType.YESNO;
                    found = getYESNO(line);
                }
                else if ((k == 3) || (k == 9) || (k == 11) || (k == 12) || (k == 13))
                {
                    type = valueType.NUMBER;
                    found = getNUMBER(line);
                }
                else if (k == 2)
                {
                    type = valueType.CONTROLTYPE;
                    found = getCONTROLTYPE(line);
                }
                else if (k == 4)
                {
                    type = valueType.SIZE;
                    found = getSIZE(line);
                }
                else if (k == 15) //END
                {
                    type = valueType.STRING;
                    found = true;
                }
                else
                {
                    type = valueType.STRING;
                    found = getSTRING(line);
                }
                
                return found;
            }

            //looks for values by trial and error (no keyword was found)
            private void findValue(String line)
            {
                //in order of most strict to least strict
                if (getINDEX(line)) { type = valueType.INDEX; }
                else if (getYESNO(line)) { type = valueType.YESNO; }
                else if (getCONTROLTYPE(line)) { type = valueType.CONTROLTYPE; }
                else if (getSIZE(line)) { type = valueType.SIZE; }
                else if (getNUMBER(line)) { type = valueType.NUMBER; }
                else { type = valueType.STRING; getSTRING(line); } //if all else fails
            }

            
            //Value getters (return bool for success)///////////////////////////

            private bool getINDEX(String line)
            {
                trim(ref line, ' ');
                String[] parts = line.Split(',');
                if (parts.Length > 1)
                {
                    indexValue = new Index();
                    indexValue.name = parts[0];
                    filterNN(ref parts[1]);
                    return int.TryParse(parts[1], out indexValue.value);
                }
                else
                {
                    return false;
                }
            }

            private bool getYESNO(String line)
            {
                String[] options = { "yes", "no" };
                KeyWord keyValue = getKey(line, options);
                switch (keyValue.index)
                {
                    case 0:
                        boolValue = true;
                        break;
                    case 1:
                        boolValue = false;
                        break;
                }
                return keyValue.found();
            }

            private bool getNUMBER(String line)
            {
                filterNN(ref line);
                return int.TryParse(line, out intValue);
            }

            private bool getCONTROLTYPE(String line)
            {
                String[] options = { "continuous", "indexed", "union", "endunion" };
                KeyWord keyValue = getKey(line, options);
                switch (keyValue.index)
                {
                    case 0:
                        typeValue = ControlType.Continuous;
                        break;
                    case 1:
                        typeValue = ControlType.Indexed;
                        break;
                    case 2:
                        typeValue = ControlType.Union;
                        break;
                    case 3:
                        typeValue = ControlType.EndUnion;
                        break;
                }
                return keyValue.found();
            }

            private bool getSIZE(String line)
            {
                String[] options = { "8bit", "16bit" };
                KeyWord keyValue = getKey(line, options);
                switch (keyValue.index)
                {
                    case 0:
                        sizeValue = ControlSize.Bit8;
                        break;
                    case 1:
                        sizeValue = ControlSize.Bit16;
                        break;
                }
                return keyValue.found();
            }

            private bool getSTRING(String line)
            {
                trim(ref line, ' ');
                stringValue = line;
                return (line.Length > 0);
            }

        }//end Line class


        //Import functions//////////////////////////////////////////////////////////////


        //check all trait/index/union objects for missing/bad data
        private static void conform(ref bool problem)
        {
            //basics
            clampName(ref Data.fixture, 16, ref problem);

            //traits
            for (int i = 0; i < Data.traits.Count; i++)
            {
                Trait trait = Data.traits[i];

                clampName(ref trait.name, 8, ref problem);
                clampInt(ref trait.channel, 1, 256, ref problem);
                clampInt(ref trait.defaultValue, 0, 255, ref problem);
                clampInt(ref trait.blackValue, 0, 255, ref problem);

                if (trait.size == ControlSize.Bit8)
                {
                    clampInt(ref trait.min, 0, 255, ref problem);
                    clampInt(ref trait.max, 0, 255, ref problem);
                }
                else if (trait.size == ControlSize.Bit16)
                {
                    clampInt(ref trait.min, 0, 65535, ref problem);
                    clampInt(ref trait.max, 0, 65535, ref problem);
                }

                //indexes
                for (int k = 0; k < trait.index.Count; k++)
                {
                    Index index = trait.index[k];
                    clampName(ref index.name, 8, ref problem);
                    clampInt(ref index.value, 0, 255, ref problem);
                }

                //unions
                for (int k = 0; k < trait.union.Count; k++)
                {
                    Union union = trait.union[k];
                    clampName(ref union.name, 8, ref problem);
                    clampInt(ref union.min, 0, 255, ref problem);
                    clampInt(ref union.max, 0, 255, ref problem);
                }

            }
        } //end conform()


        //a trait is considered complete when a new line has a keyword that has already been loaded
        private static void buildTraits(List<Line> lines)
        {
            bool[] loadStates = new bool[16]; //keeps track of which properties have already occured
            List<Line> extraKeys = new List<Line>();
            List<Line> extraValues = new List<Line>();
            Line line = null;
            Trait trait = new Trait();

            while (lines.Count > 0) //while there's another line
            {
                line = lines[0];

                if (line.state == lineState.DONE)
                {

                    if ((line.type != valueType.INDEX) && (line.type != valueType.UNION)) //unions and indexes bypass the end trait detection
                    {
                        int keyword = line.keyword.index;

                        //block recognition
                        if (loadStates[keyword] || (keyword == 15)) //duplicate keyword reached (or END), submit trait and reset
                        {
                            //TODO see if holes can be plugged with extraKeys and extraValues

                            //submit the trait
                            Data.traits.Add(trait);
                            trait = new Trait();
                            //reset load states
                            for (int i = 0; i < loadStates.Length; i++)
                            {
                                loadStates[i] = false;
                            }
                        }

                        loadStates[keyword] = true;
                    }

                    //load the current line
                    loadLine(trait, line);
                }
                else if(line.state == lineState.NOVALUE)
                {
                    extraKeys.Add(line);
                }
                else if (line.state == lineState.NOKEY)
                {
                    if (line.type == valueType.INDEX) { loadLine(trait, line); }
                    else { extraValues.Add(line); }
                }

                lines.Remove(line);
            }
        } //end buildTraits()


        //ecapsulate union information in artificial lines containing Union objects
        private static void buildUnions(List<Line> lines)
        {
            bool[] loadStates = new bool[16]; //keeps track of which properties have already occured

            bool readyUnion = false; //union header has been seen (offsets block recognition by 1, leaves union headers, destroys EndUnion footer)
            bool inUnion = false; //first keyword duplication has been reached, begin submitting data
            bool createdLine = false; //lets the line remover know if the loop counter needs decrementing
            Line line = null; //current line
            Union union = new Union(); //currently building union

            for (int i = 0; i < lines.Count; i++)
            {
                line = lines[i];
                createdLine = false;

                //Debug.WriteLine(line.original + "   =   " + inUnion.ToString());

                //set as ready if these states are detected
                if (line.typeValue == ControlType.Union) { readyUnion = true; }
                else if (line.typeValue == ControlType.EndUnion) { readyUnion = true; }

                //block recognition
                if ((line.keyword.found() && loadStates[line.keyword.index]) || (line.keyword.index == 15))
                {
                    //this keyword has been loaded before, check and submit whatever was loaded

                    if (readyUnion && !inUnion) //switch inUnion ON
                    {
                        readyUnion = false;
                        inUnion = true;
                    }
                    else if (readyUnion && inUnion) //switch inUnion OFF
                    {
                        readyUnion = false;
                        inUnion = false;
                    }

                    //submit a continuous union block
                    if (inUnion && (union.type == ControlType.Continuous))
                    {
                        Line newLine = new Line();
                        newLine.unionValue = union;
                        union = new Union();
                        newLine.type = valueType.UNION;
                        //Debug.WriteLine("[INSERT CONT]");
                        lines.Insert(i, newLine);
                        createdLine = true;
                    }

                    //always clear loadStates
                    for (int k = 0; k < loadStates.Length; k++)
                    {
                        loadStates[k] = false;
                    }
                    //ready for next block
                }

                //store applicable keywords in the corresponding union location
                switch (line.keyword.index)
                {
                    case 1:
                        union.name = line.stringValue;
                        loadStates[1] = true;//mark this property as loaded
                        break;
                    case 2:
                        union.type = line.typeValue;
                        loadStates[2] = true;//mark this property as loaded
                        break;
                    case 12:
                        union.max = line.intValue;
                        loadStates[12] = true;//mark this property as loaded
                        break;
                    case 13:
                        union.min = line.intValue;
                        loadStates[13] = true;//mark this property as loaded
                        break;
                    case 14:
                        if (inUnion) //index in a union, submit on the spot (all in one line)
                        { 
                            //Debug.WriteLine("[INSERT INDEX]");
                            Line newLine = new Line();
                            Union indexUnion = new Union();
                            indexUnion.name = line.indexValue.name;
                            indexUnion.min = line.indexValue.value;
                            indexUnion.type = ControlType.Indexed;
                            newLine.unionValue = indexUnion;
                            newLine.type = valueType.UNION;
                            lines.Insert(i, newLine);
                            createdLine = true;
                        }
                        break;
                }
                    
                if (inUnion)
                {
                    lines.Remove(line); //always remove union lines (since they've been converted)
                    if (!createdLine) { i--; } //don't need to decrement if a lines already been added
                }
            }//end line loop

        } //end buildUnions()


        //pre-processes Line objects and tries to fix NOVALUE and NOKEY errors
        private static void fixLines(List<Line> lines, ref bool problem)
        {
            bool endFound = false;

            for (int i = 0; i < lines.Count; i++)
            {
                Line current = lines[i];
                Line prev = null;
                Line next = null;
                if (i - 1 >= 0) { prev = lines[i - 1]; }
                if (i + 1 < lines.Count) { next = lines[i + 1]; }

                if (current.state == lineState.NOVALUE)
                {
                    problem = true;
                    if ((next != null) && (next.state == lineState.NOKEY) && (current.type == next.type))
                    {
                        //concat the lines
                        switch (current.type)
                        {
                            case valueType.INDEX:
                                current.indexValue = next.indexValue;
                                break;
                            case valueType.YESNO:
                                current.boolValue = next.boolValue;
                                break;
                            case valueType.NUMBER:
                                current.intValue = next.intValue;
                                break;
                            case valueType.CONTROLTYPE:
                                current.typeValue = next.typeValue;
                                break;
                            case valueType.SIZE:
                                current.sizeValue = next.sizeValue;
                                break;
                            case valueType.STRING:
                                current.stringValue = next.stringValue;
                                break;
                        }
                        current.state = lineState.DONE;
                        lines.Remove(next);
                    }
                }
                else if (current.state == lineState.NOKEY)
                {
                    problem = true;
                    if ((current.type == valueType.STRING) && (prev != null) && (prev.type == valueType.STRING))
                    {
                        //concat the lines
                        prev.stringValue += " " + current.stringValue;
                        lines.Remove(current);
                        i--;
                    }
                }


                if ((current.state == lineState.DONE) && (current.keyword.index == 0)) //catch device name
                {
                    Data.fixture = current.stringValue;
                    lines.Remove(current);
                    i--;
                }
                else if ((current.state == lineState.DONE) && (current.keyword.index == 15)) //catch END tag
                {
                    endFound = true;
                }
            } //end line loop


            //fix missing end tag (causes recognition problems on the last trait block)
            if (!endFound)
            {
                lines.Add(new Line(keyWords[15]));
            }

        } //end fixLines()


        //copies line data into the appropriate property of the given trait
        //called by buildTraits()
        private static void loadLine(Trait trait, Line line)
        {
            if (line.type == valueType.INDEX)
            {
                trait.index.Add(line.indexValue);
            }
            else if (line.type == valueType.UNION)
            {
                trait.union.Add(line.unionValue);
            }
            else //discrete value
            {
                switch (line.keyword.index)
                {
                    case 1:
                        trait.name = line.stringValue;
                        break;
                    case 2:
                        trait.type = line.typeValue;
                        break;
                    case 3:
                        trait.channel = line.intValue;
                        break;
                    case 4:
                        trait.size = line.sizeValue;
                        break;
                    case 5:
                        trait.invert = line.boolValue;
                        break;
                    case 6:
                        trait.x = line.boolValue;
                        break;
                    case 7:
                        trait.y = line.boolValue;
                        break;
                    case 8:
                        trait.black = line.boolValue;
                        break;
                    case 9:
                        trait.blackValue = line.intValue;
                        break;
                    case 10:
                        trait.grand = line.boolValue;
                        break;
                    case 11:
                        trait.defaultValue = line.intValue;
                        break;
                    case 12:
                        trait.max = line.intValue;
                        break;
                    case 13:
                        trait.min = line.intValue;
                        break;
                }
            }
        } //end loadLine()


        //extracts data from DDL header
        private static void parseHeader(List<String> header)
        {
            String lowHeader = "";
            String strHeader = "";
            int index = 0;

            bool foundBrand = false;
            bool foundCreator = false;
            bool foundNotes = false;

            for (int i = 0; i < header.Count; i++)
            {
                lowHeader = header[i].ToLower();
                strHeader += lowHeader + " ";

                if (!foundBrand)
                {
                    index = lowHeader.IndexOf("brand:");
                    if (index != -1)
                    {
                        foundBrand = true;
                        String brand = header[i].Remove(0, index + 6);
                        trim(ref brand, ' ');
                        Data.manufacturer = brand;
                    }

                    index = lowHeader.IndexOf("[manuf]");
                    if (index != -1)
                    {
                        foundBrand = true;
                        String brand = header[i].Remove(0, index + 7);
                        trim(ref brand, ' ');
                        Data.manufacturer = brand;
                    }
                }

                if (!foundCreator)
                {
                    index = lowHeader.IndexOf("by ");
                    if (index != -1)
                    {
                        foundCreator = true;
                        String creator = header[i].Remove(0, index + 3);
                        trim(ref creator, ' ');
                        Data.creator = creator;
                    }
                }

                if (!foundNotes)
                {
                    index = lowHeader.IndexOf("notes:");
                    if (index != -1)
                    {
                        foundNotes = true;
                        String notes = header[i].Remove(0, index + 6);
                        trim(ref notes, ' ');
                        Data.notes = notes;
                    }
                }

            }

            //if brand wasn't caught, search for common ones
            int b = 0;
            while (!foundBrand && (b < brands.Length))
            {
                if (strHeader.Contains(brands[b].ToLower()))
                {
                    foundBrand = true;
                    Data.manufacturer = brands[b];
                }
                b++;
            }


        } //end parseHeader()


        //IO Support functions//////////////////////////////////////////////////////////////

        //ensures that a string is present, and that it is not too long
        private static void clampName(ref String name, int maxLen, ref bool problem)
        {
            if ((name == null) || (name == "")) //no trait name
            {
                name = "[NF]";
                problem = true;
            }
            else if (name.Length > maxLen) //trait name too long
            {
                name = name.Substring(0, maxLen);
                problem = true;
            }
        }

        //clamps a given value within min and max
        private static void clampInt(ref int value, int min, int max, ref bool problem)
        {
            if (value < min) //channel too low
            {
                value = min;
                problem = true;
            }
            else if (value > max) //channel too high
            {
                value = max;
                problem = true;
            }
        }

        //finds the earliest occurence of a matching keyword
        private static KeyWord getKey(String line, String[] options)
        {
            //check against all keywords
            line = line.ToLower();
            KeyWord answer = new KeyWord();
            answer.pos = line.Length; //looking for lowest, preset at max
            for (int i = 0; i < options.Length; i++)
            {
                int search = find(line, options[i]);
                if ((search != -1) && (search < answer.pos))
                {
                    answer.pos = search;
                    answer.index = i;
                }
            }
            return answer;
        }

        //checks for presence of string in string (allows for slight spelling errors) returns index of occurence
        private static int find(String haystack, String needle)
        {
            int i = haystack.IndexOf(needle);
            if (i == -1)
            {
                //TODO check for spelling errors (edit distance of 1 or 2 is acceptable)
            }
            return i;
        }

        //Levenshtein Distance algorithm adapted from http://en.wikibooks.org/wiki/Algorithm_Implementation/Strings/Levenshtein_distance#C.23
        private static int LevenshteinDistance(String source, String target)
        {
            if (String.IsNullOrEmpty(source))
            {
                if (String.IsNullOrEmpty(target)) return 0;
                return target.Length;
            }
            if (String.IsNullOrEmpty(target)) return source.Length;

            if (source.Length > target.Length)
            {
                String temp = target;
                target = source;
                source = temp;
            }

            int m = target.Length;
            int n = source.Length;
            int[,] distance = new int[2, m + 1];
            // Initialize the distance 'matrix'
            for (int j = 1; j <= m; j++) { distance[0, j] = j; }

            int currentRow = 0;
            for (int i = 1; i <= n; ++i)
            {
                currentRow = i & 1;
                distance[currentRow, 0] = i;
                int previousRow = currentRow ^ 1;
                for (int j = 1; j <= m; j++)
                {
                    int cost = (target[j - 1] == source[i - 1] ? 0 : 1);
                    distance[currentRow, j] = Math.Min(Math.Min(
                                            distance[previousRow, j] + 1,
                                            distance[currentRow, j - 1] + 1),
                                            distance[previousRow, j - 1] + cost);
                }
            }
            return distance[currentRow, m];
        }

        //condense blocks of characters down to one
        private static void trim(ref String text, char character)
        {
            String c = character.ToString();
            String twoC = c + c;

            while (text.Contains(twoC))
            {
                text = text.Replace(twoC, c);
            }
            //trim edges (causes extraneous elements with .Split())
            if (text.StartsWith(c)) { text = text.Remove(0, 1); }
            if (text.EndsWith(c)) { text = text.Remove(text.Length - 1, 1); }
        }

        //removes all non-numeric characters
        private static void filterNN(ref String line)
        {
            for (char c = '\u0021'; c <= '\u002F'; c++) { line = line.Replace(c, ' '); }
            for (char c = '\u003A'; c <= '\u00FF'; c++) { line = line.Replace(c, ' '); }
        }

        //replace non-printable characters with spaces (even newlines)
        private static void filterNPC(ref String text, char newC)
        {
            for (char c = '\u0000'; c <= '\u0009'; c++) { text = text.Replace(c, newC); }
            for (char c = '\u000B'; c <= '\u001F'; c++) { text = text.Replace(c, newC); }
            text.Replace('\u007F', newC);
        }

        private static int maxChannel()
        {
            int max = 0;
            for (int i = 0; i < Data.traits.Count; i++)
            {
                if (Data.traits[i].channel > max)
                {
                    max = Data.traits[i].channel;
                }
            }
            return max;
        }

        private static String boolToString(bool val)
        {
            if (val) { return "Yes"; }
            else { return "No"; }
        }

        private static String sizeToString(ControlSize val)
        {
            if (val == ControlSize.Bit16) { return "16Bit"; }
            else { return "8Bit"; }
        }

    }//end IO class

}