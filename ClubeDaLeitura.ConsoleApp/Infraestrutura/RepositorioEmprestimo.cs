using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioEmprestimo : RepositorioBase
{
    public bool AmigoPossuiEmprestimoAtivo(Amigo? amigo)
    {
        if (amigo == null)
            return false;

        return registros
            .OfType<Emprestimo>()
            .Any(x =>
                x.Amigo != null &&
                x.Amigo.Id == amigo.Id &&
                x.DataDevolucaoRealizada == null);
    }

    public bool ExisteEmprestimoParaAmigo(string amigoId)
    {
        if (string.IsNullOrWhiteSpace(amigoId))
            return false;

        return registros
            .OfType<Emprestimo>()
            .Any(x =>
                x.Amigo != null &&
                x.Amigo.Id == amigoId);
    }

    public bool ExisteEmprestimoAbertoParaRevista(string revistaId)
    {
        if (string.IsNullOrWhiteSpace(revistaId))
            return false;

        return registros
            .OfType<Emprestimo>()
            .Any(x =>
                x.Revista != null &&
                x.Revista.Id == revistaId &&
                x.DataDevolucaoRealizada == null);
    }

    public Emprestimo[] SelecionarAbertos()
    {
        return registros
            .OfType<Emprestimo>()
            .Where(x => x.DataDevolucaoRealizada == null)
            .ToArray();
    }

    public Emprestimo[] SelecionarFechados()
    {
        return registros
            .OfType<Emprestimo>()
            .Where(x => x.DataDevolucaoRealizada != null)
            .ToArray();
    }

    public Emprestimo[] SelecionarPorAmigo(string amigoId)
    {
        if (string.IsNullOrWhiteSpace(amigoId))
            return Array.Empty<Emprestimo>();

        return registros
            .OfType<Emprestimo>()
            .Where(x =>
                x.Amigo != null &&
                x.Amigo.Id == amigoId)
            .ToArray();
    }
}