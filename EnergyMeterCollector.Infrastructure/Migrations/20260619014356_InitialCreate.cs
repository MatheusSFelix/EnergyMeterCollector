using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyMeterCollector.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medidores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NumeroSerie = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medidores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leituras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MedidorId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TensaoVolts = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    CorrenteAmperes = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false),
                    EnergiaAtivaKwh = table.Column<decimal>(type: "TEXT", precision: 18, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leituras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leituras_Medidores_MedidorId",
                        column: x => x.MedidorId,
                        principalTable: "Medidores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leituras_MedidorId_Timestamp",
                table: "Leituras",
                columns: new[] { "MedidorId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Medidores_NumeroSerie",
                table: "Medidores",
                column: "NumeroSerie",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leituras");

            migrationBuilder.DropTable(
                name: "Medidores");
        }
    }
}
