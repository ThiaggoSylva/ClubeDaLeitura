using System;
using ClubeDaLeitura.ConsoleApp.Dominio;

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
        Revista? RevistaSelecionada = null;

        for (int i = 0; i < revistas.Length; i++)
        {
            Revista? r = revistas[i];

            if (r == null)
                continue;

            if (r.Id == idSelecionado)
            {
                RevistaSelecionada = r;
                break;
            }
        }

        return RevistaSelecionada;
    }


    public Revista?[] SelecionarTodas()
    {
        return revistas;
    }

    public bool ExisteRevistaComMesmoTituloEdicao(string titulo, int numeroEdicao, string? idIgnorado = null)
{
    for (int i = 0; i < revistas.Length; i++)
    {
        Revista? revista = revistas[i];

        if (revista == null)
            continue;

        bool mesmoTitulo = revista.Titulo.Trim().ToUpper() == titulo.Trim().ToUpper();
        bool mesmaEdicao = revista.NumeroEdicao == numeroEdicao;
        bool mesmoId = revista.Id == idIgnorado;

        if (mesmoTitulo && mesmaEdicao && !mesmoId)
            return true;
    }

    return false;
}
}