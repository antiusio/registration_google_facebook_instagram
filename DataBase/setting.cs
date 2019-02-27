namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class setting
    {
        public int id { get; set; }

        [StringLength(40)]
        public string sms_reg_api_key { get; set; }

        [StringLength(40)]
        public string rucaptcha_api_key { get; set; }
    }
}
