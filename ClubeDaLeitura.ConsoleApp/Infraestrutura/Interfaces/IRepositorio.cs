using ClubeDaLeitura.ConsoleApp.Dominio.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura.Interfaces;

public interface IRepositorio
{
    void Cadastrar(EntidadeBase entidade);
    bool Editar(string id, EntidadeBase entidadeAtualizada);
    bool Excluir(string id);
    EntidadeBase[] SelecionarTodos();
    EntidadeBase? SelecionarPorId(string id);
}