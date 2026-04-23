using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioEmprestimo : RepositorioBase
{
    public bool AmigoPossuiEmprestimoAtivo(Amigo amigo)
    {
        return registros
            .OfType<Emprestimo>()
            .Any(x => x.Amigo.Id == amigo.Id && x.DataDevolucaoRealizada == null);
    }

    public bool ExisteEmprestimoParaAmigo(string amigoId)
    {
        return registros
            .OfType<Emprestimo>()
            .Any(x => x.Amigo.Id == amigoId);
    }

    public bool ExisteEmprestimoAbertoParaRevista(string revistaId)
    {
        return registros
            .OfType<Emprestimo>()
            .Any(x => x.Revista.Id == revistaId && x.DataDevolucaoRealizada == null);
    }

    public Emprestimo[] SelecionarAbertos()
    {
        return registros.OfType<Emprestimo>()
            .Where(x => x.DataDevolucaoRealizada == null)
            .ToArray();
    }

    public Emprestimo[] SelecionarFechados()
    {
        return registros.OfType<Emprestimo>()
            .Where(x => x.DataDevolucaoRealizada != null)
            .ToArray();
    }

    public Emprestimo[] SelecionarPorAmigo(string amigoId)
    {
        return registros.OfType<Emprestimo>()
            .Where(x => x.Amigo.Id == amigoId)
            .ToArray();
    }
}