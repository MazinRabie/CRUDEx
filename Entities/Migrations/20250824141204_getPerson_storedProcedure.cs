using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class getPerson_storedProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = @"create procedure [dbo].[GetAllPersons]
                           as begin 
select * from [dbo].[Persons]
end


";

            migrationBuilder.Sql(sql);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = @"Drop procedure [dbo].[GetAllPersons]";
            migrationBuilder.Sql(sql);
        }
    }
}
