using katio.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace katio.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{

    private readonly KatioContext _context;
    private bool _disposed = false;
    private IRepository<int, Book> _bookRepository;
    private IRepository<int, Author> _authorRepository;
    private IRepository<int, AudioBook> _audioBookRepository;
    private IRepository<int, Genre> _genreRepository;
    private IRepository<int, Narrator> _narratorRepository;

    public UnitOfWork(KatioContext context)
    {
        _context = context;
    }


    #region Repositories
    public IRepository<int, Book> BookRepository
    {
        get
        {
            _bookRepository ??= new Repository<int, Book>(_context);
            return _bookRepository;
        }
    }
    public IRepository<int, Author> AuthorRepository
    {
        get
        {
            _authorRepository ??= new Repository<int, Author>(_context);
            return _authorRepository;
        }
    }
    public IRepository<int, AudioBook> AudioBookRepository
    {
        get
        {
            _audioBookRepository ??= new Repository<int, AudioBook>(_context);
            return _audioBookRepository;
        }

    }
    public IRepository<int, Genre> GenreRepository
    {
        get
        {
            _genreRepository ??= new Repository<int, Genre>(_context);
            return _genreRepository;
        }
    }
    public IRepository<int, Narrator> NarratorRepository
    {
        get
        {
            _narratorRepository ??= new Repository<int, Narrator>(_context);
            return _narratorRepository;
        }
    }

    #endregion

    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {

            ex.Entries.Single().Reload();
        }
    }


    #region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.DisposeAsync();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion

}