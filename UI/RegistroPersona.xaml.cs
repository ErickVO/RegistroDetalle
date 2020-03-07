using RegistroConDetalle.BLL;
using RegistroConDetalle.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegistroConDetalle.UI
{
    /// <summary>
    /// Interaction logic for RegistroPersona.xaml
    /// </summary>
    public partial class RegistroPersona : Window
    {
        public List<TelefonosDetalle> Detalle { get; set; }

        public RegistroPersona()
        {
            InitializeComponent();
            this.Detalle = new List<TelefonosDetalle>();
        }

        private void Limpiar()
        {
            IdTextBox.Text = "0";
            NombreTextBox.Text = string.Empty;
            CedulaTextBox.Text = string.Empty;
            DireccionTextBox.Text = string.Empty;
            FechaDatePicker.Text = string.Empty;

            this.Detalle = new List<TelefonosDetalle>();
            CargarGrid();
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private Personas LlenaClase()
        {
            Personas persona = new Personas();
            persona.PersonaId = Convert.ToInt32(IdTextBox.Text);
            persona.Nombre = NombreTextBox.Text;
            persona.Cedula = CedulaTextBox.Text;
            persona.Direccion = DireccionTextBox.Text;
            persona.FechaNacimiento = FechaDatePicker.DisplayDate;
            persona.Telefonos = this.Detalle;

            return persona;
            
        }
        
        private void LlenaCampos(Personas persona)
        {
            IdTextBox.Text = Convert.ToString(persona.PersonaId);
            NombreTextBox.Text = persona.Nombre;
            CedulaTextBox.Text = persona.Cedula;
            DireccionTextBox.Text = persona.Direccion;
            FechaDatePicker.SelectedDate = persona.FechaNacimiento;

            this.Detalle = persona.Telefonos;
            CargarGrid();
        }
        
        private bool Validar()
        {
            bool paso = true;

            if (NombreTextBox.Text == string.Empty)
            {
                MessageBox.Show("El campo no puede estar vacio","Error");
                NombreTextBox.Focus();
                paso = false;
            }

            if (string.IsNullOrWhiteSpace(DireccionTextBox.Text))
            {
                MessageBox.Show("El campo Direccion no puede estar vacio","Error");
                DireccionTextBox.Focus();
                paso = false;
            }

            if (string.IsNullOrWhiteSpace(CedulaTextBox.Text.Replace("-", "")))
            {
                MessageBox.Show("El campo Cedula no puede estar vacio","Error");
                CedulaTextBox.Focus();
                paso = false;

            }


            if (string.IsNullOrEmpty(FechaDatePicker.Text))
            {
                MessageBox.Show("El campo Fecha no puede estar vacio", "Error");
                FechaDatePicker.Focus();
                paso = false;
            }

            if (this.Detalle.Count == 0) {
                MessageBox.Show("Esta campo no puede estar vacio", "Error");
                DetalleDataGrid.Focus();
                paso = false;
            }

            return paso;
        }

        private bool ExisteEnLaBaseDeDatos()
        {
            Personas p = PersonasBll.Buscar((int)Convert.ToInt32(IdTextBox.Text));

            return (p != null);
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            Personas persona;
            bool paso = false;

            if (!Validar())
                return;

            persona = LlenaClase();

            if ((Convert.ToInt32(IdTextBox.Text)) == 0)
            {
                paso = PersonasBll.Guardar(persona);
            }
            else
            {
                if (!ExisteEnLaBaseDeDatos())
                {
                    MessageBox.Show("No se puede Modificar la persona que no existe", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                paso = PersonasBll.Modificar(persona);
            }

            if (paso)
            {
                Limpiar();
                MessageBox.Show("Guardado!!", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No fue posible guardar!!", "Fallo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(IdTextBox.Text, out id);

            Limpiar();

            if (PersonasBll.Eliminar(id))
                MessageBox.Show("Eliminado", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(IdTextBox.Text, "No se puede eliminar una persona que no existe");

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(DetalleDataGrid.DataContext != null)
            {
                this.Detalle = (List<TelefonosDetalle>)DetalleDataGrid.ItemsSource;
            }

            this.Detalle.Add(
                new TelefonosDetalle
                {
                    Id = 0,
                    PersonaId = Convert.ToInt32(IdTextBox.Text),
                    Telefono = TelefonoTextBox.Text,
                    TipoTelefono = TipoTextBox.Text
                }
                ) ;
            CargarGrid();
            TelefonoTextBox.Focus();
            TelefonoTextBox.Clear();
            TipoTextBox.Clear();
        }

        private void RemoverButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetalleDataGrid.Items.Count > 1 && DetalleDataGrid.SelectedIndex < DetalleDataGrid.Items.Count - 1)
            {
                Detalle.RemoveAt(DetalleDataGrid.SelectedIndex);

                CargarGrid();
            }
        }

       

        private void CargarGrid()
        {
            DetalleDataGrid.ItemsSource = null;
            DetalleDataGrid.ItemsSource = Detalle;
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(IdTextBox.Text, out id);
            Personas p = PersonasBll.Buscar(id);

            if (p != null)
            {
                Limpiar();
                LlenaCampos(p);
            }
            else
            {
                MessageBox.Show("No Encontrado", "ERROR");
            }
        }
    }
}
