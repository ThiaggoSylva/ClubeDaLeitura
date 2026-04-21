using ClubeDaLeitura.ConsoleApp.Dominio.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioEmprestimo : RepositorioBase
{
    public bool RegistrarEmprestimo(Emprestimo emprestimo)
    {
        if (AmigoPossuiEmprestimoAtivo(emprestimo.Amigo.Id))
            return false;

        if (emprestimo.Revista.Status != StatusRevista.Disponivel)
            return false;

        emprestimo.Revista.Status = StatusRevista.Emprestada;
        Cadastrar(emprestimo);

        return true;
    }

    public bool RegistrarDevolucao(string idSelecionado)
    {
        Emprestimo? emprestimo = SelecionarPorId(idSelecionado) as Emprestimo;

        if (emprestimo == null)
            return false;

        if (emprestimo.Status == StatusEmprestimo.Concluido)
            return false;

        emprestimo.RegistrarDevolucao();

        return true;
    }

    public bool AmigoPossuiEmprestimoAtivo(string idAmigo)
    {
        for (int i = 0; i < registros.Length; i++)
        {
            Emprestimo? emprestimo = registros[i] as Emprestimo;

            if (emprestimo == null)
                continue;

            if (emprestimo.Amigo.Id == idAmigo &&
                emprestimo.Status != StatusEmprestimo.Concluido)
                return true;
        }

        return false;
    }

    public bool AmigoTemEmprestimosVinculados(string idAmigo)
    {
        for (int i = 0; i < registros.Length; i++)
        {
            Emprestimo? emprestimo = registros[i] as Emprestimo;

            if (emprestimo == null)
                continue;

            if (emprestimo.Amigo.Id == idAmigo)
                return true;
        }

        return false;
    }

    public Emprestimo?[] SelecionarAbertos()
    {
        Emprestimo?[] emprestimos = new Emprestimo[100];
        int indice = 0;

        for (int i = 0; i < registros.Length; i++)
        {
            Emprestimo? emprestimo = registros[i] as Emprestimo;

            if (emprestimo == null)
                continue;

            if (emprestimo.Status == StatusEmprestimo.Aberto ||
                emprestimo.Status == StatusEmprestimo.Atrasado)
            {
                emprestimos[indice] = emprestimo;
                indice++;
            }
        }

        return emprestimos;
    }

    public Emprestimo?[] SelecionarFechados()
    {
        Emprestimo?[] emprestimos = new Emprestimo[100];
        int indice = 0;

        for (int i = 0; i < registros.Length; i++)
        {
            Emprestimo? emprestimo = registros[i] as Emprestimo;

            if (emprestimo == null)
                continue;

            if (emprestimo.Status == StatusEmprestimo.Concluido)
            {
                emprestimos[indice] = emprestimo;
                indice++;
            }
        }

        return emprestimos;
    }

    public Emprestimo?[] SelecionarPorAmigo(string idAmigo)
    {
        Emprestimo?[] emprestimos = new Emprestimo[100];
        int indice = 0;

        for (int i = 0; i < registros.Length; i++)
        {
            Emprestimo? emprestimo = registros[i] as Emprestimo;

            if (emprestimo == null)
                continue;

            if (emprestimo.Amigo.Id == idAmigo)
            {
                emprestimos[indice] = emprestimo;
                indice++;
            }
        }

        return emprestimos;
    }
}