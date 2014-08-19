using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA;
using FISCA.Presentation;
using FISCA.Permission;

namespace K12.Sports.FitnessInput
{
    public class Program
    {
        [MainMethod()]
        static public void Main()
        {
            RibbonBarItem item = MotherForm.RibbonBarItems["學生", "體適能"];
            item["設定"].Image = Properties.Resources.設定;
            item["設定"].Size = RibbonBarButton.MenuButtonSize.Large;
            item["設定"]["體適能輸入時間設定"].Enable = Permissions.體適能輸入時間設定權限;
            item["設定"]["體適能輸入時間設定"].Click += delegate
            {
                new InputDateSettingForm().ShowDialog();
            };
            Catalog detail1 = RoleAclSource.Instance["學生"]["功能按鈕"];
            detail1.Add(new RibbonFeature(Permissions.體適能輸入時間設定, "體適能輸入時間設定"));
        }
    }
}
