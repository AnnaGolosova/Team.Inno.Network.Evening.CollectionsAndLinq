using AutoFixture;
using FluentAssertions;
using Team.Inno.Network.Evening.CollectionsAndLinq;

/// <summary>
/// Provides comprehensive test coverage for the InventoryManager utility class using xUnit and FluentAssertions.
/// </summary>
public class InventoryManagerTests
{
    private readonly IFixture _fixture;
    private readonly List<PlayerItem> _standardInventory;

    public InventoryManagerTests()
    {
        // ARRANGE: Initialize AutoFixture and define the standard test set
        _fixture = new Fixture();

        _standardInventory = new List<PlayerItem>
        {
            // Weapon Total Durability: 100 + 80 + 90 = 270
            new PlayerItem { Name = "Iron Sword", Durability = 100, Type = "Weapon" },
            new PlayerItem { Name = "Iron Axe", Durability = 80, Type = "Weapon" },
            new PlayerItem { Name = "Bow", Durability = 90, Type = "Weapon" },

            // Armor Total Durability: 30 + 10 = 40
            new PlayerItem { Name = "Wooden Shield", Durability = 30, Type = "Armor" },
            new PlayerItem { Name = "Leather Boots", Durability = 10, Type = "Armor" }
        };
    }

    // ====================================================================
    // TASK 1: CountDamagedItems
    // ====================================================================

    [Theory]
    [InlineData(90, 3)] // Standard case: Items < 90 (10, 30, 80)
    [InlineData(100, 4)] // Items < 100 (10, 30, 80, 90)
    [InlineData(0, 0)]   // Edge case: Threshold is zero
    public void CountDamagedItems_Standard_ReturnsCorrectCount(int threshold, int expectedCount)
    {
        // ACT
        int actualCount = InventoryManager.CountDamagedItems(_standardInventory, threshold);

        // ASSERT
        actualCount.Should().Be(expectedCount);
    }

    [Fact]
    public void CountDamagedItems_WhenInventoryIsEmpty_ReturnsZero()
    {
        // ARRANGE
        var emptyList = new List<PlayerItem>();

        // ACT
        int actualCount = InventoryManager.CountDamagedItems(emptyList, 50);

        // ASSERT
        actualCount.Should().Be(0);
    }

    // ====================================================================
    // TASK 2: GetNamesOfItemsToRepair
    // ====================================================================

    [Fact]
    public void GetNamesOfItemsToRepair_Standard_ReturnsSortedNames()
    {
        // ARRANGE
        int maxDurability = 100;
        // Expected order by ascending Durability: (10, 30, 80, 90)
        var expectedNames = new List<string> { "Leather Boots", "Wooden Shield", "Iron Axe", "Bow" };

        // ACT
        List<string> actualNames = InventoryManager.GetNamesOfItemsToRepair(_standardInventory, maxDurability);

        // ASSERT
        actualNames.Take(4).Should().ContainInOrder(expectedNames);
        actualNames.Should().NotBeEmpty();
    }

    [Fact]
    public void GetNamesOfItemsToRepair_WhenNoItemsNeedRepair_ReturnsEmptyList()
    {
        // ARRANGE
        // Max Durability is set to 1, all items have Durability >= 10
        int maxDurability = 1;

        // ACT
        List<string> actualNames = InventoryManager.GetNamesOfItemsToRepair(_standardInventory, maxDurability);

        // ASSERT
        actualNames.Should().BeEmpty();
    }

    // ====================================================================
    // TASK 3: GetTotalDurabilityByType
    // ====================================================================

    [Fact]
    public void GetTotalDurabilityByType_WhenWeaponType_Returns270()
    {
        // ARRANGE
        string itemType = "Weapon";
        // Expected sum: 100 + 80 + 90 = 270

        // ACT
        int actualTotal = InventoryManager.GetTotalDurabilityByType(_standardInventory, itemType);

        // ASSERT
        actualTotal.Should().Be(270);
    }

    [Fact]
    public void GetTotalDurabilityByType_WhenTypeNotFound_ReturnsZero()
    {
        // ARRANGE
        string itemType = "Potion"; // Non-existent type

        // ACT
        int actualTotal = InventoryManager.GetTotalDurabilityByType(_standardInventory, itemType);

        // ASSERT
        actualTotal.Should().Be(0);
    }

    // ====================================================================
    // TASK 4: GetTypeWithLowestTotalDurability
    // ====================================================================

    [Fact]
    public void GetTypeWithLowestTotalDurability_Standard_ReturnsArmor()
    {
        // ARRANGE
        // Weapon Total: 270; Armor Total: 40.

        // ACT
        string actualType = InventoryManager.GetTypeWithLowestTotalDurability(_standardInventory);

        // ASSERT
        actualType.Should().Be("Armor");
    }

    [Fact]
    public void GetTypeWithLowestTotalDurability_WhenEmptyList_ReturnsEmptyString()
    {
        // ARRANGE
        var emptyList = new List<PlayerItem>();

        // ACT
        string actualType = InventoryManager.GetTypeWithLowestTotalDurability(emptyList);

        // ASSERT
        actualType.Should().Be(string.Empty);
    }

    [Fact]
    public void GetTypeWithLowestTotalDurability_WhenDurabilitiesAreUniform_ReturnsFirstType()
    {
        // ARRANGE (Total Durability: TypeA=10, TypeB=10)
        var uniformList = new List<PlayerItem>
        {
            new PlayerItem { Name = "A1", Durability = 10, Type = "TypeA" },
            new PlayerItem { Name = "B1", Durability = 10, Type = "TypeB" }
        };

        // ACT
        string actualType = InventoryManager.GetTypeWithLowestTotalDurability(uniformList);

        // ASSERT (Grouping logic should select the first one encountered: TypeA)
        actualType.Should().Be("TypeA");
    }
}