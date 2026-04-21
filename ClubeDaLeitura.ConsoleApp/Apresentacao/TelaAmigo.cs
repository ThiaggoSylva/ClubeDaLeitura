using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;
public class TelaAmigo : TelaBase
{
    private RepositorioAmigo repositorioAmigo;
    private RepositorioEmprestimo repositorioEmprestimo;

    public TelaAmigo(RepositorioAmigo repositorioAmigo, RepositorioEmprestimo repositorioEmprestimo)
        : base("Amigo", repositorioAmigo)
    {
        this.repositorioAmigo = repositorioAmigo;
        this.repositorioEmprestimo = repositorioEmprestimo;
    }

    public new void Excluir()
    {
        ExibirCabecalho("Exclusão de Amigo");
        VisualizarTodos(false);

        Console.WriteLine("---------------------------------");
        Console.Write("Digite o ID do amigo que deseja excluir: ");
        string idSelecionado = Console.ReadLine() ?? string.Empty;

        if (repositorioEmprestimo.AmigoTemEmprestimosVinculados(idSelecionado))
        {
            ExibirMensagem("Não é permitido excluir um amigo com empréstimos vinculados.");
            return;
        }

        bool conseguiuExcluir = repositorioAmigo.Excluir(idSelecionado);

        if (!conseguiuExcluir)
        {
            ExibirMensagem("Não foi possível encontrar o registro requisitado.");
            return;
        }

        ExibirMensagem($"O registro \"{idSelecionado}\" foi excluído com sucesso.");
    }

    public override void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Amigos");

        Console.WriteLine(
            "{0, -7} | {1, -15} | {2, -20} | {3, -13}",
            "Id", "Nome", "Responsável", "Telefone"
        );

        EntidadeBase?[] amigos = repositorioAmigo.SelecionarTodas();

        for (int i = 0; i < amigos.Length; i++)
        {
            Amigo? a = (Amigo?)amigos[i];

            if (a == null)
                continue;

            Console.WriteLine(
                "{0, -7} | {1, -15} | {2, -20} | {3, -13}",
                a.Id, a.Nome, a.NomeResponsavel, a.Telefone
            );
        }

        if (deveExibirCabecalho)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Digite ENTER para continuar...");
            Console.ReadLine();
        }
    }

    protected override EntidadeBase ObterDadosCadastrais()
    {
        Console.Write("Digite o nome: ");
        string nome = Console.ReadLine() ?? string.Empty;

        Console.Write("Digite o nome do responsável: ");
        string nomeResponsavel = Console.ReadLine() ?? string.Empty;

        Console.Write("Digite o telefone: ");
        string telefone = Console.ReadLine() ?? string.Empty;

        return new Amigo(nome, nomeResponsavel, telefone);
    }
}