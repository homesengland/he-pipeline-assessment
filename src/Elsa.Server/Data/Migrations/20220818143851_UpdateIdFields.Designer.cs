﻿// <auto-generated />
using System;
using Elsa.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Elsa.Server.Data.Migrations
{
    [DbContext(typeof(PipelineAssessmentContext))]
    [Migration("20220818143851_UpdateIdFields")]
    partial class UpdateIdFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("Elsa.CustomModels.MultipleChoiceQuestionModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActivityId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Answer")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("FinishWorkflow")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("NavigateBack")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PreviousActivityId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkflowInstanceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MultipleChoiceQuestions");
                });
#pragma warning restore 612, 618
        }
    }
}
