using System.Text.RegularExpressions;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Amigo : EntidadeBase
{
    public string Nome { get; set; }
    public string NomeResponsavel { get; set; }
    public string Telefone { get; set; }

    public Amigo(string nome, string nomeResponsavel, string telefone)
    {
        Nome = nome;
        NomeResponsavel = nomeResponsavel;
        Telefone = telefone;
    }

    public override string[] Validar()
    {
        List<string> erros = new();

        if (string.IsNullOrWhiteSpace(Nome) || Nome.Length < 3 || Nome.Length > 100)
            erros.Add("O nome deve ter entre 3 e 100 caracteres.");

        if (string.IsNullOrWhiteSpace(NomeResponsavel) || NomeResponsavel.Length < 3 || NomeResponsavel.Length > 100)
            erros.Add("O nome do responsável deve ter entre 3 e 100 caracteres.");

        string somenteNumeros = Regex.Replace(Telefone ?? string.Empty, @"\D", "");
        if (somenteNumeros.Length < 10 || somenteNumeros.Length > 11)
            erros.Add("O telefone deve ter entre 10 e 11 dígitos.");

        return erros.ToArray();
    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Amigo amigoAtualizado = (Amigo)entidadeAtualizada;

        Nome = amigoAtualizado.Nome;
        NomeResponsavel = amigoAtualizado.NomeResponsavel;
        Telefone = amigoAtualizado.Telefone;
    }
}