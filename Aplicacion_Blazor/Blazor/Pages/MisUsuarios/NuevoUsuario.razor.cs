using Blazor.Interfaces;
using Microsoft.AspNetCore.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Modelos;

namespace Blazor.Pages.MisUsuarios
{
    partial class NuevoUsuario
    {
        [Inject] private IUsuarioServicio _usuarioServicio { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }

        private Usuario user = new Usuario();

        protected void Cancelar()
        {
            _navigationManager.NavigateTo("/Usuarios");
        }

        protected async void Guardar()
        {
            if (string.IsNullOrEmpty(user.Codigo) || string.IsNullOrEmpty(user.Nombre) || string.IsNullOrEmpty(user.Clave) || string.IsNullOrEmpty(user.Rol) || user.Rol=="Seleccionar")
            {
                await Swal.FireAsync("Atención!", "Debe ingresar todos los campos requeridos", SweetAlertIcon.Info);
                return;
            }

            bool insert = await _usuarioServicio.Nuevo(user);
            if (insert)
            {
                await Swal.FireAsync("Felicidades!", "Usuario(a) guardado con éxito", SweetAlertIcon.Success);
            }
            else
            {
                await Swal.FireAsync("Error!!!", "No se pudo guardar el usuario(a)", SweetAlertIcon.Error);
            }
            _navigationManager.NavigateTo("/Usuarios");
        }
    }
}

//El método siguiente hace la enumeración de las opciones que debe tener un "ComboBox".
enum Roles
{
    Seleccionar,
    Administrador,
    Usuario
}
