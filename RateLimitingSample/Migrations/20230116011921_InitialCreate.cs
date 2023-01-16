using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RateLimitingSample.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "IsDone", "Title" },
                values: new object[,]
                {
                    { 1, "Cotton", false, "asymmetric" },
                    { 2, "ADP", true, "Future" },
                    { 3, "empowering", true, "solid state" },
                    { 4, "Director", false, "digital" },
                    { 5, "Fantastic Metal Chicken", false, "Tasty Concrete Bike" },
                    { 6, "Granite", false, "incremental" },
                    { 7, "Intelligent Rubber Chair", true, "backing up" },
                    { 8, "transmitter", true, "Expanded" },
                    { 9, "Ergonomic Metal Computer", false, "mint green" },
                    { 10, "Village", true, "Concrete" },
                    { 11, "Industrial", true, "salmon" },
                    { 12, "Toys, Clothing & Books", false, "Checking Account" },
                    { 13, "local area network", true, "Keys" },
                    { 14, "Circles", false, "quantifying" },
                    { 15, "Creek", true, "back-end" },
                    { 16, "Manors", false, "discrete" },
                    { 17, "Money Market Account", false, "yellow" },
                    { 18, "deposit", true, "Checking Account" },
                    { 19, "Personal Loan Account", true, "Graphical User Interface" },
                    { 20, "Toys", false, "algorithm" },
                    { 21, "Accounts", false, "Lempira" },
                    { 22, "engineer", false, "Grove" },
                    { 23, "gold", false, "programming" },
                    { 24, "Consultant", false, "sensor" },
                    { 25, "multimedia", true, "Personal Loan Account" },
                    { 26, "TCP", true, "transmit" },
                    { 27, "local area network", false, "Coordinator" },
                    { 28, "Union", true, "Ecuador" },
                    { 29, "sky blue", false, "mindshare" },
                    { 30, "fuchsia", true, "Missouri" },
                    { 31, "application", true, "Pines" },
                    { 32, "incubate", false, "Baby, Baby & Sports" },
                    { 33, "Intelligent Granite Fish", false, "Team-oriented" },
                    { 34, "Money Market Account", true, "Way" },
                    { 35, "deposit", true, "dedicated" },
                    { 36, "quantify", false, "overriding" },
                    { 37, "Sports & Health", false, "Buckinghamshire" },
                    { 38, "Implementation", false, "connecting" },
                    { 39, "blue", false, "red" },
                    { 40, "Springs", true, "Organized" },
                    { 41, "portals", false, "Shoes" },
                    { 42, "gold", true, "hack" },
                    { 43, "auxiliary", true, "Home Loan Account" },
                    { 44, "Stravenue", false, "Holy See (Vatican City State)" },
                    { 45, "Meadows", true, "RSS" },
                    { 46, "deposit", false, "programming" },
                    { 47, "Infrastructure", true, "viral" },
                    { 48, "gold", true, "synergies" },
                    { 49, "invoice", true, "Senior" },
                    { 50, "South Carolina", false, "protocol" },
                    { 51, "driver", false, "infomediaries" },
                    { 52, "generating", true, "Rustic Soft Chips" },
                    { 53, "Isle of Man", true, "Multi-layered" },
                    { 54, "Court", false, "Jewelery" },
                    { 55, "Producer", true, "convergence" },
                    { 56, "Metal", false, "Wooden" },
                    { 57, "index", true, "utilisation" },
                    { 58, "Divide", false, "Auto Loan Account" },
                    { 59, "Granite", true, "Metal" },
                    { 60, "Port", true, "digital" },
                    { 61, "Auto Loan Account", true, "Generic Granite Pizza" },
                    { 62, "Oval", true, "Berkshire" },
                    { 63, "array", true, "bandwidth" },
                    { 64, "capacitor", false, "Buckinghamshire" },
                    { 65, "Intelligent Fresh Fish", true, "generating" },
                    { 66, "Place", false, "River" },
                    { 67, "reintermediate", true, "invoice" },
                    { 68, "Gambia", false, "Cote d'Ivoire" },
                    { 69, "Streamlined", true, "target" },
                    { 70, "withdrawal", false, "Savings Account" },
                    { 71, "Home, Books & Books", false, "cyan" },
                    { 72, "Vermont", true, "Incredible Steel Pizza" },
                    { 73, "GB", false, "compelling" },
                    { 74, "Bulgaria", false, "Savings Account" },
                    { 75, "copy", true, "Division" },
                    { 76, "Handcrafted", false, "programming" },
                    { 77, "Engineer", true, "indexing" },
                    { 78, "reboot", false, "application" },
                    { 79, "Unbranded Soft Chips", false, "withdrawal" },
                    { 80, "Vermont", false, "THX" },
                    { 81, "Faroe Islands", false, "Avon" },
                    { 82, "cross-media", true, "Incredible" },
                    { 83, "Credit Card Account", true, "quantifying" },
                    { 84, "Berkshire", true, "Unbranded Soft Hat" },
                    { 85, "redefine", true, "Computers, Jewelery & Baby" },
                    { 86, "orange", true, "Coordinator" },
                    { 87, "protocol", true, "Intranet" },
                    { 88, "auxiliary", false, "Fields" },
                    { 89, "compress", false, "XML" },
                    { 90, "relationships", true, "Liberian Dollar" },
                    { 91, "compressing", false, "Rustic Cotton Bike" },
                    { 92, "Toys, Shoes & Home", true, "programming" },
                    { 93, "Unbranded", true, "New Hampshire" },
                    { 94, "Creek", false, "Lodge" },
                    { 95, "Solutions", true, "Cotton" },
                    { 96, "withdrawal", true, "Handmade Granite Pants" },
                    { 97, "hybrid", false, "array" },
                    { 98, "virtual", true, "Personal Loan Account" },
                    { 99, "Handmade Wooden Fish", false, "Avon" },
                    { 100, "Buckinghamshire", false, "infomediaries" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
