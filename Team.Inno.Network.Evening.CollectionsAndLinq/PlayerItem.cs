namespace Team.Inno.Network.Evening.CollectionsAndLinq;

/// <summary>
/// Represents a single item in a player's inventory, defined by its properties like name, durability, and type.
/// </summary>
public class PlayerItem
{
    /// <summary>
    /// Gets or sets the name of the item.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the current durability of the item.
    /// </summary>
    public int Durability { get; set; }

    /// <summary>
    /// Gets or sets the type or category of the item (e.g., "Weapon", "Armor").
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Returns a string representation of the item.
    /// </summary>
    public override string ToString() => $"[{Type}] {Name}: {Durability}";
}
