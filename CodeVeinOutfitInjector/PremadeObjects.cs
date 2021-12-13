using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAssetAPI;
using UAssetAPI.PropertyTypes;
using UAssetAPI.StructTypes;

namespace CodeVeinOutfitInjector
{
    internal class PremadeObjects
    {
        public static FName AvatarCustomizeDataTableInner(string Type)
        {
            FName fname = new FName();
            fname.Number = 0;
            fname.Value.Value = $"AvatarCustomizeDataTableInner{Type}";
            return fname;
        }

        public static SoftObjectPropertyData SoftObject(string name, string value)
        {
            SoftObjectPropertyData obj = new SoftObjectPropertyData();
            obj.Name = new FName(name);
            obj.Value = new FName(value);
            return obj;
        }

        public static StructPropertyData Color(int DupIndex, string Palette = "StandardColor_Gray1", string Color = "palette_stg_monotone00")
        {
            StructPropertyData obj = new StructPropertyData();
            obj.Name = new FName("Colors");
            obj.StructType = AvatarCustomizeDataTableInner("Color");
            obj.Value = new List<PropertyData>();
            obj.Value.Add(new BoolPropertyData() { Name = new FName("IsSpecialColor"), Value = false });
            obj.Value.Add(new NamePropertyData() { Name = new FName("ColorPaletteRowName"), Value = new FName(Palette) });
            obj.Value.Add(new NamePropertyData() { Name = new FName("ColorName"), Value = new FName(Color) });
            obj.DuplicationIndex = DupIndex;

            return obj;
        }

        public static ArrayPropertyData HideParts()
        {
            ArrayPropertyData obj = new ArrayPropertyData();
            obj.ArrayType = new FName("StructProperty");
            obj.DummyStruct = new StructPropertyData() { Name = new FName("HidePartsInfoDetails"), StructType = new FName("AvatarCustomizeDataTableInnerHidePartsInfoDetail") };
            obj.Name = new FName("HidePartsInfoDetails");

            return obj;
        }

        public static StrPropertyData CheckFlag()
        {
            StrPropertyData obj = new StrPropertyData();
            obj.Name = new FName("CheckFlagSymbol");
            obj.Value = null;

            return obj;
        }

        public static StructPropertyData HidePartDetail(string name)
        {
            StructPropertyData obj = new StructPropertyData();
            obj.Name = new FName("HidePartsInfoDetails");
            obj.StructType = new FName("AvatarCustomizeDataTableInnerHidePartsInfoDetail");
            obj.Value = new List<PropertyData>();
            obj.Value.Add(new SoftObjectPropertyData() { Name = new FName("Thumbnail"), Value = new FName("/Game/UserInterface/AvatarCustomize/Textures/Thumbnail/Inner/") });
            obj.Value.Add(new NamePropertyData() { Name = new FName("HidePartsName"), Value = new FName(name) });

            return obj;
        }
    }
}
