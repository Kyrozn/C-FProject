public abstract class ArmorType
{
    public int ADDefense { get; set; }
    public int APDefense { get; set; }

    protected ArmorType() {}
}
public class ArmorFabric : ArmorType
{

    public ArmorFabric()
    {
        APDefense = 30;
        ADDefense = 0;
    }
}
public class ArmorLeather : ArmorType
{
    public ArmorLeather()
    {
        APDefense = 20;
        ADDefense = 15;
    }
}

public class ArmorMeshes : ArmorType
{

    public ArmorMeshes()
    {
        APDefense = 10;
        ADDefense = 30;
    }
}
public class ArmorPlates : ArmorType
{
    public ArmorPlates()
    {
        APDefense = 0;
        ADDefense = 45;
    }
}
