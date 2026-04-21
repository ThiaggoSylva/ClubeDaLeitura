using ClubeDaLeitura.ConsoleApp.Dominio;

namespace ClubeDaLeitura.ConsoleApp.Dominio.Base;

public class Emprestimo : EntidadeBase
{
    public Amigo Amigo { get; set; }
    public Revista Revista { get; set; }
    public DateTime DataEmprestimo { get; private set; }
    public DateTime DataDevolucao { get; private set; }
    public DateTime? DataDevolucaoRealizada { get; private set; }

    public StatusEmprestimo Status
    {
        get
        {
            if (DataDevolucaoRealizada.HasValue)
                return StatusEmprestimo.Concluido;

            if (DateTime.Today > DataDevolucao.Date)
                return StatusEmprestimo.Atrasado;

            return StatusEmprestimo.Aberto;
        }
    }

    public Emprestimo(Amigo amigo, Revista revista)
    {
        Amigo = amigo;
        Revista = revista;
        DataEmprestimo = DateTime.Today;
        DataDevolucao = DataEmprestimo.AddDays(revista.Caixa.DiasDeEmprestimo);
    }

    public void RegistrarDevolucao()
    {
        DataDevolucaoRealizada = DateTime.Today;
        Revista.Status = StatusRevista.Disponivel;
    }

    public override string[] Validar()
    {
        string erros = string.Empty;

        if (Amigo == null)
            erros += "O campo \"Amigo\" é obrigatório;";

        if (Revista == null)
            erros += "O campo \"Revista\" é obrigatório;";
        else if (Revista.Status != StatusRevista.Disponivel)
            erros += "A revista selecionada não está disponível no momento;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Emprestimo emprestimoAtualizado = (Emprestimo)entidadeAtualizada;

        Amigo = emprestimoAtualizado.Amigo;
        Revista = emprestimoAtualizado.Revista;
    }
}