// See https://aka.ms/new-console-template for more information

using Csharp.Enumeration;
using Csharp.Enumeration.Entities;
using Csharp.Enumeration.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection services = new ServiceCollection();

services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite("Data Source=db.db"), ServiceLifetime.Singleton);

var provider = services.BuildServiceProvider();

var context = provider.GetRequiredService<LibraryContext>();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

var existingBook = await context.Books.SingleOrDefaultAsync(b => b.Id == 1);

if (existingBook is null)
{

    existingBook = new Book()
    {
        Genre = BookGenre.Biography,
        Editor = new Editor()
        {
            Name = Guid.NewGuid().ToString()
        },
        Pages = 50,
        Title = "Mega super book"
    };

    context.Books.Add(existingBook);
    await context.SaveChangesAsync();
}

await context.Entry(existingBook!).ReloadAsync();

var grt = BookGenre.Anthology < BookGenre.Biography;

var eq = BookGenre.Anthology == BookGenre.Anthology;
var noteq  = BookGenre.Anthology == BookGenre.Biography;

bool gt = BookGenre.Biography > BookGenre.Anthology;

var i = 1;


var context2 = provider.GetRequiredService<LibraryContext>();

var b = context2.Books.Include(x => x.Editor). First();

Console.Read();

