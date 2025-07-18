// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using spitifi.Data;
using spitifi.Models.DbModels;
using spitifi.Services.Email;

namespace spitifi.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ICustomMailer _mailer;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            ICustomMailer mailer,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            //Importar e usar a interface definida para o envio do Email de confirmação
            _mailer = mailer;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{1,4}$")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "A {0} tem de ter no mínimo {2} e no máximo {1} caracteres.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Palavra-Passe")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "As Palavras-Passe não conrrespondem.")]
            public string ConfirmPassword { get; set; }

            public Utilizadores Utilizador { get; set; }

            public IFormFile Logo { get; set; }

            public Boolean eArtista { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //Foi necessário fazer um asp-for para conseguir trazer o IFormFile do Form sem vir a null, mas como há utilizadores que podem preferir não meter foto
            //Tirámos o modelstate do Logo da validação e iremos meter uma default caso o utilizador opte por não escolher nenhuma foto
            ModelState.Remove("Logo");
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //Ir buscar o resultado da query à Identity para o nome e email submetidos pelo utilizador
                var utilizadorJaExiste = await _userManager.FindByEmailAsync(Input.Email);
                var nomeJaExiste = await _userManager.FindByNameAsync(Input.Utilizador.Username);

                //Verificar se o nome ou o email (que devem ser únicos) já existem na Identity
                if (utilizadorJaExiste != null)
                {
                    ModelState.AddModelError("Input.Email", "Email já existente");
                }
                else if (nomeJaExiste != null)
                {
                    ModelState.AddModelError("Input.Utilizador.Username", "Nome já existente");
                }
                else
                {
                    var user = CreateUser();

                    await _userStore.SetUserNameAsync(user, Input.Utilizador.Username, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        bool haImagem = false;
                        string nomeImagem = "";
                        string fotoUser = "";
                        var fotoUserFile = Input.Logo;

                        _logger.LogInformation("Utilizador criou uma conta.");

                        if (fotoUserFile == null)
                        {
                            fotoUser = "assets/icons/user.png";
                        }
                        else
                        {
                            //Criar aqui validação FOTO
                            if (!(fotoUserFile.ContentType == "image/png" || fotoUserFile.ContentType == "image/jpeg"))
                            {
                                ModelState.AddModelError("",
                                    "Formato Inválido. Insira uma foto com formato JPEG ou PNG");
                            }
                            else
                            {
                                haImagem = true;
                                // gerar nome imagem
                                Guid g = Guid.NewGuid();
                                // atrás do nome adicionamos a pasta onde a escrevemos
                                nomeImagem = g.ToString();
                                string extensaoImagem = Path.GetExtension(fotoUserFile.FileName).ToLowerInvariant();
                                nomeImagem += extensaoImagem;
                                // guardar o nome do ficheiro na BD
                                fotoUser = "imagens/utilizadores/" + nomeImagem;
                            }

                            // se existe uma imagem para escrever no disco
                            if (haImagem)
                            {
                                // vai construir o path para o diretório onde são guardadas as imagens
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                    @"wwwroot/imagens/utilizadores");

                                // antes de escrevermos o ficheiro, vemos se o diretório existe
                                if (!Directory.Exists(filePath))
                                    Directory.CreateDirectory(filePath);

                                // atualizamos o Path para incluir o nome da imagem
                                filePath = Path.Combine(filePath, nomeImagem);

                                // escreve a imagem
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await fotoUserFile.CopyToAsync(fileStream);
                                }
                            }
                        }

                        var utilizador = new Utilizadores
                        {
                            Username = Input.Utilizador.Username,
                            IdentityUser = user.Id,
                            IsArtista = Input.Utilizador.IsArtista,
                            Foto = fotoUser
                        };
                        _context.Add(utilizador);
                        _context.SaveChanges();

                        if (Input.Utilizador.IsArtista)
                        {
                            _userManager.AddToRoleAsync(user, "Artista").Wait();
                        }

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        _mailer.SendEmail(Input.Email, "Email de Confirmação",
                            $"Por favor clique <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aqui</a>. para confirmar o seu email");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation",
                                new { email = Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // If we got this far, something failed, redisplay form
                    return Page();
                }
            }

            return null;
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                                                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}