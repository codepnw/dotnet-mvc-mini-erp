using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniERP.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class TblLeaveTypeInitData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequest_Employees_EmployeeId",
                table: "LeaveRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequest_LeaveType_LeaveTypeId",
                table: "LeaveRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveType",
                table: "LeaveType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveRequest",
                table: "LeaveRequest");

            migrationBuilder.RenameTable(
                name: "LeaveType",
                newName: "LeaveTypes");

            migrationBuilder.RenameTable(
                name: "LeaveRequest",
                newName: "LeaveRequests");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveType_Title",
                table: "LeaveTypes",
                newName: "IX_LeaveTypes_Title");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequest_LeaveTypeId",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_LeaveTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequest_EmployeeId",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveRequests",
                table: "LeaveRequests",
                column: "Id");

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "IsDeleted", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, false, "Sick Leave", null },
                    { 2, false, "Vacation Leave", null },
                    { 3, false, "Personal Leave", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Employees_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Employees_EmployeeId",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveRequests",
                table: "LeaveRequests");

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "LeaveTypes",
                newName: "LeaveType");

            migrationBuilder.RenameTable(
                name: "LeaveRequests",
                newName: "LeaveRequest");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveTypes_Title",
                table: "LeaveType",
                newName: "IX_LeaveType_Title");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequest",
                newName: "IX_LeaveRequest_LeaveTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequest",
                newName: "IX_LeaveRequest_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveType",
                table: "LeaveType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveRequest",
                table: "LeaveRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequest_Employees_EmployeeId",
                table: "LeaveRequest",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequest_LeaveType_LeaveTypeId",
                table: "LeaveRequest",
                column: "LeaveTypeId",
                principalTable: "LeaveType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
