using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pragma_API.Models;

namespace Pragma_API.Infraestrutura
{
    public partial class IntegracaoTMContext : DbContext
    {
        public IntegracaoTMContext()
        {
        }

        public IntegracaoTMContext(DbContextOptions<IntegracaoTMContext> options) : base(options) { }

        public virtual DbSet<UsuarioDB> Usuarios { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
