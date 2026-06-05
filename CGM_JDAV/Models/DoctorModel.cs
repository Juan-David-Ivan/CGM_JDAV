using Newtonsoft.Json;

namespace CGM_JDAV.Models;

public class DoctorModel
{
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public int? Id { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; } = "";

    [JsonProperty("apellido")]
    public string Apellido { get; set; } = "";

    [JsonProperty("tipo")]
    public string Tipo { get; set; } = "";

    [JsonProperty("especialidadid")]
    public int EspecialidadId { get; set; }
    
    [JsonProperty("especialidadnombre")]
    public string EspecialidadNombre { get; set; } = "";

    public override string ToString() => $"{Nombre} {Apellido}";
}