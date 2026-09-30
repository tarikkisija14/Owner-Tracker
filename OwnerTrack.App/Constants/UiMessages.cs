namespace OwnerTrack.App.Constants
{
    
    internal static class UiMessages
    {
        

        public const string StartupErrorTitle = "Greška pokretanja";
        public const string StartupErrorFormat =
            "Greška pri pokretanju aplikacije:\n\n{0}\n\n" +
            "Baza podataka: {1}\n\n" +
            "Detalji su sačuvani u log fajlu:\n{2}\n\n" +
            "Ako problem ostane, kontaktiraj podršku.";

        public const string UnhandledExceptionTitle = "Neočekivana greška";
        public const string UnhandledExceptionFormat =
            "Desila se neočekivana greška.\n\nDetalji su sačuvani u:\n{0}\n\nGreška: {1}";

        

        public const string ArchiveKlijentPrompt = "Firma će biti arhivirana i neće biti vidljiva.";
        public const string ArchiveVlasnikPrompt = "Arhivirati vlasnika?";
        public const string ArchiveDirektorPrompt = "Arhivirati direktora?";

        public const string ArchiveKlijentSuccess = "Firma je arhivirana.";
        public const string ArchiveVlasnikSuccess = "Vlasnik arhiviran.";
        public const string ArchiveDirektorSuccess = "Direktor arhiviran.";

        

        public const string AuditArchivedKlijent = "Arhivirana firma: '{0}'";
        public const string AuditArchivedVlasnik = "Arhiviran: '{0}'";
        public const string AuditArchivedDirektor = "Arhiviran: '{0}'";

        

        public const string SelectFirmFirst = "Prvo odaberi firmu!";
        public const string SelectFirm = "Odaberi firmu!";
        public const string SelectFirmOnKlijentiScreen = "Odaberi firmu iz liste i klikni 'Obriši' da je arhiviraš.";
        public const string SelectVlasnik = "Odaberi vlasnika!";
        public const string SelectDirektor = "Odaberi direktora!";

       

        public const string PdfNoClientsToExport = "Nema klijenata za export.";
        public const string PdfNoClientSelected = "Odaberi firmu iz liste.";
        public const string PdfCannotReadSelected = "Nije moguće pročitati odabranu firmu.";
        public const string PdfTableSaveTitle = "Sačuvaj tabelu klijenata";
        public const string PdfReportSaveTitle = "Sačuvaj izvještaj firme";
        public const string PdfTableFilePrefix = "Klijenti_tabela_";
        public const string PdfNoDataToExport = "Nema podataka za export.";
        public const string PdfNoRowSelected = "Odaberi red iz liste.";
        public const string PdfSavedPromptFormat = "PDF je sačuvan:\n{0}\n\nŽeliš li ga otvoriti?";
        public const string PdfSavedTitle = "PDF kreiran";
        public const string PdfErrorFormat = "Greška pri generisanju PDF-a. Detalji su sačuvani u logu:\n{0}";

        

        public const string ImportInProgressMessage = "Import je u toku. Molimo sačekajte da se završi ili pritisnite Otkaži.";
        public const string ImportInProgressTitle = "Import u toku";
        public const string ImportCancellingText = "Otkazivanje...";
        public const string ImportCancelledByUser = "Import je otkazan od strane korisnika.";
        public const string ImportErrorsTitle = "Upozorenje";
        public const string ImportErrorsFormat = "Greške tokom importa:\n{0}";
        public const string ImportErrorTitle = "Greška";
        public const string ImportErrorFormat = "Greška: {0}";

        public const string ResetImportDialogTitle = "Odaberi Excel fajl za reimport";
        public const string ResetImportFileNotFound = "Odabrani fajl nije pronađen.";
        public const string ResetImportFileNotFoundTitle = "Greška";
        public const string ExcelValidationErrorTitle = "Greška validacije";
        public const string ExcelValidationErrorFormat = "Odabrani fajl nije validan:\n\n{0}";

        public const string BackupRestorePromptFormat =
            "Import nije završen u potpunosti (greška, otkazivanje, greške u redovima ili nijedan uvezen red), a baza je već bila obrisana.\n\n" +
            "Hoćeš li vratiti podatke iz backupa?\n\nBackup: {0}";
        public const string BackupRestoreTitle = "Vraćanje podataka";
        public const string BackupKeptFormat = "Backup ostaje sačuvan na:\n{0}\n\nMožeš ga ručno vratiti ako zatreba.";
        public const string BackupKeptTitle = "Backup sačuvan";
        public const string BackupRestoredSuccess = "Podaci su uspješno vraćeni iz backupa.";
        public const string BackupRestoredSuccessTitle = "Vraćanje uspješno";
        public const string BackupRestoreFailedFormat =
            "Vraćanje nije uspjelo automatski.\n\n" +
            "Ručno kopiraj ovaj fajl:\n{0}\n\ni preimenuj ga u 'Firme.db' na istoj lokaciji.";
        public const string BackupRestoreFailedTitle = "Ručno vraćanje";

        

        public const string KlijentNazivRequired = "Naziv klijenta je obavezan.";
        public const string KlijentIdBrojRequired = "Molimo unesite ID broj.";
        public const string KlijentUgovorDeleteConfirm =
            "Status ugovora je prazan, pa će postojeći ugovor ovog klijenta biti OBRISAN " +
            "(uključujući vrstu, datum i napomenu ugovora).";
        public const string KlijentSaveChangesButton = "Spremi izmjene";
        public const string KlijentSaveNewButton = "Dodaj";
        public const string KlijentEditTitle = "Izmijeni firmu";
        public const string KlijentSavedUpdate = "Klijent ažuriran!";
        public const string KlijentSavedNew = "Klijent dodan!";
        public const string KlijentDuplicateIdBrojFormat = "Klijent s ID brojem '{0}' već postoji!";
        public const string KlijentDuplicateNazivFormat = "Klijent s nazivom '{0}' već postoji!";
        public const string KlijentDuplicateTitle = "Duplikat";

        

        public const string VlasnikNameRequired = "Upiši ime i prezime!";
        public const string VlasnikDuplicateFormat = "Vlasnik '{0}' već postoji za ovu firmu!";
        public const string VlasnikDuplicateTitle = "Duplikat";
        public const string VlasnikPercentageError = "Procenat vlasništva mora biti broj između 0 i 100!";
        public const string VlasnikPercentageErrorTitle = "Greška validacije";
        public const string VlasnikEditTitle = "Izmijeni vlasnika";
        public const string VlasnikAddTitle = "Dodaj novog vlasnika";
        public const string VlasnikSavedUpdate = "Ažurirano!";
        public const string VlasnikSavedNew = "Dodano!";

        

        public const string DirektorNameRequired = "Upiši ime i prezime direktora!";
        public const string DirektorDuplicateFormat = "Direktor '{0}' već postoji za ovu firmu!";
        public const string DirektorDuplicateTitle = "Duplikat";
        public const string DirektorEditTitle = "Izmijeni direktora";
        public const string DirektorAddTitle = "Dodaj novog direktora";
        public const string DirektorSavedUpdate = "Ažurirano!";
        public const string DirektorSavedNew = "Dodano!";

        

        public const string WarningSummaryFormat =
            "Ukupno {0} firma s upozorenjima   |   " +
            " Isteklo: {1}   " +
            " Kritično (≤{2} dana): {3}   " +
            " Uskoro ({4}–{5} dana): {6}";

        public const string WarningStatusExpired = "Isteklo";
        public const string WarningStatusCritical = "Kritično";
        public const string WarningStatusUpcoming = "Uskoro";

        public const string WarningAcknowledgedSaved = "Upozorenje označeno kao pregledano.";
        public const string WarningAlreadyAcknowledged = "Ovo upozorenje je već označeno kao pregledano.";
        public const string SelectWarningRow = "Odaberi upozorenje iz liste.";



        public const string SavedTitle = "Sačuvano";

        public const string UnsavedChangesTitle = "Nesačuvane promjene";
        public const string UnsavedChangesPrompt = "Imate nesačuvane promjene. Želite li zatvoriti bez spremanja?";
        public const string UnsavedChangesSavePrompt =
            "Imate nesačuvane promjene.\n\nŽelite li ih sačuvati prije zatvaranja?";

        public const string GenericErrorPrefix = "Greška";
        public const string GenericErrorFormat = "Greška: {0}";
        public const string GenericErrorWithPrefixFormat = "{0}: {1}";

        public const string ConcurrencyConflictTitle = "Zapis je izmijenjen";
        public const string ConcurrencyConflictMessage =
            "Ovaj zapis je u međuvremenu izmijenjen (u drugom prozoru ili od strane drugog korisnika).\n\n" +
            "Tvoje izmjene NISU sačuvane da ne bi prepisale tuđe.\n\n" +
            "Zatvori ovaj prozor i ponovo otvori zapis da vidiš najnovije podatke, pa unesi izmjene ponovo.";
    }
}