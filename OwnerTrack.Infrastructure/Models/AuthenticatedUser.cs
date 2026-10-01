namespace OwnerTrack.Infrastructure.Models
{
    // Neosjetljiv snapshot prijavljenog korisnika (bez hasha/salta) koji se
    // drži u sesiji dok je korisnik prijavljen.
    public sealed record AuthenticatedUser(int Id, string Username, string DisplayName, bool IsActive);
}
