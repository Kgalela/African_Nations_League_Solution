using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using Web.Infrustructure.Enums;
using Web.Infrustructure.Models;
using Web.Infrustructure.Services;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class RegisterUser
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IAfricanNationsLeagueApi? africanNationsLeagueApi { get; set; }

        public UserDto userDto = new UserDto();

        public List<Country> countries = new List<Country>();

        public List<Role> roles = new List<Role>();

        Snackbar snackbar;

        // Properties to bind to the form inputs
        public string firstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SelectedCountry { get; set; }
        public string SelectedRole { get; set; } = string.Empty;
        private bool isSubmitting = false;
        protected override async Task OnInitializedAsync()
        {
            if (africanNationsLeagueApi != null)
            {
                countries = await LoadCountryNames();

            }
        }

        public async Task<List<Country>> LoadCountryNames()
        {

            var results = await africanNationsLeagueApi.GetCountriesFlags();

            return results;

        }


        public async Task CreatUser()
        {
            isSubmitting = true;
            StateHasChanged();

            try
            {
                var selectedCountryObj = countries.FirstOrDefault(c => c.Code == SelectedCountry);

                var registerDto = new UserDto
                {
                    FullName = firstName,
                    Email = Email,
                    PasswordHash = Password,
                    Country = new Country
                    {

                        Code = selectedCountryObj?.Code ?? string.Empty,
                        Name = selectedCountryObj?.Name ?? string.Empty,
                        FlagUrl = selectedCountryObj?.FlagUrl ?? string.Empty
                    },
                    Role = (Role)(int)Enum.Parse<Role>(SelectedRole),
                    CreatedAt = DateTime.UtcNow
                };

                await africanNationsLeagueApi.RegisterUser(registerDto);

                Snackbar.Add("You have successfully registered! We are redirecting you to the Login page!", Severity.Success);
                await Task.Delay(4000);
                NavigationManager?.NavigateTo("/login");
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Registration failed: {ex.Message}", Severity.Error);
            }
            finally
            {
                isSubmitting = false;
                StateHasChanged();
                ClearFormFields();
            }
        }



        //public async Task CreatUser()
        //{

        //    var selectedCountryObj = countries.FirstOrDefault(c => c.Code == SelectedCountry);

        //    var registerDto = new UserDto
        //    {
        //        FullName = firstName,
        //        Email = Email,
        //        PasswordHash = Password,
        //        Country = new Country
        //        {
        //            Code = selectedCountryObj?.Code ?? string.Empty,
        //            Name = selectedCountryObj?.Name ?? string.Empty,
        //            FlagUrl = selectedCountryObj?.FlagUrl ?? string.Empty
        //        },
        //        Role = (Role)(int)Enum.Parse<Role>(SelectedRole),
        //        CreatedAt = DateTime.UtcNow
        //    };


        //    await africanNationsLeagueApi.RegisterUser(registerDto);

        //    Snackbar.Add("Form submitted successfully!", Severity.Success);
        //    ClearFormFields();

        //    StateHasChanged();


        //}

        private void ClearFormFields()
        {
            firstName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            SelectedCountry = string.Empty;
            SelectedRole = string.Empty;

        }

        private RegisterModel _registerModel = new();

        private void CloseModal()
        {
            NavigationManager.NavigateTo("/");
        }



        public class RegisterModel
        {
            [Required(ErrorMessage = "Full Name is required")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
                ErrorMessage = "Password must contain upper, lower, digit, and special character")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Country is required")]
            public string Country { get; set; } = string.Empty;

            [Required(ErrorMessage = "Role is required")]
            public string Role { get; set; } = string.Empty;
        }

    }
}
