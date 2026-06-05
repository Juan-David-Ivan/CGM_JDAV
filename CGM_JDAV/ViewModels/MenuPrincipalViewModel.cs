using System;
using System.Threading.Tasks;
using Avalonia.Collections;
using CGM_JDAV.Models;
using CGM_JDAV.Services;
using CGM_JDAV.Views.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;

namespace CGM_JDAV.ViewModels;

public partial class MenuPrincipalViewModel : ViewModelBase
{
    
    [ObservableProperty] private int totalEspecialidades = 0;
    [ObservableProperty] private int totalPersonal = 0;
    [ObservableProperty] private int totalPacientes = 0;
    [ObservableProperty] private int totalCitasHoy = 0;
    
    [ObservableProperty] private bool editando = false;
    
    [ObservableProperty] private string mensajeErrorDialog = "";
    [ObservableProperty] private bool hayErrorDialog = false;

    [ObservableProperty] private string tituloSeccion = "Especialidades";
    [ObservableProperty] private string textoBotonNuevo = "Nueva especialidad";
    
    [ObservableProperty] private TimeSpan? horaInicioSel;
    [ObservableProperty] private TimeSpan? horaFinSel;
    
    [ObservableProperty] private bool diaLunes = false;
    [ObservableProperty] private bool diaMartes = false;
    [ObservableProperty] private bool diaMiercoles = false;
    [ObservableProperty] private bool diaJueves = false;
    [ObservableProperty] private bool diaViernes = false;
    [ObservableProperty] private bool diaSabado = false;
    [ObservableProperty] private bool diaDomingo = false;

    
    [ObservableProperty] private bool esEspecialidades = true;
    [ObservableProperty] private bool esPersonal = false;
    [ObservableProperty] private bool esPacientes = false;
    [ObservableProperty] private bool esCitas = false;
    
    [ObservableProperty] private string horarioEspecialidadCita = "";
    
    [ObservableProperty] private AvaloniaList<string> tiposPersonal = new() { "Medico", "Enfermero" };
    [ObservableProperty] private EspecialidadModel especialidadDelDoctor = new();
    
    [ObservableProperty] private DateTimeOffset? fechaNacimientoSel;
    
    [ObservableProperty] private PacienteModel pacienteDeLaCita = new();
    [ObservableProperty] private EspecialidadModel especialidadDeLaCita = new();
    [ObservableProperty] private DoctorModel doctorDeLaCita = new();
    [ObservableProperty] private DateTimeOffset? fechaCitaSel;
    [ObservableProperty] private TimeSpan? horaCitaSel;

    [ObservableProperty] private AvaloniaList<EspecialidadModel> listaEspecialidades = new();
    [ObservableProperty] private AvaloniaList<DoctorModel> listaPersonal = new();
    [ObservableProperty] private AvaloniaList<PacienteModel> listaPacientes = new();
    [ObservableProperty] private AvaloniaList<CitaModel> listaCitas = new();

    [ObservableProperty] private EspecialidadModel especialidadSeleccionada = new();
    [ObservableProperty] private DoctorModel doctorSeleccionado = new();
    [ObservableProperty] private PacienteModel pacienteSeleccionado = new();
    [ObservableProperty] private CitaModel citaSeleccionada = new();

    // --- SERVICIOS ---
    
    private readonly NavigationService _navigationService;
    
    private SupabaseService supabaseService = new();
    

