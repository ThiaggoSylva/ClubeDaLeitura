using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaRevista : TelaBase
{
    private readonly RepositorioRevista repositorioRevista;
    private readonly RepositorioCaixa repositorioCaixa;
    private readonly RepositorioEmprestimo repositorioEmprestimo;

    public override string NomeEntidade => "Revistas";

    public TelaRevista(
        RepositorioRevista repositorioRevista,
        RepositorioCaixa repositorioCaixa,
        RepositorioEmprestimo repositorioEmprestimo) : base(repositorioRevista)
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    protected override EntidadeBase ObterDados()
    {
        Console.Write("Título: ");
        string titulo = Console.ReadLine() ?? string.Empty;

        int numeroEdicao = LerInteiro("Número da edição: ");
        int ano = LerInteiro("Ano de publicação: ");

        Caixa caixa = SelecionarCaixa();

        return new Revista(titulo, numeroEdicao, ano, caixa);
    }

    public override void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Revista");
        Revista revista = (Revista)ObterDados();

        if (repositorioRevista.ExisteRevistaComMesmoTituloEdicao(revista.Titulo, revista.NumeroEdicao))
        {
            Mensagem("Já existe uma revista com esse título e edição.");
            return;
        }

        SalvarCadastro(revista);
    }

    public override void Editar()
    {
        ExibirCabecalho("Edição de Revista");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        Revista revista = (Revista)ObterDados();

        if (repositorioRevista.ExisteRevistaComMesmoTituloEdicao(revista.Titulo, revista.NumeroEdicao, id))
        {
            Mensagem("Já existe uma revista com esse título e edição.");
            return;
        }

        SalvarEdicao(id, revista);
    }

    public override void Excluir()
    {
        ExibirCabecalho("Exclusão de Revista");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        if (repositorioEmprestimo.ExisteEmprestimoAbertoParaRevista(id))
        {
            Mensagem("Não é permitido excluir revista com empréstimo aberto.");
            return;
        }

        base.Excluir();
    }

    public override void VisualizarTodos(bool exibirCabecalho)
    {
        if (exibirCabecalho)
            ExibirCabecalho("Visualização de Revistas");

        Console.WriteLine("{0,-8} | {1,-20} | {2,-6} | {3,-6} | {4,-15} | {5,-12}",
            "ID", "Título", "Edição", "Ano", "Caixa", "Status");

        foreach (Revista revista in repositorioRevista.SelecionarTodos().OfType<Revista>())
        {
            Console.WriteLine("{0,-8} | {1,-20} | {2,-6} | {3,-6} | {4,-15} | {5,-12}",
                revista.Id, revista.Titulo, revista.NumeroEdicao, revista.AnoPublicacao, revista.Caixa.Etiqueta, revista.Status);
        }

        if (exibirCabecalho)
            Mensagem("Fim da listagem.");
    }

    private Caixa SelecionarCaixa()
    {
        Console.WriteLine();
        Console.WriteLine("Caixas disponíveis:");

        foreach (Caixa caixa in repositorioCaixa.SelecionarTodos().OfType<Caixa>())
            Console.WriteLine($"{caixa.Id} | {caixa.Etiqueta}");

        Console.Write("Digite o ID da caixa: ");
        string id = Console.ReadLine() ?? string.Empty;

        return (Caixa)repositorioCaixa.SelecionarPorId(id)!;
    }
}