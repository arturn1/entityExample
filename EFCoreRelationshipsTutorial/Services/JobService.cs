public class JobService
{
    private readonly DataContext _context;

    public JobService(DataContext context)
    {
        _context = context;
    }

    // Método para buscar os characters e deve ser chamado pelo Hangfire
    public void FetchCharacters()
    {
        var characters = _context.Characters
            .Include(c => c.User)
            .Include(c => c.Weapons)
            .Include(c => c.Skills)!
            .ThenInclude(c => c.ElementType)
            .ToList();  // Executa a consulta de forma síncrona

        // Aqui você pode processar os dados de "characters"
        Console.WriteLine($"Encontrados {characters.Count} personagens.");
    }
}
