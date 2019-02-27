namespace DataBase
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RegBase : DbContext
    {
        public RegBase()
            : base("name=RegBase")
        {
        }

        public virtual DbSet<city> citys { get; set; }
        public virtual DbSet<country> countrys { get; set; }
        public virtual DbSet<google_accs> google_accs { get; set; }
        public virtual DbSet<i_ua_accs> i_ua_accs { get; set; }
        public virtual DbSet<i_ua_domen_names> i_ua_domen_names { get; set; }
        public virtual DbSet<secret_questions> secret_questions { get; set; }
        public virtual DbSet<sex> sexes { get; set; }
        public virtual DbSet<user_agents> user_agents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<city>()
                .HasMany(e => e.i_ua_accs)
                .WithOptional(e => e.city)
                .HasForeignKey(e => e.citys_id);

            modelBuilder.Entity<country>()
                .HasMany(e => e.i_ua_accs)
                .WithOptional(e => e.country)
                .HasForeignKey(e => e.country_id);

            modelBuilder.Entity<i_ua_accs>()
                .HasMany(e => e.google_accs)
                .WithOptional(e => e.i_ua_accs)
                .HasForeignKey(e => e.alt_email_id);

            modelBuilder.Entity<i_ua_domen_names>()
                .HasMany(e => e.i_ua_accs)
                .WithOptional(e => e.i_ua_domen_names)
                .HasForeignKey(e => e.domen_id);

            modelBuilder.Entity<secret_questions>()
                .HasMany(e => e.i_ua_accs)
                .WithOptional(e => e.secret_questions)
                .HasForeignKey(e => e.secret_question_id);

            modelBuilder.Entity<sex>()
                .HasMany(e => e.google_accs)
                .WithRequired(e => e.sex)
                .HasForeignKey(e => e.sex_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sex>()
                .HasMany(e => e.i_ua_accs)
                .WithOptional(e => e.sex)
                .HasForeignKey(e => e.sex_id);
        }
    }
}
