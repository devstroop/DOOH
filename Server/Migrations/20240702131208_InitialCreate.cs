using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOOH.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Brand",
                schema: "dbo",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandLogo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Campaign",
                schema: "dbo",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetType = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))"),
                    Budget = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "((0))"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.CampaignId);
                });

            migrationBuilder.CreateTable(
                name: "CampaignCriteria",
                schema: "dbo",
                columns: table => new
                {
                    CampaignCriteriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinRotationPerDay = table.Column<long>(type: "bigint", nullable: true),
                    MaxRotationPerDay = table.Column<long>(type: "bigint", nullable: true),
                    MinPlaytimePerDay = table.Column<long>(type: "bigint", nullable: true),
                    MaxPlaytimePerDay = table.Column<long>(type: "bigint", nullable: true),
                    MinAdboardPerCampaign = table.Column<long>(type: "bigint", nullable: true),
                    MaxAdboardPerCampaign = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCriteria", x => x.CampaignCriteriaId);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "dbo",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Commission = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValueSql: "((0.00))"),
                    CategoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryColor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "dbo",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GSTIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                schema: "dbo",
                columns: table => new
                {
                    FaqId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.FaqId);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                schema: "dbo",
                columns: table => new
                {
                    Slag = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Slag);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                schema: "dbo",
                columns: table => new
                {
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.ProviderId);
                });

            migrationBuilder.CreateTable(
                name: "Tax",
                schema: "dbo",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValueSql: "((0.00))"),
                    ParentTaxId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tax", x => x.TaxId);
                    table.ForeignKey(
                        name: "FK_Tax_Tax_ParentTaxId",
                        column: x => x.ParentTaxId,
                        principalSchema: "dbo",
                        principalTable: "Tax",
                        principalColumn: "TaxId");
                });

            migrationBuilder.CreateTable(
                name: "UserInformation",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    SuspendedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsSuspended = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInformation", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Display",
                schema: "dbo",
                columns: table => new
                {
                    DisplayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PixelWidth = table.Column<int>(type: "int", nullable: true),
                    PixelHeight = table.Column<int>(type: "int", nullable: true),
                    ScreenWidth = table.Column<double>(type: "float", nullable: true),
                    ScreenHeight = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Display", x => x.DisplayId);
                    table.ForeignKey(
                        name: "FK_Display_Brand_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "dbo",
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Motherboard",
                schema: "dbo",
                columns: table => new
                {
                    MotherboardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cpu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rom = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motherboard", x => x.MotherboardId);
                    table.ForeignKey(
                        name: "FK_Motherboard_Brand_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "dbo",
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                schema: "dbo",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    Rotation = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((1))"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedule_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "dbo",
                        principalTable: "Campaign",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Upload",
                schema: "dbo",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AspectRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: true),
                    CodecName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    BitRate = table.Column<int>(type: "int", nullable: true),
                    FrameRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Owner = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upload", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Upload_UserInformation_Owner",
                        column: x => x.Owner,
                        principalSchema: "dbo",
                        principalTable: "UserInformation",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Adboard",
                schema: "dbo",
                columns: table => new
                {
                    AdboardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: true),
                    DisplayId = table.Column<int>(type: "int", nullable: true),
                    MotherboardId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    BaseRatePerSecond = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false, defaultValueSql: "((0.0))"),
                    Longitude = table.Column<double>(type: "float", nullable: false, defaultValueSql: "((0.0))"),
                    SignName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adboard", x => x.AdboardId);
                    table.ForeignKey(
                        name: "FK_Adboard_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Adboard_Display_DisplayId",
                        column: x => x.DisplayId,
                        principalSchema: "dbo",
                        principalTable: "Display",
                        principalColumn: "DisplayId");
                    table.ForeignKey(
                        name: "FK_Adboard_Motherboard_MotherboardId",
                        column: x => x.MotherboardId,
                        principalSchema: "dbo",
                        principalTable: "Motherboard",
                        principalColumn: "MotherboardId");
                    table.ForeignKey(
                        name: "FK_Adboard_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "dbo",
                        principalTable: "Provider",
                        principalColumn: "ProviderId");
                });

            migrationBuilder.CreateTable(
                name: "Advertisement",
                schema: "dbo",
                columns: table => new
                {
                    AdvertisementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    UploadKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisement", x => x.AdvertisementId);
                    table.ForeignKey(
                        name: "FK_Advertisement_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "dbo",
                        principalTable: "Campaign",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisement_Upload_UploadKey",
                        column: x => x.UploadKey,
                        principalSchema: "dbo",
                        principalTable: "Upload",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdboardImage",
                schema: "dbo",
                columns: table => new
                {
                    AdboardImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdboardId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdboardImage", x => x.AdboardImageId);
                    table.ForeignKey(
                        name: "FK_AdboardImage_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdboardNetwork",
                schema: "dbo",
                columns: table => new
                {
                    AdboardId = table.Column<int>(type: "int", nullable: false),
                    PhysicalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subnet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gateway = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdboardNetwork", x => x.AdboardId);
                    table.ForeignKey(
                        name: "FK_AdboardNetwork_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdboardStatus",
                schema: "dbo",
                columns: table => new
                {
                    AdboardId = table.Column<int>(type: "int", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((0))"),
                    ConnectedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    Delay = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((10000))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdboardStatus", x => x.AdboardId);
                    table.ForeignKey(
                        name: "FK_AdboardStatus_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdboardWifi",
                schema: "dbo",
                columns: table => new
                {
                    AdboardId = table.Column<int>(type: "int", nullable: false),
                    SSID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    HasConnected = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdboardWifi", x => x.AdboardId);
                    table.ForeignKey(
                        name: "FK_AdboardWifi_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignAdboard",
                schema: "dbo",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    AdboardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignAdboard", x => new { x.CampaignId, x.AdboardId });
                    table.ForeignKey(
                        name: "FK_CampaignAdboard_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignAdboard_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalSchema: "dbo",
                        principalTable: "Campaign",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleAdboard",
                schema: "dbo",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    AdboardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleAdboard", x => new { x.ScheduleId, x.AdboardId });
                    table.ForeignKey(
                        name: "FK_ScheduleAdboard_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleAdboard_Schedule_ScheduleId",
                        column: x => x.ScheduleId,
                        principalSchema: "dbo",
                        principalTable: "Schedule",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Analytic",
                schema: "dbo",
                columns: table => new
                {
                    AnalyticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdboardId = table.Column<int>(type: "int", nullable: false),
                    AdvertisementId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    StoppedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())"),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    TotalAttention = table.Column<int>(type: "int", nullable: true),
                    UniqueAttention = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(sysdatetime())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analytic", x => x.AnalyticId);
                    table.ForeignKey(
                        name: "FK_Analytic_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Analytic_Advertisement_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalSchema: "dbo",
                        principalTable: "Advertisement",
                        principalColumn: "AdvertisementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Billing",
                schema: "dbo",
                columns: table => new
                {
                    BillingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalyticId = table.Column<int>(type: "int", nullable: false),
                    AdvertisementId = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    TotalDuration = table.Column<int>(type: "int", nullable: true),
                    RatePerSecond = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Taxable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RoundOff = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Payable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billing", x => x.BillingId);
                    table.ForeignKey(
                        name: "FK_Billing_Analytic_AnalyticId",
                        column: x => x.AnalyticId,
                        principalSchema: "dbo",
                        principalTable: "Analytic",
                        principalColumn: "AnalyticId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Earning",
                schema: "dbo",
                columns: table => new
                {
                    EarningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    AnalyticId = table.Column<int>(type: "int", nullable: false),
                    AdboardId = table.Column<int>(type: "int", nullable: false),
                    TotalDuration = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))"),
                    RatePerSecond = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmountBeforeTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EarningPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EarningAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Earning", x => x.EarningId);
                    table.ForeignKey(
                        name: "FK_Earning_Adboard_AdboardId",
                        column: x => x.AdboardId,
                        principalSchema: "dbo",
                        principalTable: "Adboard",
                        principalColumn: "AdboardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Earning_Analytic_AnalyticId",
                        column: x => x.AnalyticId,
                        principalSchema: "dbo",
                        principalTable: "Analytic",
                        principalColumn: "AnalyticId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Earning_Provider_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "dbo",
                        principalTable: "Provider",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adboard_CategoryId",
                schema: "dbo",
                table: "Adboard",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Adboard_DisplayId",
                schema: "dbo",
                table: "Adboard",
                column: "DisplayId");

            migrationBuilder.CreateIndex(
                name: "IX_Adboard_MotherboardId",
                schema: "dbo",
                table: "Adboard",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Adboard_ProviderId",
                schema: "dbo",
                table: "Adboard",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_AdboardImage_AdboardId",
                schema: "dbo",
                table: "AdboardImage",
                column: "AdboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_CampaignId",
                schema: "dbo",
                table: "Advertisement",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_UploadKey",
                schema: "dbo",
                table: "Advertisement",
                column: "UploadKey");

            migrationBuilder.CreateIndex(
                name: "IX_Analytic_AdboardId",
                schema: "dbo",
                table: "Analytic",
                column: "AdboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Analytic_AdvertisementId",
                schema: "dbo",
                table: "Analytic",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_Billing_AnalyticId",
                schema: "dbo",
                table: "Billing",
                column: "AnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignAdboard_AdboardId",
                schema: "dbo",
                table: "CampaignAdboard",
                column: "AdboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Display_BrandId",
                schema: "dbo",
                table: "Display",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Earning_AdboardId",
                schema: "dbo",
                table: "Earning",
                column: "AdboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Earning_AnalyticId",
                schema: "dbo",
                table: "Earning",
                column: "AnalyticId");

            migrationBuilder.CreateIndex(
                name: "IX_Earning_ProviderId",
                schema: "dbo",
                table: "Earning",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboard_BrandId",
                schema: "dbo",
                table: "Motherboard",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_CampaignId",
                schema: "dbo",
                table: "Schedule",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleAdboard_AdboardId",
                schema: "dbo",
                table: "ScheduleAdboard",
                column: "AdboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Tax_ParentTaxId",
                schema: "dbo",
                table: "Tax",
                column: "ParentTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Upload_Owner",
                schema: "dbo",
                table: "Upload",
                column: "Owner");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdboardImage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AdboardNetwork",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AdboardStatus",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AdboardWifi",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Billing",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CampaignAdboard",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CampaignCriteria",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Earning",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Faq",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Page",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ScheduleAdboard",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Tax",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Analytic",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Schedule",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Adboard",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Advertisement",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Display",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Motherboard",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Provider",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Campaign",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Upload",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Brand",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserInformation",
                schema: "dbo");
        }
    }
}
