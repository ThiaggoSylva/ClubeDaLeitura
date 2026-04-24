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
            Console.WriteLine("6 - Reservar revista");
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
            else if (opcao == "6")
                ReservarRevista();
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

        foreach (var emprestimo in repositorioEmprestimo.SelecionarAbertos())
        {
        Console.Write($"{emprestimo.Id} | {emprestimo.Amigo.Nome} | {emprestimo.Revista.Titulo} | {emprestimo.DataDevolucao:dd/MM/yyyy} | ");

        Console.ForegroundColor = ObterCorStatus(emprestimo.Status);
        Console.WriteLine(emprestimo.Status);
        Console.ResetColor();
        }

        Console.Write("Digite o ID do empréstimo: ");
        string id = Console.ReadLine() ?? string.Empty;

        Emprestimo? emprestimoSelecionado = (Emprestimo?)repositorioEmprestimo.SelecionarPorId(id);

        if (emprestimoSelecionado == null)
        {
            Pausar("Empréstimo não encontrado.");
            return;
        }

        emprestimoSelecionado.RegistrarDevolucao();
        Pausar("Devolução registrada com sucesso.");
    }

    private void ReservarRevista()
    {
        Console.Clear();
        Console.WriteLine("Reservar Revista");
        Console.WriteLine();

        foreach (Revista revista in repositorioRevista.SelecionarTodos().OfType<Revista>()
                     .Where(x => x.Status == StatusRevista.Disponivel))
        {
            Console.WriteLine($"{revista.Id} | {revista.Titulo} | {revista.Status}");
        }

        Console.Write("Digite o ID da revista: ");
        string id = Console.ReadLine() ?? string.Empty;

        Revista? revistaSelecionada = (Revista?)repositorioRevista.SelecionarPorId(id);

        if (revistaSelecionada == null)
        {
            Pausar("Revista não encontrada.");
            return;
        }

        revistaSelecionada.Status = StatusRevista.Reservada;
        Pausar("Revista reservada com sucesso.");
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

    Console.WriteLine("{0,-8} | {1,-15} | {2,-20} | {3,-12} | {4,-12} | {5,-10}",
        "ID", "Amigo", "Revista", "Empréstimo", "Devolução", "Status");

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

    Pausar("Fim da listagem.");
}
    private Amigo? SelecionarAmigo()
{
    Console.WriteLine("Amigos cadastrados:");

    foreach (Amigo amigo in repositorioAmigo.SelecionarTodos().OfType<Amigo>())
        Console.WriteLine($"{amigo.Id} | {amigo.Nome} | {amigo.Telefone}");

    Console.WriteLine();
    Console.Write("Digite o ID do amigo: ");
    string id = Console.ReadLine() ?? string.Empty;

    Amigo? amigoSelecionado = repositorioAmigo.SelecionarPorId(id) as Amigo;

    if (amigoSelecionado == null)
    {
        Pausar("Amigo não encontrado.");
        return null;
    }

    return amigoSelecionado;
}
   private Revista? SelecionarRevistaDisponivel()
{
    Console.WriteLine("Revistas disponíveis:");

    foreach (Revista revista in repositorioRevista.SelecionarDisponiveis())
        Console.WriteLine($"{revista.Id} | {revista.Titulo} | Edição {revista.NumeroEdicao}");

    Console.WriteLine();
    Console.Write("Digite o ID da revista: ");
    string id = Console.ReadLine() ?? string.Empty;

    Revista? revistaSelecionada = repositorioRevista.SelecionarPorId(id) as Revista;

    if (revistaSelecionada == null)
    {
        Pausar("Revista não encontrada.");
        return null;
    }

    return revistaSelecionada;
}

    private void Pausar(string mensagem)
    {
        Console.WriteLine();
        Console.WriteLine(mensagem);
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
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
}