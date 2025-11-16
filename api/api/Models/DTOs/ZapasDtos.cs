namespace api.Models.DTOs
{
    public class ZapasDto
    {
        public int IdZapas { get; set; }
        public string Datum { get; set; } = null!;
        public int IdTym1 { get; set; }
        public int IdTym2 { get; set; }
        public int IdSezona { get; set; }
        public int? SkoreTym1 { get; set; }
        public int? SkoreTym2 { get; set; }
        public int? Vitez { get; set; }
        public string? NazevTym1 { get; set; }
        public string? NazevTym2 { get; set; }
        public string? NazevSezona { get; set; }
        public string? NazevVitez { get; set; }
    }

    public class ZapasFilterDto
    {
        public string? Datum { get; set; }
        public int? IdSezona { get; set; }
        public int? IdTym1 { get; set; }
        public int? IdTym2 { get; set; }
    }

    public class CreateZapasDto
    {
        public string Datum { get; set; } = null!;
        public int IdSezona { get; set; }
        public int IdTym1 { get; set; }
        public int IdTym2 { get; set; }
    }

    public class UpdateZapasDto
    {
        public int IdZapas { get; set; }
        public string Datum { get; set; } = null!;
        public int IdSezona { get; set; }
        public int IdTym1 { get; set; }
        public int IdTym2 { get; set; }
        public int? SkoreTym1 { get; set; }
        public int? SkoreTym2 { get; set; }
        public int? Vitez { get; set; }
    }
}
