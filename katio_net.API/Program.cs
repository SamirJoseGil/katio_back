using katio.Business.Services;
using katio.Business.Interfaces;
using katio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Conection to Database.
builder.Services.AddDbContext<KatioContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("KatioDBPSQL")));

// Configure CrossOrigin
builder.Services.AddCors(Options => {
    Options.AddPolicy(name: "katioRules", builder => {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Inject services to the container.
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<INarratorService, NarratorService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAudioBookService, AudioBookService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// PopulateDB(app);


app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();

// App run
app.Run();


// Datos de Base de Datos en Memoria
#region PopulateDB
async void PopulateDB(WebApplication app)
{
    using (var scope = app.Services.CreateAsyncScope())
    
    {
        // Tabla de Autores
        #region author service
        var AuthorService = scope.ServiceProvider.GetService<IAuthorService>();
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Gabriel",
            LastName = "García Márquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Jorge",
            LastName = "Isaacs",
            Country = "Colombia",
            BirthDate = new DateOnly(1836, 04, 01)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Germán",
            LastName = "Castro-Caycedo",
            Country = "Colombia",
            BirthDate = new DateOnly(1940, 03, 03)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Silvia",
            LastName = "Moreno García",
            Country = "México",
            BirthDate = new DateOnly(1981, 04, 25)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Irene",
            LastName = "Vallejo",
            Country = "España",
            BirthDate = new DateOnly(1979, 06, 06)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Sarah J",
            LastName = "Maas",
            Country = "EEUU",
            BirthDate = new DateOnly(1986, 03, 05)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Mario",
            LastName = "Mendoza",
            Country = "Colombia",
            BirthDate = new DateOnly(1964, 01, 10)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Hector",
            LastName = "Abad Faciolince",
            Country = "Colombia",
            BirthDate = new DateOnly(1958, 10, 01)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Laura",
            LastName = "Restrepo",
            Country = "Colombia",
            BirthDate = new DateOnly(1950, 01, 01)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Piedad",
            LastName = "Bonnet",
            Country = "Colombia",
            BirthDate = new DateOnly(1951, 01, 01)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Fernando",
            LastName = "Vallejo",
            Country = "Colombia",
            BirthDate = new DateOnly(1942, 10, 24)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Antonio",
            LastName = "Caballero",
            Country = "Colombia",
            BirthDate = new DateOnly(1945, 05, 15)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "William",
            LastName = "Ospina",
            Country = "Colombia",
            BirthDate = new DateOnly(1954, 03, 02)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Juan Gabriel",
            LastName = "Vasquez",
            Country = "Colombia",
            BirthDate = new DateOnly(1973, 01, 01)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Santiago",
            LastName = "Gamboa",
            Country = "Colombia",
            BirthDate = new DateOnly(1965, 01, 01)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Angela",
            LastName = "Becerra",
            Country = "Colombia",
            BirthDate = new DateOnly(1957, 07, 17)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Stephen",
            LastName = "King",
            Country = "EEUU",
            BirthDate = new DateOnly(1947, 09, 21)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Anne",
            LastName = "Rice",
            Country = "EEUU",
            BirthDate = new DateOnly(1941, 10, 04)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Jeff",
            LastName = "Vandermeer",
            Country = "EEUU",
            BirthDate = new DateOnly(1968, 07, 07)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Liu",
            LastName = "Cixin",
            Country = "China",
            BirthDate = new DateOnly(1963, 06, 30)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Fyodor",
            LastName = "Dvostoesky",
            Country = "Rusia",
            BirthDate = new DateOnly(1821, 11, 11)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Leo",
            LastName = "Tolstoy",
            Country = "Rusia",
            BirthDate = new DateOnly(1928, 09, 09)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Anton",
            LastName = "Chekhov",
            Country = "Rusia",
            BirthDate = new DateOnly(1860, 01, 29)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Issac",
            LastName = "Asimov",
            Country = "Rusia, EEUU",
            BirthDate = new DateOnly(1920, 01, 02)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Rudyard",
            LastName = "Kipling",
            Country = "India",
            BirthDate = new DateOnly(1865, 12, 30)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Jon Ronald Reuel",
            LastName = "Tolkien",
            Country = "Surafrica",
            BirthDate = new DateOnly(1892, 01, 03)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Clive Staples",
            LastName = "Lewis",
            Country = "Reino Unido",
            BirthDate = new DateOnly(1898, 11, 29)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "George Raymond Richard",
            LastName = "Martin",
            Country = "EEUU",
            BirthDate = new DateOnly(1948, 09, 20)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Frank",
            LastName = "Herbert",
            Country = "EEUU",
            BirthDate = new DateOnly(1920, 10, 28)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Albert",
            LastName = "Camus",
            Country = "Francia",
            BirthDate = new DateOnly(1913, 11, 07)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Margaret",
            LastName = "Atwood",
            Country = "Canadá",
            BirthDate = new DateOnly(1939, 11, 18)
        });

        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Mary",
            LastName = "Shelley",
            Country = "Inglaterra",
            BirthDate = new DateOnly(1890, 09, 15)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Agatha",
            LastName = "Christie",
            Country = "Inglaterra",
            BirthDate = new DateOnly(1890, 09, 15)
        });
        await AuthorService.CreateAuthor(new katio.Data.Models.Author
        {
            Name = "Ursula K",
            LastName = "Le Guin",
            Country = "EEUU",
            BirthDate = new DateOnly(1929, 10, 21)
        });
        #endregion

        // Tabla de Libros
        #region book service
        var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Cien años de soledad",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "RAE Obra Académica",
            DeweyIndex = "800",
            AuthorId = 1
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Huellas",
            ISBN10 = "9584277278",
            ISBN13 = "978-958427275",
            Published = new DateOnly(2019, 01, 01),
            Edition = "1ra Edicion",
            DeweyIndex = "800",
            AuthorId = 3
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "María",
            ISBN10 = "14802722922",
            ISBN13 = "978-148027292",
            Published = new DateOnly(1867, 01, 01),
            Edition = "1ra edición",
            DeweyIndex = "800",
            AuthorId = 2
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Mexico Gothic",
            ISBN10 = "8420471836",
            ISBN13 = "978-05256620785",
            Published = new DateOnly(2020, 06, 30),
            Edition = "Del Rey",
            DeweyIndex = "800",
            AuthorId = 4
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Sin remedio",
            ISBN10 = "3161484100",
            ISBN13 = "978-3161484100",
            Published = new DateOnly(1984, 01, 01),
            Edition = "Alfaguara",
            DeweyIndex = "800",
            AuthorId = 12
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Delirio",
            ISBN10 = "9587041453",
            ISBN13 = "978-9587041453",
            Published = new DateOnly(2004, 01, 01),
            Edition = "Alfaguara",
            DeweyIndex = "800",
            AuthorId = 9
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Infinito en un junco",
            ISBN10 = "8417860790",
            ISBN13 = "9788417860790",
            Published = new DateOnly(2019, 01, 01),
            Edition = "Siruela",
            DeweyIndex = "800",
            AuthorId = 5
        });

        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El olvido que seremos",
            ISBN10 = "8420426402",
            ISBN13 = "978-8420426402",
            Published = new DateOnly(2017, 10, 16),
            Edition = "Alfaguara",
            DeweyIndex = "800",
            AuthorId = 8
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El país de la canela",
            ISBN10 = "8439738831",
            ISBN13 = "978-8439738831",
            Published = new DateOnly(2020, 08, 22),
            Edition = "ndom House",
            DeweyIndex = "800",
            AuthorId = 13
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Lo que no tiene nombre",
            ISBN10 = "6287659216",
            ISBN13 = "978-6287659216",
            Published = new DateOnly(2024, 03, 19),
            Edition = "Alfaguara",
            DeweyIndex = "800",
            AuthorId = 10
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El ruido de las cosas al caer",
            ISBN10 = "6073137515",
            ISBN13 = "978-6073137515",
            Published = new DateOnly(2015, 12, 29),
            Edition = "ebolsillo",
            DeweyIndex = "800",
            AuthorId = 14
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El síndrome de Ulises",
            ISBN10 = "9584211903",
            ISBN13 = "978-9584211903",
            Published = new DateOnly(2005, 03, 30),
            Edition = "Planeta",
            DeweyIndex = "800",
            AuthorId = 15
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "La puta de Babilonia",
            ISBN10 = "6073158855",
            ISBN13 = "978-6073158855",
            Published = new DateOnly(2018, 01, 30),
            Edition = "ebolsillo",
            DeweyIndex = "800",
            AuthorId = 11

        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Memorias de un sinvergüenza de siete suelas",
            ISBN10 = "9504932611",
            ISBN13 = "978-9504932611",
            Published = new DateOnly(2012, 01, 01),
            Edition = "Planeta",
            DeweyIndex = "800",
            AuthorId = 16
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Satanás",
            ISBN10 = "9584273543",
            ISBN13 = "978-9584273543",
            Published = new DateOnly(2018, 01, 01),
            Edition = "Planeta DeAgostini Comic",
            DeweyIndex = "800",
            AuthorId = 7
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "It (Eso)",
            ISBN10 = "0525566267",
            ISBN13 = "978-0525566267",
            Published = new DateOnly(2019, 01, 27),
            Edition = "Vinntage Espanol",
            DeweyIndex = "800",
            AuthorId = 17
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El Resplandor",
            ISBN10 = "0593311233",
            ISBN13 = "978-0593311233",
            Published = new DateOnly(2005, 08, 25),
            Edition = "Vintage",
            DeweyIndex = "800",
            AuthorId = 17

        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Cujo",
            ISBN10 = "1501192241",
            ISBN13 = "978-1501192241",
            Published = new DateOnly(2018, 02, 20),
            Edition = "Scribner",
            DeweyIndex = "800",
            AuthorId = 17
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Trono de Cristal",
            ISBN10 = "8890981547",
            ISBN13 = "979-8890981547",
            Published = new DateOnly(2022, 05, 13),
            Edition = "Alfaguara",
            DeweyIndex = "800",
            AuthorId = 6
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Entrevista con el Vampiro",
            ISBN10 = "6073198929",
            ISBN13 = "978-6073198929",
            Published = new DateOnly(2021, 05, 18),
            Edition = "de Bolsillo",
            DeweyIndex = "800",
            AuthorId = 18
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Anniquilación",
            ISBN10 = "0374104092",
            ISBN13 = "978-0374104092",
            Published = new DateOnly(2014, 02, 04),
            Edition = "G Originals",
            DeweyIndex = "800",
            AuthorId = 19
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Autoridad",
            ISBN10 = "0374104108",
            ISBN13 = "978-0374104108",
            Published = new DateOnly(2014, 05, 06),
            Edition = "G Originals",
            DeweyIndex = "800",
            AuthorId = 19
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Aceptación",
            ISBN10 = "374104115",
            ISBN13 = "978-0374104115",
            Published = new DateOnly(2014, 09, 02),
            Edition = "G Originals",
            DeweyIndex = "800",
            AuthorId = 19
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Historia de Colombia y sus oligarquias",
            ISBN10 = "9584268754",
            ISBN13 = "978-9584268754",
            Published = new DateOnly(2019, 01, 01),
            Edition = "Crítica",
            DeweyIndex = "800",
            AuthorId = 12
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El problema de los tres cuerpos",
            ISBN10 = "8466659734",
            ISBN13 = "978-8466659734",
            Published = new DateOnly(2016, 11, 01),
            Edition = "Nova",
            DeweyIndex = "800",
            AuthorId = 20
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El Bosque Oscuro",
            ISBN10 = "978-8413146454",
            ISBN13 = "978-8413146454",
            Published = new DateOnly(2024, 05, 01),
            Edition = "Nova",
            DeweyIndex = "800",
            AuthorId = 20
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El fin de la muerte",
            ISBN10 = "8417347017",
            ISBN13 = "978-8417347017",
            Published = new DateOnly(2018, 08, 01),
            Edition = "1",
            DeweyIndex = "800",
            AuthorId = 20
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Crimen y Castigo",
            ISBN10 = "8872132677",
            ISBN13 = "979-8872132677",
            Published = new DateOnly(1866, 12, 01),
            Edition = "dependiente",
            DeweyIndex = "800",
            AuthorId = 21
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Las obras de Leo Tolstoy",
            ISBN10 = "1016243247",
            ISBN13 = "978-1016243247",
            Published = new DateOnly(2022, 10, 27),
            Edition = "CLassic",
            DeweyIndex = "800",
            AuthorId = 22
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Historias Cortas",
            ISBN10 = "9389717105",
            ISBN13 = "978-9389717105",
            Published = new DateOnly(2019, 01, 12),
            Edition = "Finngerprint",
            DeweyIndex = "800",
            AuthorId = 23
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Trilogía Fundación",
            ISBN10 = "8499083209",
            ISBN13 = "978-8499083209",
            Published = new DateOnly(2023, 03, 23),
            Edition = "debolsillo",
            DeweyIndex = "800",
            AuthorId = 24
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El libro de la selva",
            ISBN10 = "8467871029",
            ISBN13 = "978-8467871029",
            Published = new DateOnly(1894, 01, 01),
            Edition = "Classic",
            DeweyIndex = "800",
            AuthorId = 25
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El señor de los anillos",
            ISBN10 = "8445013830",
            ISBN13 = "978-8445013830",
            Published = new DateOnly(2023, 11, 02),
            Edition = "Fantasia epica",
            DeweyIndex = "800",
            AuthorId = 26
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Juego de tronos",
            ISBN10 = "1644736135",
            ISBN13 = "978-1644736135",
            Published = new DateOnly(2022, 06, 21),
            Edition = "Vintage",
            DeweyIndex = "800",
            AuthorId = 28
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Duna",
            ISBN10 = "6073194648",
            ISBN13 = "978-6073194648",
            Published = new DateOnly(2020, 11, 07),
            Edition = "Classic",
            DeweyIndex = "800",
            AuthorId = 29
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El extranjero",
            ISBN10 = "1518660016",
            ISBN13 = "978-1518660016",
            Published = new DateOnly(2015, 10, 06),
            Edition = "Ciencia ficcion",
            DeweyIndex = "800",
            AuthorId = 30
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "El cuento de la criada",
            ISBN10 = "8498388015",
            ISBN13 = "978-8498388015",
            Published = new DateOnly(2017, 06, 17),
            Edition = "Salamandra",
            DeweyIndex = "800",
            AuthorId = 31
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Asesinato en el Orient Express",
            ISBN10 = "6070743986",
            ISBN13 = "978-6070743986",
            Published = new DateOnly(2022, 02, 15),
            Edition = "Planeta",
            DeweyIndex = "800",
            AuthorId = 33
        });
        await bookService.CreateBook(new katio.Data.Models.Book
        {
            Name = "Cuentos de Terramar",
            ISBN10 = "8467437560",
            ISBN13 = "978-8467437560",
            Published = new DateOnly(2007, 01, 01),
            Edition = "Planeta",
            DeweyIndex = "800",
            AuthorId = 34
        });
        #endregion

        // Tabla de Narradores
        #region narrator service

        var NarratorService = scope.ServiceProvider.GetService<INarratorService>();
        await NarratorService.CreateNarrator(new katio.Data.Models.Narrator
        {
            Name = "Maria Camila",
            LastName = "Gil Rojas",
            Genre = "Ficcion"
        });
        await NarratorService.CreateNarrator(new katio.Data.Models.Narrator
        {
            Name = "Juan",
            LastName = "Perez",
            Genre = "Ficcion"
        });
        await NarratorService.CreateNarrator(new katio.Data.Models.Narrator
        {
            Name = "Pedro",
            LastName = "Gonzalez",
            Genre = "Ficcion"
        }); 
        await NarratorService.CreateNarrator(new katio.Data.Models.Narrator
        {
            Name = "Luisa",
            LastName = "Fernanda",
            Genre = "Ficcion"
        });
        #endregion

        // Tabla de Generos
        #region genre service

        var GenreService = scope.ServiceProvider.GetService<IGenreService>();
        await GenreService.CreateGenre(new katio.Data.Models.Genre
        {
            Name = "Ficcion",
            Description = "La ficcion es ...",
        });
        await GenreService.CreateGenre(new katio.Data.Models.Genre
        {
            Name = "Terror",
            Description = "El terror es ...",
        });
        await GenreService.CreateGenre(new katio.Data.Models.Genre
        {
            Name = "Romance",
            Description = "El romance es ...",
        });
        await GenreService.CreateGenre(new katio.Data.Models.Genre
        {
            Name = "Aventura",
            Description = "La aventura es ...",
        });

        #endregion

        // Tabla de AudioLibros
        #region audiobook service

        var AudioBookService = scope.ServiceProvider.GetService<IAudioBookService>();
        await AudioBookService.CreateAudioBook(new katio.Data.Models.AudioBook
        {
            Id = 1,
            Name = "Cien años de soledad",
            ISBN10 = "8420471836",
            ISBN13 = "978-8420471839",
            Published = new DateOnly(1967, 06, 05),
            Edition = "RAE Obra Académica",
            Genre = "Ficcion",
            LenghtInSeconds = 1,
            Path = "C:/Users/Usuario/Downloads/Cien años de soledad.mp3",
            NarratorId = 1
        });
        await AudioBookService.CreateAudioBook(new katio.Data.Models.AudioBook
        {
            Id = 2,
            Name = "Huellas",
            ISBN10 = "9584277278",
            ISBN13 = "978-958427275",
            Published = new DateOnly(2019, 01, 01),
            Edition = "1ra Edicion",
            Genre = "Ficcion",
            LenghtInSeconds = 10,
            Path = "C:/Users/Usuario/Downloads/Huellas.mp3",
            NarratorId = 3
        });
        await AudioBookService.CreateAudioBook(new katio.Data.Models.AudioBook
        {
            Id = 3,
            Name = "María",
            ISBN10 = "14802722922",
            ISBN13 = "978-148027292",
            Published = new DateOnly(1867, 01, 01),
            Edition = "1ra edición",
            Genre = "Ficcion",
            LenghtInSeconds = 20,
            Path = "C:/Users/Usuario/Downloads/Maria.mp3",
            NarratorId = 2
        });
        await AudioBookService.CreateAudioBook(new katio.Data.Models.AudioBook
        {
            Id = 4,
            Name = "Mexico Gothic",
            ISBN10 = "8420471836",
            ISBN13 = "978-05256620785",
            Published = new DateOnly(2020, 06, 30),
            Edition = "Del Rey",
            Genre = "Ficcion",
            LenghtInSeconds = 30,
            Path = "C:/Users/Usuario/Downloads/Mexico Gothic.mp3",
            NarratorId = 4
        });

        #endregion
    }
}
#endregion