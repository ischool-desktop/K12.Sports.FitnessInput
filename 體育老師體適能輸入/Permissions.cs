using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.Sports.FitnessInput
{
    class Permissions
    {
        public static string 體適能輸入時間設定 { get { return "K12.Sports.FitnessInput.cs"; } }
        public static bool 體適能輸入時間設定權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[體適能輸入時間設定].Executable;
            }
        }
    }
}
