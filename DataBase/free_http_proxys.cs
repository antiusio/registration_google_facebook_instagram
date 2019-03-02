namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class free_http_proxys
    {
        public int id { get; set; }

        [StringLength(15)]
        public string ip { get; set; }

        public int? port { get; set; }
    }
}
