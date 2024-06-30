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
        Generico generico = new Generico();
        private void Form1_Load(object sender, EventArgs e)
        {                
            cargar();
            if (!chbCategoria.Checked && !chbMarca.Checked)
            {
                btnFiltrar.Enabled = false;
            }
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            
            try
            {
                articuloList = negocio.listar();
                dgvArticulo.DataSource= articuloList;
                ocultarColumns();
                generico.CargarImg(pbArticulo,articuloList[0].imagenUrl);

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
   

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                generico.CargarImg(pbArticulo,seleccionado.imagenUrl);
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
            if(dgvArticulo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                AltaArticulo abrir = new AltaArticulo(seleccionado);
                abrir.ShowDialog();
                cargar();
            }else
            {
                MessageBox.Show("Por favor, seleccione un artículo","Editar");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if(dgvArticulo.CurrentRow != null) { 
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
            else
            {
                MessageBox.Show("Por favor, seleccione un artículo","Eliminar");
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if (dgvArticulo.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
                AltaDetalle abrir = new AltaDetalle(seleccionado);
                abrir.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un artículo","Detalle");
            }

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            bool filtroCategoriaActivo = chbCategoria.Checked && cbFiltroCategoria.SelectedItem.ToString() != null;
            bool filtroMarcaActivo = chbMarca.Checked && cbFiltroMarca.SelectedItem.ToString() != null;
            
            if( filtroCategoriaActivo && filtroMarcaActivo) {
                 listaFiltrada = articuloList.FindAll((item) => item.categoria.descripcion == cbFiltroCategoria.SelectedItem.ToString() && item.marca.descripcion == cbFiltroMarca.SelectedItem.ToString());
                 refrescarDGV(listaFiltrada);      
            }else if(filtroCategoriaActivo)
            {
                listaFiltrada = articuloList.FindAll((item) => item.categoria.descripcion == cbFiltroCategoria.SelectedItem.ToString());
                refrescarDGV(listaFiltrada);
            }else if(filtroMarcaActivo)
            {
                listaFiltrada = articuloList.FindAll((item) => item.marca.descripcion == cbFiltroMarca.SelectedItem.ToString());
                refrescarDGV(listaFiltrada);
            }
            
            
        }

        private void refrescarDGV (List<Articulo> lista)
        {
            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = lista;
            ocultarColumns();
        }
      

        private void chbCategoria_CheckedChanged(object sender, EventArgs e)
        {
            CategoriaNegocio categoria = new CategoriaNegocio();
            if(chbCategoria.Checked)
            {
                cbFiltroCategoria.DataSource = categoria.listar();
                btnFiltrar.Enabled = true;
                txtBuscar.Text = "";
                txtBuscar.Enabled = false;
            }
            else
            {
                cbFiltroCategoria.DataSource = null;
                if (!chbMarca.Checked)
                {
                    btnFiltrar.Enabled = false;
                    txtBuscar.Enabled=true;
                }
            }
        }

        private void chbMarca_CheckedChanged(object sender, EventArgs e)
        {
            MarcaNegocio marca = new MarcaNegocio();
            if (chbMarca.Checked)
            {
                cbFiltroMarca.DataSource = marca.listar();
                btnFiltrar.Enabled = true;
                txtBuscar.Text = "";
                txtBuscar.Enabled = false;
            }
            else
            {
                cbFiltroMarca.DataSource = null;
                if (!chbCategoria.Checked)
                {
                    btnFiltrar.Enabled = false;
                    txtBuscar.Enabled= true;
                }
            }
        }

        private void btnReestablecer_Click_1(object sender, EventArgs e)
        {
            cargar();
            chbCategoria.Checked = false;
            chbMarca.Checked = false;
            txtBuscar.Text = "";
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listafiltrada;

            listafiltrada = articuloList.FindAll(item => item.nombre.ToUpper().Contains(txtBuscar.Text.ToUpper()));
            refrescarDGV(listafiltrada);
        }
    }
}
