using DoodleBoard.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DoodleBoard.Repository
{

    public class DoodleBoardContext : DbContext
    {
        public DoodleBoardContext() : base("DoodleBoardContext")
        {

        }

        public DbSet<Whiteboard> Whiteboards { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Connection>().HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Whiteboard>().HasKey(x => x.Id).Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Connection>().HasKey(x => new { x.UserId, x.WhiteboardId });


            //.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Connection>()
                .HasRequired(x => x.User)
                .WithMany(x => x.Connections)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Connection>()
                .HasRequired(x => x.Whiteboard)
                .WithMany(x => x.Connections)
                .HasForeignKey(x => x.WhiteboardId);




            //modelBuilder.Entity<User>()
            //    .HasMany<Connection>(x => x.Connections)
            //    .WithRequired()
            //    .HasForeignKey(X => X.UserId);


            //modelBuilder.Entity<Whiteboard>()
            //    .HasMany<Connection>(x => x.Connections)
            //    .WithRequired()
            //    .HasForeignKey(x => x.WhiteboardId)
            //    .WillCascadeOnDelete(true);
        }
    }
}
