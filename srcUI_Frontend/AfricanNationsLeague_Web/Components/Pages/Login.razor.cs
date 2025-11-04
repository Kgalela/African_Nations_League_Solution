using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using Web.Infrustructure.Enums;
using Web.Infrustructure.Models;
using Web.Infrustructure.Services;

namespace AfricanNationsLeague_Web.Components.Pages
{
    public partial class Login
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IAfricanNationsLeagueApi? africanNationsLeagueApi { get; set; }

        public UserDto userDto = new UserDto();

        public List<Role> roles = new List<Role>();

        private LoginModel _loginModel = new();
        public string SelectedRole { get; set; } = string.Empty;

        public bool isSubmitting = false;
        Snackbar snackbar;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        private void CloseModal()
        {
            NavigationManager.NavigateTo("/");
        }



        public class LoginModel
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; } = string.Empty;
            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; } = string.Empty;
        }


        public async Task LoginUser()
        {
            isSubmitting = true;
            StateHasChanged();

            try
            {


                var loginDetailsDto = new LoginDto
                {
                    Email = Email,
                    Password = Password
                };


                var results = await africanNationsLeagueApi.LoginUser(loginDetailsDto);

                Snackbar.Add("Welcome back!", Severity.Success);
                await Task.Delay(4000);
                if (results.Role == Role.Representative)
                {
                    NavigationManager.NavigateTo($"/register-team/{results.Email}");

                }
                else
                {
                    NavigationManager.NavigateTo("/");
                }


            }
            catch (Exception ex)
            {
                Snackbar.Add($"Email or password is invald! Please try again: {ex.Message}", Severity.Error);
                NavigationManager?.NavigateTo("/login");
            }
            finally
            {
                isSubmitting = false;
                StateHasChanged();
                ClearFormFields();
            }
        }

        public void NavigateToRegister()
        {
            NavigationManager.NavigateTo("/register");
        }

        private void ClearFormFields()
        {

            Email = string.Empty;
            Password = string.Empty;

        }
    }
}
