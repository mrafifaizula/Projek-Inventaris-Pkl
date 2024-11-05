using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Kategori
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public virtual ICollection<Barang> Barangs { get; set; } = new List<Barang>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
