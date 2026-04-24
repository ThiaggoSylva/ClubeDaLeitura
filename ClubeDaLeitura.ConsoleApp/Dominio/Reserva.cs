using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Dominio.Enums;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Reserva : EntidadeBase
{
    public Amigo Amigo { get; set; }
    public Revista Revista { get; set; }
    public DateTime DataReserva { get; set; }
    public DateTime DataExpiracao { get; set; }
    public StatusReserva Status { get; set; }

    public Reserva(Amigo amigo, Revista revista)
    {
        Amigo = amigo;
        Revista = revista;

        DataReserva = DateTime.Now.Date;
        DataExpiracao = DataReserva.AddDays(2);

        Status = StatusReserva.Ativa;
        Revista.Status = StatusRevista.Reservada;
    }

    public bool EstaExpirada()
    {
        return Status == StatusReserva.Ativa && DateTime.Now.Date > DataExpiracao.Date;
    }

    public void Cancelar()
    {
        Status = StatusReserva.Cancelada;
        Revista.Status = StatusRevista.Disponivel;
    }

    public void Expirar()
    {
        Status = StatusReserva.Expirada;
        Revista.Status = StatusRevista.Disponivel;
    }

    public void ConverterEmEmprestimo()
    {
        Status = StatusReserva.ConvertidaEmEmprestimo;
        Revista.Status = StatusRevista.Emprestada;
    }

    public override string[] Validar()
    {
        List<string> erros = new();

        if (Amigo == null)
            erros.Add("O amigo é obrigatório.");

        if (Revista == null)
            erros.Add("A revista é obrigatória.");

        if (DataExpiracao < DataReserva)
            erros.Add("A data de expiração não pode ser menor que a data da reserva.");

        return erros.ToArray();
    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Reserva reservaAtualizada = (Reserva)entidadeAtualizada;

        Amigo = reservaAtualizada.Amigo;
        Revista = reservaAtualizada.Revista;
        DataReserva = reservaAtualizada.DataReserva;
        DataExpiracao = reservaAtualizada.DataExpiracao;
        Status = reservaAtualizada.Status;
    }
}