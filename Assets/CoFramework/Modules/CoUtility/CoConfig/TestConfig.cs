using CoFramework.Utility.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace CoFramework.Utility
{
    public class MyConfig
    {
       
        public int id;
        [ColumnDescription("描述")]
        public string des;
    }


    public class TestConfig
    {
        [MenuItem("CoFramework",menuItem ="CoFramework/TestConfig")]
        
        static void Test()
        {
            //ExcelHelper.CreateExcelWithSheet("C:\\Users\\imyue\\OneDrive\\桌面\\MyXLSX.xlsx","hh");
            ExcelHelper.WriteTypeData("C:\\Users\\imyue\\OneDrive\\桌面\\MyXLSX.xlsx", "hh", typeof(MyConfig));
        }

       
    }
}
