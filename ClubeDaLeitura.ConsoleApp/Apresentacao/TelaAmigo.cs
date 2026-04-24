using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaAmigo : TelaBase
{
    private readonly RepositorioAmigo repositorioAmigo;
    private readonly RepositorioEmprestimo repositorioEmprestimo;

    public override string NomeEntidade => "Amigos";

    public TelaAmigo(RepositorioAmigo repositorioAmigo, RepositorioEmprestimo repositorioEmprestimo)
        : base(repositorioAmigo)
    {
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public override string ObterOpcaoMenu()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Gestão de Amigos");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Cadastrar");
        Console.WriteLine("2 - Editar");
        Console.WriteLine("3 - Excluir");
        Console.WriteLine("4 - Visualizar todos");
        Console.WriteLine("5 - Visualizar empréstimos de um amigo");
        Console.WriteLine("S - Voltar");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");

        return Console.ReadLine()?.ToUpper() ?? "S";
    }

    protected override EntidadeBase ObterDados()
    {
        Console.Write("Nome: ");
        string nome = Console.ReadLine() ?? string.Empty;

        Console.Write("Nome do responsável: ");
        string responsavel = Console.ReadLine() ?? string.Empty;

        Console.Write("Telefone: ");
        string telefone = Console.ReadLine() ?? string.Empty;

        return new Amigo(nome, responsavel, telefone);
    }

    public override void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Amigo");
        Amigo amigo = (Amigo)ObterDados();

        if (repositorioAmigo.ExisteAmigoDuplicado(amigo.Nome, amigo.Telefone))
        {
            Mensagem("Já existe um amigo com esse nome e telefone.");
            return;
        }

        SalvarCadastro(amigo);
    }

    public override void Editar()
    {
        ExibirCabecalho("Edição de Amigo");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        Amigo amigo = (Amigo)ObterDados();

        if (repositorioAmigo.ExisteAmigoDuplicado(amigo.Nome, amigo.Telefone, id))
        {
            Mensagem("Já existe um amigo com esse nome e telefone.");
            return;
        }

        SalvarEdicao(id, amigo);
    }

    public override void Excluir()
    {
        ExibirCabecalho("Exclusão de Amigo");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        if (repositorioEmprestimo.ExisteEmprestimoParaAmigo(id))
        {
            Mensagem("Não é permitido excluir amigo com empréstimos vinculados.");
            return;
        }

        base.Excluir();
    }

    public override void VisualizarTodos(bool exibirCabecalho)
{
    if (exibirCabecalho)
        ExibirCabecalho("Visualização de Amigos");

    Console.WriteLine("{0,-8} | {1,-20} | {2,-20} | {3,-14}",
        "ID", "Nome", "Responsável", "Telefone");

    LinhaTabela(8, 20, 20, 14);

    foreach (Amigo amigo in repositorioAmigo.SelecionarTodos().OfType<Amigo>())
    {
        Console.WriteLine("{0,-8} | {1,-20} | {2,-20} | {3,-14}",
            amigo.Id,
            amigo.Nome,
            amigo.NomeResponsavel,
            amigo.Telefone);
    }

    LinhaTabela(8, 20, 20, 14);

    if (exibirCabecalho)
        Mensagem("Fim da listagem.");
}

    public void VisualizarEmprestimosDoAmigo()
    {
        ExibirCabecalho("Empréstimos por Amigo");
        VisualizarTodos(false);

        Console.Write("Digite o ID do amigo: ");
        string id = Console.ReadLine() ?? string.Empty;

        var emprestimos = repositorioEmprestimo.SelecionarPorAmigo(id);

        Console.WriteLine();
        Console.WriteLine("{0,-20} | {1,-12} | {2,-12} | {3,-10}",
            "Revista", "Empréstimo", "Devolução", "Status");

        foreach (var emprestimo in emprestimos)
        {
            if (emprestimo.Status == Dominio.Enums.StatusEmprestimo.Atrasado)
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("{0,-20} | {1,-12:dd/MM/yyyy} | {2,-12:dd/MM/yyyy} | {3,-10}",
                emprestimo.Revista.Titulo,
                emprestimo.DataEmprestimo,
                emprestimo.DataDevolucao,
                emprestimo.Status);

            Console.ResetColor();
        }

        Mensagem("Fim da consulta.");
    }
}