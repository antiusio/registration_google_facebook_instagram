using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public static class Repair
    {
        public static void RepairDB()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["RegBase"].ConnectionString;
            SqlCeEngine engine =
             new SqlCeEngine(connectionString);
            if (false == engine.Verify())
            {
                Console.WriteLine("Database is corrupted.");
                try
                {
                    engine.Repair(null, RepairOption.RecoverAllPossibleRows);
                }
                catch (SqlCeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            using (RegBase regBase = new RegBase())
            {
                if (regBase.sexes.Count() == 0)
                {
                    regBase.sexes.Add(new sex() { value= "мужcкой" });
                    regBase.sexes.Add(new sex() { value = "женcкий" });
                }
                if (regBase.i_ua_domen_names.Count() == 0)
                {
                    regBase.i_ua_domen_names.Add(new i_ua_domen_names() { value = "i.ua" });
                    regBase.i_ua_domen_names.Add(new i_ua_domen_names() { value = "ua.fm" });
                    regBase.i_ua_domen_names.Add(new i_ua_domen_names() { value = "email.ua" });
                }
                if (regBase.countrys.Count() == 0)
                {
                    regBase.countrys.Add(new country() { value= "Украина" });
                    regBase.countrys.Add(new country() { value = "Россия" });
                }
                if (regBase.citys.Count() == 0)
                {
                    regBase.citys.Add(new city() { value= "Харьков" });
                    regBase.citys.Add(new city() { value = "Москва" });
                }
                if (regBase.secret_questions.Count() == 0)
                {
                    regBase.secret_questions.Add(new secret_questions() { value = "Любимое блюдо" });
                    regBase.secret_questions.Add(new secret_questions() { value = "Имя животного" });
                    regBase.secret_questions.Add(new secret_questions() { value = "Памятная дата" });
                    regBase.secret_questions.Add(new secret_questions() { value = "Девичья фамилия матери" });
                    regBase.secret_questions.Add(new secret_questions() { value = "Прозвище в школе" });
                    regBase.secret_questions.Add(new secret_questions() { value = "Номер паспорта" });
                }
                if (regBase.settings.Count() == 0)
                {
                    regBase.settings.Add(new setting() { rucaptcha_api_key = "1c83a1837d692cc42475a00f6b90f0ca", sms_reg_api_key = "49blc4y5jwv7kl5shatnjk6y4t5qj4cb" });

                }
                regBase.SaveChanges();
            }
        }
    }
}
