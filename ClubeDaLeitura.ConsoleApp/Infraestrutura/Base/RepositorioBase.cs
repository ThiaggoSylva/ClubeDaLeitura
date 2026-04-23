using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Interfaces;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

public abstract class RepositorioBase : IRepositorio
{
    protected List<EntidadeBase> registros = new();

    public void Cadastrar(EntidadeBase entidade)
    {
        registros.Add(entidade);
    }

    public bool Editar(string id, EntidadeBase entidadeAtualizada)
    {
        EntidadeBase? entidade = SelecionarPorId(id);

        if (entidade == null)
            return false;

        entidade.AtualizarRegistro(entidadeAtualizada);
        return true;
    }

    public bool Excluir(string id)
    {
        EntidadeBase? entidade = SelecionarPorId(id);

        if (entidade == null)
            return false;

        registros.Remove(entidade);
        return true;
    }

    public EntidadeBase[] SelecionarTodos()
    {
        return registros.ToArray();
    }

    public EntidadeBase? SelecionarPorId(string id)
    {
        return registros.FirstOrDefault(x => x.Id == id);
    }
}