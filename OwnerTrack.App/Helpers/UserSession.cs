using OwnerTrack.Infrastructure.Models;

namespace OwnerTrack.App.Helpers
{
    // Lokalna sesija prijavljenog korisnika. Jedna instanca živi u Program.Main i
    // prosljeđuje se FrmLogin-u (SignIn) i Form1-u (čitanje, SignOut) — nema
    // static stanja, a Form1 se ne može otvoriti bez uspješne prijave.
    public sealed class UserSession
    {
        public AuthenticatedUser? CurrentUser { get; private set; }

        public bool IsAuthenticated => CurrentUser is not null;

        public void SignIn(AuthenticatedUser user) => CurrentUser = user;

        public void SignOut() => CurrentUser = null;
    }
}
