public class Pemasok
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public string Alamat { get; set; }
    public virtual ICollection<Barang> Barangs { get; set; } = new List<Barang>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
