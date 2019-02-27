namespace DataBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class i_ua_domen_names
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public i_ua_domen_names()
        {
            i_ua_accs = new HashSet<i_ua_accs>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(20)]
        public string value { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<i_ua_accs> i_ua_accs { get; set; }
    }
}
