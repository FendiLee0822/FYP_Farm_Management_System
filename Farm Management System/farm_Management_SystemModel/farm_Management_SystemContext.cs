using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Farm_Management_System.farm_Management_SystemModel
{
    public partial class farm_Management_SystemContext : DbContext
    {
        public farm_Management_SystemContext()
        {
        }

        public farm_Management_SystemContext(DbContextOptions<farm_Management_SystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Field> Fields { get; set; }
        public virtual DbSet<Financial> Financials { get; set; }
        public virtual DbSet<Growth> Growths { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Plant> Plants { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;Database=farm_Management_System;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.AdminId).HasColumnName("admin_Id");

                entity.Property(e => e.UserId).HasColumnName("user_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Admins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_User");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_Id");

                entity.Property(e => e.UserId).HasColumnName("user_Id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_User");
            });

            modelBuilder.Entity<Field>(entity =>
            {
                entity.ToTable("Field");

                entity.Property(e => e.FieldId).HasColumnName("field_Id");

                entity.Property(e => e.FieldLong).HasColumnName("field_Long");

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("field_Name");

                entity.Property(e => e.FieldWidth).HasColumnName("field_Width");
            });

            modelBuilder.Entity<Financial>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("Financial");

                entity.Property(e => e.TransactionId).HasColumnName("transaction_Id");

                entity.Property(e => e.AdminUpdate).HasColumnName("admin_Update");

                entity.Property(e => e.ExpenseAmount).HasColumnName("expense_Amount");

                entity.Property(e => e.IncomeAmount).HasColumnName("income_Amount");

                entity.Property(e => e.TransactionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("transaction_Name");

                entity.Property(e => e.TransactionProject).HasColumnName("transaction_Project");

                entity.Property(e => e.TransactionUpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("transaction_Update_Time");

                entity.HasOne(d => d.AdminUpdateNavigation)
                    .WithMany(p => p.Financials)
                    .HasForeignKey(d => d.AdminUpdate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Financial_Admin1");

                entity.HasOne(d => d.TransactionProjectNavigation)
                    .WithMany(p => p.Financials)
                    .HasForeignKey(d => d.TransactionProject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Financial_project");
            });

            modelBuilder.Entity<Growth>(entity =>
            {
                entity.ToTable("Growth");

                entity.Property(e => e.GrowthId).HasColumnName("growth_Id");

                entity.Property(e => e.GrowthField).HasColumnName("growth_Field");

                entity.Property(e => e.GrowthPlant).HasColumnName("growth_Plant");

                entity.Property(e => e.GrowthProject).HasColumnName("growth_Project");

                entity.Property(e => e.GrowthStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("growth_Status");

                entity.Property(e => e.GrowthUpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("growth_Update_Time");

                entity.Property(e => e.UserUpdate).HasColumnName("user_Update");

                entity.HasOne(d => d.GrowthFieldNavigation)
                    .WithMany(p => p.Growths)
                    .HasForeignKey(d => d.GrowthField)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Growth_Field1");

                entity.HasOne(d => d.GrowthPlantNavigation)
                    .WithMany(p => p.Growths)
                    .HasForeignKey(d => d.GrowthPlant)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Growth_Plant");

                entity.HasOne(d => d.GrowthProjectNavigation)
                    .WithMany(p => p.Growths)
                    .HasForeignKey(d => d.GrowthProject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Growth_project");

                entity.HasOne(d => d.UserUpdateNavigation)
                    .WithMany(p => p.Growths)
                    .HasForeignKey(d => d.UserUpdate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Growth_User");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventory");

                entity.Property(e => e.InventoryId).HasColumnName("inventory_Id");

                entity.Property(e => e.InventoryAmount).HasColumnName("inventory_Amount");

                entity.Property(e => e.InventoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("inventory_Name");

                entity.Property(e => e.InventoryUpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("inventory_Update_Time");

                entity.Property(e => e.UserUpdate).HasColumnName("user_Update");

                entity.HasOne(d => d.UserUpdateNavigation)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.UserUpdate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_User");
            });

            modelBuilder.Entity<Plant>(entity =>
            {
                entity.ToTable("Plant");

                entity.Property(e => e.PlantId).HasColumnName("plant_Id");

                entity.Property(e => e.AdminUpdate).HasColumnName("admin_Update");

                entity.Property(e => e.PlantBugTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("plant_Bug_Time");

                entity.Property(e => e.PlantFloweringTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("plant_Flowering_Time");

                entity.Property(e => e.PlantGrowthTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("plant_Growth_Time");

                entity.Property(e => e.PlantName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("plant_Name");

                entity.Property(e => e.PlantRipeningTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("plant_Ripening_Time");

                entity.Property(e => e.PlantSetTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("plant_Set_Time");

                entity.Property(e => e.PlantUpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("plant_Update_Time");

                entity.HasOne(d => d.AdminUpdateNavigation)
                    .WithMany(p => p.Plants)
                    .HasForeignKey(d => d.AdminUpdate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Plant_Admin");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.ProjectId).HasColumnName("project_Id");

                entity.Property(e => e.AdminCreate).HasColumnName("admin_Create");

                entity.Property(e => e.ProjectCreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("project_Create_Time");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("project_Name");

                entity.HasOne(d => d.AdminCreateNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.AdminCreate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_project_Admin");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.TaskId).HasColumnName("task_Id");

                entity.Property(e => e.AdminAssigned).HasColumnName("admin_Assigned");

                entity.Property(e => e.EmployeeIncharge).HasColumnName("employee_Incharge");

                entity.Property(e => e.TaskCreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("task_Create_Time");

                entity.Property(e => e.TaskDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("task_Description");

                entity.Property(e => e.TaskField).HasColumnName("task_Field");

                entity.Property(e => e.TaskPlant).HasColumnName("task_Plant");

                entity.Property(e => e.TaskProject).HasColumnName("task_Project");

                entity.Property(e => e.TaskStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("task_Status");

                entity.Property(e => e.TaskUpdateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("task_Update_Time");

                entity.HasOne(d => d.AdminAssignedNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.AdminAssigned)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Admin");

                entity.HasOne(d => d.EmployeeInchargeNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.EmployeeIncharge)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Employee");

                entity.HasOne(d => d.TaskFieldNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskField)
                    .HasConstraintName("FK_Task_Field");

                entity.HasOne(d => d.TaskPlantNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskPlant)
                    .HasConstraintName("FK_Task_Plant");

                entity.HasOne(d => d.TaskProjectNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.TaskProject)
                    .HasConstraintName("FK_Task_Task");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("user_Id");

                entity.Property(e => e.UserDob)
                    .HasColumnType("datetime")
                    .HasColumnName("user_Dob");

                entity.Property(e => e.UserFname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("user_Fname");

                entity.Property(e => e.UserGender)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("user_Gender");

                entity.Property(e => e.UserLname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("user_Lname");

                entity.Property(e => e.UserPwd)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("user_Pwd");

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("user_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
