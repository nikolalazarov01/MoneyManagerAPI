using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class FixTransactionInfoTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountInfos_Accounts_AccountId",
                table: "AccountInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountInfos",
                table: "AccountInfos");

            migrationBuilder.RenameTable(
                name: "AccountInfos",
                newName: "TransactionInfos");

            migrationBuilder.RenameIndex(
                name: "IX_AccountInfos_AccountId",
                table: "TransactionInfos",
                newName: "IX_TransactionInfos_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionInfos",
                table: "TransactionInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionInfos_Accounts_AccountId",
                table: "TransactionInfos",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionInfos_Accounts_AccountId",
                table: "TransactionInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionInfos",
                table: "TransactionInfos");

            migrationBuilder.RenameTable(
                name: "TransactionInfos",
                newName: "AccountInfos");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionInfos_AccountId",
                table: "AccountInfos",
                newName: "IX_AccountInfos_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountInfos",
                table: "AccountInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountInfos_Accounts_AccountId",
                table: "AccountInfos",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
