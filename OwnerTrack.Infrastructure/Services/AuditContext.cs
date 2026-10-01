namespace OwnerTrack.Infrastructure.Services
{
    // Korisničko ime trenutno prijavljenog korisnika, da AuditService može upisati
    // ko je napravio izmjenu bez prosljeđivanja korisnika kroz sve forme i servise.
    // Jedini pisac je UserSession (SignIn/SignOut); null = niko nije prijavljen.
    public static class AuditContext
    {
        public static string? CurrentUsername { get; set; }
    }
}
