using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMate.web.Migrations
{
    /// <inheritdoc />
    public partial class add_Copy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCopies");

            migrationBuilder.AddColumn<int>(
                name: "Copy",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Copy",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookCopies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditionNumber = table.Column<int>(type: "int", nullable: false),
                    IsAvailableForRental = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SerialNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCopies_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookCopies_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookCopies_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_BookId",
                table: "BookCopies",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_CreatedById",
                table: "BookCopies",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_LastUpdatedById",
                table: "BookCopies",
                column: "LastUpdatedById");
        }
    }
}
