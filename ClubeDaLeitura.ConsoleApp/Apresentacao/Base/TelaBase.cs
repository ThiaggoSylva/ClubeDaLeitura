using ClubeDaLeitura.ConsoleApp.Apresentacao.Interfaces;
using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Interfaces;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public abstract class TelaBase : ITelaCrud
{
    protected readonly IRepositorio repositorio;

    public abstract string NomeEntidade { get; }

    protected TelaBase(IRepositorio repositorio)
    {
        this.repositorio = repositorio;
    }

    public virtual string ObterOpcaoMenu()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine($"Gestão de {NomeEntidade}");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Cadastrar");
        Console.WriteLine("2 - Editar");
        Console.WriteLine("3 - Excluir");
        Console.WriteLine("4 - Visualizar todos");
        Console.WriteLine("S - Voltar");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");

        return Console.ReadLine()?.ToUpper() ?? "S";
    }

    public virtual void Cadastrar()
    {
        ExibirCabecalho($"Cadastro de {NomeEntidade}");
        EntidadeBase entidade = ObterDados();
        SalvarCadastro(entidade);
    }

    public virtual void Editar()
    {
        ExibirCabecalho($"Edição de {NomeEntidade}");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        EntidadeBase entidade = ObterDados();
        SalvarEdicao(id, entidade);
    }

    public virtual void Excluir()
    {
        ExibirCabecalho($"Exclusão de {NomeEntidade}");
        VisualizarTodos(false);

        Console.Write("Digite o ID: ");
        string id = Console.ReadLine() ?? string.Empty;

        if (!repositorio.Excluir(id))
        {
            Mensagem("Registro não encontrado.");
            return;
        }

        Mensagem("Registro excluído com sucesso.");
    }

    protected void SalvarCadastro(EntidadeBase entidade)
    {
        string[] erros = entidade.Validar();

        if (erros.Length > 0)
        {
            ExibirErros(erros);
            return;
        }

        repositorio.Cadastrar(entidade);
        Mensagem("Cadastro realizado com sucesso.");
    }

    protected void SalvarEdicao(string id, EntidadeBase entidade)
    {
        string[] erros = entidade.Validar();

        if (erros.Length > 0)
        {
            ExibirErros(erros);
            return;
        }

        if (!repositorio.Editar(id, entidade))
        {
            Mensagem("Registro não encontrado.");
            return;
        }

        Mensagem("Edição realizada com sucesso.");
    }

    protected void ExibirCabecalho(string titulo)
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine(titulo);
        Console.WriteLine("---------------------------------");
    }

    protected void Mensagem(string texto)
    {
        Console.WriteLine();
        Console.WriteLine(texto);
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    protected void ExibirErros(string[] erros)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();

        foreach (string erro in erros)
            Console.WriteLine(erro);

        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Pressione ENTER para continuar...");
        Console.ReadLine();
    }

    protected int LerInteiro(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int valor))
                return valor;

            Console.WriteLine("Valor inválido.");
        }
    }

    protected void LinhaTabela(params int[] largurasColunas)
    {
            int total = largurasColunas.Sum() + (largurasColunas.Length - 1) * 3;
            Console.WriteLine(new string('-', total));
    }

    protected abstract EntidadeBase ObterDados();
    public abstract void VisualizarTodos(bool exibirCabecalho);
}