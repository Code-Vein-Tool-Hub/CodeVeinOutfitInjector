using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UAssetAPI;
using System.IO;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace CodeVeinOutfitInjector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CB_CSet1.Items.AddRange(Enum.GetNames(typeof(ColorSet)));
            CB_CSet2.Items.AddRange(Enum.GetNames(typeof(ColorSet)));
            CB_CSet3.Items.AddRange(Enum.GetNames(typeof(ColorSet)));
            CB_CSet4.Items.AddRange(Enum.GetNames(typeof(ColorSet)));
            CB_CSet5.Items.AddRange(Enum.GetNames(typeof(ColorSet)));
            CB_CSet6.Items.AddRange(Enum.GetNames(typeof(ColorSet)));
            CB_CSet7.Items.AddRange(Enum.GetNames(typeof(ColorSet)));
            Setup();
        }
        UAsset asset;
        Dictionary<ColorSet, List<string>> palettes = new Dictionary<ColorSet, List<string>>();
        bool Female;

        private void Setup()
        {
            palettes.Add(ColorSet.StandardColor_Gray1, Enum.GetNames(typeof(StandardColor_Gray1)).ToList());
            palettes.Add(ColorSet.PastelColor1, Enum.GetNames(typeof(PastelColor1)).ToList());
            palettes.Add(ColorSet.PastelColor2, Enum.GetNames(typeof(PastelColor2)).ToList());
            palettes.Add(ColorSet.PastelColor3, Enum.GetNames(typeof(PastelColor3)).ToList());
            palettes.Add(ColorSet.PastelColor4, Enum.GetNames(typeof(PastelColor4)).ToList());
            palettes.Add(ColorSet.StandardColor_Red2, Enum.GetNames(typeof(StandardColor_Red2)).ToList());
            palettes.Add(ColorSet.StandardColor_Red3, Enum.GetNames(typeof(StandardColor_Red3)).ToList());
            palettes.Add(ColorSet.StandardColor_Yellow1, Enum.GetNames(typeof(StandardColor_Yellow1)).ToList());
            palettes.Add(ColorSet.StandardColor_Yellow2, Enum.GetNames(typeof(StandardColor_Yellow2)).ToList());
            palettes.Add(ColorSet.StandardColor_Yellow3, Enum.GetNames(typeof(StandardColor_Yellow3)).ToList());
            palettes.Add(ColorSet.StandardColor_Green1, Enum.GetNames(typeof(StandardColor_Green1)).ToList());
            palettes.Add(ColorSet.StandardColor_Blue1, Enum.GetNames(typeof(StandardColor_Blue1)).ToList());
            palettes.Add(ColorSet.StandardColor_Blue2, Enum.GetNames(typeof(StandardColor_Blue2)).ToList());
            palettes.Add(ColorSet.StandardColor_Blue3, Enum.GetNames(typeof(StandardColor_Blue3)).ToList());
            palettes.Add(ColorSet.StandardColor_Violet1, Enum.GetNames(typeof(StandardColor_Violet1)).ToList());
            palettes.Add(ColorSet.StandardColor_Violet2, Enum.GetNames(typeof(StandardColor_Violet2)).ToList());
            palettes.Add(ColorSet.StandardColor_Violet3, Enum.GetNames(typeof(StandardColor_Violet3)).ToList());
            palettes.Add(ColorSet.StandardColor_Pink1, Enum.GetNames(typeof(StandardColor_Pink1)).ToList());
            palettes.Add(ColorSet.StandardColor_Red1, Enum.GetNames(typeof(StandardColor_Red1)).ToList());
            palettes.Add(ColorSet.StandardColor_Gray2, Enum.GetNames(typeof(StandardColor_Gray2)).ToList());
            palettes.Add(ColorSet.StandardColor_Gray3, Enum.GetNames(typeof(StandardColor_Gray3)).ToList());
            palettes.Add(ColorSet.InnerColorA, Enum.GetNames(typeof(InnerColorA)).ToList());
            palettes.Add(ColorSet.InnerColorB, Enum.GetNames(typeof(InnerColorB)).ToList());
            palettes.Add(ColorSet.InnerColorC, Enum.GetNames(typeof(InnerColorC)).ToList());
            palettes.Add(ColorSet.InnerColorD, Enum.GetNames(typeof(InnerColorD)).ToList());
            palettes.Add(ColorSet.InnerColorE, Enum.GetNames(typeof(InnerColorE)).ToList());
            palettes.Add(ColorSet.InnerColorF, Enum.GetNames(typeof(InnerColorF)).ToList());
            palettes.Add(ColorSet.InnerColorG, Enum.GetNames(typeof(InnerColorG)).ToList());
            palettes.Add(ColorSet.PresetColor1, Enum.GetNames(typeof(PresetColor1)).ToList());
            palettes.Add(ColorSet.PresetColor2, Enum.GetNames(typeof(PresetColor2)).ToList());
            palettes.Add(ColorSet.PresetColor3, Enum.GetNames(typeof(PresetColor3)).ToList());
            palettes.Add(ColorSet.PresetColor4, Enum.GetNames(typeof(PresetColor4)).ToList());
            palettes.Add(ColorSet.PresetColor6, Enum.GetNames(typeof(PresetColor6)).ToList());
            palettes.Add(ColorSet.PresetColor7, Enum.GetNames(typeof(PresetColor7)).ToList());
            palettes.Add(ColorSet.PresetColor8, Enum.GetNames(typeof(PresetColor8)).ToList());
            palettes.Add(ColorSet.PresetColor9, Enum.GetNames(typeof(PresetColor9)).ToList());
            palettes.Add(ColorSet.PresetColor5, Enum.GetNames(typeof(PresetColor5)).ToList());
            palettes.Add(ColorSet.PresetColor10, Enum.GetNames(typeof(PresetColor10)).ToList());
            palettes.Add(ColorSet.PresetColor11, Enum.GetNames(typeof(PresetColor11)).ToList());
            palettes.Add(ColorSet.StandardColor_Red4, Enum.GetNames(typeof(StandardColor_Red4)).ToList());
        }

        private void Open_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "uasset (*.uasset)|*.uasset";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(Path.ChangeExtension(ofd.FileName, ".uexp")))
                        return;
                    ReadFile(ofd.FileName);
                }
            }
        }

        private void PhraseEnum(string inpath)
        {
            asset = new UAsset(UE4Version.VER_UE4_18);
            asset.FilePath = inpath;
            asset.Read(asset.PathToReader(asset.FilePath));
            var colors = (DataTableExport)asset.Exports[0];
            foreach (var palette in colors.Table.Data)
            {
                using (FileStream fs = new FileStream($"Output\\{palette.Name.Value.Value.Replace("(0)", "")}.cs", FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("namespace CodeVeinOutfitInjector");
                    sw.WriteLine("{");
                    sw.WriteLine($"\tpublic enum {palette.Name.Value.Value.Replace("(0)", "")}");
                    sw.WriteLine("\t{");
                    List<UAssetAPI.PropertyTypes.PropertyData> temp = (List<UAssetAPI.PropertyTypes.PropertyData>)palette.RawValue;
                    var temp2 = (UAssetAPI.PropertyTypes.ArrayPropertyData)temp[1];
                    foreach (var color in (UAssetAPI.PropertyTypes.PropertyData[])temp2.RawValue)
                    {
                        var temp3 = ((List<UAssetAPI.PropertyTypes.PropertyData>)color.RawValue)[0];
                        int number = ((FName)temp3.RawValue).Number;
                        string name = ((FName)temp3.RawValue).Value.Value;
                        if (number > 0)
                        {
                            sw.WriteLine($"\t\t{name}_{number},");
                        }
                        else
                        {
                            sw.WriteLine($"\t\t{name},");
                        }
                        
                    }
                    //sw.BaseStream.Position = sw.BaseStream.Position - 3;
                    //sw.WriteLine("\r\n");
                    sw.WriteLine("\t}");
                    sw.WriteLine("}");
                }
            }
        }

        private void ReadFile(string inpath)
        {
            asset = new UAsset(UE4Version.VER_UE4_18);
            asset.FilePath = inpath;
            asset.Read(asset.PathToReader(asset.FilePath));
            if (asset.Exports != null && asset.Exports[0].ObjectName.Value.Value.Contains("DT_InnerList"))
            {
                if (asset.Exports[0].ObjectName.Value.Value == "DT_InnerList_Female")
                    Female = true;
                listBox1.Items.Clear();
                var InnerList = (DataTableExport)asset.Exports[0];
                foreach (var data in InnerList.Table.Data)
                {
                    listBox1.Items.Add(data.Name.Value.Value);
                }
            }
            else
                return;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sdf = new SaveFileDialog())
            {
                sdf.Filter = "uasset (*.uasset)|*.uasset";
                sdf.RestoreDirectory = true;

                if (sdf.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sdf.FileName))
                    {
                        File.Delete(sdf.FileName);
                        File.Delete(Path.ChangeExtension(sdf.FileName, ".uexp"));
                    }
                        
                    //string json = asset.SerializeJson();
                    //asset = UAsset.DeserializeJson(json);
                    SaveFile(sdf.FileName);
                }
            }
        }

        private void SaveFile(string path)
        {
            bool loop = true;
            while (loop)
            {
                loop = false;
                try
                {
                    asset.Write(path);
                    return;
                }
                catch (NameMapOutOfRangeException ex)
                {
                    try
                    {
                        asset.AddNameReference(ex.RequiredName);
                        loop = true;
                    }
                    catch (Exception ex2)
                    {
                        //MessageBox.Show("Failed to save! " + ex2.Message, "Uh oh!");
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Failed to save! " + ex.Message, "Uh oh!");
                }
            }
        }
        
        private void RemoveDupicates()
        {
            bool hasDups = false;
            Dictionary<string, bool> NameMapRef = new Dictionary<string, bool>();
            foreach (FString fString in asset.GetNameMapIndexList())
            {
                if (NameMapRef.ContainsKey(fString.Value))
                {
                    hasDups = true;
                    break;
                }
                NameMapRef.Add(fString.Value, true);
            }
            NameMapRef = null;
        }

        #region Color Sets
        private void CB_CSet1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_C1.Items.Clear();
            CB_C1.Items.AddRange(palettes[(ColorSet)CB_CSet1.SelectedIndex].ToArray());
        }

        private void CB_CSet2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_C2.Items.Clear();
            CB_C2.Items.AddRange(palettes[(ColorSet)CB_CSet2.SelectedIndex].ToArray());
        }

        private void CB_CSet3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_C3.Items.Clear();
            CB_C3.Items.AddRange(palettes[(ColorSet)CB_CSet3.SelectedIndex].ToArray());
        }

        private void CB_CSet4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_C4.Items.Clear();
            CB_C4.Items.AddRange(palettes[(ColorSet)CB_CSet4.SelectedIndex].ToArray());
        }

        private void CB_CSet5_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_C5.Items.Clear();
            CB_C5.Items.AddRange(palettes[(ColorSet)CB_CSet5.SelectedIndex].ToArray());
        }

        private void CB_CSet6_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_C6.Items.Clear();
            CB_C6.Items.AddRange(palettes[(ColorSet)CB_CSet6.SelectedIndex].ToArray());
        }

        private void CB_CSet7_SelectedIndexChanged(object sender, EventArgs e)
        {
            CB_C7.Items.Clear();
            CB_C7.Items.AddRange(palettes[(ColorSet)CB_CSet7.SelectedIndex].ToArray());
        }
        #endregion

        #region checks
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                CB_C1.Enabled = true;
                CB_CSet1.Enabled = true;
            }
            else
            {
                CB_C1.Enabled = false;
                CB_CSet1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                CB_C2.Enabled = true;
                CB_CSet2.Enabled = true;
            }
            else
            {
                CB_C2.Enabled = false;
                CB_CSet2.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                CB_C3.Enabled = true;
                CB_CSet3.Enabled = true;
            }
            else
            {
                CB_C3.Enabled = false;
                CB_CSet3.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                CB_C4.Enabled = true;
                CB_CSet4.Enabled = true;
            }
            else
            {
                CB_C4.Enabled = false;
                CB_CSet4.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                CB_C5.Enabled = true;
                CB_CSet5.Enabled = true;
            }
            else
            {
                CB_C5.Enabled = false;
                CB_CSet5.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                CB_C6.Enabled = true;
                CB_CSet6.Enabled = true;
            }
            else
            {
                CB_C6.Enabled = false;
                CB_CSet6.Enabled = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                CB_C7.Enabled = true;
                CB_CSet7.Enabled = true;
            }
            else
            {
                CB_C7.Enabled = false;
                CB_CSet7.Enabled = false;
            }
        }
        #endregion

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            var Inner = ((DataTableExport)asset.Exports[0]).Table.Data[listBox1.SelectedIndex];
            var Data = (List<UAssetAPI.PropertyTypes.PropertyData>)Inner.RawValue;
            textBox1.Text = Inner.Name.Value.Value;
            for (int i = 0; i < Data.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        TB_Thumbnail.Text = ((FName)((SoftObjectPropertyData)Data[i]).RawValue).Value.Value;
                        break;
                    case 1:
                        TB_Model.Text = ((FName)((SoftObjectPropertyData)Data[i]).RawValue).Value.Value;
                        break;
                    case 2:
                        if (((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value == "None")
                        {
                            checkBox1.Checked = false;
                        }
                        else
                        {
                            checkBox1.Checked = true;
                            CB_CSet1.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value;
                            CB_C1.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[2].RawValue).Value.Value;
                        }
                        break;
                    case 3:
                        if (((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value == "None")
                        {
                            checkBox2.Checked = false;
                        }
                        else
                        {
                            checkBox2.Checked = true;
                            CB_CSet2.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value;
                            CB_C2.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[2].RawValue).Value.Value;
                        }
                        break;
                    case 4:
                        if (((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value == "None")
                        {
                            checkBox3.Checked = false;
                        }
                        else
                        {
                            checkBox3.Checked = true;
                            CB_CSet3.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value;
                            CB_C3.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[2].RawValue).Value.Value;
                        }
                        break;
                    case 5:
                        if (((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value == "None")
                        {
                            checkBox4.Checked = false;
                        }
                        else
                        {
                            checkBox4.Checked = true;
                            CB_CSet4.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value;
                            CB_C4.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[2].RawValue).Value.Value;
                        }
                        break;
                    case 6:
                        if (((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value == "None")
                        {
                            checkBox5.Checked = false;
                        }
                        else
                        {
                            checkBox5.Checked = true;
                            CB_CSet5.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value;
                            CB_C5.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[2].RawValue).Value.Value;
                        }
                        break;
                    case 7:
                        if (((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value == "None")
                        {
                            checkBox6.Checked = false;
                        }
                        else
                        {
                            checkBox6.Checked = true;
                            CB_CSet6.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value;
                            CB_C6.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[2].RawValue).Value.Value;
                        }
                        break;
                    case 8:
                        if (((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value == "None")
                        {
                            checkBox7.Checked = false;
                        }
                        else
                        {
                            checkBox7.Checked = true;
                            CB_CSet7.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[1].RawValue).Value.Value;
                            CB_C7.Text = ((FName)((List<PropertyData>)((StructPropertyData)Data[i]).RawValue)[2].RawValue).Value.Value;
                        }
                        break;
                    case 9:
                        var hidelist = ((PropertyData[])((ArrayPropertyData)Data[i]).RawValue).ToList();
                        listBox2.Items.Clear();
                        TB_HThumbnail.Clear();
                        TB_HName.Clear();
                        foreach (StructPropertyData item in hidelist)
                        {
                            listBox2.Items.Add(((FName)((List<PropertyData>)item.RawValue)[1].RawValue).Value.Value);
                        }
                        break;
                    case 10:
                        break;
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox2.SelectedIndex;
            if (i < 0)
                return;
            var Inner = ((DataTableExport)asset.Exports[0]).Table.Data[listBox1.SelectedIndex];
            var Data = (List<PropertyData>)Inner.RawValue;
            var HideArray = ((PropertyData[])((ArrayPropertyData)Data[9]).RawValue).ToList();

            TB_HThumbnail.Text = ((FName)((List<PropertyData>)HideArray[i].RawValue)[0].RawValue).Value.Value;
            TB_HName.Text = ((FName)((List<PropertyData>)HideArray[i].RawValue)[1].RawValue).Value.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var Inner = ((DataTableExport)asset.Exports[0]).Table.Data[0];
            //var Data = (List<PropertyData>)Inner.RawValue;
            string input = Interaction.InputBox("New Inner", "Inner Base Name", "Inner_").Replace(" ", "");
            StructPropertyData NewInner = new StructPropertyData();
            NewInner.Name = new FName(input);
            NewInner.StructType = PremadeObjects.AvatarCustomizeDataTableInner("List");
            NewInner.Value = new List<PropertyData>();
            NewInner.Value.Add(PremadeObjects.SoftObject("Thumbnail", $"/Game/UserInterface/AvatarCustomize/Textures/Thumbnail/Inner/T_Thumb_{input}.T_Thumb_{input}"));
            NewInner.Value.Add(PremadeObjects.SoftObject("Mesh", $"/Game/Costumes/Inners/{input}/Meshes/SK_{input}.SK_{input}"));
            for (int i = 0; i < 7; i++)
            {
                NewInner.Value.Add(PremadeObjects.Color(i));
            }
            NewInner.Value.Add(PremadeObjects.HideParts());
            NewInner.Value.Add(PremadeObjects.CheckFlag());
            
            //Inner.Name.Value.Value = input;
            ((DataTableExport)asset.Exports[0]).Table.Data.Add(NewInner);
            listBox1.Items.Add(NewInner.Name.Value.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 7)
            {
                ((DataTableExport)asset.Exports[0]).Table.Data.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index < 0)
                return;
            StructPropertyData Inner = ((DataTableExport)asset.Exports[0]).Table.Data[index];
            Inner.Name = new FName(textBox1.Text);
            Inner.Value[0] = PremadeObjects.SoftObject("Thumbnail", TB_Thumbnail.Text);
            Inner.Value[1] = PremadeObjects.SoftObject("Mesh", TB_Model.Text);
            if (checkBox1.Checked)
                Inner.Value[2] = PremadeObjects.Color(0, CB_CSet1.Text, CB_C1.Text);
            else
                Inner.Value[2] = PremadeObjects.Color(0, "None", "None");
            if (checkBox2.Checked)
                Inner.Value[3] = PremadeObjects.Color(1, CB_CSet2.Text, CB_C2.Text);
            else
                Inner.Value[3] = PremadeObjects.Color(1, "None", "None");
            if (checkBox3.Checked)
                Inner.Value[4] = PremadeObjects.Color(2, CB_CSet3.Text, CB_C3.Text);
            else
                Inner.Value[4] = PremadeObjects.Color(2, "None", "None");
            if (checkBox4.Checked)
                Inner.Value[5] = PremadeObjects.Color(3, CB_CSet4.Text, CB_C4.Text);
            else
                Inner.Value[5] = PremadeObjects.Color(3, "None", "None");
            if (checkBox5.Checked)
                Inner.Value[6] = PremadeObjects.Color(4, CB_CSet5.Text, CB_C5.Text);
            else
                Inner.Value[6] = PremadeObjects.Color(4, "None", "None");
            if (checkBox6.Checked)
                Inner.Value[7] = PremadeObjects.Color(5, CB_CSet6.Text, CB_C6.Text);
            else
                Inner.Value[7] = PremadeObjects.Color(5, "None", "None");
            if (checkBox7.Checked)
                Inner.Value[8] = PremadeObjects.Color(6, CB_CSet7.Text, CB_C7.Text);
            else
                Inner.Value[8] = PremadeObjects.Color(6, "None", "None");
            listBox1.Items[index] = textBox1.Text;
        }

        #region Hide Parts
        private void HideAdd_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            ArrayPropertyData HideArray = (ArrayPropertyData)(((DataTableExport)asset.Exports[0]).Table.Data[index]).Value[9];
            List<PropertyData> parts;
            if (HideArray.Value != null)
            {
                parts = ((PropertyData[])HideArray.Value).ToList();
            }
            else
            {
                parts = new List<PropertyData>();
            }
            string input = Interaction.InputBox("New Inner", "Inner Base Name", "HideParts").Replace(" ", "");
            parts.Add(PremadeObjects.HidePartDetail(input));
            listBox2.Items.Add(input);
            HideArray.Value = parts.ToArray();
            (((DataTableExport)asset.Exports[0]).Table.Data[0]).Value[9] = HideArray;
        }

        private void HideRemove_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            int index2 = listBox2.SelectedIndex;
            if (index < 0 || index2 < 0)
                return;
            ArrayPropertyData HideArray = (ArrayPropertyData)(((DataTableExport)asset.Exports[0]).Table.Data[index]).Value[9];
            List<PropertyData> parts;
            if (HideArray.Value != null)
                parts = ((PropertyData[])HideArray.Value).ToList();
            else
                return;
            parts.RemoveAt(index2);
            listBox2.Items.RemoveAt(index2);
            HideArray.Value = parts.ToArray();
            (((DataTableExport)asset.Exports[0]).Table.Data[index]).Value[9] = HideArray;
        }

        private void HideSave_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            int index2 = listBox2.SelectedIndex;
            if (index < 0 || index2 < 0)
                return;
            ArrayPropertyData HideArray = (ArrayPropertyData)(((DataTableExport)asset.Exports[0]).Table.Data[index]).Value[9];
            List<PropertyData> parts;
            if (HideArray.Value != null)
                parts = ((PropertyData[])HideArray.Value).ToList();
            else
                return;
            ((SoftObjectPropertyData)((List<PropertyData>)((StructPropertyData)parts[index2]).Value)[0]).Value = new FName(TB_HThumbnail.Text);
            ((NamePropertyData)((List<PropertyData>)((StructPropertyData)parts[index2]).Value)[1]).Value = new FName(TB_HName.Text);
            HideArray.Value = parts.ToArray();
            (((DataTableExport)asset.Exports[0]).Table.Data[index]).Value[9] = HideArray;
            listBox2.Items[index2] = TB_HName.Text;
        }
        #endregion

        private void button9_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Json file (*.json)|*.json";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StructPropertyData json = JsonConvert.DeserializeObject<StructPropertyData>(File.ReadAllText(ofd.FileName));
                    string name = json.Name.Value.Value;
                    if (listBox1.Items.Contains(name))
                    {
                        ((DataTableExport)asset.Exports[0]).Table.Data[listBox1.Items.IndexOf(name)] = json;
                    }
                    else
                    {
                        ((DataTableExport)asset.Exports[0]).Table.Data.Add(json);
                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index < 0)
                return;
            StructPropertyData Inner = ((DataTableExport)asset.Exports[0]).Table.Data[index];
            string json = JsonConvert.SerializeObject(Inner, Formatting.Indented);
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Json file (*.json)|*.json";
                sfd.RestoreDirectory = true;
                sfd.FileName = textBox1.Text;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, json);
                }
            }
        }
    }
}
