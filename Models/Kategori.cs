public class Kategori
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public virtual ICollection<Barang> Barangs { get; set; } = new List<Barang>();
}
