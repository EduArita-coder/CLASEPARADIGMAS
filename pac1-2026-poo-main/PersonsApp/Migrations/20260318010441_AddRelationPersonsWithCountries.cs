using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationPersonsWithCountries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_persons_country_id",
                table: "persons",
                column: "country_id");

            migrationBuilder.AddForeignKey(
                name: "FK_persons_countries_country_id",
                table: "persons",
                column: "country_id",
                principalTable: "countries",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_persons_countries_country_id",
                table: "persons");

            migrationBuilder.DropIndex(
                name: "IX_persons_country_id",
                table: "persons");
        }
    }
}
