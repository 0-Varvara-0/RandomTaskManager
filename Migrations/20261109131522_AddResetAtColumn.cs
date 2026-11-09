using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomTaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddResetAtColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResetAt",
                table: "UserLimits",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Period" },
                values: new object[] { "Быстрый разгон крови", 3 });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Difficulty", "Period", "Title" },
                values: new object[] { 1, "Поддержание водного баланса", 1, 3, "Выпить 2 литра воды" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "Difficulty", "Period", "Title" },
                values: new object[] { 2, "Любая художественная или учебная литература", 2, 4, "Прочитать 100 страниц книги" });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Difficulty", "DurationMinutes", "Period", "Status", "Title", "UserId" },
                values: new object[] { 4, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Завершить все лекции и практические задания", 3, 0, 5, 0, "Пройти модуль онлайн-курса", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "ResetAt",
                table: "UserLimits");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Быстрый разгон крови и бодрость на день");

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Difficulty", "Title" },
                values: new object[] { 2, "Любая художественная или техническая литература", 2, "Прочитать 15 страниц книги" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "Description", "Difficulty", "Title" },
                values: new object[] { 3, "Удалить ненужные файлы и рассортировать загрузки", 1, "Разобрать рабочий стол" });
        }
    }
}
