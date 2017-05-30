
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Model.Xone;

namespace DbContexts.Xone
{
    public class XoneContext : DbContext
    { 

        public XoneContext()  
        {
            //Database.Connection.ConnectionString = "Data Source=192.168.0.101; Initial Catalog=Xone; UID=sa; Password=sqladmin; MultipleActiveResultSets=True";
            Database.SetInitializer<XoneContext>(new MigrateDatabaseToLatestVersion<XoneContext, ConfigurationData>("XoneContext"));
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TblCitizenDetails> TblCitizenDetails { get; set; }
        public DbSet<TblRace> TblRace { get; set; }   
        public DbSet<TblLanguageDetails> TblLanguageDetails { get; set; }
        public DbSet<TblSkillDetails> TblSkillDetails { get; set; }
        public DbSet<TblDocumentTypes> TblDocumentTypes { get; set; }
        public DbSet<TblDocumentSubTypes> TblDocumentSubTypes { get; set; }



        public DbSet<TblCandidate> TblCandidate { get; set; }
        public DbSet<TblCandidateLanguage> TblCandidateLanguage { get; set; }
        public DbSet<TblCandidateSkillset> TblCandidateSkillset { get; set; }
        public DbSet<TblCandidateDocuments> TblCandidateDocuments { get; set; }

    }
}