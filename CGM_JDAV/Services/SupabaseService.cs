using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using CGM_JDAV.Models;
using Newtonsoft.Json;

namespace CGM_JDAV.Services;

public class SupabaseService
{
    private HttpClient client;

    public SupabaseService()
    {
        client = new HttpClient();
        client.BaseAddress = new Uri("https://zbqpmtdwxyanealdcjuv.supabase.co/");
        client.DefaultRequestHeaders.Add("apikey", "sb_publishable_vYINL8gMylXBzHifWCG57g_Zp-VPfJ0");
    }
    
    // --- Comprobar login ---
    public async Task<AvaloniaList<AdministradorModel>> ComprobarLogin(string usuario, string contrasena)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            "rest/v1/administrador?usuario=eq." + usuario + "&contrasena=eq." + contrasena);
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<AdministradorModel>>(listaString);
    }

    // Obtener todas las especialidades 
    public async Task<AvaloniaList<EspecialidadModel>> ObtenerEspecialidades()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/especialidad?select=*");
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<EspecialidadModel>>(listaString);
    }

    // Crear especialidad 
    public async Task CrearEspecialidad(EspecialidadModel especialidad)
    {
        var json = JsonConvert.SerializeObject(especialidad);
        var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/especialidad")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }

    // Eliminar especialidad
    public async Task EliminarEspecialidad(EspecialidadModel especialidad)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "rest/v1/especialidad?id=eq." + especialidad.Id);
        await client.SendAsync(request);
    }
    
    // Modificar especialidad
    public async Task ModificarEspecialidad(EspecialidadModel especialidad)
    {
        var json = JsonConvert.SerializeObject(especialidad);
        var request = new HttpRequestMessage(HttpMethod.Patch, "rest/v1/especialidad?id=eq." + especialidad.Id)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }
    
    // Obtener personal
    public async Task<AvaloniaList<DoctorModel>> ObtenerPersonal()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/doctor?select=*");
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<DoctorModel>>(listaString);
    }

    // Crear personal
    public async Task CrearPersonal(DoctorModel doctor)
    {
        var json = JsonConvert.SerializeObject(doctor);
        var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/doctor")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }

    // Eliminar personal 
    public async Task EliminarPersonal(DoctorModel doctor)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "rest/v1/doctor?id=eq." + doctor.Id);
        await client.SendAsync(request);
    }
    
    // Modificar personal
    public async Task ModificarPersonal(DoctorModel doctor)
    {
        var json = JsonConvert.SerializeObject(doctor);
        var request = new HttpRequestMessage(HttpMethod.Patch, "rest/v1/doctor?id=eq." + doctor.Id)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }
    
    // Obtener pacientes
    public async Task<AvaloniaList<PacienteModel>> ObtenerPacientes()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/paciente?select=*");
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<PacienteModel>>(listaString);
    }

    // Crear paciente
    public async Task CrearPaciente(PacienteModel paciente)
    {
        var json = JsonConvert.SerializeObject(paciente);
        var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/paciente")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }

    // Eliminar paciente
    public async Task EliminarPaciente(PacienteModel paciente)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "rest/v1/paciente?id=eq." + paciente.Id);
        await client.SendAsync(request);
    }
    
    //Modificar paciente
    public async Task ModificarPaciente(PacienteModel paciente)
    {
        var json = JsonConvert.SerializeObject(paciente);
        var request = new HttpRequestMessage(HttpMethod.Patch, "rest/v1/paciente?id=eq." + paciente.Id)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }
    
    // Obtener todas las citas
    public async Task<AvaloniaList<CitaModel>> ObtenerCitas()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "rest/v1/cita?select=*");
        var response = await client.SendAsync(request);
        var listaString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<AvaloniaList<CitaModel>>(listaString);
    }

    // Crear cita 
    public async Task CrearCita(CitaModel cita)
    {
        var json = JsonConvert.SerializeObject(cita);
        var request = new HttpRequestMessage(HttpMethod.Post, "rest/v1/cita")
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }

    // Eliminar cita 
    public async Task EliminarCita(CitaModel cita)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, "rest/v1/cita?codigocita=eq." + cita.CodigoCita);
        await client.SendAsync(request);
    }
    
    //  Modificar cita 
    public async Task ModificarCita(CitaModel cita)
    {
        var json = JsonConvert.SerializeObject(cita);
        var request = new HttpRequestMessage(HttpMethod.Patch, "rest/v1/cita?codigocita=eq." + cita.CodigoCita)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        await client.SendAsync(request);
    }
}