using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeeksForLess.Migrations
{
    /// <inheritdoc />
    public partial class addKeyToFolderModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChildFoldersIds",
                table: "Folder",
                newName: "ChildFoldersKeys");

            migrationBuilder.AddColumn<int>(
                name: "FolderKey",
                table: "Folder",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FolderKey",
                table: "Folder");

            migrationBuilder.RenameColumn(
                name: "ChildFoldersKeys",
                table: "Folder",
                newName: "ChildFoldersIds");
        }
    }
}
