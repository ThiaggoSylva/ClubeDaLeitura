using System;
using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Infraestrutura;

namespace ClubeDaLeitura.ConsoleApp.Apresentacao;

public class TelaRevista
{
    private RepositorioRevista repositorioRevista;
    private RepositorioCaixa repositorioCaixa;

    public TelaRevista(RepositorioRevista rR, RepositorioCaixa rC)
    {
        repositorioRevista = rR;
        repositorioCaixa = rC;
    }

    public string ObterOpcaoMenu()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Gestão de Revistas");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1 - Cadastrar revista");
        Console.WriteLine("2 - Editar revista");
        Console.WriteLine("3 - Excluir revista");
        Console.WriteLine("4 - Visualizar revistas");
        Console.WriteLine("S - Voltar para o início");
        Console.WriteLine("---------------------------------");
        Console.Write("> ");

        string? opcaoMenu = Console.ReadLine()?.ToUpper();
        return opcaoMenu;
    }

    public void Cadastrar()
    {
        ExibirCabecalho("Cadastro de Revista");

        Revista novaRevista = ObterDadosCadastrais();

        string[] erros = novaRevista.Validar();

        if (erros.Length > 0)
        {
            ExibirErrosValidacao(erros);
            Cadastrar();
            return;
        }

    
    if (repositorioRevista.ExisteRevistaComMesmoTituloEdicao(novaRevista.Titulo, novaRevista.NumeroEdicao))
        {
        ExibirMensagem("Já existe uma revista com o mesmo título e número de edição.");
        return;
        }

        repositorioRevista.Cadastrar(novaRevista);

        ExibirMensagem($"O registro \"{novaRevista.Id}\" foi cadastrado com sucesso!");
    }

    public void Editar()
    {
        ExibirCabecalho("Edição de Revista");
        VisualizarTodos(false);

        string idSelecionado = ObterIdRegistro("editar");

        Revista novaRevista = ObterDadosCadastrais();

        string[] erros = novaRevista.Validar();

        if (erros.Length > 0)
        {
            ExibirErrosValidacao(erros);
            Editar();
            return;
        }

        
        if (repositorioRevista.ExisteRevistaComMesmoTituloEdicao(
            novaRevista.Titulo,
            novaRevista.NumeroEdicao,
            idSelecionado))
        {
            ExibirMensagem("Já existe outra revista com o mesmo título e número de edição.");
            return;
        }

        bool conseguiuEditar = repositorioRevista.Editar(idSelecionado, novaRevista);

        if (!conseguiuEditar)
        {
            ExibirMensagem("Não foi possível encontrar o registro requisitado.");
            return;
        }

        ExibirMensagem($"O registro \"{idSelecionado}\" foi editado com sucesso.");
    }

    public void Excluir()
    {
        ExibirCabecalho("Exclusão de Revista");
        VisualizarTodos(false);

        string idSelecionado = ObterIdRegistro("excluir");

        bool conseguiuExcluir = repositorioRevista.Excluir(idSelecionado);

        if (!conseguiuExcluir)
        {
            ExibirMensagem("Não foi possível encontrar o registro requisitado.");
            return;
        }

        ExibirMensagem($"O registro \"{idSelecionado}\" foi excluído com sucesso.");
    }

    public void VisualizarTodos(bool deveExibirCabecalho)
    {
        if (deveExibirCabecalho)
            ExibirCabecalho("Visualização de Revistas");

        Console.WriteLine(
            "{0, -7} | {1, -25} | {2, -6} | {3, -4} | {4, -15}",
            "Id", "Título", "Edição", "Ano", "Caixa"
        );

        Revista?[] revistas = repositorioRevista.SelecionarTodas();

        for (int i = 0; i < revistas.Length; i++)
        {
            Revista? r = revistas[i];

            if (r == null)
                continue;

            Console.Write("{0, -7} | ", r.Id);
            Console.Write("{0, -25} | ", r.Titulo);
            Console.Write("{0, -6} | ", r.NumeroEdicao);
            Console.Write("{0, -4} | ", r.AnoPublicacao);

            string corSelecionada = r.Caixa.Cor;

            if (corSelecionada == "Vermelho")
                Console.ForegroundColor = ConsoleColor.Red;
            else if (corSelecionada == "Verde")
                Console.ForegroundColor = ConsoleColor.Green;
            else if (corSelecionada == "Azul")
                Console.ForegroundColor = ConsoleColor.Blue;

            Console.Write("{0, -15}", r.Caixa.Etiqueta);
            Console.ResetColor();
            Console.WriteLine();
        }

        if (deveExibirCabecalho)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Digite ENTER para continuar...");
            Console.ReadLine();
        }
    }

    private void ExibirCabecalho(string titulo)
    {
        Console.Clear();
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Gestão de Revistas");
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

    private void ExibirErrosValidacao(string[] erros)
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

    private string ObterIdRegistro(string acao)
    {
        Console.WriteLine("---------------------------------");

        string? idSelecionado;

        do
        {
            Console.Write($"Digite o ID do registro que deseja {acao}: ");
            idSelecionado = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(idSelecionado) && idSelecionado.Length == 7)
                break;

        } while (true);

        return idSelecionado;
    }

    private Revista ObterDadosCadastrais()
    {
        Console.Write("Digite o título da revista: ");
        string? titulo = Console.ReadLine();

        Console.Write("Digite o número da edição: ");
        int numeroEdicao = Convert.ToInt32(Console.ReadLine());

        Console.Write("Digite o ano de publicação: ");
        int anoPublicacao = Convert.ToInt32(Console.ReadLine());

        string idSelecionado = SelecionarCaixa();
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(idSelecionado);

        return new Revista(titulo, numeroEdicao, anoPublicacao, caixaSelecionada);
    }

    private string SelecionarCaixa()
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine(
            "{0, -7} | {1, -20} | {2, -10} | {3, -20}",
            "Id", "Etiqueta", "Cor", "Tempo de Empréstimo"
        );

        Caixa?[] caixas = repositorioCaixa.SelecionarTodas();

        for (int i = 0; i < caixas.Length; i++)
        {
            Caixa? c = caixas[i];

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
        Console.WriteLine("---------------------------------");

        string? idSelecionado;

        do
        {
            Console.Write("Digite o ID da caixa em que deseja guardar a revista: ");
            idSelecionado = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(idSelecionado) && idSelecionado.Length == 7)
                break;

        } while (true);

        return idSelecionado;
    }
}