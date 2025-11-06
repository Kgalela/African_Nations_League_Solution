using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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

        private bool _showPassword = false;


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
                    Email = _loginModel.Email,
                    Password = _loginModel.Password
                };

                var results = await africanNationsLeagueApi.LoginUser(loginDetailsDto);

                if (results == null)
                {
                    loginFailed = true;
                    isSubmitting = false;
                    StateHasChanged();
                    return;


                }

                var claims = new List<System.Security.Claims.Claim>
                {
                  new Claim(ClaimTypes.Name, results.FullName),
                    new Claim(ClaimTypes.Email, results.Email),
                    new Claim(ClaimTypes.Role, results.Role.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);


                await Task.Delay(2000);

                if (results.Role == Role.Representative)
                {
                    NavigationManager?.NavigateTo($"/register-team/{results.Email}");
                    Snackbar.Add("Welcome back!", Severity.Success);
                }
                else if (results.Role == Role.Admin)
                {
                    NavigationManager?.NavigateTo($"/admin/{results.Email}");
                    Snackbar.Add("Welcome back!", Severity.Success);
                }
                else
                {
                    Snackbar.Add("You not have access", Severity.Error);
                    NavigationManager?.NavigateTo("/access-denied");
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Login failed invalid details", Severity.Error);
                //NavigationManager?.NavigateTo("/login");
            }

            isSubmitting = false;
            StateHasChanged();
            ClearFormFields();

        }

        private bool loginFailed = false;



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
