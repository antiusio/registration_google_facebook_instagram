using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data
{
    public enum Sex
    {
        Male = 2,
        Female = 1
    }
    public enum SexIua
    {
        мужcкой = 1,
        женcкий = 2
    }
    public static class SexConverter
    {
        public static Sex ConvertToSexIua(SexIua sex)
        {
            if (sex == SexIua.мужcкой)
                return Sex.Male;
            else
                return Sex.Female;
        }
    }
}
