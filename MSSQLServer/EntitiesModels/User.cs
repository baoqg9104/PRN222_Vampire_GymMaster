using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public string Role { get; set; } = null!;

    public string? Specialization { get; set; }

    public string? Certification { get; set; }

    public decimal? HourlyRate { get; set; }

    public string? FitnessGoals { get; set; }

    public string? MedicalNotes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    public virtual ICollection<BodyMeasurement> BodyMeasurements { get; set; } = new List<BodyMeasurement>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<TrainerAssignment> TrainerAssignmentMembers { get; set; } = new List<TrainerAssignment>();

    public virtual ICollection<TrainerAssignment> TrainerAssignmentTrainers { get; set; } = new List<TrainerAssignment>();

    public virtual ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();

    public virtual ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
}
