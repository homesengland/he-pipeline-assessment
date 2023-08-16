﻿// <auto-generated />
using System;
using He.PipelineAssessment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace He.PipelineAssessment.Infrastructure.Migrations
{
    [DbContext(typeof(PipelineAssessmentContext))]
    [Migration("20230131110306_AddNameToAssessmentToolWorkflow")]
    partial class AddNameToAssessmentToolWorkflow
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("He.PipelineAssessment.Models.Assessment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Counterparty")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ProjectManager")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProjectManagerEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SiteName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("SpId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Assessment");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentTool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AssessmentTool");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentToolInstanceNextWorkflow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AssessmentId")
                        .HasColumnType("int");

                    b.Property<int>("AssessmentToolWorkflowInstanceId")
                        .HasColumnType("int");

                    b.Property<bool>("IsStarted")
                        .HasColumnType("bit");

                    b.Property<string>("NextWorkflowDefinitionId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentToolWorkflowInstanceId");

                    b.ToTable("AssessmentToolInstanceNextWorkflow");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentToolWorkflow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AssessmentToolId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFirstWorkflow")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLatest")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.Property<string>("WorkflowDefinitionId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentToolId");

                    b.ToTable("AssessmentToolWorkflow");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentToolWorkflowInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AssessmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrentActivityId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CurrentActivityType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("SubmittedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkflowDefinitionId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WorkflowInstanceId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WorkflowName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.ToTable("AssessmentToolWorkflowInstance");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FriendlyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Xml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentToolInstanceNextWorkflow", b =>
                {
                    b.HasOne("He.PipelineAssessment.Models.AssessmentToolWorkflowInstance", "AssessmentToolWorkflowInstance")
                        .WithMany("AssessmentToolInstanceNextWorkflows")
                        .HasForeignKey("AssessmentToolWorkflowInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssessmentToolWorkflowInstance");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentToolWorkflow", b =>
                {
                    b.HasOne("He.PipelineAssessment.Models.AssessmentTool", "AssessmentTool")
                        .WithMany("AssessmentToolWorkflows")
                        .HasForeignKey("AssessmentToolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssessmentTool");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentToolWorkflowInstance", b =>
                {
                    b.HasOne("He.PipelineAssessment.Models.Assessment", "Assessment")
                        .WithMany("AssessmentToolWorkflowInstances")
                        .HasForeignKey("AssessmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assessment");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.Assessment", b =>
                {
                    b.Navigation("AssessmentToolWorkflowInstances");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentTool", b =>
                {
                    b.Navigation("AssessmentToolWorkflows");
                });

            modelBuilder.Entity("He.PipelineAssessment.Models.AssessmentToolWorkflowInstance", b =>
                {
                    b.Navigation("AssessmentToolInstanceNextWorkflows");
                });
#pragma warning restore 612, 618
        }
    }
}
