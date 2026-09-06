using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoZeroDays.Api.Migrations.App;
    /// <inheritdoc />
    public partial class Add_UserId_Reference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql(
                """
                DELETE FROM no_zero_days.habit_tags;
                DELETE FROM no_zero_days.habits;
                DELETE FROM no_zero_days.tags;
                """
            );
            
            migrationBuilder.DropIndex(
                name: "ix_tags_name",
                schema: "no_zero_days",
                table: "tags");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                schema: "no_zero_days",
                table: "tags",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                schema: "no_zero_days",
                table: "habits",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_tags_user_id_name",
                schema: "no_zero_days",
                table: "tags",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_habits_user_id",
                schema: "no_zero_days",
                table: "habits",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_habits_users_user_id",
                schema: "no_zero_days",
                table: "habits",
                column: "user_id",
                principalSchema: "no_zero_days",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tags_users_user_id",
                schema: "no_zero_days",
                table: "tags",
                column: "user_id",
                principalSchema: "no_zero_days",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_habits_users_user_id",
                schema: "no_zero_days",
                table: "habits");

            migrationBuilder.DropForeignKey(
                name: "fk_tags_users_user_id",
                schema: "no_zero_days",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "ix_tags_user_id_name",
                schema: "no_zero_days",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "ix_habits_user_id",
                schema: "no_zero_days",
                table: "habits");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "no_zero_days",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "no_zero_days",
                table: "habits");

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                schema: "no_zero_days",
                table: "tags",
                column: "name",
                unique: true);
        }
    }
