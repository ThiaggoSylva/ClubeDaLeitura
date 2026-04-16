using System;
using System.Security.Cryptography;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Revista
{
    public string Id { get; set; }
    public string Titulo { get; set; }
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }
    public Caixa Caixa { get; set; }

    public Revista(string titulo, int numeroEdicao, int anoPublicacao, Caixa caixa)
    {
        Id = Convert
                .ToHexString(RandomNumberGenerator.GetBytes(20))
                .ToLower()
                .Substring(0, 7);

        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        Caixa = caixa;
    }

    public string[] Validar()
    {
        string erros = string.Empty;

        if (string.IsNullOrWhiteSpace(Titulo))
            erros += "O campo \"Título\" é obrigatório;";

        else if (Titulo.Length < 2 || Titulo.Length > 100)
            erros += "O campo \"Título\" deve conter entre 2 e 100 caracteres;";

        if (NumeroEdicao < 0)
            erros += "O campo \"Numero da Edição\" deve conter um valor igual ou maior que 0;";

        int anoAtual = DateTime.Now.Year;

        if (AnoPublicacao < 1 || AnoPublicacao > anoAtual)
            erros += "O campo \"Ano de Publicação\" deve conter uma data válida;";

        if (Caixa == null)
            erros += "O campo \"Caixa\" deve conter uma caixa válida;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }

    public void AtualizarRegistro(Revista novaRevista)
    {
        Titulo = novaRevista.Titulo;
        NumeroEdicao = novaRevista.NumeroEdicao;
        AnoPublicacao = novaRevista.AnoPublicacao;
        Caixa = novaRevista.Caixa;
    }
}