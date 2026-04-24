using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Enums;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaReserva
{
    private readonly RepositorioReserva repositorioReserva;
    private readonly RepositorioAmigo repositorioAmigo;
    private readonly RepositorioRevista repositorioRevista;
    private readonly RepositorioEmprestimo repositorioEmprestimo;

    public TelaReserva(
        RepositorioReserva repositorioReserva,
        RepositorioAmigo repositorioAmigo,
        RepositorioRevista repositorioRevista,
        RepositorioEmprestimo repositorioEmprestimo)
    {
        this.repositorioReserva = repositorioReserva;
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioRevista = repositorioRevista;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public void ApresentarMenu()
    {
        while (true)
        {
            repositorioReserva.AtualizarReservasExpiradas();

            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Gestão de Reservas");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1 - Registrar reserva");
            Console.WriteLine("2 - Cancelar reserva");
            Console.WriteLine("3 - Converter reserva em empréstimo");
            Console.WriteLine("4 - Visualizar reservas ativas");
            Console.WriteLine("5 - Visualizar histórico de reservas");
            Console.WriteLine("6 - Atualizar reservas expiradas");
            Console.WriteLine("S - Voltar");
            Console.WriteLine("---------------------------------");
            Console.Write("> ");

            string opcao = Console.ReadLine()?.ToUpper() ?? "S";

            if (opcao == "S")
                break;
            else if (opcao == "1")
                RegistrarReserva();
            else if (opcao == "2")
                CancelarReserva();
            else if (opcao == "3")
                ConverterReservaEmEmprestimo();
            else if (opcao == "4")
                VisualizarReservasAtivas();
            else if (opcao == "5")
                VisualizarHistoricoReservas();
            else if (opcao == "6")
                AtualizarReservasExpiradas();
        }
    }

    private void RegistrarReserva()
    {
        repositorioReserva.AtualizarReservasExpiradas();

        Console.Clear();
        Console.WriteLine("Registrar Reserva");
        Console.WriteLine();

        Amigo amigo = SelecionarAmigo();

        if (repositorioEmprestimo.AmigoPossuiEmprestimoAtivo(amigo))
        {
            Pausar("Este amigo possui empréstimo ativo e não pode fazer reserva.");
            return;
        }

        if (repositorioReserva.ExisteReservaAtivaParaAmigo(amigo.Id))
        {
            Pausar("Este amigo já possui uma reserva ativa.");
            return;
        }

        Revista revista = SelecionarRevistaDisponivel();

        if (repositorioReserva.ExisteReservaAtivaParaRevista(revista.Id))
        {
            Pausar("Esta revista já possui uma reserva ativa.");
            return;
        }

        if (revista.Status != StatusRevista.Disponivel)
        {
            Pausar("A revista precisa estar disponível para ser reservada.");
            return;
        }

        Reserva reserva = new Reserva(amigo, revista);

        string[] erros = reserva.Validar();

        if (erros.Length > 0)
        {
            ExibirErros(erros);
            return;
        }

        repositorioReserva.Cadastrar(reserva);

        Pausar($"Reserva registrada com sucesso. Expira em {reserva.DataExpiracao:dd/MM/yyyy}.");
    }

    private void CancelarReserva()
    {
        repositorioReserva.AtualizarReservasExpiradas();

        Console.Clear();
        Console.WriteLine("Cancelar Reserva");
        Console.WriteLine();

        VisualizarReservasAtivas(false);

        Console.Write("Digite o ID da reserva: ");
        string id = Console.ReadLine() ?? string.Empty;

        Reserva? reserva = repositorioReserva.SelecionarPorId(id) as Reserva;

        if (reserva == null)
        {
            Pausar("Reserva não encontrada.");
            return;
        }

        if (reserva.Status != StatusReserva.Ativa)
        {
            Pausar("Apenas reservas ativas podem ser canceladas.");
            return;
        }

        reserva.Cancelar();

        Pausar("Reserva cancelada com sucesso.");
    }

    private void ConverterReservaEmEmprestimo()
    {
        repositorioReserva.AtualizarReservasExpiradas();

        Console.Clear();
        Console.WriteLine("Converter Reserva em Empréstimo");
        Console.WriteLine();

        VisualizarReservasAtivas(false);

        Console.Write("Digite o ID da reserva: ");
        string id = Console.ReadLine() ?? string.Empty;

        Reserva? reserva = repositorioReserva.SelecionarPorId(id) as Reserva;

        if (reserva == null)
        {
            Pausar("Reserva não encontrada.");
            return;
        }

        if (reserva.Status != StatusReserva.Ativa)
        {
            Pausar("Apenas reservas ativas podem ser convertidas em empréstimo.");
            return;
        }

        if (reserva.EstaExpirada())
        {
            reserva.Expirar();
            Pausar("Esta reserva expirou e não pode mais ser convertida.");
            return;
        }

        if (repositorioEmprestimo.AmigoPossuiEmprestimoAtivo(reserva.Amigo))
        {
            Pausar("Este amigo já possui empréstimo ativo.");
            return;
        }

        reserva.ConverterEmEmprestimo();

        Emprestimo emprestimo = new Emprestimo(reserva.Amigo, reserva.Revista);
        repositorioEmprestimo.Cadastrar(emprestimo);

        Pausar($"Reserva convertida em empréstimo. Devolução prevista: {emprestimo.DataDevolucao:dd/MM/yyyy}.");
    }

    private void VisualizarReservasAtivas()
    {
        repositorioReserva.AtualizarReservasExpiradas();

        Console.Clear();
        Console.WriteLine("Reservas Ativas");
        Console.WriteLine();

        VisualizarReservasAtivas(false);

        Pausar("Fim da listagem.");
    }

    private void VisualizarReservasAtivas(bool pausar)
    {
        Console.WriteLine("{0,-8} | {1,-20} | {2,-20} | {3,-12} | {4,-12} | {5,-12}",
            "ID", "Amigo", "Revista", "Reserva", "Expiração", "Status");

        foreach (Reserva reserva in repositorioReserva.SelecionarAtivas())
        {
            ExibirLinhaReserva(reserva);
        }

        if (pausar)
            Pausar("Fim da listagem.");
    }

    private void VisualizarHistoricoReservas()
    {
        repositorioReserva.AtualizarReservasExpiradas();

        Console.Clear();
        Console.WriteLine("Histórico de Reservas");
        Console.WriteLine();

        Console.WriteLine("{0,-8} | {1,-20} | {2,-20} | {3,-12} | {4,-12} | {5,-22}",
            "ID", "Amigo", "Revista", "Reserva", "Expiração", "Status");

        foreach (Reserva reserva in repositorioReserva.SelecionarTodasReservas())
        {
            ExibirLinhaReserva(reserva);
        }

        Pausar("Fim do histórico.");
    }

    private void AtualizarReservasExpiradas()
    {
        repositorioReserva.AtualizarReservasExpiradas();
        Pausar("Reservas expiradas atualizadas com sucesso.");
    }

    private void ExibirLinhaReserva(Reserva reserva)
    {
        Console.Write("{0,-8} | {1,-20} | {2,-20} | {3,-12:dd/MM/yyyy} | {4,-12:dd/MM/yyyy} | ",
            reserva.Id,
            reserva.Amigo.Nome,
            reserva.Revista.Titulo,
            reserva.DataReserva,
            reserva.DataExpiracao);

        Console.ForegroundColor = ObterCorStatusReserva(reserva.Status);
        Console.WriteLine("{0,-22}", reserva.Status);
        Console.ResetColor();
    }

    private ConsoleColor ObterCorStatusReserva(StatusReserva status)
    {
        return status switch
        {
            StatusReserva.Ativa => ConsoleColor.Yellow,
            StatusReserva.Cancelada => ConsoleColor.DarkGray,
            StatusReserva.ConvertidaEmEmprestimo => ConsoleColor.Green,
            StatusReserva.Expirada => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }

    private Amigo SelecionarAmigo()
    {
        Console.WriteLine("Amigos cadastrados:");

        foreach (Amigo amigo in repositorioAmigo.SelecionarTodos().OfType<Amigo>())
            Console.WriteLine($"{amigo.Id} | {amigo.Nome}");

        while (true)
        {
            Console.Write("Digite o ID do amigo: ");
            string id = Console.ReadLine() ?? string.Empty;

            Amigo? amigo = repositorioAmigo.SelecionarPorId(id) as Amigo;

            if (amigo != null)
                return amigo;

            Console.WriteLine("ID inválido. Tente novamente.");
        }
    }

    private Revista SelecionarRevistaDisponivel()
    {
        Console.WriteLine();
        Console.WriteLine("Revistas disponíveis:");

        foreach (Revista revista in repositorioRevista.SelecionarDisponiveis())
            Console.WriteLine($"{revista.Id} | {revista.Titulo} | {revista.Status}");

        while (true)
        {
            Console.Write("Digite o ID da revista: ");
            string id = Console.ReadLine() ?? string.Empty;

            Revista? revista = repositorioRevista.SelecionarPorId(id) as Revista;

            if (revista != null && revista.Status == StatusRevista.Disponivel)
                return revista;

            Console.WriteLine("ID inválido ou revista indisponível. Tente novamente.");
        }
    }

    private void ExibirErros(string[] erros)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        foreach (string erro in erros)
            Console.WriteLine(erro);

        Console.ResetColor();

        Pausar("Corrija os erros acima.");
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