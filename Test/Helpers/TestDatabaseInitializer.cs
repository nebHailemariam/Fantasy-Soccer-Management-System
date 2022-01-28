using System.Threading.Tasks;
using API.Data;

namespace Test.Helpers
{
    public static class TestDatabaseInitializer
    {
        public static async Task InitializeRole(DataContext db)
        {
            db.Roles.Add(new()
            {
                Name = "User",
                NormalizedName = "USER",
            });
            db.Roles.Add(new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
            });
            db.Roles.Add(new()
            {
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
            });
            await db.SaveChangesAsync();
        }
    }
}