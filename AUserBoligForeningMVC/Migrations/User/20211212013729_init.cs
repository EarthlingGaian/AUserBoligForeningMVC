using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AUserBoligForeningMVC.Migrations.User
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calendar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentUserMail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dokumentArkivs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LejeKontrakt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndflytningsPapir = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeboerMail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dokumentArkivs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lejers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fornavn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Efternavn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostNr = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TlfNr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    By = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lejers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "regningers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Regning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeboerMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calendar = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regningers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "dokumentArkivs");

            migrationBuilder.DropTable(
                name: "lejers");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "regningers");
        }
    }
}
