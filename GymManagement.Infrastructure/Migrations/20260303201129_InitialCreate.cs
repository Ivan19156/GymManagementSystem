using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__3213E83F3FBFB0EA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__3213E83F685A2642", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SessionS__3213E83F09D3A44E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Speciali__3213E83FDAB2530F", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Training__3213E83F6E580E8E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3213E83F0AEA0218", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membersh__3213E83F3E1F5BB7", x => x.Id);
                    table.ForeignKey(
                        name: "fk_membership_plans_type",
                        column: x => x.TypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DefaultDuration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Training__3213E83F8711FA15", x => x.Id);
                    table.ForeignKey(
                        name: "fk_training_types_category",
                        column: x => x.CategoryId,
                        principalTable: "TrainingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccessLevel = table.Column<int>(type: "int", nullable: true, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admins__B9BE370F5EAC4B29", x => x.UserId);
                    table.ForeignKey(
                        name: "fk_admins_user",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MedicalNotes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clients__B9BE370F76341260", x => x.UserId);
                    table.ForeignKey(
                        name: "fk_clients_user",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SpecializationId = table.Column<int>(type: "int", nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Trainers__B9BE370FEA91E35D", x => x.UserId);
                    table.ForeignKey(
                        name: "fk_trainers_specialization",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_trainers_user",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMemberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    MembershipPlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    SessionsUsed = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserMemb__3213E83FFEE12C1D", x => x.Id);
                    table.ForeignKey(
                        name: "fk_user_memberships_client",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "fk_user_memberships_plan",
                        column: x => x.MembershipPlanId,
                        principalTable: "MembershipPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_user_memberships_status",
                        column: x => x.StatusId,
                        principalTable: "MembershipStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScheduledSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingTypeId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SessionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedule__3213E83F39228270", x => x.Id);
                    table.ForeignKey(
                        name: "fk_sessions_client",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "fk_sessions_status",
                        column: x => x.StatusId,
                        principalTable: "SessionStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "fk_sessions_trainer",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_sessions_training_type",
                        column: x => x.TrainingTypeId,
                        principalTable: "TrainingTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MembershipPlans_TypeId",
                table: "MembershipPlans",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipStatuses_Name",
                table: "MembershipStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipTypes_Name",
                table: "MembershipTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_sessions_client",
                table: "ScheduledSessions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "idx_sessions_date_time",
                table: "ScheduledSessions",
                columns: new[] { "SessionDate", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "idx_sessions_trainer",
                table: "ScheduledSessions",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSessions_StatusId",
                table: "ScheduledSessions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledSessions_TrainingTypeId",
                table: "ScheduledSessions",
                column: "TrainingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionStatuses_Name",
                table: "SessionStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Name",
                table: "Specializations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_SpecializationId",
                table: "Trainers",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCategories_Name",
                table: "TrainingCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTypes_CategoryId",
                table: "TrainingTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTypes_Name",
                table: "TrainingTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMemberships_ClientId",
                table: "UserMemberships",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMemberships_MembershipPlanId",
                table: "UserMemberships",
                column: "MembershipPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMemberships_StatusId",
                table: "UserMemberships",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ScheduledSessions");

            migrationBuilder.DropTable(
                name: "UserMemberships");

            migrationBuilder.DropTable(
                name: "SessionStatuses");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "TrainingTypes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "MembershipPlans");

            migrationBuilder.DropTable(
                name: "MembershipStatuses");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "TrainingCategories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MembershipTypes");
        }
    }
}
