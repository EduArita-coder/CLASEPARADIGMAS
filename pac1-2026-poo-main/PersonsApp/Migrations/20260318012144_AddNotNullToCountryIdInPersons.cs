using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonsApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNotNullToCountryIdInPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_persons_countries_country_id",
                table: "persons");

            migrationBuilder.AlterColumn<string>(
                name: "country_id",
                table: "persons",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_persons_countries_country_id",
                table: "persons",
                column: "country_id",
                principalTable: "countries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_persons_countries_country_id",
                table: "persons");

            migrationBuilder.AlterColumn<string>(
                name: "country_id",
                table: "persons",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_persons_countries_country_id",
                table: "persons",
                column: "country_id",
                principalTable: "countries",
                principalColumn: "id");
        }
    }
}
