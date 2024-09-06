using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaControlador;

namespace CapaVista
{
    public partial class Consulta : Form
    {
        String tabla = "empleados";
        Controlador cn = new Controlador();
        public Consulta()
        {
            InitializeComponent();
        }

        public void actualizardatagridview()
        {
            // Mostrar un mensaje de confirmación antes de proceder con la carga de datos
            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea cargar los datos?",
                                                        "Confirmar carga de datos",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);

            // Si el usuario confirma la carga
            if (confirmacion == DialogResult.Yes)
            {

                try
                {
                    // Llamar al controlador para obtener los datos desde la base de datos
                    DataTable dt = cn.llenarTbl(tabla);
                    // Verificar si la tabla tiene filas antes de cargar los datos
                    if (dt.Rows.Count > 0)
                    {
                        // Asignar los datos al DataGridView
                        Dgv_consulta.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron registros en la tabla.", "Consulta vacía",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Mostrar un mensaje de error si algo sale mal
                    MessageBox.Show("Error al consultar los registros: " + ex.Message, "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void BtnConsulta_Click(object sender, EventArgs e)
        {
            //Llena los datos de la tabla en el datagridview
            actualizardatagridview();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_ingresar_Click(object sender, EventArgs e)
        {
            string codigotext = txt_codigo.Text;
            int codigo = Convert.ToInt32(codigotext);
            string nombre = txt_nombre.Text;
            string puesto = txt_puesto.Text;
            string departamento = txt_departamento.Text;
            string estadotexto = txt_estado.Text;
            int estado = Convert.ToInt32(estadotexto);

            try
            {
                cn.saveEmpleado(codigo, nombre, puesto, departamento, estado);
                MessageBox.Show("Registro Agregado correctamente :)");
            }
            catch
            {
                MessageBox.Show("Registro No ingresado");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("¿Esta seguro que desea eliminar este registro?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Controlador ctriv = new Controlador();
                if (Dgv_consulta.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = Dgv_consulta.SelectedRows[0];

                    if (selectedRow.Cells[0].Value != null)
                    {
                        int llave = Convert.ToInt32(selectedRow.Cells[0].Value);
                        ctriv.eliminar(llave);
                        MessageBox.Show("Eliminado Exitosamente");
                    }
                }
                else
                {
                    MessageBox.Show("No hay filas seleccionadas en el DataGridView.");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Llama al metodo de actualizar datagridview
            actualizardatagridview();
        }

        private void Dgv_consulta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_codigo.Text = Dgv_consulta.CurrentRow.Cells[0].Value.ToString();
            txt_nombre.Text = Dgv_consulta.CurrentRow.Cells[1].Value.ToString();
            txt_puesto.Text = Dgv_consulta.CurrentRow.Cells[2].Value.ToString();
            txt_departamento.Text = Dgv_consulta.CurrentRow.Cells[3].Value.ToString();
            txt_estado.Text = Dgv_consulta.CurrentRow.Cells[4].Value.ToString();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_codigo.Text) &&
                    !string.IsNullOrWhiteSpace(txt_nombre.Text) &&
                    !string.IsNullOrWhiteSpace(txt_puesto.Text) &&
                    !string.IsNullOrWhiteSpace(txt_departamento.Text) &&
                    !string.IsNullOrWhiteSpace(txt_estado.Text))
                {
                    DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea modificar este registro?",
                                                                "Confirmar modificación",
                                                                MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Question);

                    if (confirmacion == DialogResult.Yes)
                    {
                        Controlador controlador = new Controlador();

                        int id = Convert.ToInt32(txt_codigo.Text);
                        string nombre = txt_nombre.Text;
                        string puesto = txt_puesto.Text;
                        string departamento = txt_departamento.Text;
                        int estado = Convert.ToInt32(txt_estado.Text);

                        controlador.modificar(id, nombre, puesto, departamento, estado);

                        MessageBox.Show("Registro modificado correctamente.", "Modificación exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        actualizardatagridview();
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, complete todos los campos antes de modificar.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el registro: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
