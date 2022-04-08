using System.Collections;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace MiClase
{
    public partial class Form1 : Form
    {
        List <Persona> Personas= new List<Persona>();

        public object XmlSerializer { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(File.Exists("Persona.xml"))
            {
                XmlSerializer serializador = new XmlSerializer(typeof(List<Persona>));
                FileStream lector = File.OpenRead("Personas.xml");
                Personas = (List<Persona>)serializador.Deserialize(lector);
                lector.Close();
            }

            dgvDatos.DataSource = Personas;
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            if (!ValidarID()) return;

            if (Existe(txtID.Text))
            {
                errorProvider1.SetError(txtID, "Este ID ya existe");
                txtID.Focus();
                return;
            }
            errorProvider1.SetError(txtID, "");

            if (!ValidarOtrosCampos()) return;           

            Persona miPersona = new Persona();

            miPersona.ID = txtID.Text;
            miPersona.Nombres = txtNombre.Text;
            miPersona.Apellidos = txtApellido.Text;
            miPersona.Correo = txtCorreo.Text;
            miPersona.FechaNacimiento = dtpFecha.Value;
            miPersona.Salario = Convert.ToDecimal(txtSalario.Text);
            Personas.Add(miPersona);

            dgvDatos.DataSource = null;
            dgvDatos.DataSource = Personas;

            txtID.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtSalario.Clear();
            txtID.Focus();
        }

        private bool ValidarOtrosCampos()
        {
            if (txtNombre.Text == "")
            {
                errorProvider1.SetError(txtNombre, "Debe ingresar nombre(s) de la persona");
                txtNombre.Focus();
                return false;
            }
            errorProvider1.SetError(txtNombre, "");

            if (txtApellido.Text == "")
            {
                errorProvider1.SetError(txtApellido, "Debe ingresar apellido(s) de la persona");
                txtApellido.Focus();
                return false;
            }
            errorProvider1.SetError(txtApellido, "");

            Regex regEmail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*", RegexOptions.Compiled);

            if (!regEmail.IsMatch(txtCorreo.Text))
            {
                errorProvider1.SetError(txtCorreo, "Debe ingresar una direccion de correo valida");
                txtCorreo.Focus();
                return false;
            }
            errorProvider1.SetError(txtCorreo, "");

            decimal Salario;

            if (!Decimal.TryParse(txtSalario.Text, out Salario))
            {
                errorProvider1.SetError(txtSalario, "Debe ingresar numeros en el campo salario");
                txtSalario.Focus();
                return false;
            }

            if (Salario < 0)
            {
                errorProvider1.SetError(txtSalario, "Debe ingresar un numeros positivo");
                txtSalario.Focus();
                return false;
            }
            return true;
        }

        private bool Existe(string  ID)
        {
            foreach(Persona Persona in Personas)
            {
                if(Persona.ID == ID)
                return true;
            }
            return false;
        }

        private void tsbBuscar_Click(object sender, EventArgs e)
        {
            if (!ValidarID()) return;

            Persona miPersona = GetPersona(txtID.Text);
            
            if(miPersona == null)
            {
                errorProvider1.SetError(txtID, "Esta persona no existe!");
                txtID.Focus();
                return;
            }
            errorProvider1.SetError(txtID, "");

            txtNombre.Text = miPersona.Nombres;
            txtApellido.Text = miPersona.Apellidos;
            txtCorreo.Text = miPersona.Correo;
            dtpFecha.Value = miPersona.FechaNacimiento;
            txtSalario.Text = miPersona.Salario.ToString();
        }
        
        private bool ValidarID()
        {
            if (txtID.Text == "")
            {
                errorProvider1.SetError(txtID, "Debe ingresar un ID de la persona");
                txtID.Focus();
                return false;
            }
            errorProvider1.SetError(txtID, "");
            return true;
        }

        private Persona GetPersona(string ID)
        {
            foreach(Persona miPersona in Personas)
            {
                if (miPersona.ID == ID) return miPersona;
            }
            return null;
        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (!ValidarID()) return;

            if (GetPersona(txtID.Text) == null)
            {
                errorProvider1.SetError(txtID, "Persona no existe");
                txtID.Focus();
                return;
            }
            errorProvider1.SetError(txtID, "");

            if (!ValidarOtrosCampos()) return;
            
            foreach(Persona miPersona in Personas)
            {
                if(miPersona.ID == txtID.Text)
                {
                    miPersona.ID = txtID.Text;
                    miPersona.Nombres = txtNombre.Text;
                    miPersona.Apellidos = txtApellido.Text;
                    miPersona.Correo = txtCorreo.Text;
                    miPersona.FechaNacimiento = dtpFecha.Value;
                    miPersona.Salario = Convert.ToDecimal(txtSalario.Text);
                    break;
                }
            }

            dgvDatos.DataSource = null;
            dgvDatos.DataSource = Personas;
        }

        private void tsbLimpiar_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtSalario.Clear();
        }

        private void tsbBorrar_Click(object sender, EventArgs e)
        {
            DialogResult rta = MessageBox.Show(this, "¿Esta seguro de borrar el registro?", 
                "Confimacion",MessageBoxButtons.YesNo, MessageBoxIcon.Question, 
                MessageBoxDefaultButton.Button2);

            if (rta == DialogResult.No) return;
            
            if (!ValidarID()) return;

            if (GetPersona(txtID.Text) == null)
            {
                errorProvider1.SetError(txtID, "Persona no existe");
                txtID.Focus();
                return;
            }
            errorProvider1.SetError(txtID, "");

            foreach(Persona miPersona in Personas)
            {
                if(miPersona.ID == txtID.Text)
                {
                    Personas.Remove(miPersona);
                    break;
                }
            }

           tsbLimpiar_Click(sender, e);
           dgvDatos.DataSource = null;
           dgvDatos.DataSource = Personas;
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlSerializer serializador = new XmlSerializer(typeof(List<Persona>));
            TextWriter escritor = new StreamWriter("Personas.xml");
            serializador.Serialize(escritor, Personas);
        }
    }
}