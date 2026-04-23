namespace ClubeDaLeitura.ConsoleApp.Apresentacao.Interfaces;

public interface ITelaCrud
{
    string NomeEntidade { get; }
    string ObterOpcaoMenu();
    void Cadastrar();
    void Editar();
    void Excluir();
    void VisualizarTodos(bool exibirCabecalho);
}