using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace presentacion
{
    public partial class Form1 : Form
    {
        private List<Articulo> articuloList = new List<Articulo>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cbFiltro.Items.Add("Categoria");
            cbFiltro.Items.Add("Marca");
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            try
            {
                articuloList = negocio.listar();
                dgvArticulo.DataSource= articuloList;
                ocultarColumns();
                cargarImg(articuloList[0].imagenUrl);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); ;
            }

        }

        private void ocultarColumns()
        {
            dgvArticulo.Columns["ImagenUrl"].Visible = false;
            dgvArticulo.Columns["id"].Visible = false;

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

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                cargarImg(seleccionado.imagenUrl);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulo abrir = new AltaArticulo();
            abrir.ShowDialog();
            cargar();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            AltaArticulo abrir = new AltaArticulo(seleccionado);
            abrir.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado= (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Desea eliminar este articulo?", "Eliminar", MessageBoxButtons.YesNo,MessageBoxIcon.Warning );
                if(respuesta == DialogResult.Yes )
                {
                    negocio.eliminar(seleccionado.id);
                    cargar();

                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            AltaDetalle abrir = new AltaDetalle(seleccionado);
            abrir.ShowDialog();
            
        }

        private void cbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            MarcaNegocio marca = new MarcaNegocio();
            CategoriaNegocio categoria = new CategoriaNegocio();
            string opcion = cbFiltro.SelectedItem.ToString();   
            
            try
            {
                if(opcion == "Categoria")
                {
                    cbCriterio.DataSource = categoria.listar();
                    cargar();
                }else if(opcion == "Marca")
                {
                    cbCriterio.DataSource = marca.listar();
                    cargar();
                }

            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            if (cbFiltro.SelectedItem.ToString() == "Categoria")
            {
               listaFiltrada = articuloList.FindAll((item) => item.categoria.descripcion == cbCriterio.SelectedItem.ToString());
               dgvArticulo.DataSource = null;
               dgvArticulo.DataSource = listaFiltrada;
                ocultarColumns();

            }
            else if (cbFiltro.SelectedItem.ToString() == "Marca")
            {
                listaFiltrada = articuloList.FindAll((item) => item.marca.descripcion == cbCriterio.SelectedItem.ToString());
                dgvArticulo.DataSource = null;
                dgvArticulo.DataSource = listaFiltrada;
                ocultarColumns();
            }
        }
    }
}
