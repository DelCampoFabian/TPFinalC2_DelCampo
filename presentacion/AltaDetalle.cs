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
        Generico generico= new Generico();
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
                generico.CargarImg(pbArticulo, articulo.imagenUrl);
                lblDetalleImg.Text = "Url imagen: " + articulo.imagenUrl;
                lblDetallePrecio.Text = "Precio: " + articulo.precio.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

       
    }
}
