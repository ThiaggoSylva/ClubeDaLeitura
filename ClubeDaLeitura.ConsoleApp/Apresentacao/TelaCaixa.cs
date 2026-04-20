using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaCaixa : TelaBase
{
    private RepositorioCaixa repositorioCaixa;

    public TelaCaixa(RepositorioCaixa rC) : base("Caixa", rC)
    {
        repositorioCaixa = rC;
    }

    public override void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Caixas");

        Console.WriteLine(
            "{0, -7} | {1, -20} | {2, -10} | {3, -20}",
            "Id", "Etiqueta", "Cor", "Tempo de Empréstimo"
        );

        EntidadeBase?[] caixas = repositorioCaixa.SelecionarTodas();

        for (int i = 0; i < caixas.Length; i++)
        {
            Caixa? c = (Caixa?)caixas[i];

            if (c == null)
                continue;

            string corSelecionada = c.Cor;

            if (corSelecionada == "Vermelho")
                Console.ForegroundColor = ConsoleColor.Red;

            else if (corSelecionada == "Verde")
                Console.ForegroundColor = ConsoleColor.Green;

            else if (corSelecionada == "Azul")
                Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(
                "{0, -7} | {1, -20} | {2, -10} | {3, -20}",
                c.Id, c.Etiqueta, c.Cor, c.DiasDeEmprestimo
            );
        }

        Console.ResetColor();

        if (deveExibirCabecalho)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Digite ENTER para continuar...");
            Console.ReadLine();
        }
    }

    protected override EntidadeBase ObterDadosCadastrais()
    {
        Console.Write("Informe a etiqueta da caixa: ");
        string? etiqueta = Console.ReadLine();

        Console.WriteLine("---------------------------------");
        Console.WriteLine("Selecione uma das cores válidas");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("1 - Vermelho");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("2 - Verde");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("3 - Azul");
        Console.ResetColor();
        Console.WriteLine("4 - Branco (Padrão)");
        Console.WriteLine("---------------------------------");

        Console.Write("Informe a cor da caixa: ");
        string? codigoCor = Console.ReadLine();

        string cor;

        if (codigoCor == "1")
            cor = "Vermelho";
        else if (codigoCor == "2")
            cor = "Verde";
        else if (codigoCor == "3")
            cor = "Azul";
        else
            cor = "Branco";

        Console.Write("Informe o tempo de empréstimo das revistas da caixa: ");
        int diasDeEmprestimo = Convert.ToInt32(Console.ReadLine());

        Caixa novaCaixa = new Caixa(etiqueta, cor, diasDeEmprestimo);

        return novaCaixa;
    }
}