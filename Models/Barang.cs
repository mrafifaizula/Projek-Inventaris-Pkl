public class Barang
{
    public int Id { get; set; }

    public string? Nama { get; set; }
    public decimal Harga { get; set; }
    public int Stok { get; set; }

    public string? Keterangan { get; set; }
    public string? Gambar { get; set; }

    public int KategoriId { get; set; }
    public Kategori? Kategori { get; set; } // Tidak perlu diinisialisasi

    public int PemasokId { get; set; }
    public Pemasok? Pemasok { get; set; } // Tidak perlu diinisialisasi
}
