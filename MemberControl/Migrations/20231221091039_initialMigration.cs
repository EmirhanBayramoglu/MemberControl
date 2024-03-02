using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembersControlSystem.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    companyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    catchPhrase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bs = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.companyId);
                });

            migrationBuilder.CreateTable(
                name: "Geos",
                columns: table => new
                {
                    geoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lat = table.Column<float>(type: "real", nullable: false),
                    lng = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geos", x => x.geoId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    addressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    streetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    suiteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zipcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    geoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.addressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Geos_geoId",
                        column: x => x.geoId,
                        principalTable: "Geos",
                        principalColumn: "geoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    memberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    memberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    memberEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    memberPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    webSite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    addressId = table.Column<int>(type: "int", nullable: false),
                    companyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.memberId);
                    table.ForeignKey(
                        name: "FK_Members_Addresses_addressId",
                        column: x => x.addressId,
                        principalTable: "Addresses",
                        principalColumn: "addressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Companies_companyId",
                        column: x => x.companyId,
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "25cb7949-2182-4e7e-8d6d-8da7434b1350", "5c6820a5-af44-4b11-95f5-cf35a685a3f8", "User", "USER" },
                    { "58a50b2e-7b19-4ea0-a938-3acf0c153349", "5f4085e5-8e40-4db3-a5df-0c1df8c1d7eb", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "companyId", "bs", "catchPhrase", "companyName" },
                values: new object[,]
                {
                    { 1, "harness real-time e-markets", "Multi-layered client-server neural-net", "Romaguera-Crona" },
                    { 2, "synergize scalable supply-chains", "Proactive didactic contingency", "Deckow-Crist" },
                    { 3, "e-enable strategic applications", "Face to face bifurcated interface", "Romaguera-Jacobson" },
                    { 4, "transition cutting-edge web services", "Multi-tiered zero tolerance productivity", "Robel-Corkery" },
                    { 5, "revolutionize end-to-end systems", "User-centric fault-tolerant solution", "Keebler LLC" },
                    { 6, "e-enable innovative applications", "Synchronised bottom-line interface", "Considine-Lockman" },
                    { 7, "generate enterprise e-tailers", "Configurable multimedia task-force", "Johns Group" },
                    { 8, "e-enable extensible e-tailers", "Implemented secondary concept", "Abernathy Group" },
                    { 9, "aggregate real-time technologies", "Switchable contextually-based project", "Yost and Sons" },
                    { 10, "target end-to-end models", "Centralized empowering task-force", "Hoeger LLC" }
                });

            migrationBuilder.InsertData(
                table: "Geos",
                columns: new[] { "geoId", "lat", "lng" },
                values: new object[,]
                {
                    { 1, -37.3159f, 81.1496f },
                    { 2, -43.9509f, -34.4618f },
                    { 3, -68.6102f, -47.0653f },
                    { 4, 29.4572f, -164.299f },
                    { 5, -31.8129f, 62.5342f },
                    { 6, -71.4197f, 71.7478f },
                    { 7, 24.8918f, 21.8984f },
                    { 8, -14.399f, -120.7677f },
                    { 9, 24.6463f, -168.8889f },
                    { 10, -38.2386f, 57.2232f }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "addressId", "cityName", "geoId", "streetName", "suiteName", "zipcode" },
                values: new object[,]
                {
                    { 1, "Gwenborough", 1, "Kulas Light", "Apt. 556", "92998-3874" },
                    { 2, "Wisokyburgh", 2, "Victor Plains", "suiteName 879", "90566-7771" },
                    { 3, "McKenziehaven", 3, "Douglas Extension", "suiteName 847", "59590-4157" },
                    { 4, "South Elvis", 4, "Hoeger Mall", "Apt. 692", "53919-4257" },
                    { 5, "Roscoeview", 5, "Skiles Walks", "suiteName 351", "33263" },
                    { 6, "South Christy", 6, "Norberto Crossing", "Apt. 950", "23505-1337" },
                    { 7, "Howemouth", 7, "Rex Trail", "suiteName 280", "58804-1099" },
                    { 8, "Aliyaview", 8, "Ellsworth Summit", "suiteName 729", "45169" },
                    { 9, "Bartholomebury", 9, "Dayna Park", "suiteName 449", "76495-3109" },
                    { 10, "Lebsackbury", 10, "Kattie Turnpike", "suiteName 198", "31428-2261" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "memberId", "addressId", "companyId", "memberEmail", "memberName", "memberPhoneNumber", "password", "userName", "webSite" },
                values: new object[,]
                {
                    { 1, 1, 1, "Sincere@april.biz", "Leanne Graham", "1-770-736-8031 x56442", "mahmut123", "Bret", "hildegard.org" },
                    { 2, 2, 2, "Shanna@melissa.tv", "Ervin Howell", "010-692-6593 x09125", "mahmut123", "Antonette", "anastasia.net" },
                    { 3, 3, 3, "Nathan@yesenia.net", "Clementine Bauch", "1-463-123-4447", "mahmut123", "Samantha", "ramiro.info" },
                    { 4, 4, 4, "Julianne.OConner@kory.org", "Patricia Lebsack", "493-170-9623 x156", "mahmut123", "Karianne", "kale.biz" },
                    { 5, 5, 5, "Lucio_Hettinger@annie.ca", "Chelsey Dietrich", "(254)954-1289", "mahmut123", "Kamren", "demarco.info" },
                    { 6, 6, 6, "Karley_Dach@jasper.info", "Mrs. Dennis Schulist", "1-477-935-8478 x6430", "mahmut123", "Leopoldo_Corkery", "ola.org" },
                    { 7, 7, 7, "Telly.Hoeger@billy.biz", "Kurtis Weissnat", "210.067.6132", "mahmut123", "Elwyn.Skiles", "elvis.io" },
                    { 8, 8, 8, "Sherwood@rosamond.me", "Nicholas Runolfsdottir V", "586.493.6943 x140", "mahmut123", "Maxime_Nienow", "jacynthe.com" },
                    { 9, 9, 9, "Chaim_McDermott@dana.io", "Glenna Reichert", "(775)976-6794 x41206", "mahmut123", "Delphine", "conrad.com" },
                    { 10, 10, 10, "Rey.Padberg@karina.biz", "Clementina DuBuque", "024-648-3804", "mahmut123", "Moriah.Stanton", "ambrose.net" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_geoId",
                table: "Addresses",
                column: "geoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Members_addressId",
                table: "Members",
                column: "addressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_companyId",
                table: "Members",
                column: "companyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_userName",
                table: "Members",
                column: "userName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Geos");
        }
    }
}
