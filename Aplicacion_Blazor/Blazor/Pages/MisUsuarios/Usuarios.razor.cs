using Blazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Modelos;

namespace Blazor.Pages.MisUsuarios
{
    //"partial" para proyectos grandes (en varios achivos).
    partial class Usuarios
    {
        [Inject] private IUsuarioServicio _usuarioServicio { get; set; }
        private IEnumerable<Usuario> listaUsuarios { get; set; }
        protected override async Task OnInitializedAsync()
        {
            listaUsuarios = await _usuarioServicio.GetLista();
        }
    }
}
