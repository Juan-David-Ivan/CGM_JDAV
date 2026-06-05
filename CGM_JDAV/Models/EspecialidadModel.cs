using Newtonsoft.Json;

namespace CGM_JDAV.Models;

public class EspecialidadModel
{
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public int? Id { get; set; }

    [JsonProperty("nombre")]
    public string Nombre { get; set; } = "";

    [JsonProperty("dias")]
    public string Dias { get; set; } = "";

    [JsonProperty("hora_inicio")]
    public string HoraInicio { get; set; } = "";

    [JsonProperty("hora_fin")]
    public string HoraFin { get; set; } = "";

    public override string ToString() => Nombre;
}