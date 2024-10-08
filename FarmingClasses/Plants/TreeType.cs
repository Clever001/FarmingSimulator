using System.ComponentModel.DataAnnotations;

public enum TreeType {
    [Display(Name = "Лиственное дерево")]
    Deciduous,
    [Display(Name = "Хвойное дерево")]
    Coniferous,
    [Display(Name = "Кустарник")]
    Shrub,
    [Display(Name = "Лиана")]
    Vine,
    [Display(Name = "Пальма")]
    Palm,
    [Display(Name = "Цитрусовое дерево")]
    Citrus
}
