using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlturaCMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Metadata");

            migrationBuilder.CreateTable(
                name: "Contents",
                schema: "Metadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.CheckConstraint("CK_BaseEntity_Deleted", "[IsDeleted] = 0 OR ([DeletedDate] IS NOT NULL AND [DeletedBy] IS NOT NULL)");
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                schema: "Metadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.CheckConstraint("CK_BaseEntity_Deleted2", "[IsDeleted] = 0 OR ([DeletedDate] IS NOT NULL AND [DeletedBy] IS NOT NULL)");
                });

            migrationBuilder.CreateTable(
                name: "ContentFields",
                schema: "Metadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsUnique = table.Column<bool>(type: "bit", nullable: false),
                    MinLength = table.Column<int>(type: "int", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    MinValue = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    MaxValue = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    MinDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegexPattern = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AllowedValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceTableName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferenceDisplayFieldName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFields", x => x.Id);
                    table.CheckConstraint("CK_BaseEntity_Deleted1", "[IsDeleted] = 0 OR ([DeletedDate] IS NOT NULL AND [DeletedBy] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_ContentFields_Contents_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "Metadata",
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormFields",
                schema: "Metadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    MinLength = table.Column<int>(type: "int", nullable: true),
                    MaxLength = table.Column<int>(type: "int", nullable: true),
                    MinValue = table.Column<int>(type: "int", precision: 18, scale: 8, nullable: true),
                    MaxValue = table.Column<int>(type: "int", precision: 18, scale: 8, nullable: true),
                    RegexPattern = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AllowedValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormFields", x => x.Id);
                    table.CheckConstraint("CK_BaseEntity_Deleted3", "[IsDeleted] = 0 OR ([DeletedDate] IS NOT NULL AND [DeletedBy] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_FormFields_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "Metadata",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentFields_ContentId",
                schema: "Metadata",
                table: "ContentFields",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_FormId",
                schema: "Metadata",
                table: "FormFields",
                column: "FormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentFields",
                schema: "Metadata");

            migrationBuilder.DropTable(
                name: "FormFields",
                schema: "Metadata");

            migrationBuilder.DropTable(
                name: "Contents",
                schema: "Metadata");

            migrationBuilder.DropTable(
                name: "Forms",
                schema: "Metadata");
        }
    }
}
