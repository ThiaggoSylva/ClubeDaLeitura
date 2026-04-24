using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Enums;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaEmprestimo
{
    private readonly RepositorioEmprestimo repositorioEmprestimo;
    private readonly RepositorioAmigo repositorioAmigo;
    private readonly RepositorioRevista repositorioRevista;

    public TelaEmprestimo(
        RepositorioEmprestimo repositorioEmprestimo,
        RepositorioAmigo repositorioAmigo,
        RepositorioRevista repositorioRevista)
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    public void ApresentarMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Gestão de Empréstimos");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1 - Registrar empréstimo");
            Console.WriteLine("2 - Registrar devolução");
            Console.WriteLine("3 - Visualizar empréstimos abertos");
            Console.WriteLine("4 - Visualizar empréstimos fechados");
            Console.WriteLine("5 - Visualizar todos");
            Console.WriteLine("S - Voltar");
            Console.WriteLine("---------------------------------");
            Console.Write("> ");

            string opcao = Console.ReadLine()?.ToUpper() ?? "S";

            if (opcao == "S")
                break;
            else if (opcao == "1")
                RegistrarEmprestimo();
            else if (opcao == "2")
                RegistrarDevolucao();
            else if (opcao == "3")
                VisualizarAbertos();
            else if (opcao == "4")
                VisualizarFechados();
            else if (opcao == "5")
                VisualizarTodos();
        }
    }

    private void RegistrarEmprestimo()
    {
        Console.Clear();
        Console.WriteLine("Registrar Empréstimo");
        Console.WriteLine();

        Amigo? amigo = SelecionarAmigo();

        if (amigo == null)
            return;

        if (repositorioEmprestimo.AmigoPossuiEmprestimoAtivo(amigo))
        {
            Pausar("Esse amigo já possui empréstimo ativo.");
            return;
        }

        Revista? revista = SelecionarRevistaDisponivel();

        if (revista == null)
            return;

        if (revista.Status != StatusRevista.Disponivel)
        {
            Pausar("A revista selecionada não está disponível.");
            return;
        }

        Emprestimo emprestimo = new Emprestimo(amigo, revista);

        revista.Status = StatusRevista.Emprestada;
        repositorioEmprestimo.Cadastrar(emprestimo);

        Pausar($"Empréstimo registrado. Devolução prevista: {emprestimo.DataDevolucao:dd/MM/yyyy}");
    }

    private void RegistrarDevolucao()
    {
        Console.Clear();
        Console.WriteLine("Registrar Devolução");
        Console.WriteLine();

        Emprestimo[] emprestimosAbertos = repositorioEmprestimo.SelecionarAbertos();

        if (emprestimosAbertos.Length == 0)
        {
            Pausar("Não há empréstimos abertos para devolver.");
            return;
        }

        Console.WriteLine("{0,-8} | {1,-15} | {2,-20} | {3,-12} | {4,-10}",
            "ID", "Amigo", "Revista", "Devolução", "Status");

        LinhaTabela(8, 15, 20, 12, 10);

        foreach (var emprestimo in emprestimosAbertos)
        {
            Console.Write("{0,-8} | {1,-15} | {2,-20} | {3,-12:dd/MM/yyyy} | ",
                emprestimo.Id,
                emprestimo.Amigo.Nome,
                emprestimo.Revista.Titulo,
                emprestimo.DataDevolucao);

            Console.ForegroundColor = ObterCorStatus(emprestimo.Status);
            Console.Write("{0,-10}", emprestimo.Status);
            Console.ResetColor();

            Console.WriteLine();
        }

        LinhaTabela(8, 15, 20, 12, 10);

        Emprestimo? emprestimoSelecionado = SelecionarEmprestimoAberto();

        if (emprestimoSelecionado == null)
            return;

        emprestimoSelecionado.RegistrarDevolucao();

        Pausar("Devolução registrada com sucesso.");
    }

    private void VisualizarAbertos()
    {
        ExibirLista("Empréstimos Abertos", repositorioEmprestimo.SelecionarAbertos());
    }

    private void VisualizarFechados()
    {
        ExibirLista("Empréstimos Fechados", repositorioEmprestimo.SelecionarFechados());
    }

    private void VisualizarTodos()
    {
        ExibirLista("Todos os Empréstimos", repositorioEmprestimo.SelecionarTodos().OfType<Emprestimo>().ToArray());
    }

    private void ExibirLista(string titulo, Emprestimo[] emprestimos)
    {
        Console.Clear();
        Console.WriteLine(titulo);
        Console.WriteLine();

        if (emprestimos.Length == 0)
        {
            Pausar("Nenhum empréstimo encontrado.");
            return;
        }

        Console.WriteLine("{0,-8} | {1,-15} | {2,-20} | {3,-12} | {4,-12} | {5,-10}",
            "ID", "Amigo", "Revista", "Empréstimo", "Devolução", "Status");

        LinhaTabela(8, 15, 20, 12, 12, 10);

        foreach (var emprestimo in emprestimos)
        {
            Console.Write("{0,-8} | {1,-15} | {2,-20} | {3,-12:dd/MM/yyyy} | {4,-12:dd/MM/yyyy} | ",
                emprestimo.Id,
                emprestimo.Amigo.Nome,
                emprestimo.Revista.Titulo,
                emprestimo.DataEmprestimo,
                emprestimo.DataDevolucao);

            Console.ForegroundColor = ObterCorStatus(emprestimo.Status);
            Console.Write("{0,-10}", emprestimo.Status);
            Console.ResetColor();

            Console.WriteLine();
        }

        LinhaTabela(8, 15, 20, 12, 12, 10);

        Pausar("Fim da listagem.");
    }

    private Amigo? SelecionarAmigo()
    {
        Console.WriteLine("Amigos cadastrados:");
        Console.WriteLine();

        foreach (Amigo amigo in repositorioAmigo.SelecionarTodos().OfType<Amigo>())
            Console.WriteLine($"{amigo.Id} | {amigo.Nome} | {amigo.Telefone}");

        Console.WriteLine();

        while (true)
        {
            Console.Write("Digite o ID do amigo ou S para cancelar: ");
            string id = Console.ReadLine()?.ToUpper() ?? string.Empty;

            if (id == "S")
                return null;

            Amigo? amigoSelecionado = repositorioAmigo.SelecionarPorId(id) as Amigo;

            if (amigoSelecionado != null)
                return amigoSelecionado;

            Console.WriteLine("ID inválido. Tente novamente.");
        }
    }

    private Revista? SelecionarRevistaDisponivel()
    {
        Console.WriteLine("Revistas disponíveis:");
        Console.WriteLine();

        Revista[] revistasDisponiveis = repositorioRevista.SelecionarDisponiveis();

        if (revistasDisponiveis.Length == 0)
        {
            Pausar("Não há revistas disponíveis para empréstimo.");
            return null;
        }

        foreach (Revista revista in revistasDisponiveis)
            Console.WriteLine($"{revista.Id} | {revista.Titulo} | Edição {revista.NumeroEdicao}");

        Console.WriteLine();

        while (true)
        {
            Console.Write("Digite o ID da revista ou S para cancelar: ");
            string id = Console.ReadLine()?.ToUpper() ?? string.Empty;

            if (id == "S")
                return null;

            Revista? revistaSelecionada = repositorioRevista.SelecionarPorId(id) as Revista;

            if (revistaSelecionada != null && revistaSelecionada.Status == StatusRevista.Disponivel)
                return revistaSelecionada;

            Console.WriteLine("ID inválido ou revista indisponível. Tente novamente.");
        }
    }

    private Emprestimo? SelecionarEmprestimoAberto()
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write("Digite o ID do empréstimo ou S para cancelar: ");
            string id = Console.ReadLine()?.ToUpper() ?? string.Empty;

            if (id == "S")
                return null;

            Emprestimo? emprestimo = repositorioEmprestimo.SelecionarPorId(id) as Emprestimo;

            if (emprestimo != null && emprestimo.DataDevolucaoRealizada == null)
                return emprestimo;

            Console.WriteLine("ID inválido ou empréstimo já concluído. Tente novamente.");
        }
    }

    private ConsoleColor ObterCorStatus(StatusEmprestimo status)
    {
        return status switch
        {
            StatusEmprestimo.Concluido => ConsoleColor.Green,
            StatusEmprestimo.Aberto => ConsoleColor.Yellow,
            StatusEmprestimo.Atrasado => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }

    private void LinhaTabela(params int[] largurasColunas)
    {
        int total = largurasColunas.Sum() + (largurasColunas.Length - 1) * 3;
        Console.WriteLine(new string('-', total));
    }

    private void Pausar(string mensagem)
    {
        Console.WriteLine();
        Console.WriteLine(mensagem);
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }
}