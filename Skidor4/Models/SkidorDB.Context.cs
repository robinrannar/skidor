﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Skidor4.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db4Entities : DbContext
    {
        public db4Entities()
            : base("name=db4Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<chiplink> chiplink { get; set; }
        public virtual DbSet<chips> chips { get; set; }
        public virtual DbSet<covers> covers { get; set; }
        public virtual DbSet<equipment> equipment { get; set; }
        public virtual DbSet<permissions> permissions { get; set; }
        public virtual DbSet<persons> persons { get; set; }
        public virtual DbSet<prices> prices { get; set; }
        public virtual DbSet<scoreboard> scoreboard { get; set; }
        public virtual DbSet<stations> stations { get; set; }
        public virtual DbSet<tracks> tracks { get; set; }
    }
}
