using System.Text.RegularExpressions;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Caixa : EntidadeBase
{
    public string Etiqueta { get; set; }
    public string Cor { get; set; }
    public int DiasDeEmprestimo { get; set; }

    public Caixa(string etiqueta, string cor, int diasDeEmprestimo)
    {
        Etiqueta = etiqueta;
        Cor = cor;
        DiasDeEmprestimo = diasDeEmprestimo <= 0 ? 7 : diasDeEmprestimo;
    }

    public override string[] Validar()
    {
        List<string> erros = new();

        if (string.IsNullOrWhiteSpace(Etiqueta) || Etiqueta.Length > 50)
            erros.Add("A etiqueta é obrigatória e deve ter no máximo 50 caracteres.");

        if (string.IsNullOrWhiteSpace(Cor))
            erros.Add("A cor é obrigatória.");
        else if (!CorEhValida(Cor))
            erros.Add("A cor deve ser um nome da paleta ou um hexadecimal válido. Ex: Vermelho ou #FF0000");

        if (DiasDeEmprestimo <= 0)
            erros.Add("Os dias de empréstimo devem ser maiores que zero.");

        return erros.ToArray();
    }

    private bool CorEhValida(string cor)
    {
        string[] paleta =
        {
            "Vermelho", "Azul", "Verde", "Amarelo", "Roxo",
            "Laranja", "Preto", "Branco", "Cinza", "Rosa"
        };

        bool pertencePaleta = paleta.Any(p => p.Equals(cor, StringComparison.OrdinalIgnoreCase));
        bool hexadecimalValido = Regex.IsMatch(cor, "^#([A-Fa-f0-9]{6})$");

        return pertencePaleta || hexadecimalValido;
    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Caixa caixaAtualizada = (Caixa)entidadeAtualizada;

        Etiqueta = caixaAtualizada.Etiqueta;
        Cor = caixaAtualizada.Cor;
        DiasDeEmprestimo = caixaAtualizada.DiasDeEmprestimo;
    }
}