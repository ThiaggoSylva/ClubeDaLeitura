using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;
using ClubeDaLeitura.ConsoleApp.Util;
namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaCaixa : TelaBase
{
    private readonly RepositorioCaixa repositorioCaixa;
    private readonly RepositorioRevista repositorioRevista;

    public override string NomeEntidade => "Caixas";

    public TelaCaixa(RepositorioCaixa repositorioCaixa, RepositorioRevista repositorioRevista)
        : base(repositorioCaixa)
    {
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioRevista = repositorioRevista;
    }

    protected override EntidadeBase ObterDados()
    {
        Console.Write("Etiqueta: ");
        string etiqueta = Console.ReadLine() ?? string.Empty;

        Console.Write("Cor (paleta ou hexadecimal): ");
        string cor = Console.ReadLine() ?? string.Empty;

        Console.Write("Dias de empréstimo (padrão 7): ");
        string? entrada = Console.ReadLine();

        int dias = 7;
        if (!string.IsNullOrWhiteSpace(entrada) && int.TryParse(entrada, out int valor))
            dias = valor;

        return new Caixa(etiqueta, cor, dias);
    }

    public override void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Caixa");
        Caixa caixa = (Caixa)ObterDados();

        if (repositorioCaixa.ExisteEtiquetaDuplicada(caixa.Etiqueta))
        {
            Mensagem("Já existe uma caixa com essa etiqueta.");
            return;
        }

        SalvarCadastro(caixa);
    }

    public override void Editar()
    {
        ExibirCabecalho("Edição de Caixa");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        Caixa caixa = (Caixa)ObterDados();

        if (repositorioCaixa.ExisteEtiquetaDuplicada(caixa.Etiqueta, id))
        {
            Mensagem("Já existe uma caixa com essa etiqueta.");
            return;
        }

        SalvarEdicao(id, caixa);
    }

    public override void Excluir()
    {
        ExibirCabecalho("Exclusão de Caixa");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        if (repositorioRevista.ExisteRevistaVinculadaNaCaixa(id))
        {
            Mensagem("Não é permitido excluir caixa com revistas vinculadas.");
            return;
        }

        base.Excluir();
    }

  public override void VisualizarTodos(bool exibirCabecalho)
{
    if (exibirCabecalho)
        ExibirCabecalho("Visualização de Caixas");

    Console.WriteLine("{0,-8} | {1,-20} | {2,-12} | {3,-5}", "ID", "Etiqueta", "Cor", "Dias");
    LinhaTabela(8, 20, 12, 5);

    foreach (Caixa caixa in repositorioCaixa.SelecionarTodos().OfType<Caixa>())
    {
        Console.Write("{0,-8} | {1,-20} | ", caixa.Id, caixa.Etiqueta);

        Console.ForegroundColor = ConsoleColorHelper.ObterCor(caixa.Cor);
        Console.Write("{0,-12}", caixa.Cor);
        Console.ResetColor();

        Console.WriteLine(" | {0,-5}", caixa.DiasDeEmprestimo);
    }

    LinhaTabela(8, 20, 12, 5);

    if (exibirCabecalho)
        Mensagem("Fim da listagem.");
}
}