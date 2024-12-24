// Abstract class that defines the base properties of an armor type
public abstract class ArmorType
{
    public int ADDefense { get; set; }  // Defense against physical damage (Attack Damage)
    public int APDefense { get; set; }  // Defense against magic damage (Ability Power)

    protected ArmorType() { }  // Protected constructor to allow instantiation by subclasses
}

// Fabric Armor: A basic armor offering magic defense but no physical defense
public class ArmorFabric : ArmorType
{
    public ArmorFabric()
    {
        APDefense = 30; 
        ADDefense = 0;
    }
}

// Leather Armor: Provides balanced defense against both physical and magic attacks
public class ArmorLeather : ArmorType
{
    public ArmorLeather()
    {
        APDefense = 20; 
        ADDefense = 15; 
    }
}

// Mesh Armor: Provides more physical defense at the expense of magic defense
public class ArmorMeshes : ArmorType
{
    public ArmorMeshes()
    {
        APDefense = 10;  
        ADDefense = 30;  
    }
}

// Plate Armor: Provides the highest physical defense, but no magic defense
public class ArmorPlates : ArmorType
{
    public ArmorPlates()
    {
        APDefense = 0;
        ADDefense = 45; 
    }
}
