using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaPrincipal
{
    private readonly RepositorioCaixa repositorioCaixa = new();
    private readonly RepositorioRevista repositorioRevista = new();
    private readonly RepositorioAmigo repositorioAmigo = new();
    private readonly RepositorioEmprestimo repositorioEmprestimo = new();

    private readonly TelaCaixa telaCaixa;
    private readonly TelaRevista telaRevista;
    private readonly TelaAmigo telaAmigo;
    private readonly TelaEmprestimo telaEmprestimo;

    public TelaPrincipal()
    {
        telaCaixa = new TelaCaixa(repositorioCaixa, repositorioRevista);
        telaRevista = new TelaRevista(repositorioRevista, repositorioCaixa, repositorioEmprestimo);
        telaAmigo = new TelaAmigo(repositorioAmigo, repositorioEmprestimo);
        telaEmprestimo = new TelaEmprestimo(repositorioEmprestimo, repositorioAmigo, repositorioRevista);

        Seed();
    }

    public void Executar()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Clube da Leitura");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1 - Caixas");
            Console.WriteLine("2 - Revistas");
            Console.WriteLine("3 - Amigos");
            Console.WriteLine("4 - Empréstimos");
            Console.WriteLine("S - Sair");
            Console.WriteLine("---------------------------------");
            Console.Write("> ");

            string opcao = Console.ReadLine()?.ToUpper() ?? "S";

            if (opcao == "S")
                break;
            else if (opcao == "1")
                ExecutarTelaCrud(telaCaixa);
            else if (opcao == "2")
                ExecutarTelaCrud(telaRevista);
            else if (opcao == "3")
                ExecutarTelaAmigo();
            else if (opcao == "4")
                telaEmprestimo.ApresentarMenu();
        }
    }

    private void ExecutarTelaCrud(TelaBase tela)
    {
        while (true)
        {
            string opcao = tela.ObterOpcaoMenu();

            if (opcao == "S")
                break;
            else if (opcao == "1")
                tela.Cadastrar();
            else if (opcao == "2")
                tela.Editar();
            else if (opcao == "3")
                tela.Excluir();
            else if (opcao == "4")
                tela.VisualizarTodos(true);
        }
    }

    private void ExecutarTelaAmigo()
    {
        while (true)
        {
            string opcao = telaAmigo.ObterOpcaoMenu();

            if (opcao == "S")
                break;
            else if (opcao == "1")
                telaAmigo.Cadastrar();
            else if (opcao == "2")
                telaAmigo.Editar();
            else if (opcao == "3")
                telaAmigo.Excluir();
            else if (opcao == "4")
                telaAmigo.VisualizarTodos(true);
            else if (opcao == "5")
                telaAmigo.VisualizarEmprestimosDoAmigo();
        }
    }

    private void Seed()
    {
        if (repositorioCaixa.SelecionarTodos().Length > 0)
            return;

        var caixa = new Dominio.Caixa("Lançamentos", "Vermelho", 7);
        repositorioCaixa.Cadastrar(caixa);

        var revista = new Dominio.Revista("Action Comics", 155, 1990, caixa);
        repositorioRevista.Cadastrar(revista);

        var amigo = new Dominio.Amigo("Joãozinho", "Dona Cleide", "49982224353");
        repositorioAmigo.Cadastrar(amigo);
    }
}