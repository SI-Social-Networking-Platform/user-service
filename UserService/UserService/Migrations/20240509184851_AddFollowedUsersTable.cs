using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowedUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowedUsers",
                columns: table => new
                {
                    FollowerId = table.Column<int>(type: "integer", nullable: false),
                    FollowedId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedUsers", x => new { x.FollowerId, x.FollowedId });
                    table.ForeignKey(
                        name: "FK_FollowedUsers_Users_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowedUsers_Users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowedUsers_FollowedId",
                table: "FollowedUsers",
                column: "FollowedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowedUsers");
        }
    }
}
