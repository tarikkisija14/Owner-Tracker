namespace OwnerTrack.Infrastructure.Models
{
    // Prikaz korisnika za popis računa (bez hasha/salta; Password je čitljiva lozinka ako je poznata).
    public sealed record KorisnikInfo(
        int Id, string Username, string DisplayName, bool IsActive, DateTime? LastLogin, string? Password);
}
