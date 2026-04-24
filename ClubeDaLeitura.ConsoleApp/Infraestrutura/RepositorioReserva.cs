using ClubeDaLeitura.ConsoleApp.Dominio;
using ClubeDaLeitura.ConsoleApp.Dominio.Enums;
using ClubeDaLeitura.ConsoleApp.Infraestrutura.Base;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioReserva : RepositorioBase
{
    public bool ExisteReservaAtivaParaRevista(string revistaId)
    {
        AtualizarReservasExpiradas();

        return registros
            .OfType<Reserva>()
            .Any(x =>
                x.Revista.Id == revistaId &&
                x.Status == StatusReserva.Ativa);
    }

    public bool ExisteReservaAtivaParaAmigo(string amigoId)
    {
        AtualizarReservasExpiradas();

        return registros
            .OfType<Reserva>()
            .Any(x =>
                x.Amigo.Id == amigoId &&
                x.Status == StatusReserva.Ativa);
    }

    public Reserva[] SelecionarAtivas()
    {
        AtualizarReservasExpiradas();

        return registros
            .OfType<Reserva>()
            .Where(x => x.Status == StatusReserva.Ativa)
            .ToArray();
    }

    public Reserva[] SelecionarTodasReservas()
    {
        AtualizarReservasExpiradas();

        return registros
            .OfType<Reserva>()
            .ToArray();
    }

    public Reserva[] SelecionarPorAmigo(string amigoId)
    {
        AtualizarReservasExpiradas();

        return registros
            .OfType<Reserva>()
            .Where(x => x.Amigo.Id == amigoId)
            .ToArray();
    }

    public void AtualizarReservasExpiradas()
    {
        foreach (Reserva reserva in registros.OfType<Reserva>())
        {
            if (reserva.EstaExpirada())
                reserva.Expirar();
        }
    }
}