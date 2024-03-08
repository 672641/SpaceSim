using System;
using System.Drawing;

namespace Spacelib {
public class SpaceObject {
    public string Name { get; set; }
    public double OrbitalRadius { get; set; }
    public double OrbitalPeriod { get; set; }
    public double Radius { get; set; }
    public double RotationalPeriod { get; set; }

     public SpaceObject(string name, double orbitalRadius, double orbitalPeriod, double radius, double rotationalPeriod)
        {
            Name = name;
            OrbitalRadius = orbitalRadius;
            OrbitalPeriod = orbitalPeriod;
            Radius = radius;
            RotationalPeriod = rotationalPeriod;
        }

    public SpaceObject(string name, double radius)
    {
        Name = name;
        Radius = radius;
    }

    public virtual void Draw()
    {
        Console.WriteLine(Name);
    }

    public (double, double) CalculatePosition(double time)
    {
        double radian = 2 * Math.PI * time / OrbitalPeriod;
        double x = OrbitalRadius * Math.Cos(radian);
        double y = OrbitalRadius * Math.Sin(radian);

        return (x, y);
    }
}

public class Star : SpaceObject {
    public Star(string name, double radius) : base(name, (int)radius){}

        public override void Draw()
        {
            Console.Write("Star : ");
            base.Draw();
        }
    }

 public class Planet : SpaceObject{
        public Planet(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod) { }
        public override void Draw()
        {
            Console.Write("Planet : ");
            base.Draw();
        }
    }

    public class Moon : Planet{
        public Moon(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color) { }
        public override void Draw()
        {
            Console.Write("Moon : ");
            base.Draw();
        }
    }
   public class DwarfPlanet : SpaceObject{
        public DwarfPlanet(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod) { }

        public override void Draw()
        {
            Console.Write("Dwarfplanet : ");
            base.Draw();
        }
    }

    public class SolarSystem
    {   
        public string Name { get; set; }

        public Star Sun { get; set; }
        public List<SpaceObject> SpaceObjects { get; set; }
        public SolarSystem(string name, Star sun)
        {
            Name = name;
            Sun = sun;
            SpaceObjects = new List<SpaceObject>();
        }
        public void AddSpaceObject(SpaceObject spaceObject)
        {
            SpaceObjects.Add(spaceObject);
        }
        public void Draw()
        {
            foreach (var spaceObject in SpaceObjects)
            {
                spaceObject.Draw();
            }
        }
    }


}