using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;

namespace presentacion
{
    public partial class AltaArticulo : Form
    {
        Articulo articulo = null;
        public AltaArticulo()
        {
            InitializeComponent();
        }

        public AltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar articulo";
        }

        private void AltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marca = new MarcaNegocio();
            CategoriaNegocio categoria = new CategoriaNegocio();

            try
            {
                cbMarca.DataSource = marca.listar();
                cbMarca.ValueMember = "id";
                cbMarca.DisplayMember = "descripcion";
                cbCategoria.DataSource = categoria.listar();
                cbCategoria.ValueMember = "id";
                cbCategoria.DisplayMember = "descripcion";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.codigo;
                    txtNombre.Text = articulo.nombre;
                    txtDescripcion.Text = articulo.descripcion;
                    txtImg.Text = articulo.imagenUrl;
                    cargarImg(articulo.imagenUrl);
                    txtPrecio.Text = articulo.precio.ToString();
                    cbMarca.SelectedValue = articulo.marca.id;
                    cbCategoria.SelectedValue = articulo.categoria.id;
                }
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
                pbAlta.Load(imagen);
            }
            catch (Exception)
            {
                pbAlta.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQD4qmuiXoOrmp-skck7b7JjHA8Ry4TZyPHkw&s");

            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if(articulo == null)
                    articulo = new Articulo();

                articulo.codigo = txtCodigo.Text;
                articulo.nombre = txtNombre.Text;
                articulo.descripcion = txtDescripcion.Text;
                articulo.imagenUrl = txtImg.Text;
                articulo.precio = decimal.Parse(txtPrecio.Text);
                articulo.categoria = (Categoria)cbCategoria.SelectedItem;
                articulo.marca= (Marca)cbMarca.SelectedItem;


                if(articulo.id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Articulo editado");
                    Close();

                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado");
                    Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImg_Leave(object sender, EventArgs e)
        {
            cargarImg(txtImg.Text);
        }
    }
}
