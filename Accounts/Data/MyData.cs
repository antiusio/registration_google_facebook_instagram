using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data
{
    public static class MyData
    {
        public static Dictionary<int, string> Sex = new Dictionary<int, string>()
        {
            { 2, "мужcкой" },
            { 1, "женcкий"}
        };
        public static Dictionary<int, string> Countrys = new Dictionary<int, string>()
        {
            {1, "Украина" },
            {2,"Россия" }
        };
        public static Dictionary<int, string> Citys = new Dictionary<int, string>()
        {
            {1,"Харьков" },
            {2, "Москва" }
        };
        public static Dictionary<int, string> SecretQuestions = new Dictionary<int, string>()
        {
            {1,"Любимое блюдо" },
            {2,"Имя животного" },
            {3,"Памятная дата" },
            {4,"Девичья фамилия матери" },
            {5,"Прозвище в школе" },
            {6,"Номер паспорта" }
        };
        public static Dictionary<int, string> Domen = new Dictionary<int, string>()
        {
            {1,"i.ua" },
            {2,"ua.fm" },
            {3,"email.ua" }
        };
    }
}
