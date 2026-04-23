using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioAmigo : RepositorioBase
{
    public bool ExisteAmigoDuplicado(string nome, string telefone, string? idIgnorado = null)
    {
        return registros
            .OfType<Amigo>()
            .Any(x =>
                x.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase) &&
                x.Telefone == telefone &&
                x.Id != idIgnorado);
    }
}