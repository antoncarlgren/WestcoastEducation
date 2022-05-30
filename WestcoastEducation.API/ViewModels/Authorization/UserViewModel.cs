namespace WestcoastEducation.API.ViewModels.Authorization;

public class UserViewModel
{
    public DateTime? Expires { get; set; }
    public string? UserName { get; set; }
    public string? Token { get; set; }
}