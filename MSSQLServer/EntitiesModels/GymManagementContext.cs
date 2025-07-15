using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MSSQLServer.EntitiesModels;

public partial class GymManagementContext : DbContext
{
    public GymManagementContext()
    {
    }

    public GymManagementContext(DbContextOptions<GymManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<BodyMeasurement> BodyMeasurements { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<TrainerAssignment> TrainerAssignments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMembership> UserMemberships { get; set; }

    public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }

    public virtual DbSet<WorkoutSession> WorkoutSessions { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__BlogPost__3ED7876615D73334");

            entity.HasIndex(e => e.Slug, "UQ__BlogPost__32DD1E4C3FB521E2").IsUnique();

            entity.HasIndex(e => new { e.IsPublished, e.PublishedAt }, "idx_blog_published");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Category)
                .HasMaxLength(20)
                .HasColumnName("category");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.IsPublished)
                .HasDefaultValue(false)
                .HasColumnName("is_published");
            entity.Property(e => e.PublishedAt).HasColumnName("published_at");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .HasColumnName("slug");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.BlogPosts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogPosts__autho__72C60C4A");
        });

        modelBuilder.Entity<BodyMeasurement>(entity =>
        {
            entity.HasKey(e => e.MeasurementId).HasName("PK__BodyMeas__E3D1E1C1E8BE6684");

            entity.HasIndex(e => new { e.MemberId, e.MeasuredAt }, "idx_member_measurements");

            entity.Property(e => e.MeasurementId).HasColumnName("measurement_id");
            entity.Property(e => e.BodyFatPct)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("body_fat_pct");
            entity.Property(e => e.MeasuredAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("measured_at");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.MuscleMassKg)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("muscle_mass_kg");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.WeightKg)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight_kg");

            entity.HasOne(d => d.Member).WithMany(p => p.BodyMeasurements)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BodyMeasu__membe__5EBF139D");
        });

        modelBuilder.Entity<MembershipPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__Membersh__BE9F8F1D45239F29");

            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.DurationDays).HasColumnName("duration_days");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__E059842FCDD89736");

            entity.HasIndex(e => new { e.UserId, e.IsRead }, "idx_notification_user");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.NotificationType)
                .HasMaxLength(20)
                .HasDefaultValue("info")
                .HasColumnName("notification_type");
            entity.Property(e => e.ScheduledAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("scheduled_at");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__user___6C190EBB");
        });

        modelBuilder.Entity<TrainerAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__TrainerA__DA891814A53280B1");

            entity.HasIndex(e => e.IsActive, "idx_assignment_active");

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Goals)
                .HasColumnType("text")
                .HasColumnName("goals");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.MembershipId).HasColumnName("membership_id");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("start_date");
            entity.Property(e => e.TrainerId).HasColumnName("trainer_id");

            entity.HasOne(d => d.Member).WithMany(p => p.TrainerAssignmentMembers)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrainerAs__membe__4E88ABD4");

            entity.HasOne(d => d.Membership).WithMany(p => p.TrainerAssignments)
                .HasForeignKey(d => d.MembershipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrainerAs__membe__5070F446");

            entity.HasOne(d => d.Trainer).WithMany(p => p.TrainerAssignmentTrainers)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrainerAs__train__4F7CD00D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FF85CD1D8");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164E87C57B0").IsUnique();

            entity.HasIndex(e => e.Role, "idx_user_role");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Certification)
                .HasColumnType("text")
                .HasColumnName("certification");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.FitnessGoals)
                .HasColumnType("text")
                .HasColumnName("fitness_goals");
            entity.Property(e => e.HourlyRate)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("hourly_rate");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MedicalNotes)
                .HasColumnType("text")
                .HasColumnName("medical_notes");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("member")
                .HasColumnName("role");
            entity.Property(e => e.Specialization)
                .HasMaxLength(255)
                .HasColumnName("specialization");
        });

        modelBuilder.Entity<UserMembership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__UserMemb__CAE49DDDF2E04C70");

            entity.HasIndex(e => new { e.IsActive, e.EndDate }, "idx_membership_active");

            entity.Property(e => e.MembershipId).HasColumnName("membership_id");
            entity.Property(e => e.AutoRenew)
                .HasDefaultValue(false)
                .HasColumnName("auto_renew");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("start_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Plan).WithMany(p => p.UserMemberships)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__plan___48CFD27E");

            entity.HasOne(d => d.User).WithMany(p => p.UserMemberships)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__user___47DBAE45");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__WorkoutP__BE9F8F1D0EBFAF5A");

            entity.HasIndex(e => new { e.AssignmentId, e.DayOfWeek }, "idx_workout_day");

            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.ExerciseName)
                .HasMaxLength(255)
                .HasColumnName("exercise_name");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Reps).HasColumnName("reps");
            entity.Property(e => e.Sets).HasColumnName("sets");
            entity.Property(e => e.WeightDescription)
                .HasMaxLength(100)
                .HasDefaultValue("Bodyweight")
                .HasColumnName("weight_description");

            entity.HasOne(d => d.Assignment).WithMany(p => p.WorkoutPlans)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkoutPl__assig__5812160E");
        });

        modelBuilder.Entity<WorkoutSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__WorkoutS__69B13FDC9C82D0ED");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.ActualReps).HasColumnName("actual_reps");
            entity.Property(e => e.ActualSets).HasColumnName("actual_sets");
            entity.Property(e => e.ActualWeight)
                .HasMaxLength(100)
                .HasColumnName("actual_weight");
            entity.Property(e => e.CompletedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("completed_at");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Member).WithMany(p => p.WorkoutSessions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkoutSe__membe__6383C8BA");

            entity.HasOne(d => d.Plan).WithMany(p => p.WorkoutSessions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WorkoutSe__plan___6477ECF3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
