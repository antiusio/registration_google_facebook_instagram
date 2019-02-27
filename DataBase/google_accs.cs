namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class google_accs
    {
        public int id { get; set; }

        [StringLength(50)]
        public string login { get; set; }

        public int? alt_email_id { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(50)]
        public string first_name { get; set; }

        [StringLength(50)]
        public string last_name { get; set; }

        public int sex_id { get; set; }

        public DateTime date_birth { get; set; }

        [StringLength(12)]
        public string phone { get; set; }

        public DateTime? date_registered { get; set; }

        public virtual i_ua_accs i_ua_accs { get; set; }

        public virtual sex sex { get; set; }
    }
}
