namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class i_ua_accs
    {
        public int id { get; set; }

        [StringLength(50)]
        public string login { get; set; }

        public int? domen_id { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(50)]
        public string first_name { get; set; }

        [StringLength(50)]
        public string last_name { get; set; }

        public int? sex_id { get; set; }

        public DateTime date_birth { get; set; }

        public int? country_id { get; set; }

        public int? citys_id { get; set; }

        public int? secret_question_id { get; set; }

        [StringLength(50)]
        public string answer { get; set; }

        public DateTime? date_registered { get; set; }

        public virtual city city { get; set; }

        public virtual country country { get; set; }

        public virtual i_ua_domen_names i_ua_domen_names { get; set; }

        public virtual secret_questions secret_questions { get; set; }

        public virtual sex sex { get; set; }
    }
}
