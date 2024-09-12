using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTodo.App.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnRoleToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "varchar(5)",
                defaultValue: "User"
            );

            migrationBuilder.AddCheckConstraint(
                name: "CK_Role_User_Admin",
                table: "Users",
                sql: "Role = 'User' or Role = 'Admin'"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Role", table: "Users");
            migrationBuilder.DropCheckConstraint(name: "CK_Role_User_Admin", table: "Users");
        }
    }
}
