using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Dominio.Enums;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Revista : EntidadeBase
{
    public string Titulo { get; set; }
    public int NumeroEdicao { get; set; }
    public int AnoPublicacao { get; set; }
    public Caixa Caixa { get; set; }
    public StatusRevista Status { get; set; }

    public Revista(string titulo, int numeroEdicao, int anoPublicacao, Caixa caixa)
    {
        Titulo = titulo;
        NumeroEdicao = numeroEdicao;
        AnoPublicacao = anoPublicacao;
        Caixa = caixa;
        Status = StatusRevista.Disponivel;
    }

    public override string[] Validar()
    {
        List<string> erros = new();

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Length < 2 || Titulo.Length > 100)
            erros.Add("O título deve ter entre 2 e 100 caracteres.");

        if (NumeroEdicao <= 0)
            erros.Add("O número da edição deve ser positivo.");

        if (AnoPublicacao < 1900 || AnoPublicacao > DateTime.Now.Year)
            erros.Add("O ano de publicação deve ser válido.");

        if (Caixa == null)
            erros.Add("A caixa é obrigatória.");

        return erros.ToArray();
    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Revista revistaAtualizada = (Revista)entidadeAtualizada;

        Titulo = revistaAtualizada.Titulo;
        NumeroEdicao = revistaAtualizada.NumeroEdicao;
        AnoPublicacao = revistaAtualizada.AnoPublicacao;
        Caixa = revistaAtualizada.Caixa;
        Status = revistaAtualizada.Status;
    }
}