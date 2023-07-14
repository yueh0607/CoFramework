using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System;
using System.IO;
using System.ComponentModel;
using System.Reflection;

namespace CoFramework.Utility.Editor
{

    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property,AllowMultiple =false)]
    public class ColumnIgnoreAttribute : System.Attribute
    { 
        
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnDescriptionAttribute : System.Attribute
    {
        public string Description { get; set; } 
        public ColumnDescriptionAttribute(string description) 
        {
            Description = description;
            
        }
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnMainKeyAttribute : System.Attribute
    {
        public string Description { get; set; }
        public ColumnMainKeyAttribute(string description)
        {
            Description = description;

        }
    }


    public static class ExcelHelper
    {
        public static void CreateExcelWithSheet(string path,string sheetName)
        {
            FileStream fileStream = new FileStream(path,FileMode.OpenOrCreate, FileAccess.Write);
            ExcelPackage package= new ExcelPackage();
            ExcelWorksheet sheet =package.Workbook.Worksheets.Add(sheetName);
            package.SaveAs(fileStream);
            package.Dispose();
            fileStream.Dispose();
        }


        public static void WriteTypeData(string path,string sheetName,Type type)
        {
            ////读表
            //FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      
            ExcelPackage package = new ExcelPackage();
            //package.Load(fileStream);
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add(sheetName);

            var fields = ReflectionHelper.FindFields(type);
            var property = ReflectionHelper.FindProperty(type);
            Debug.Log("扫描到:"+(fields.Count + property.Count));

            int maxDesCount = 0;
            int desTemp = 0;
            Dictionary<string, string> map = new Dictionary<string, string>();
            Dictionary<string ,List<string>> des = new Dictionary<string, List<string>>();

            

            //生成excel字符串表
            foreach (var field in fields)
            {
                if (map.ContainsKey(field.Name)) throw new InvalidDataException("name conflict");
                map.Add(field.Name, field.FieldType.Name);
                des.Add(field.Name, new List<string>());
                var descriptionAtt = field.GetCustomAttributes<ColumnDescriptionAttribute>();
                foreach(var attr in descriptionAtt)
                {
                    des[field.Name].Add(attr.Description);
                    desTemp++;
                }
                maxDesCount = Math.Max(maxDesCount, desTemp);
                desTemp= 0;
            }
            foreach (var p in property)
            {
                if (map.ContainsKey(p.Name)) throw new InvalidDataException("name conflict");
                map.Add(p.Name, p.PropertyType.Name);
                des.Add(p.Name, new List<string>());
                var descriptionAtt = p.GetCustomAttributes<ColumnDescriptionAttribute>();
                foreach (var attr in descriptionAtt)
                {
                    des[p.Name].Add(attr.Description);
                    desTemp++;
                }
                maxDesCount = Math.Max(maxDesCount, desTemp);
                desTemp = 0;
            }

            int column = 1;
            foreach(var cell in map)
            {
                sheet.SetValue(1, column, cell.Value);
                sheet.SetValue(2, column, cell.Key);
                for(int i=3,j=0;i<maxDesCount+3&&j<des[cell.Key].Count; i++,j++)
                {
                    sheet.SetValue(i, column, des[cell.Key][j]);
                }
                column++;
            }
            for (int r = 4; r < maxDesCount + 4; r++)
                sheet.SetValue(r, 0, "#");

            string outputPath = Path.Combine(Path.GetDirectoryName(path), "Modified_" + Path.GetFileName(path));
            using (FileStream fileStreamWrite = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                package.SaveAs(fileStreamWrite);
            }
            package.Dispose();
        }
    }
}
