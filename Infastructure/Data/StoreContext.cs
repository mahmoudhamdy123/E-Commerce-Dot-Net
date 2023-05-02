
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infastructure .Data
{
	public class StoreContext:DbContext
	{
		public StoreContext(DbContextOptions options):base(options)
		{

		}

		public DbSet<Product> Products { get; set; }
		
	}
}

