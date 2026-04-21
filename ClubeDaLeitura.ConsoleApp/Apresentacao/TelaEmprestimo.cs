using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaEmprestimo
{
    private RepositorioEmprestimo repositorioEmprestimo;
    private RepositorioAmigo repositorioAmigo;
    private RepositorioRevista repositorioRevista;

    public TelaEmprestimo(
        RepositorioEmprestimo repositorioEmprestimo,
        RepositorioAmigo repositorioAmigo,
        RepositorioRevista repositorioRevista)
    {
        this.repositorioEmprestimo = repositorioEmprestimo;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
    }

    public string? ObterOpcaoMenu()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Gestão de Empréstimos");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Registrar empréstimo");
        Console.WriteLine("2 - Registrar devolução");
        Console.WriteLine("3 - Visualizar empréstimos abertos");
        Console.WriteLine("4 - Visualizar empréstimos fechados");
        Console.WriteLine("5 - Visualizar empréstimos por amigo");
        Console.WriteLine("S - Voltar para o início");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");

        return Console.ReadLine()?.ToUpper();
    }

    public void Cadastrar()
    {
        ExibirCabecalho("Novo Empréstimo");

        Amigo? amigoSelecionado = SelecionarAmigo();

        if (amigoSelecionado == null)
        {
            ExibirMensagem("Amigo não encontrado.");
            return;
        }

        if (repositorioEmprestimo.AmigoPossuiEmprestimoAtivo(amigoSelecionado.Id))
        {
            ExibirMensagem("Este amigo já possui um empréstimo ativo.");
            return;
        }

        Revista? revistaSelecionada = SelecionarRevistaDisponivel();

        if (revistaSelecionada == null)
        {
            ExibirMensagem("Revista indisponível ou não encontrada.");
            return;
        }

        Emprestimo novoEmprestimo = new Emprestimo(amigoSelecionado, revistaSelecionada);

        string[] erros = novoEmprestimo.Validar();

        if (erros.Length > 0)
        {
            ExibirErros(erros);
            return;
        }

        bool conseguiuRegistrar = repositorioEmprestimo.RegistrarEmprestimo(novoEmprestimo);

        if (!conseguiuRegistrar)
        {
            ExibirMensagem("Não foi possível registrar o empréstimo.");
            return;
        }

        ExibirMensagem(
            $"Empréstimo registrado com sucesso.\n" +
            $"Data do empréstimo: {novoEmprestimo.DataEmprestimo:dd/MM/yyyy}\n" +
            $"Data prevista para devolução: {novoEmprestimo.DataDevolucao:dd/MM/yyyy}"
        );
    }

    public void RegistrarDevolucao()
    {
        ExibirCabecalho("Registrar Devolução");
        VisualizarAbertos(false);

        Console.WriteLine("---------------------------------");
        Console.Write("Digite o ID do empréstimo que deseja concluir: ");
        string idSelecionado = Console.ReadLine() ?? string.Empty;

        bool conseguiuRegistrar = repositorioEmprestimo.RegistrarDevolucao(idSelecionado);

        if (!conseguiuRegistrar)
        {
            ExibirMensagem("Não foi possível registrar a devolução.");
            return;
        }

        ExibirMensagem("Devolução registrada com sucesso.");
    }

    public void VisualizarAbertos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Empréstimos Abertos");

        ExibirTabela(repositorioEmprestimo.SelecionarAbertos());

        if (deveExibirCabecalho)
            Aguardar();
    }

    public void VisualizarFechados(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Empréstimos Fechados");

        ExibirTabela(repositorioEmprestimo.SelecionarFechados());

        if (deveExibirCabecalho)
            Aguardar();
    }

    public void VisualizarPorAmigo()
    {
        ExibirCabecalho("Empréstimos por Amigo");

        Amigo? amigoSelecionado = SelecionarAmigo();

        if (amigoSelecionado == null)
        {
            ExibirMensagem("Amigo não encontrado.");
            return;
        }

        Console.WriteLine($"Amigo selecionado: {amigoSelecionado.Nome}");
        Console.WriteLine("---------------------------------");

        ExibirTabela(repositorioEmprestimo.SelecionarPorAmigo(amigoSelecionado.Id));
        Aguardar();
    }

    private void ExibirTabela(Emprestimo?[] emprestimos)
    {
        Console.WriteLine(
            "{0, -7} | {1, -15} | {2, -20} | {3, -10} | {4, -10} | {5, -12}",
            "Id", "Amigo", "Revista", "Empréstimo", "Devolução", "Status"
        );

        for (int i = 0; i < emprestimos.Length; i++)
        {
            Emprestimo? e = emprestimos[i];

            if (e == null)
                continue;

            if (e.Status == StatusEmprestimo.Atrasado)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (e.Status == StatusEmprestimo.Concluido)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ResetColor();

            Console.WriteLine(
                "{0, -7} | {1, -15} | {2, -20} | {3, -10} | {4, -10} | {5, -12}",
                e.Id,
                e.Amigo.Nome,
                e.Revista.Titulo,
                e.DataEmprestimo.ToString("dd/MM/yyyy"),
                e.DataDevolucao.ToString("dd/MM/yyyy"),
                e.Status
            );

            Console.ResetColor();
        }
    }

    private Amigo? SelecionarAmigo()
    {
        Console.WriteLine(
            "{0, -7} | {1, -15} | {2, -20} | {3, -13}",
            "Id", "Nome", "Responsável", "Telefone"
        );

        EntidadeBase?[] amigos = repositorioAmigo.SelecionarTodas();

        for (int i = 0; i < amigos.Length; i++)
        {
            Amigo? amigo = amigos[i] as Amigo;

            if (amigo == null)
                continue;

            Console.WriteLine(
                "{0, -7} | {1, -15} | {2, -20} | {3, -13}",
                amigo.Id, amigo.Nome, amigo.NomeResponsavel, amigo.Telefone
            );
        }

        Console.WriteLine("---------------------------------");
        Console.Write("Digite o ID do amigo: ");
        string idSelecionado = Console.ReadLine() ?? string.Empty;

        return repositorioAmigo.SelecionarPorId(idSelecionado) as Amigo;
    }

    private Revista? SelecionarRevistaDisponivel()
    {
        Console.WriteLine(
            "{0, -7} | {1, -25} | {2, -6} | {3, -10}",
            "Id", "Título", "Edição", "Status"
        );

        Revista?[] revistas = repositorioRevista.SelecionarDisponiveis();

        for (int i = 0; i < revistas.Length; i++)
        {
            Revista? revista = revistas[i];

            if (revista == null)
                continue;

            Console.WriteLine(
                "{0, -7} | {1, -25} | {2, -6} | {3, -10}",
                revista.Id, revista.Titulo, revista.NumeroEdicao, revista.Status
            );
        }

        Console.WriteLine("---------------------------------");
        Console.Write("Digite o ID da revista disponível: ");
        string idSelecionado = Console.ReadLine() ?? string.Empty;

        Revista? revistaSelecionada = repositorioRevista.SelecionarPorId(idSelecionado);

        if (revistaSelecionada == null)
            return null;

        if (revistaSelecionada.Status != StatusRevista.Disponivel)
            return null;

        return revistaSelecionada;
    }

    private void ExibirCabecalho(string titulo)
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Gestão de Empréstimos");
        Console.WriteLine("---------------------------------");
        Console.WriteLine(titulo);
        Console.WriteLine("---------------------------------");
    }

    private void ExibirMensagem(string mensagem)
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine(mensagem);
        Console.WriteLine("---------------------------------");
        Console.Write("Digite ENTER para continuar...");
        Console.ReadLine();
    }

    private void ExibirErros(string[] erros)
    {
        Console.WriteLine("---------------------------------");
        Console.ForegroundColor = ConsoleColor.Red;

        for (int i = 0; i < erros.Length; i++)
            Console.WriteLine(erros[i]);

        Console.ResetColor();
        Console.WriteLine("---------------------------------");
        Console.Write("Digite ENTER para continuar...");
        Console.ReadLine();
    }

    private void Aguardar()
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Digite ENTER para continuar...");
        Console.ReadLine();
    }
}