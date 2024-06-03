using Microsoft.EntityFrameworkCore;
using Project.Core.Models;
using Project.Core.Settings;
using System.Reflection;

namespace Project.Infrastructure.DAL
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(AppSettings.Settings.AppDbConnectionModel.ToString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<PointModel>().HasIndex(r => new { r.X, r.Y});
            modelBuilder.Entity<RectangleModel>().Navigation(r=>r.TopLeftPoint).AutoInclude();
            modelBuilder.Entity<RectangleModel>().Navigation(r=>r.TopRightPoint).AutoInclude();
            modelBuilder.Entity<RectangleModel>().Navigation(r=>r.BottomLeftPoint).AutoInclude();
            modelBuilder.Entity<RectangleModel>().Navigation(r=>r.BottomRightPoint).AutoInclude();

            SetNoKeyForProceduresAndFunctions(modelBuilder);
        }

        private static void SetNoKeyForProceduresAndFunctions(ModelBuilder modelBuilder)
        {
            var spTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name.StartsWith("SP_")
                            || t.Name.StartsWith(value: "FN_"))
                .ToHashSet();

            var orderedTypes = new List<Type>();

            while (spTypes.Count > 0)
            {
                foreach (var spType in spTypes)
                {
                    var isParentClass = spTypes.Any(spTypeInner => spTypeInner.IsSubclassOf(spType));

                    if (isParentClass)
                    {
                        continue;
                    }

                    orderedTypes.Add(spType);
                    spTypes.Remove(spType);
                }
            }

            foreach (var spType in orderedTypes)
            {
                modelBuilder.Entity(spType).HasNoKey();
            }
        }

        public DbSet<RectangleModel> Rectangles { get; set; }
    }
}
