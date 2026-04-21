using ClubeDaLeitura.ConsoleApp.Dominio.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioRevista
{
    private Revista?[] revistas = new Revista[100];

    public void Cadastrar(Revista novaRevista)
    {
        for (int i = 0; i < revistas.Length; i++)
        {
            if (revistas[i] == null)
            {
                revistas[i] = novaRevista;
                break;
            }
        }
    }

    public bool Editar(string idSelecionado, Revista novaRevista)
    {
        Revista? revistaSelecionada = SelecionarPorId(idSelecionado);

        if (revistaSelecionada == null)
            return false;

        revistaSelecionada.AtualizarRegistro(novaRevista);

        return true;
    }

    public bool Excluir(string idSelecionado)
    {
        for (int i = 0; i < revistas.Length; i++)
        {
            Revista? r = revistas[i];

            if (r == null)
                continue;

            if (r.Id == idSelecionado)
            {
                revistas[i] = null;
                return true;
            }
        }

        return false;
    }

    public Revista? SelecionarPorId(string idSelecionado)
    {
        Revista? revistaSelecionada = null;

        for (int i = 0; i < revistas.Length; i++)
        {
            Revista? r = revistas[i];

            if (r == null)
                continue;

            if (r.Id == idSelecionado)
            {
                revistaSelecionada = r;
                break;
            }
        }

        return revistaSelecionada;
    }

    public Revista?[] SelecionarTodas()
    {
        return revistas;
    }

    public Revista?[] SelecionarDisponiveis()
    {
        Revista?[] disponiveis = new Revista[100];
        int indice = 0;

        for (int i = 0; i < revistas.Length; i++)
        {
            Revista? revista = revistas[i];

            if (revista == null)
                continue;

            if (revista.Status == StatusRevista.Disponivel)
            {
                disponiveis[indice] = revista;
                indice++;
            }
        }

        return disponiveis;
    }

    public bool ExisteRevistaComMesmoTituloEdicao(string titulo, int numeroEdicao)
    {
        for (int i = 0; i < revistas.Length; i++)
        {
            Revista? revista = revistas[i];

            if (revista == null)
                continue;

            if (revista.Titulo.Equals(titulo, StringComparison.OrdinalIgnoreCase) &&
                revista.NumeroEdicao == numeroEdicao)
                return true;
        }

        return false;
    }
}