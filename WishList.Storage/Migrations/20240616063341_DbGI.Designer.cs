﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WishList.Storage;

#nullable disable

namespace WishList.Storage.Migrations
{
    [DbContext(typeof(WishListDbContext))]
    [Migration("20240616063341_DbGI")]
    partial class DbGI
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TelegramUserWishList", b =>
                {
                    b.Property<long>("ReadUsersId")
                        .HasColumnType("bigint");

                    b.Property<int>("SubscribeWishListsId")
                        .HasColumnType("integer");

                    b.HasKey("ReadUsersId", "SubscribeWishListsId");

                    b.HasIndex("SubscribeWishListsId");

                    b.ToTable("TelegramUserWishList");
                });

            modelBuilder.Entity("TelegramUserWishList1", b =>
                {
                    b.Property<long>("WriteUsersId")
                        .HasColumnType("bigint");

                    b.Property<int>("WriteWishListsId")
                        .HasColumnType("integer");

                    b.HasKey("WriteUsersId", "WriteWishListsId");

                    b.HasIndex("WriteWishListsId");

                    b.ToTable("TelegramUserWishList1");
                });

            modelBuilder.Entity("WishList.Storage.Entities.Present", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Reference")
                        .HasColumnType("text");

                    b.Property<long?>("ReserveForUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("WishListId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WishListId");

                    b.ToTable("Presents");
                });

            modelBuilder.Entity("WishList.Storage.Entities.TelegramUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastCommand")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WishList.Storage.Entities.WishList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("AuthorId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("WishLists");
                });

            modelBuilder.Entity("TelegramUserWishList", b =>
                {
                    b.HasOne("WishList.Storage.Entities.TelegramUser", null)
                        .WithMany()
                        .HasForeignKey("ReadUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WishList.Storage.Entities.WishList", null)
                        .WithMany()
                        .HasForeignKey("SubscribeWishListsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TelegramUserWishList1", b =>
                {
                    b.HasOne("WishList.Storage.Entities.TelegramUser", null)
                        .WithMany()
                        .HasForeignKey("WriteUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WishList.Storage.Entities.WishList", null)
                        .WithMany()
                        .HasForeignKey("WriteWishListsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WishList.Storage.Entities.Present", b =>
                {
                    b.HasOne("WishList.Storage.Entities.WishList", "WishList")
                        .WithMany("Presents")
                        .HasForeignKey("WishListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WishList");
                });

            modelBuilder.Entity("WishList.Storage.Entities.WishList", b =>
                {
                    b.HasOne("WishList.Storage.Entities.TelegramUser", "Author")
                        .WithMany("WishLists")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("WishList.Storage.Entities.TelegramUser", b =>
                {
                    b.Navigation("WishLists");
                });

            modelBuilder.Entity("WishList.Storage.Entities.WishList", b =>
                {
                    b.Navigation("Presents");
                });
#pragma warning restore 612, 618
        }
    }
}
