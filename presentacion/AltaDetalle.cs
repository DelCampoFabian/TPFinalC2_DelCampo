using dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class AltaDetalle : Form
    {
        private Articulo articulo;
        public AltaDetalle()
        {
            InitializeComponent();
            
        }
        public AltaDetalle(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void AltaDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                lblDetalleCodigo.Text = "Código: " + articulo.codigo;
                lblDetalleNombre.Text = "Nombre: " + articulo.nombre;
                lblDetalleDescripcion.Text = "Descripción: " + articulo.descripcion;
                lblDetalleMarca.Text = "Marca: " + articulo.marca.descripcion;
                lblDetalleCategoria.Text = "Categoria: " + articulo.categoria.descripcion;
                cargarImg(articulo.imagenUrl);
                lblDetalleImg.Text = "Url imagen: " + articulo.imagenUrl;
                lblDetallePrecio.Text = "Precio: " + articulo.precio.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void cargarImg(string imagen)
        {
            try
            {
                pbArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbArticulo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQD4qmuiXoOrmp-skck7b7JjHA8Ry4TZyPHkw&s");

            }
        }
    }
}
