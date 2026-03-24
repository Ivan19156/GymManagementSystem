namespace GymManagement.WebMVC.Models;

public class ClientRowDto
{
    public string FirstName    { get; set; } = "";
    public string LastName     { get; set; } = "";
    public string Email        { get; set; } = "";
    public string? Phone       { get; set; }
    public string? MedicalNotes { get; set; }
}
