using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Aurigma.DirectMail.Sample.DAL.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "RecipientImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Path = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientImage", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "RecipientList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipientList", x => x.Id);
                }
            );

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(
                        type: "character varying(256)",
                        maxLength: 256,
                        nullable: true
                    ),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaign_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Recipient",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Signature = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    AddressLine1 = table.Column<string>(type: "text", nullable: true),
                    AddressLine2 = table.Column<string>(type: "text", nullable: true),
                    Zip = table.Column<string>(type: "text", nullable: true),
                    ReturnState = table.Column<string>(type: "text", nullable: true),
                    ReturnCity = table.Column<string>(type: "text", nullable: true),
                    ReturnZip = table.Column<string>(type: "text", nullable: true),
                    ReturnAddressLine1 = table.Column<string>(type: "text", nullable: true),
                    ReturnAddressLine2 = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    FsCode = table.Column<string>(type: "text", nullable: true),
                    QRCoreUrl = table.Column<string>(type: "text", nullable: true),
                    RecipientListId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipient_RecipientList_RecipientListId",
                        column: x => x.RecipientListId,
                        principalTable: "RecipientList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "LineItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TemplateId = table.Column<string>(type: "text", nullable: true),
                    DesignId = table.Column<string>(type: "text", nullable: true),
                    ProductVariantId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineItem_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_LineItem_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "RecipientListCampaign",
                columns: table => new
                {
                    RecipientListId = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_RecipientListCampaign",
                        x => new { x.RecipientListId, x.CampaignId }
                    );
                    table.ForeignKey(
                        name: "FK_RecipientListCampaign_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_RecipientListCampaign_RecipientList_RecipientListId",
                        column: x => x.RecipientListId,
                        principalTable: "RecipientList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "RecipientImageRecipientDal",
                columns: table => new
                {
                    RecipientId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientImageId = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_RecipientImageRecipientDal",
                        x => new { x.RecipientId, x.RecipientImageId }
                    );
                    table.ForeignKey(
                        name: "FK_RecipientImageRecipientDal_RecipientImage_RecipientImageId",
                        column: x => x.RecipientImageId,
                        principalTable: "RecipientImage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_RecipientImageRecipientDal_Recipient_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Recipient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomersCanvasProjectId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LineItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    LastModificationTime = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Job_LineItem_LineItemId",
                        column: x => x.LineItemId,
                        principalTable: "LineItem",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CreationTime", "LastModificationTime", "Title" },
                values: new object[,]
                {
                    {
                        new Guid("cfd22d2b-cf98-4c72-8992-c791790c90bd"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        "Letters",
                    },
                    {
                        new Guid("dd4d7f75-a6a7-45d2-a74e-d2d686719e96"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        "Selfmailers",
                    },
                    {
                        new Guid("f95777aa-16e5-4e49-a464-b86139e861ab"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        "Postcards",
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "CreationTime", "LastModificationTime", "Name" },
                values: new object[,]
                {
                    {
                        new Guid("28394c53-2b8a-48c3-96de-b0c066b3aa9a"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        "Aurigma",
                    },
                    {
                        new Guid("6e3f0333-01e9-4e74-8a2f-bbe1e2d5b5fc"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        "Some Company",
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "RecipientImage",
                columns: new[] { "Id", "CreationTime", "LastModificationTime", "Name", "Path" },
                values: new object[]
                {
                    new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                    null,
                    "image-1.png",
                    "App_Data/PrivateImages/image-1.png",
                }
            );

            migrationBuilder.InsertData(
                table: "RecipientList",
                columns: new[] { "Id", "CreationTime", "LastModificationTime", "Title" },
                values: new object[]
                {
                    new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                    new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                    null,
                    "Recipients",
                }
            );

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[]
                {
                    "Id",
                    "CategoryId",
                    "CreationTime",
                    "LastModificationTime",
                    "Price",
                    "Title",
                },
                values: new object[,]
                {
                    {
                        new Guid("8b844daf-a3cf-4b4c-9758-79404b903379"),
                        new Guid("cfd22d2b-cf98-4c72-8992-c791790c90bd"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        55.9m,
                        "Letter",
                    },
                    {
                        new Guid("d7f28121-9b3a-4927-87f7-97a6f9c79381"),
                        new Guid("dd4d7f75-a6a7-45d2-a74e-d2d686719e96"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        65.9m,
                        "Selfmailer",
                    },
                    {
                        new Guid("e5f8b300-83e7-4754-af1e-86a9946e303e"),
                        new Guid("f95777aa-16e5-4e49-a464-b86139e861ab"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        55.9m,
                        "Folded Card",
                    },
                    {
                        new Guid("fe9971da-6721-478c-8256-5d10951ee7f8"),
                        new Guid("f95777aa-16e5-4e49-a464-b86139e861ab"),
                        new DateTime(2024, 2, 22, 4, 33, 56, 146, DateTimeKind.Utc),
                        null,
                        65.9m,
                        "Postcard",
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "Recipient",
                columns: new[]
                {
                    "Id",
                    "AddressLine1",
                    "AddressLine2",
                    "City",
                    "CreationTime",
                    "FirstName",
                    "FsCode",
                    "FullName",
                    "LastModificationTime",
                    "QRCoreUrl",
                    "RecipientListId",
                    "ReturnAddressLine1",
                    "ReturnAddressLine2",
                    "ReturnCity",
                    "ReturnState",
                    "ReturnZip",
                    "Signature",
                    "State",
                    "Title",
                    "Zip",
                },
                values: new object[,]
                {
                    {
                        new Guid("061bd192-a37a-4996-b9af-cae96fc9fcbc"),
                        "04616 Valley Edge Trail",
                        "Apt 137",
                        "San Francisco",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Jeffie",
                        "21695-939",
                        "Jeffie Langdridge",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "California",
                        "Customer's Canvas CEO",
                        "94159",
                    },
                    {
                        new Guid("23a6a26a-a655-49b5-97a2-9e4168ab8d3d"),
                        "3672 Sutteridge Point",
                        "Room 1746",
                        "Tacoma",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Anna-diana",
                        "16590-372",
                        "Anna-diana Clynmans",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Washington",
                        "Customer's Canvas CEO",
                        "98442",
                    },
                    {
                        new Guid("3df4fa1f-78e3-4972-8332-7f52e4e8654a"),
                        "8 Gerald Plaza",
                        "Apt 1031",
                        "Tyler",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Perren",
                        "21695-939",
                        "Perren Ivanishev",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Texas",
                        "Customer's Canvas CEO",
                        "75710",
                    },
                    {
                        new Guid("4fc6ffdb-ed5d-4423-8b1a-09ed8f2cf568"),
                        "27 Onsgard Plaza",
                        "Apt 975",
                        "Springfield",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Tremain",
                        "21695-939",
                        "Tremain St. Aubyn",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Illinois",
                        "Customer's Canvas CEO",
                        "62723",
                    },
                    {
                        new Guid("71de1460-5cfe-4338-a69c-b4b36627b018"),
                        "628 Vahlen Plaza",
                        "PO Box 26324",
                        "Yonkers",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Cory",
                        "16590-372",
                        "Cory Merricks",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "New York",
                        "Customer's Canvas CEO",
                        "10705",
                    },
                    {
                        new Guid("7c6d555d-b18b-407c-afad-886d9fe8be26"),
                        "909 Mallard Plaza",
                        "Suite 4",
                        "Las Vegas",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Ceciley",
                        "21695-939",
                        "Ceciley Crudginton",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Nevada",
                        "Customer's Canvas CEO",
                        "89193",
                    },
                    {
                        new Guid("7dccb8e6-eef0-4684-9361-f4441d827e60"),
                        "616 Superior Terrace",
                        "PO Box 99333",
                        "Louisville",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Patsy",
                        "21695-939",
                        "Patsy Hocking",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Kentucky",
                        "Customer's Canvas CEO",
                        "40298",
                    },
                    {
                        new Guid("84267050-2581-44f2-bc35-f203cbfe3b79"),
                        "0 5th Crossing",
                        "2nd Floor",
                        "Scottsdale",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Davis",
                        "16590-372",
                        "Davis Troak",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Arizona",
                        "Customer's Canvas CEO",
                        "85260",
                    },
                    {
                        new Guid("98042e62-8073-486d-8bd6-b60a8ae3253d"),
                        "52 Mariners Cove Crossing",
                        "Apt 730",
                        "Shreveport",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Franni",
                        "21695-939",
                        "Franni De Haven",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Louisiana",
                        "Customer's Canvas CEO",
                        "71151",
                    },
                    {
                        new Guid("b3f39871-59cc-480b-8dfa-ad85c19cdf85"),
                        "75420 Shopko Center",
                        "Suite 6",
                        "Baton Rouge",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Raul",
                        "16590-372",
                        "Raul Garioch",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Louisiana",
                        "Customer's Canvas CEO",
                        "70826",
                    },
                    {
                        new Guid("bca1aab6-1212-403b-8d37-4ee768dbc73a"),
                        "4896 Tomscot Court",
                        "Room 1808",
                        "Jacksonville",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Vivi",
                        "21695-939",
                        "Vivi Wardrope",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Florida",
                        "Customer's Canvas CEO",
                        "32277",
                    },
                    {
                        new Guid("c169b56b-5cd5-4625-aa5e-a6d393e87e89"),
                        "2018 La Follette Crossing",
                        "1st Floor",
                        "Shawnee Mission",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Marley",
                        "21695-939",
                        "Marley Retchford",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Kansas",
                        "Customer's Canvas CEO",
                        "66276",
                    },
                    {
                        new Guid("dc631b26-ae82-435e-a798-67cac1270153"),
                        "71207 Eliot Circle",
                        "Suite 96",
                        "Columbus",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Lavinie",
                        "16590-372",
                        "Lavinie Torrecilla",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Ohio",
                        "Customer's Canvas CEO",
                        "43215",
                    },
                    {
                        new Guid("dea4f26a-6559-46b2-8f7e-40adebe6e28d"),
                        "26031 6th Street",
                        "1st Floor",
                        "Taber",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Katherine",
                        "16590-372",
                        "Katherine Freebury",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Alberta",
                        "Customer's Canvas CEO",
                        "T1G",
                    },
                    {
                        new Guid("f8fe101f-ab09-41f9-8f37-222222bf9c34"),
                        "64612 Menomonie Parkway",
                        "Suite 58",
                        "Savannah",
                        new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc),
                        "Dania",
                        "16590-372",
                        "Dania Roberds",
                        null,
                        "https://customerscanvas.com/",
                        new Guid("5146239e-aa2f-4797-a250-1ddeed5dee53"),
                        "66 Canal Center Plaza",
                        " Suite 505",
                        "Alexandria",
                        "Virginia",
                        "22314",
                        "Dmitry Sevostyanov",
                        "Georgia",
                        "Customer's Canvas CEO",
                        "31416",
                    },
                }
            );

            migrationBuilder.InsertData(
                table: "RecipientImageRecipientDal",
                columns: new[] { "RecipientId", "RecipientImageId" },
                values: new object[,]
                {
                    {
                        new Guid("061bd192-a37a-4996-b9af-cae96fc9fcbc"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("23a6a26a-a655-49b5-97a2-9e4168ab8d3d"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("3df4fa1f-78e3-4972-8332-7f52e4e8654a"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("4fc6ffdb-ed5d-4423-8b1a-09ed8f2cf568"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("71de1460-5cfe-4338-a69c-b4b36627b018"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("7c6d555d-b18b-407c-afad-886d9fe8be26"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("7dccb8e6-eef0-4684-9361-f4441d827e60"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("84267050-2581-44f2-bc35-f203cbfe3b79"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("98042e62-8073-486d-8bd6-b60a8ae3253d"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("b3f39871-59cc-480b-8dfa-ad85c19cdf85"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("bca1aab6-1212-403b-8d37-4ee768dbc73a"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("c169b56b-5cd5-4625-aa5e-a6d393e87e89"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("dc631b26-ae82-435e-a798-67cac1270153"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("dea4f26a-6559-46b2-8f7e-40adebe6e28d"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                    {
                        new Guid("f8fe101f-ab09-41f9-8f37-222222bf9c34"),
                        new Guid("21a24e53-f58d-3252-f652-fb48370e1539"),
                    },
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_CompanyId",
                table: "Campaign",
                column: "CompanyId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Job_LineItemId",
                table: "Job",
                column: "LineItemId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_LineItem_CampaignId",
                table: "LineItem",
                column: "CampaignId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_LineItem_ProductId",
                table: "LineItem",
                column: "ProductId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Recipient_RecipientListId",
                table: "Recipient",
                column: "RecipientListId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RecipientImageRecipientDal_RecipientImageId",
                table: "RecipientImageRecipientDal",
                column: "RecipientImageId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_RecipientListCampaign_CampaignId",
                table: "RecipientListCampaign",
                column: "CampaignId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Job");

            migrationBuilder.DropTable(name: "RecipientImageRecipientDal");

            migrationBuilder.DropTable(name: "RecipientListCampaign");

            migrationBuilder.DropTable(name: "LineItem");

            migrationBuilder.DropTable(name: "RecipientImage");

            migrationBuilder.DropTable(name: "Recipient");

            migrationBuilder.DropTable(name: "Campaign");

            migrationBuilder.DropTable(name: "Product");

            migrationBuilder.DropTable(name: "RecipientList");

            migrationBuilder.DropTable(name: "Company");

            migrationBuilder.DropTable(name: "Category");
        }
    }
}