    public MenuPrincipalViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        CargarEspecialidadesCommand.Execute(null);
        CargarPersonalCommand.Execute(null);
        CargarPacientesCommand.Execute(null);
        CargarCitasCommand.Execute(null);
    }
    
    [RelayCommand]
    public void CerrarSesion()
    {
        _navigationService.NavigateTo(NavigationService.LOGIN_VIEW);
    }
    
    [RelayCommand]
    public void IrAEspecialidades()
    {
        TituloSeccion = "Especialidades";
        TextoBotonNuevo = "Nueva especialidad";
        EsEspecialidades = true;
        EsPersonal = false;
        EsPacientes = false;
        EsCitas = false;
        CargarEspecialidadesCommand.Execute(null);
    }

    [RelayCommand]
    public void IrAPersonal()
    {
        TituloSeccion = "Personal médico";
        TextoBotonNuevo = "Nuevo profesional";
        EsEspecialidades = false;
        EsPersonal = true;
        EsPacientes = false;
        EsCitas = false;
        CargarPersonalCommand.Execute(null);
    }

    [RelayCommand]
    public void IrAPacientes()
    {
        TituloSeccion = "Pacientes";
        TextoBotonNuevo = "Nuevo paciente";
        EsEspecialidades = false;
        EsPersonal = false;
        EsPacientes = true;
        EsCitas = false;
        CargarPacientesCommand.Execute(null);
    }

    [RelayCommand]
    public void IrACitas()
    {
        TituloSeccion = "Citas";
        TextoBotonNuevo = "Nueva cita";
        EsEspecialidades = false;
        EsPersonal = false;
        EsPacientes = false;
        EsCitas = true;
        CargarCitasCommand.Execute(null);
    }

    [RelayCommand]
    public void AbrirDialog()
    {
        if (EsEspecialidades) OpenEspecialidadDialog();
        if (EsPersonal) OpenPersonalDialog();
        if (EsPacientes) OpenPacienteDialog();
        if (EsCitas) OpenCitaDialog();
    }

    // --- Especialidades ---
    public void OpenEspecialidadDialog()
    {
        HayErrorDialog = false;
        MensajeErrorDialog = "";
        Editando = false;
        EspecialidadSeleccionada = new EspecialidadModel();
        DiaLunes = false;
        DiaMartes = false;
        DiaMiercoles = false;
        DiaJueves = false;
        DiaViernes = false;
        DiaSabado = false;
        DiaDomingo = false;
        HoraInicioSel = null;
        HoraFinSel = null;
        var dialog = new EspecialidadDialog() { DataContext = this };
        DialogHost.Show(dialog, "HomeDialog");
    }
    
    [RelayCommand]
    public void EditarEspecialidad(EspecialidadModel e)
    {
        
        HayErrorDialog = false;
        MensajeErrorDialog = "";
        
        if (e != null)
        {
            Editando = true;
            EspecialidadSeleccionada = e;

            // Marca los checkboxes según los días guardados
            DiaLunes = e.Dias.Contains("Lun");
            DiaMartes = e.Dias.Contains("Mar");
            DiaMiercoles = e.Dias.Contains("Mié");
            DiaJueves = e.Dias.Contains("Jue");
            DiaViernes = e.Dias.Contains("Vie");
            DiaSabado = e.Dias.Contains("Sáb");
            DiaDomingo = e.Dias.Contains("Dom");

            // Carga las horas en los relojes
            if (TimeSpan.TryParse(e.HoraInicio, out var hi)) HoraInicioSel = hi;
            if (TimeSpan.TryParse(e.HoraFin, out var hf)) HoraFinSel = hf;

            var dialog = new EspecialidadDialog() { DataContext = this };
            DialogHost.Show(dialog, "HomeDialog");
        }
    }
    
    partial void OnEspecialidadDeLaCitaChanged(EspecialidadModel value)
    {
        if (value != null && !string.IsNullOrWhiteSpace(value.Nombre))
        {
            HorarioEspecialidadCita = "Atiende: " + value.Dias + "  |  " + value.HoraInicio + " - " + value.HoraFin;
        }
        else
        {
            HorarioEspecialidadCita = "";
        }
    }

    [RelayCommand]
    public void CloseEspecialidadDialog()
    {
        DialogHost.Close("HomeDialog");
    }

    [RelayCommand]
    public async Task GuardarEspecialidad()
    {
        // Validación
        if (string.IsNullOrWhiteSpace(EspecialidadSeleccionada.Nombre))
        {
            MensajeErrorDialog = "El nombre es obligatorio.";
            HayErrorDialog = true;
            return;
        }
        if (!HoraInicioSel.HasValue || !HoraFinSel.HasValue)
        {
            MensajeErrorDialog = "Debes indicar la hora de inicio y de fin.";
            HayErrorDialog = true;
            return;
        }
        if (!DiaLunes && !DiaMartes && !DiaMiercoles && !DiaJueves && !DiaViernes && !DiaSabado && !DiaDomingo)
        {
            MensajeErrorDialog = "Selecciona al menos un día de atención.";
            HayErrorDialog = true;
            return;
        }
        if (HoraInicioSel >= HoraFinSel)
        {
            MensajeErrorDialog = "La hora de fin debe ser posterior a la de inicio.";
            HayErrorDialog = true;
            return;
        }
        if (!Editando)
        {
            foreach (var esp in ListaEspecialidades)
            {
                if (esp.Nombre.ToLower() == EspecialidadSeleccionada.Nombre.ToLower())
                {
                    MensajeErrorDialog = "Ya existe una especialidad con ese nombre.";
                    HayErrorDialog = true;
                    return;
                }
            }
        }

        HayErrorDialog = false;
        
        
            var dias = new System.Collections.Generic.List<string>();
            if (DiaLunes) dias.Add("Lun");
            if (DiaMartes) dias.Add("Mar");
            if (DiaMiercoles) dias.Add("Mié");
            if (DiaJueves) dias.Add("Jue");
            if (DiaViernes) dias.Add("Vie");
            if (DiaSabado) dias.Add("Sáb");
            if (DiaDomingo) dias.Add("Dom");
            EspecialidadSeleccionada.Dias = string.Join(", ", dias);

            EspecialidadSeleccionada.HoraInicio = HoraInicioSel.HasValue ? HoraInicioSel.Value.ToString(@"hh\:mm") : "";
            EspecialidadSeleccionada.HoraFin = HoraFinSel.HasValue ? HoraFinSel.Value.ToString(@"hh\:mm") : "";

            if (Editando)
            {
                await supabaseService.ModificarEspecialidad(EspecialidadSeleccionada);
            }
            else
            {
                await supabaseService.CrearEspecialidad(EspecialidadSeleccionada);
            }

            CloseEspecialidadDialog();
            await CargarEspecialidades();
        
    }

    [RelayCommand]
    public async Task EliminarEspecialidad(EspecialidadModel e)
    {
        if (e != null)
        {
            await supabaseService.EliminarEspecialidad(e);
            await CargarEspecialidades();
        }
    }
    
    
    [RelayCommand]
    public async Task CargarEspecialidades()
    {
        var lista = await supabaseService.ObtenerEspecialidades();
        if (lista != null)
        {
            ListaEspecialidades = lista;
            TotalEspecialidades = ListaEspecialidades.Count;
        }
    }

    // --- Personal ---
    public void OpenPersonalDialog()
    {
        HayErrorDialog = false;
        MensajeErrorDialog = "";
        Editando = false;
        DoctorSeleccionado = new DoctorModel();
        EspecialidadDelDoctor = new EspecialidadModel();
        var dialog = new PersonalDialog() { DataContext = this };
        DialogHost.Show(dialog, "HomeDialog");
    }

    [RelayCommand]
    public void ClosePersonalDialog()
    {
        DialogHost.Close("HomeDialog");
    }
    [RelayCommand]
    public async Task GuardarPersonal()
    {
        if (string.IsNullOrWhiteSpace(DoctorSeleccionado.Nombre) || string.IsNullOrWhiteSpace(DoctorSeleccionado.Apellido))
        {
            MensajeErrorDialog = "El nombre y el apellido son obligatorios.";
            HayErrorDialog = true;
            return;
        }
        if (string.IsNullOrWhiteSpace(DoctorSeleccionado.Tipo))
        {
            MensajeErrorDialog = "Selecciona el tipo (Médico o Enfermero).";
            HayErrorDialog = true;
            return;
        }
        HayErrorDialog = false;
        
            if (EspecialidadDelDoctor != null && EspecialidadDelDoctor.Id != null)
            {
                DoctorSeleccionado.EspecialidadId = EspecialidadDelDoctor.Id ?? 0;
                DoctorSeleccionado.EspecialidadNombre = EspecialidadDelDoctor.Nombre;
            }

            if (Editando)
            {
                await supabaseService.ModificarPersonal(DoctorSeleccionado);
            }
            else
            {
                await supabaseService.CrearPersonal(DoctorSeleccionado);
            }

            ClosePersonalDialog();
            await CargarPersonal();
        
    }

    [RelayCommand]
    public async Task EliminarPersonal(DoctorModel d)
    {
        if (d != null)
        {
            await supabaseService.EliminarPersonal(d);
            await CargarPersonal();
        }
    }
    
    [RelayCommand]
    public async Task CargarPersonal()
    {
        var lista = await supabaseService.ObtenerPersonal();
        if (lista != null)
        {
            ListaPersonal = lista;
            TotalPersonal = ListaPersonal.Count;
        }
    }
    
    [RelayCommand]
    public void EditarPersonal(DoctorModel d)
    {
        if (d != null)
        {
            Editando = true;
            DoctorSeleccionado = d;
            var dialog = new PersonalDialog() { DataContext = this };
            DialogHost.Show(dialog, "HomeDialog");
        }
    }

    // --- Pacientes ---
    public void OpenPacienteDialog()
    {
        HayErrorDialog = false;
        MensajeErrorDialog = "";
        Editando = false;
        PacienteSeleccionado = new PacienteModel();
        PacienteSeleccionado.Identificador = GenerarIdentificadorPaciente();
        FechaNacimientoSel = null;
        var dialog = new PacienteDialog() { DataContext = this };
        DialogHost.Show(dialog, "HomeDialog");
    }

    [RelayCommand]
    public void ClosePacienteDialog()
    {
        DialogHost.Close("HomeDialog");
    }

    [RelayCommand]
    public async Task GuardarPaciente()
    {
        if (string.IsNullOrWhiteSpace(PacienteSeleccionado.Nombre) || string.IsNullOrWhiteSpace(PacienteSeleccionado.Apellido))
        {
            MensajeErrorDialog = "El nombre y el apellido son obligatorios.";
            HayErrorDialog = true;
            return;
        }
        if (string.IsNullOrWhiteSpace(PacienteSeleccionado.Dni))
        {
            MensajeErrorDialog = "El DNI es obligatorio.";
            HayErrorDialog = true;
            return;
        }
        if (!FechaNacimientoSel.HasValue)
        {
            MensajeErrorDialog = "Indica la fecha de nacimiento.";
            HayErrorDialog = true;
            return;
        }
        // Validación: la fecha de nacimiento no puede ser futura
        if (FechaNacimientoSel.Value.Date > DateTime.Now.Date)
        {
            MensajeErrorDialog = "La fecha de nacimiento no puede ser futura.";
            HayErrorDialog = true;
            return;
        }
        if (!string.IsNullOrWhiteSpace(PacienteSeleccionado.CorreoElectronico)
            && !PacienteSeleccionado.CorreoElectronico.Contains("@"))
        {
            MensajeErrorDialog = "El correo electrónico no es válido.";
            HayErrorDialog = true;
            return;
        }
        if (PacienteSeleccionado.Dni.Length < 8 || PacienteSeleccionado.Dni.Length > 9)
        {
            MensajeErrorDialog = "El DNI debe tener entre 8 y 9 caracteres.";
            HayErrorDialog = true;
            return;
        }
        if (!Editando)
        {
            foreach (var pac in ListaPacientes)
            {
                if (pac.Dni.ToLower() == PacienteSeleccionado.Dni.ToLower())
                {
                    MensajeErrorDialog = "Ya existe un paciente con ese DNI.";
                    HayErrorDialog = true;
                    return;
                }
            }
        }
        HayErrorDialog = false;
        
            PacienteSeleccionado.FechaNacimiento = FechaNacimientoSel.HasValue
                ? FechaNacimientoSel.Value.ToString("dd/MM/yyyy")
                : "";

            if (Editando)
            {
                await supabaseService.ModificarPaciente(PacienteSeleccionado);
            }
            else
            {
                await supabaseService.CrearPaciente(PacienteSeleccionado);
            }

            ClosePacienteDialog();
            await CargarPacientes();
        
    }

   [RelayCommand]
   public async Task EliminarPaciente(PacienteModel p)
   {
       if (p != null)
       {
           await supabaseService.EliminarPaciente(p);
           await CargarPacientes();
       }
   }
    
    [RelayCommand]
    public async Task CargarPacientes()
    {
        var lista = await supabaseService.ObtenerPacientes();
        if (lista != null)
        {
            ListaPacientes = lista;
            TotalPacientes = ListaPacientes.Count;
        }
    }
    
    [RelayCommand]
    public void EditarPaciente(PacienteModel p)
    {
        if (p != null)
        {
            Editando = true;
            PacienteSeleccionado = p;

            // Carga la fecha en el calendario
            if (DateTime.TryParseExact(p.FechaNacimiento, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var fecha))
            {
                FechaNacimientoSel = new DateTimeOffset(fecha);
            }

            var dialog = new PacienteDialog() { DataContext = this };
            DialogHost.Show(dialog, "HomeDialog");
        }
    }

    // --- Citas ---
    public void OpenCitaDialog()
    {
        HayErrorDialog = false;
        MensajeErrorDialog = "";
        Editando = false;
        CitaSeleccionada = new CitaModel();
        PacienteDeLaCita = new PacienteModel();
        EspecialidadDeLaCita = new EspecialidadModel();
        DoctorDeLaCita = new DoctorModel();
        FechaCitaSel = null;
        HoraCitaSel = null;
        var dialog = new CitaDialog() { DataContext = this };
        DialogHost.Show(dialog, "HomeDialog");
    }

    [RelayCommand]
    public void CloseCitaDialog()
    {
        DialogHost.Close("HomeDialog");
    }
    [RelayCommand]
    public async Task GuardarCita()
    {
        if (PacienteDeLaCita == null || PacienteDeLaCita.Id == null)
        {
            MensajeErrorDialog = "Selecciona un paciente.";
            HayErrorDialog = true;
            return;
        }
        if (EspecialidadDeLaCita == null || EspecialidadDeLaCita.Id == null)
        {
            MensajeErrorDialog = "Selecciona una especialidad.";
            HayErrorDialog = true;
            return;
        }
        if (DoctorDeLaCita == null || DoctorDeLaCita.Id == null)
        {
            MensajeErrorDialog = "Selecciona un doctor.";
            HayErrorDialog = true;
            return;
        }
        if (!FechaCitaSel.HasValue || !HoraCitaSel.HasValue)
        {
            MensajeErrorDialog = "Indica la fecha y la hora de la cita.";
            HayErrorDialog = true;
            return;
        }
        HayErrorDialog = false;
        
        // Validación de fecha pasada 
        if (FechaCitaSel.Value.Date < DateTime.Now.Date)
        {
            MensajeErrorDialog = "No puedes agendar una cita en una fecha pasada.";
            HayErrorDialog = true;
            return;
        }

        // Validación del día de atención 
        string diaSemana = FechaCitaSel.Value.DayOfWeek switch
        {
            DayOfWeek.Monday => "Lun",
            DayOfWeek.Tuesday => "Mar",
            DayOfWeek.Wednesday => "Mié",
            DayOfWeek.Thursday => "Jue",
            DayOfWeek.Friday => "Vie",
            DayOfWeek.Saturday => "Sáb",
            DayOfWeek.Sunday => "Dom",
            _ => ""
        };

        if (!EspecialidadDeLaCita.Dias.Contains(diaSemana))
        {
            MensajeErrorDialog = "Esa especialidad no atiende ese día. Atiende: " + EspecialidadDeLaCita.Dias;
            HayErrorDialog = true;
            return;
        }

        // Validación de la hora dentro del horario
        TimeSpan.TryParse(EspecialidadDeLaCita.HoraInicio, out var inicio);
        TimeSpan.TryParse(EspecialidadDeLaCita.HoraFin, out var fin);

        if (HoraCitaSel.Value < inicio || HoraCitaSel.Value >= fin)
        {
            MensajeErrorDialog = "La hora está fuera del horario. Atiende de " + EspecialidadDeLaCita.HoraInicio + " a " + EspecialidadDeLaCita.HoraFin + ".";
            HayErrorDialog = true;
            return;
        }
        
            if (PacienteDeLaCita != null && PacienteDeLaCita.Id != null)
            {
                CitaSeleccionada.PacienteId = PacienteDeLaCita.Id ?? 0;
                CitaSeleccionada.PacienteNombre = PacienteDeLaCita.Nombre + " " + PacienteDeLaCita.Apellido;
            }
            if (EspecialidadDeLaCita != null && EspecialidadDeLaCita.Id != null)
            {
                CitaSeleccionada.EspecialidadId = EspecialidadDeLaCita.Id ?? 0;
                CitaSeleccionada.EspecialidadNombre = EspecialidadDeLaCita.Nombre;
            }
            if (DoctorDeLaCita != null && DoctorDeLaCita.Id != null)
            {
                CitaSeleccionada.DoctorId = DoctorDeLaCita.Id ?? 0;
                CitaSeleccionada.DoctorNombre = DoctorDeLaCita.Nombre + " " + DoctorDeLaCita.Apellido;
            }

            CitaSeleccionada.FechaCita = FechaCitaSel.HasValue ? FechaCitaSel.Value.ToString("dd/MM/yyyy") : "";
            CitaSeleccionada.HoraCita = HoraCitaSel.HasValue ? HoraCitaSel.Value.ToString(@"hh\:mm") : "";

            if (Editando)
            {
                await supabaseService.ModificarCita(CitaSeleccionada);
            }
            else
            {
                await supabaseService.CrearCita(CitaSeleccionada);
            }

            CloseCitaDialog();
            await CargarCitas();
        
    }

    [RelayCommand]
    public async Task EliminarCita(CitaModel c)
    {
        if (c != null)
        {
            await supabaseService.EliminarCita(c);
            await CargarCitas();
        }
    }
    
    [RelayCommand]
    public void EditarCita(CitaModel c)
    {
        if (c != null)
        {
            Editando = true;
            CitaSeleccionada = c;

            // Preselecciona el paciente que coincide con el Id guardado
            foreach (var p in ListaPacientes)
            {
                if (p.Id == c.PacienteId) PacienteDeLaCita = p;
            }

            // Preselecciona la especialidad
            foreach (var e in ListaEspecialidades)
            {
                if (e.Id == c.EspecialidadId) EspecialidadDeLaCita = e;
            }

            // Preselecciona el doctor
            foreach (var d in ListaPersonal)
            {
                if (d.Id == c.DoctorId) DoctorDeLaCita = d;
            }

            // Carga la fecha en el calendario
            if (DateTime.TryParseExact(c.FechaCita, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var fecha))
            {
                FechaCitaSel = new DateTimeOffset(fecha);
            }

            // Carga la hora en el reloj
            if (TimeSpan.TryParse(c.HoraCita, out var hora))
            {
                HoraCitaSel = hora;
            }

            var dialog = new CitaDialog() { DataContext = this };
            DialogHost.Show(dialog, "HomeDialog");
        }
    }   
    
    [RelayCommand]
    public async Task CargarCitas()
    {
        var lista = await supabaseService.ObtenerCitas();
        if (lista != null)
        {
            ListaCitas = lista;

            
            string hoy = DateTime.Now.ToString("dd/MM/yyyy");
            int contadorHoy = 0;
            foreach (var c in ListaCitas)
            {
                if (c.FechaCita == hoy) contadorHoy++;
            }
            TotalCitasHoy = contadorHoy;
        }
    } 
    
    public string GenerarIdentificadorPaciente()
    {
        int siguiente = ListaPacientes.Count + 1;
        return "PAC-" + siguiente.ToString("D4");
    }
}