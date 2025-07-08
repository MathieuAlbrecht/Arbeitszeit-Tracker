namespace Arbeitszeitaufschreiben;

public class ArbeitszeitEintrag
{
    public int Startzeit { get; set; }
    public int Endzeit { get; set; }
    public int Pause { get; set; }
    public int Gesamt { get; set; }
    public string? Datum { get; set; }
    
    public string StartzeitString => $"{Startzeit / 60:00}:{Startzeit % 60:00}";
    public string EndzeitString => $"{Endzeit / 60:00}:{Endzeit % 60:00}";
    public string PauseString => $"{Pause / 60:00}:{Pause % 60:00}";
    public string GesamtString => $"{Gesamt / 60:00}:{Gesamt % 60:00}";
}
