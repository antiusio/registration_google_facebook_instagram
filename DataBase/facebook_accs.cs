namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class facebook_accs
    {
        public int id { get; set; }

        [StringLength(50)]
        public string first_name { get; set; }

        [StringLength(50)]
        public string last_name { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(12)]
        public string phone { get; set; }

        [StringLength(20)]
        public string password { get; set; }

        public DateTime date_birth { get; set; }

        public int sex_id { get; set; }

        public virtual sex sex { get; set; }
    }
}
