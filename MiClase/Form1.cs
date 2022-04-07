using System.Collections;
using System.Text.RegularExpressions;


namespace MiClase
{
    public partial class Form1 : Form
    {
        ArrayList Personas = new ArrayList();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Persona Persona1 = new Persona();
            Persona1.ID = "1010";
            Persona1.Nombres = "Lady";
            Persona1.Apellidos = "Gaga";
            Persona1.Correo = "ladygaga@gmail.com";
            Persona1.FechaNacimineto = new DateTime (1990, 3, 14);
            Persona1.Salario = 1200000;
            Personas.Add(Persona1);

            Persona Persona2 = new Persona();
            Persona2.ID = "2020";
            Persona2.Nombres = "Daddy";
            Persona2.Apellidos = "Yankee";
            Persona2.Correo = "daddy@gmail.com";
            Persona2.FechaNacimineto = new DateTime(1987, 7, 20);
            Persona2.Salario = 2000000;
            Personas.Add(Persona2);

            dgvDatos.DataSource = Personas;
        }

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                errorProvider1.SetError(txtID, "Debe ingresar un ID de la persona");
                txtID.Focus();
                return;
            }
            errorProvider1.SetError(txtID, "");
            
            if (Existe(txtID.Text))
            {
                errorProvider1.SetError(txtID, "Este ID ya existe");
                txtID.Focus();
                return;
            }
            errorProvider1.SetError(txtID, "");
            
            if (txtNombre.Text == "")
            {
                errorProvider1.SetError(txtNombre, "Debe ingresar nombre(s) de la persona");
                txtNombre.Focus();
                return;
            }
            errorProvider1.SetError(txtNombre, "");            

            if (txtApellido.Text == "")
            {
                errorProvider1.SetError(txtApellido, "Debe ingresar apellido(s) de la persona");
                txtApellido.Focus();
                return;
            }
            errorProvider1.SetError(txtApellido, "");

            Regex regEmail = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*",RegexOptions.Compiled);

            if (!regEmail.IsMatch(txtCorreo.Text))
            {
                errorProvider1.SetError(txtCorreo, "Debe ingresar una direccion de correo valida");
                txtCorreo.Focus();
                return;
            }
            errorProvider1.SetError(txtCorreo, "");

            decimal Salario;
            
            if(!Decimal.TryParse(txtSalario.Text, out Salario))
            {
                errorProvider1.SetError(txtSalario, "Debe ingresar numeros en el campo salario");
                txtSalario.Focus();
                return;
            }

            if (Salario < 0)
            {
                errorProvider1.SetError(txtSalario, "Debe ingresar un numeros positivo");
                txtSalario.Focus();
                return;
            }

            Persona miPersona = new Persona();
            miPersona.ID = txtID.Text;
            miPersona.Nombres = txtNombre.Text;
            miPersona.Apellidos = txtApellido.Text;
            miPersona.Correo = txtCorreo.Text;
            miPersona.FechaNacimineto = dtpFecha.Value;
            miPersona.Salario = Salario;
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

        private bool Existe(string  ID)
        {
            foreach(Persona Persona in Personas)
            {
                if(Persona.ID == ID)
                return true;
            }
            return false;
        }
    }
}