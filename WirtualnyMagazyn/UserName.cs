using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WirtualnyMagazyn
{
    public class UserName
    {
        private string LoginName = "";
        public string loginName
        {
            get { return LoginName; }
            set { LoginName = value; }
        }

        private int userLevel = 0;
        public int Userlevel
        {
            get { return userLevel; }
            set
            {
                userLevel = value;
            }
        }

    }
}
