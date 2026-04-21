using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioAmigo : RepositorioBase
{
    public bool ExisteAmigoComMesmoNomeTelefone(string nome, string telefone)
    {
        for (int i = 0; i < registros.Length; i++)
        {
            Amigo? amigo = registros[i] as Amigo;

            if (amigo == null)
                continue;

            if (amigo.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase) &&
                amigo.Telefone == telefone)
                return true;
        }

        return false;
    }
}