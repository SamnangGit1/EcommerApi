using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eletronic_Api.Migrations
{
    /// <inheritdoc />
    public partial class initailAppuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Brands_TargetID",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Categories_TargetID",
                table: "Promotions");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Items_TargetID",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_TargetID",
                table: "Promotions");

            migrationBuilder.AlterColumn<string>(
                name: "PromotionType",
                table: "Promotions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AddressType",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HouseNo",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AppUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "HouseNo",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AppUsers");

            migrationBuilder.UpdateData(
                table: "Promotions",
                keyColumn: "PromotionType",
                keyValue: null,
                column: "PromotionType",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PromotionType",
                table: "Promotions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_TargetID",
                table: "Promotions",
                column: "TargetID");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Brands_TargetID",
                table: "Promotions",
                column: "TargetID",
                principalTable: "Brands",
                principalColumn: "BrandID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Categories_TargetID",
                table: "Promotions",
                column: "TargetID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Items_TargetID",
                table: "Promotions",
                column: "TargetID",
                principalTable: "Items",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
