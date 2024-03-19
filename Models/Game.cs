using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Game
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id  {get; set;}

    [BsonElement("Nombre")]
    public string? Nombre {get; set;}

    [BsonElement("Email")]
    public string? Email {get; set;}

    [BsonElement("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [BsonElement("level")]
    public int? Level { get; set; }

    [BsonElement("experience")]
    public int? Experience { get; set; }
 

     [BsonElement("strength")]
    public int? Strength { get; set; }

    [BsonElement("currentDungeon")]
    public string? CurrentDungeon { get; set; }

    [BsonElement("currentFloor")]
    public int? CurrentFloor { get; set; }

    [BsonElement("enemiesDefeated")]
    public int? EnemiesDefeated { get; set; }

    [BsonElement("bossesDefeated")]
    public int? BossesDefeated { get; set; }

    [BsonElement("inventory")]
    public List<Item>? Inventory { get; set; }

    [BsonElement("equippedWeapon")]
    public Weapon? EquippedWeapon { get; set; }

    [BsonElement("equippedArmor")]
    public Armor? EquippedArmor { get; set; }

    public class Item
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ItemId { get; set; }

    [BsonElement("itemName")]
    public string? ItemName { get; set; }

    [BsonElement("quantity")]
    public int? Quantity { get; set; }

    // Add any other properties specific to items
}

public class Weapon
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? WeaponId { get; set; }

    [BsonElement("weaponName")]
    public string? WeaponName { get; set; }

    [BsonElement("damage")]
    public int? Damage { get; set; }

    // Add any other properties specific to weapons
}

public class Armor
{
   [BsonId]
   [BsonRepresentation(BsonType.ObjectId)]
    public string? ArmorId { get; set; }

    [BsonElement("armorName")]
    public string? ArmorName { get; set; }

    [BsonElement("defense")]
    public int? Defense { get; set; }

    // Add any other properties specific to armor
}

public class Achievement
{
    [BsonElement("achievementId")]
    public string? AchievementId { get; set; }

    [BsonElement("achievementName")]
    public string? AchievementName { get; set; }

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("unlockedAt")]
    public DateTime? UnlockedAt { get; set; }

    // Add any other properties specific to achievements
}


}