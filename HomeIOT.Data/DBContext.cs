using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<SensorData> SensorData { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorGroup> SensorGroups { get; set; }
        public DbSet<SensorInSensorGroup> SensorInSensorGroups { get; set; }
        public DbSet<ApplicationEvent> ApplicationEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorData>()
                .ToTable("SensorData")
                .HasOne(sd => sd.Sensor)
                .WithMany(s => s.Data)
                .HasForeignKey(sd => sd.SensorId);

            modelBuilder.Entity<Sensor>()
                .ToTable("Sensor");

            modelBuilder.Entity<SensorGroup>()
                .ToTable("SensorGroup");

            modelBuilder.Entity<SensorInSensorGroup>()
                .ToTable("SensorInSensorGroup")
                .HasOne(sig => sig.Sensor)
                .WithMany(s => s.Groups)
                .HasForeignKey(sig => sig.SensorId);

            modelBuilder.Entity<SensorInSensorGroup>()
                .HasOne(sig => sig.SensorGroup)
                .WithMany(sg => sg.Sensors)
                .HasForeignKey(sig => sig.SensorGroupId);

            modelBuilder.Entity<ApplicationEvent>()
                .ToTable("ApplicationEvent")
                .HasOne(sd => sd.Sensor)
                .WithMany(s => s.Events)
                .HasForeignKey(sd => sd.SensorId);
        }
    }
}