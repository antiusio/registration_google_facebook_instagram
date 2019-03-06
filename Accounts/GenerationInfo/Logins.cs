using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.GenerationInfo
{
    public static class Logins
    {
        public static string GenerateLogin(string firstName, string lastName)
        {
            string result = "";
            string firstNameTranslit = Translit(firstName.ToLower());
            string lastNameTranslit = Translit(lastName.ToLower());
            if ((firstName + lastName).Count() > 14)
            {
                if (firstNameTranslit.Count() > 7)
                    result = firstNameTranslit.Remove(7);
                else
                    result = firstNameTranslit;
                if (lastNameTranslit.Length > 7)
                    result += "." + lastNameTranslit.Remove(7);
                else
                    result += "." + lastNameTranslit;
            }
            else
            {
                result = firstNameTranslit + "_" + lastNameTranslit;
            }
            Random r = new Random((int)DateTime.Now.ToBinary());
            result = result.Replace("" + '\ufeff', "");
            result = result + r.Next();
            if (result.Length > 15)
            {
                result = result.Remove(15);
            }

            //result = result + "i";
            return result;
        }
        public static string Translit(string str)
        {

            string[] lat_low = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "", "y", "", "e", "yu", "ya" };

            string[] rus_low = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
            for (int i = 0; i <= 32; i++)
            {
                str = str.Replace(rus_low[i], lat_low[i]);
            }
            return str;
        }
    }
}
