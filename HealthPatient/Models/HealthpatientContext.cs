﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HealthPatient.Models;

public partial class HealthpatientContext : DbContext
{
    public HealthpatientContext()
    {
    }

    public HealthpatientContext(DbContextOptions<HealthpatientContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<AnalyzedDatum> AnalyzedData { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorAchievement> DoctorAchievements { get; set; }

    public virtual DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<ItemsInShop> ItemsInShops { get; set; }

    public virtual DbSet<LoyaltyPointsHistory> LoyaltyPointsHistories { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SchedulesDoctor> SchedulesDoctors { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServicePrice> ServicePrices { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<Symptom> Symptoms { get; set; }

    public virtual DbSet<SymptomSpecialty> SymptomSpecialties { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=healthpatient;Username=postgres;Password=");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("achievements_pkey");

            entity.ToTable("achievements");

            entity.Property(e => e.AchievementId).HasColumnName("achievement_id");
            entity.Property(e => e.BadgeImage)
                .HasMaxLength(255)
                .HasColumnName("badge_image");
            entity.Property(e => e.Criteria).HasColumnName("criteria");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.AdministratorId).HasName("administrators_pkey");

            entity.ToTable("administrators");

            entity.Property(e => e.AdministratorId).HasColumnName("administrator_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.Image)
                .HasMaxLength(20)
                .HasColumnName("image");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Gender).WithMany(p => p.Administrators)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("administrators_genders_fk");
        });

        modelBuilder.Entity<AnalyzedDatum>(entity =>
        {
            entity.HasKey(e => e.IdData).HasName("analyzed_data_pk");

            entity.ToTable("analyzed_data");

            entity.Property(e => e.IdData)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_data");
            entity.Property(e => e.Averagerating)
                .HasPrecision(10, 2)
                .HasColumnName("averagerating");
            entity.Property(e => e.Countbadrating).HasColumnName("countbadrating");
            entity.Property(e => e.Countgoodrating).HasColumnName("countgoodrating");
            entity.Property(e => e.HoursInWork).HasColumnName("hours_in_work");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.MoneyCounted)
                .HasPrecision(10, 2)
                .HasColumnName("money_counted");
            entity.Property(e => e.PatientsCounted).HasColumnName("patients_counted");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.AnalyzedData)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("analyzed_data_doctors_fk");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("doctors_pkey");

            entity.ToTable("doctors");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.IsAnalyzed)
                .HasDefaultValue(false)
                .HasColumnName("is_analyzed");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Gender).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("doctors_genders_fk");
        });

        modelBuilder.Entity<DoctorAchievement>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("doctor_achievements");

            entity.Property(e => e.AchievementId).HasColumnName("achievement_id");
            entity.Property(e => e.DateAchieved).HasColumnName("date_achieved");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Isachieved).HasColumnName("isachieved");

            entity.HasOne(d => d.Achievement).WithMany()
                .HasForeignKey(d => d.AchievementId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("doctor_achievements_achievement_id_fkey");

            entity.HasOne(d => d.Doctor).WithMany()
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("doctor_achievements_doctor_id_fkey");
        });

        modelBuilder.Entity<DoctorSpecialty>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("doctor_specialties");

            entity.Property(e => e.CertificationDate).HasColumnName("certification_date");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Qualification)
                .HasMaxLength(100)
                .HasColumnName("qualification");
            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");

            entity.HasOne(d => d.Doctor).WithMany()
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("doctor_specialties_doctor_id_fkey");

            entity.HasOne(d => d.Specialty).WithMany()
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("doctor_specialties_specialty_id_fkey");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("genders_pk");

            entity.ToTable("genders");

            entity.Property(e => e.GenderId)
                .ValueGeneratedNever()
                .HasColumnName("gender_id");
            entity.Property(e => e.NameGender)
                .HasMaxLength(20)
                .HasColumnName("name_gender");
        });

        modelBuilder.Entity<ItemsInShop>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("items_in_shop_pk");

            entity.ToTable("items_in_shop");

            entity.Property(e => e.IdItem)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_item");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.CountInStock).HasColumnName("count_in_stock");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<LoyaltyPointsHistory>(entity =>
        {
            entity.HasKey(e => e.LoyaltyHistoryId).HasName("loyalty_points_history_pkey");

            entity.ToTable("loyalty_points_history");

            entity.Property(e => e.LoyaltyHistoryId).HasColumnName("loyalty_history_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.PointsAdded).HasColumnName("points_added");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");

            entity.HasOne(d => d.Patient).WithMany(p => p.LoyaltyPointsHistories)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("loyalty_points_history_patient_id_fkey");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Newid).HasName("news_pk");

            entity.ToTable("news");

            entity.Property(e => e.Newid)
                .ValueGeneratedNever()
                .HasColumnName("newid");
            entity.Property(e => e.Maintext)
                .HasMaxLength(1000)
                .HasColumnName("maintext");
            entity.Property(e => e.Titlenew)
                .HasMaxLength(100)
                .HasColumnName("titlenew");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("notifications_doctor_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("notifications_patient_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("orders");

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_id");

            entity.HasOne(d => d.Doctor).WithMany()
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("orders_doctors_fk");

            entity.HasOne(d => d.Item).WithMany()
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("orders_items_in_shop_fk");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("patients_pkey");

            entity.ToTable("patients");

            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20)
                .HasColumnName("contact_phone");
            entity.Property(e => e.Countloyaltypoints).HasColumnName("countloyaltypoints");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Discount)
                .HasDefaultValue(false)
                .HasColumnName("discount");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.Isanalyzed)
                .HasDefaultValue(false)
                .HasColumnName("isanalyzed");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.LoyaltyPoints)
                .HasDefaultValue(0)
                .HasColumnName("loyalty_points");
            entity.Property(e => e.MedicalNotes).HasColumnName("medical_notes");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.PercentsDiscount)
                .HasDefaultValue(0)
                .HasColumnName("percents_discount");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Gender).WithMany(p => p.Patients)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("patients_genders_fk");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("reviews_pkey");

            entity.ToTable("reviews");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Rating).HasColumnName("rating");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("reviews_doctor_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("reviews_patient_id_fkey");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("schedules_pkey");

            entity.ToTable("schedules");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.DatestartSchedule)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("datestart_schedule");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.IsBusy)
                .HasDefaultValue(false)
                .HasColumnName("is_busy");
            entity.Property(e => e.Lengthinmins).HasColumnName("lengthinmins");
        });

        modelBuilder.Entity<SchedulesDoctor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("schedules_doctors");

            entity.Property(e => e.DocSchedId)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("doc_sched_id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");

            entity.HasOne(d => d.Doctor).WithMany()
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("schedules_doctors_doctors_fk");

            entity.HasOne(d => d.Schedule).WithMany()
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("schedules_doctors_schedules_fk");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ServicePrice>(entity =>
        {
            entity.HasKey(e => e.ServicePriceId).HasName("service_prices_pkey");

            entity.ToTable("service_prices");

            entity.Property(e => e.ServicePriceId).HasColumnName("service_price_id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.ValidFrom).HasColumnName("valid_from");
            entity.Property(e => e.ValidTo).HasColumnName("valid_to");

            entity.HasOne(d => d.Doctor).WithMany(p => p.ServicePrices)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("service_prices_doctor_id_fkey");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicePrices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("service_prices_service_id_fkey");
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("specialties_pkey");

            entity.ToTable("specialties");

            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Symptom>(entity =>
        {
            entity.HasKey(e => e.SymptomId).HasName("symptoms_pkey");

            entity.ToTable("symptoms");

            entity.Property(e => e.SymptomId).HasColumnName("symptom_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SymptomSpecialty>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("symptom_specialty");

            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");
            entity.Property(e => e.SymptomId).HasColumnName("symptom_id");

            entity.HasOne(d => d.Specialty).WithMany()
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("symptom_specialty_specialty_id_fkey");

            entity.HasOne(d => d.Symptom).WithMany()
                .HasForeignKey(d => d.SymptomId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("symptom_specialty_symptom_id_fkey");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("visits_pkey");

            entity.ToTable("visits");

            entity.Property(e => e.VisitId).HasColumnName("visit_id");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Diagnosis).HasColumnName("diagnosis");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.IsAnalyzed)
                .HasDefaultValue(false)
                .HasColumnName("is_analyzed");
            entity.Property(e => e.IsConfirmed)
                .HasDefaultValue(false)
                .HasColumnName("is_confirmed");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Prescriptions).HasColumnName("prescriptions");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.VisitDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("visit_date");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Visits)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("visits_doctor_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Visits)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("visits_patient_id_fkey");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Visits)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("visits_schedule_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
