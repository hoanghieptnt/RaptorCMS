﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Raptor.Data;
using Raptor.Data.Models;
using Raptor.Data.Models.Logging;
using Raptor.Data.Models.Users;

namespace Raptor.Data.Migrations
{
    [DbContext(typeof(RaptorDbContext))]
    [Migration("20170722102433_AddPermissionRecordTable")]
    partial class AddPermissionRecordTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Raptor.Data.Configuration.Setting", b =>
                {
                    b.Property<int>("SettingId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateModifiedUtc");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("SettingId");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Raptor.Data.Models.Blog.BlogComment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Agent")
                        .HasMaxLength(255);

                    b.Property<bool>("Approved");

                    b.Property<string>("Author")
                        .HasMaxLength(50);

                    b.Property<string>("AuthorEmail")
                        .HasMaxLength(100);

                    b.Property<string>("AuthorIp")
                        .HasMaxLength(100);

                    b.Property<string>("AuthorUrl")
                        .HasMaxLength(200);

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateCreatedGmt");

                    b.Property<int>("Karma");

                    b.Property<int>("PostId");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("BlogComments");
                });

            modelBuilder.Entity("Raptor.Data.Models.Blog.BlogPost", b =>
                {
                    b.Property<int>("BlogPostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogPostCategoryId");

                    b.Property<int>("CommentsCount");

                    b.Property<string>("CommentsStatus");

                    b.Property<string>("Content");

                    b.Property<int>("CreatedById");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateCreatedGmt");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("DateModifiedGmt");

                    b.Property<string>("Excerpt");

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Link")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .HasMaxLength(200);

                    b.Property<string>("Password")
                        .HasMaxLength(20);

                    b.Property<int>("PostType");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.HasKey("BlogPostId");

                    b.HasIndex("BlogPostCategoryId");

                    b.HasIndex("CreatedById");

                    b.ToTable("BlogPosts");
                });

            modelBuilder.Entity("Raptor.Data.Models.Blog.BlogPostCategory", b =>
                {
                    b.Property<int>("PostCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<int>("ParentId");

                    b.Property<string>("Slug")
                        .HasMaxLength(100);

                    b.HasKey("PostCategoryId");

                    b.ToTable("BlogPostCategories");
                });

            modelBuilder.Entity("Raptor.Data.Models.Content.Taxonomy", b =>
                {
                    b.Property<int>("TaxonomyId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Count");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("TermId");

                    b.HasKey("TaxonomyId");

                    b.HasIndex("TermId");

                    b.ToTable("Taxonomies");
                });

            modelBuilder.Entity("Raptor.Data.Models.Content.Term", b =>
                {
                    b.Property<int>("TermId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Slug");

                    b.HasKey("TermId");

                    b.ToTable("Terms");
                });

            modelBuilder.Entity("Raptor.Data.Models.Content.TermRelationship", b =>
                {
                    b.Property<int>("ObjectId");

                    b.Property<int>("TaxonomyId");

                    b.Property<Guid>("RowGuid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.HasKey("ObjectId", "TaxonomyId");

                    b.HasIndex("TaxonomyId");

                    b.ToTable("TermRelationships");
                });

            modelBuilder.Entity("Raptor.Data.Models.Logging.ActivityLog", b =>
                {
                    b.Property<int>("ActivityLogId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ActivityLogTypeId");

                    b.Property<int>("BusinessEntityId");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<string>("IpAddress");

                    b.HasKey("ActivityLogId");

                    b.HasIndex("ActivityLogTypeId");

                    b.HasIndex("BusinessEntityId");

                    b.ToTable("ActivityLogs");
                });

            modelBuilder.Entity("Raptor.Data.Models.Logging.ActivityLogType", b =>
                {
                    b.Property<int>("ActivityLogTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<bool>("Enabled");

                    b.Property<string>("SystemKeyword");

                    b.HasKey("ActivityLogTypeId");

                    b.ToTable("ActivityLogTypes");
                });

            modelBuilder.Entity("Raptor.Data.Models.Logging.Log", b =>
                {
                    b.Property<int>("LogId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<string>("FullMessage");

                    b.Property<int>("LogLevel");

                    b.Property<int>("LogLevelId");

                    b.Property<string>("PageUrl");

                    b.Property<string>("ShortMessage");

                    b.HasKey("LogId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Raptor.Data.Models.Security.PermissionRecord", b =>
                {
                    b.Property<int>("PermissionRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("Name");

                    b.Property<string>("SystemName");

                    b.HasKey("PermissionRecordId");

                    b.ToTable("PermissionRecords");
                });

            modelBuilder.Entity("Raptor.Data.Models.Security.RolePermission", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("PermissionRecordId");

                    b.HasKey("RoleId", "PermissionRecordId");

                    b.HasIndex("PermissionRecordId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<DateTime>("DateModifiedUtc");

                    b.Property<string>("PostalCode");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.BusinessEntity", b =>
                {
                    b.Property<int>("BusinessEntityId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("RowGuid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.HasKey("BusinessEntityId");

                    b.ToTable("BusinessEntities");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.BusinessEntityAddress", b =>
                {
                    b.Property<int>("BusinessEntityId");

                    b.Property<int>("AddressId");

                    b.Property<int>("AddressType");

                    b.Property<int>("AddressTypeId");

                    b.HasKey("BusinessEntityId", "AddressId");

                    b.HasIndex("AddressId");

                    b.ToTable("BusinessEntityAddresses");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.Password", b =>
                {
                    b.Property<int>("BusinessEntityId");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordSalt");

                    b.Property<Guid>("RowGuid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.HasKey("BusinessEntityId");

                    b.ToTable("Password");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.Person", b =>
                {
                    b.Property<int>("BusinessEntityId");

                    b.Property<string>("About");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<DateTime>("DateLastLoginUtc");

                    b.Property<DateTime>("DateModifiedUtc");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(180);

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<bool>("IsBlocked");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50);

                    b.Property<string>("Username")
                        .HasMaxLength(50);

                    b.Property<string>("Website");

                    b.HasKey("BusinessEntityId");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.PersonRole", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("BusinessEntityId");

                    b.Property<Guid>("RowGuid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.HasKey("RoleId", "BusinessEntityId");

                    b.HasAlternateKey("BusinessEntityId", "RoleId");

                    b.ToTable("PersonRoles");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.PhoneNumber", b =>
                {
                    b.Property<int>("PhoneNumberId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<DateTime>("DateModifiedUtc");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Number");

                    b.Property<int>("PersonId");

                    b.Property<int>("PhoneNumberType");

                    b.Property<int>("PhoneNumberTypeId");

                    b.HasKey("PhoneNumberId");

                    b.HasIndex("PersonId");

                    b.ToTable("PhoneNumber");
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<string>("SystemKeyword");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Raptor.Data.Models.Blog.BlogComment", b =>
                {
                    b.HasOne("Raptor.Data.Models.Blog.BlogPost", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Blog.BlogPost", b =>
                {
                    b.HasOne("Raptor.Data.Models.Blog.BlogPostCategory", "BlogPostCategory")
                        .WithMany("Posts")
                        .HasForeignKey("BlogPostCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Raptor.Data.Models.Users.BusinessEntity", "BusinessEntity")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Content.Taxonomy", b =>
                {
                    b.HasOne("Raptor.Data.Models.Content.Term", "Term")
                        .WithMany()
                        .HasForeignKey("TermId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Content.TermRelationship", b =>
                {
                    b.HasOne("Raptor.Data.Models.Content.Taxonomy", "Taxonomy")
                        .WithMany()
                        .HasForeignKey("TaxonomyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Logging.ActivityLog", b =>
                {
                    b.HasOne("Raptor.Data.Models.Logging.ActivityLogType", "ActivityLogType")
                        .WithMany()
                        .HasForeignKey("ActivityLogTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Raptor.Data.Models.Users.BusinessEntity", "BusinessEntity")
                        .WithMany()
                        .HasForeignKey("BusinessEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Security.RolePermission", b =>
                {
                    b.HasOne("Raptor.Data.Models.Security.PermissionRecord", "PermissionRecord")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionRecordId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Raptor.Data.Models.Users.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.BusinessEntityAddress", b =>
                {
                    b.HasOne("Raptor.Data.Models.Users.Address", "Address")
                        .WithMany("BusinessEntityAddresses")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Raptor.Data.Models.Users.BusinessEntity", "BusinessEntity")
                        .WithMany("BusinessEntityAddresses")
                        .HasForeignKey("BusinessEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.Password", b =>
                {
                    b.HasOne("Raptor.Data.Models.Users.Person", "Person")
                        .WithOne("Password")
                        .HasForeignKey("Raptor.Data.Models.Users.Password", "BusinessEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.Person", b =>
                {
                    b.HasOne("Raptor.Data.Models.Users.BusinessEntity", "BusinessEntity")
                        .WithOne("Person")
                        .HasForeignKey("Raptor.Data.Models.Users.Person", "BusinessEntityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.PersonRole", b =>
                {
                    b.HasOne("Raptor.Data.Models.Users.Person", "Person")
                        .WithMany("UserRoles")
                        .HasForeignKey("BusinessEntityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Raptor.Data.Models.Users.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Raptor.Data.Models.Users.PhoneNumber", b =>
                {
                    b.HasOne("Raptor.Data.Models.Users.Person", "Person")
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
