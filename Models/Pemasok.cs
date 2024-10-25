public class Pemasok
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public string Alamat { get; set; }

    // Navigasi 09ike Barang
    public virtual ICollection<Barang> Barangs { get; set; } = new List<Barang>();
}
