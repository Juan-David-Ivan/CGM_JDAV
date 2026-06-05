using Newtonsoft.Json;

namespace CGM_JDAV.Models;

public class CitaModel
{
    [JsonProperty("codigocita", NullValueHandling = NullValueHandling.Ignore)]
    public int? CodigoCita { get; set; }

    [JsonProperty("especialidadid")]
    public int EspecialidadId { get; set; }

    [JsonProperty("pacienteid")]
    public int PacienteId { get; set; }

    [JsonProperty("doctorid")]
    public int DoctorId { get; set; }

    [JsonProperty("estado")]
    public string Estado { get; set; } = "Pendiente";

    [JsonProperty("fechacita")]
    public string FechaCita { get; set; } = "";

    [JsonProperty("horacita")]
    public string HoraCita { get; set; } = "";
    
    [JsonProperty("pacientenombre")]
    public string PacienteNombre { get; set; } = "";

    [JsonProperty("doctornombre")]
    public string DoctorNombre { get; set; } = "";

    [JsonProperty("especialidadnombre")]
    public string EspecialidadNombre { get; set; } = "";

    public override string ToString() => $"{FechaCita} {HoraCita} - {Estado}";
}