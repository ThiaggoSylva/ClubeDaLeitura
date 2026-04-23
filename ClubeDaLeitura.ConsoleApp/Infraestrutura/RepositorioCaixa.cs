using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioCaixa : RepositorioBase
{
    public bool ExisteEtiquetaDuplicada(string etiqueta, string? idIgnorado = null)
    {
        return registros
            .OfType<Caixa>()
            .Any(x => x.Etiqueta.Equals(etiqueta, StringComparison.OrdinalIgnoreCase) && x.Id != idIgnorado);
    }
}