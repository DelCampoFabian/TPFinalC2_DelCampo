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
using System.IO;
using System.Configuration;

namespace presentacion
{
    public partial class AltaArticulo : Form
    {
        Articulo articulo = null;
        OpenFileDialog archivo = null; 
        public AltaArticulo()
        {
            InitializeComponent();
        }

        public AltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar articulo";
            lblTitulo.Text = "Editar artículo";
        }

        private void AltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marca = new MarcaNegocio();
            CategoriaNegocio categoria = new CategoriaNegocio();
            ttPrecio.SetToolTip(txtPrecio, "Ingrese solo números");
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
                pbArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbArticulo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQD4qmuiXoOrmp-skck7b7JjHA8Ry4TZyPHkw&s");

            }
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private bool validadCampos()
        {
            if(string.IsNullOrEmpty(txtCodigo.Text) && string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                lblRequerido.Visible = true;
                lblRequerido2.Visible = true;
                return true;
            }
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                lblRequerido2.Visible = true;
                return true;
            }

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                lblRequerido.Visible = true;
                return true;
            }
            

            return false;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if(articulo == null)
                    articulo = new Articulo();

                if (validadCampos())
                    return;
               
                articulo.codigo = txtCodigo.Text;
                articulo.nombre = txtNombre.Text;
                articulo.descripcion = txtDescripcion.Text;
                articulo.imagenUrl = txtImg.Text;
                if (txtPrecio.Text == null || txtPrecio.Text == "") articulo.precio = 0;
                else articulo.precio = decimal.Parse(txtPrecio.Text);
                articulo.categoria = (Categoria)cbCategoria.SelectedItem;
                articulo.marca= (Marca)cbMarca.SelectedItem;

                if(articulo.id != 0) { 
                    negocio.modificar(articulo);
                    MessageBox.Show("Articulo editado");
                }
                else { 
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado");
                }
                if (archivo != null && !(txtImg.Text.ToUpper().Contains("HTTP"))){
                    DateTime fecha = DateTime.Now;
                    string extenderNombre = fecha.ToString("yyyyMMdd_HHmmss");
                    string nombreArchivo = extenderNombre + archivo.SafeFileName;
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Articulos-Img"] + nombreArchivo);

                }
                Close();
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

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar < 48 || e.KeyChar > 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
           
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImg.Text = archivo.FileName;
                cargarImg(txtImg.Text);

                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Articulos-Img"] + archivo.SafeFileName);
            } 
        }
    }
}
