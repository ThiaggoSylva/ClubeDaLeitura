using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioRevista : RepositorioBase
{
    public bool ExisteRevistaComMesmoTituloEdicao(string titulo, int numeroEdicao, string? idIgnorado = null)
    {
        return registros
            .OfType<Revista>()
            .Any(x =>
                x.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase) &&
                x.NumeroEdicao == numeroEdicao &&
                x.Id != idIgnorado);
    }

    public bool ExisteRevistaVinculadaNaCaixa(string caixaId)
    {
        return registros
            .OfType<Revista>()
            .Any(x => x.Caixa.Id == caixaId);
    }

    public Revista[] SelecionarDisponiveis()
    {
        return registros
            .OfType<Revista>()
            .Where(x => x.Status == Dominio.Enums.StatusRevista.Disponivel)
            .ToArray();
    }
}