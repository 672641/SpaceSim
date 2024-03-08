using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CoreTelephony;
using Microsoft.Maui.Graphics;

using Spacelib;
namespace MAUI {

    internal class SpaceGraphics : IDrawable {

        public GraphicalSolarSystem SolarSystem { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {   
            
            Point center = new(dirtyRect.Width / 2, dirtyRect.Height / 2);

            InitSolarSystem(center.X, center.Y);
            SolarSystem?.Draw(canvas);

        }

        public void InitSolarSystem(double centreX, double centerY){
            SolarSystem = new GraphicalSolarSystem("Solar System", new Star("Sun", 40), centreX, centerY);
            SolarSystem.Planets.Add(new GraphicalPlanet("Mercury", 100, 2.4397, 87.97, 100, Colors.Gray, 5, new Point(centreX + 100, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Venus", 150, 6.0518, 224.70, 200, Colors.Orange, 10, new Point(centreX + 150, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Earth", 200, 6.371, 365.2, 300, Colors.Blue, 10, new Point(centreX + 200, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Mars", 250, 3.3895, 686.98, 400, Colors.Red, 7, new Point(centreX + 250, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Jupiter", 300, 69.911, 4333, 500, Colors.Brown, 15, new Point(centreX + 300, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Saturn", 350, 58.232, 10755.7, 600, Colors.Yellow, 12, new Point(centreX + 350, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Uranus", 400, 25.362, 30687, 700, Colors.LightBlue, 10, new Point(centreX + 400, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Neptune", 450, 24.622, 60195 , 800, Colors.DarkBlue, 10, new Point(centreX + 450, centerY)));
            SolarSystem.Planets.Add(new GraphicalPlanet("Pluto", 500, 1.151, 90560, 900, Colors.Gray, 3, new Point(centreX + 500, centerY)));

            SolarSystem.Planets[2].AddMoon(new GraphicalMoon("Moon", 500, 50, 27.3, 27.3, Colors.Gray, 50, new Point(centreX + 200, centerY - 20)));
            SolarSystem.Planets[3].AddMoon(new GraphicalMoon("Phobos", 500, 50, 27.3, 27.3, Colors.Gray, 50, new Point(centreX + 250, centerY - 20)));
            SolarSystem.Planets[3].AddMoon(new GraphicalMoon("Deimos", 600, 50, 27.3, 27.3, Colors.Gray, 50, new Point(centreX + 250, centerY + 20)));
            SolarSystem.Planets[4].AddMoon(new GraphicalMoon("Io", 500, 50, 27.3, 27.3, Colors.Gray, 50, new Point(centreX + 300, centerY - 20)));
            SolarSystem.Planets[4].AddMoon(new GraphicalMoon("Europa", 650, 50, 40, 27.3, Colors.Gray, 50, new Point(centreX + 300, centerY + 20)));
        }


        public class GraphicalSolarSystem : SolarSystem {

            public new GraphicalStar Sun { get; set; }

            public List<GraphicalPlanet> Planets { get; set; }
            public static bool ShowOrbits { get; set; } = true;

            public  static bool ShowNames { get; set; } = true;

            public static bool IsZoomedIn { get; set; }

            public static GraphicalPlanet? SelectedPlanet { get; set; }


            public static int Time { get; set; } = 0;

            public GraphicalSolarSystem(string name, Star sun, double drawingX, double drawingY) : base(name, sun) { 
                Sun = new GraphicalStar(sun.Name, sun.Radius, (int)sun.Radius, Colors.Yellow, new Point(drawingX, drawingY));
                Planets = [];
            }

            public void DoTick()
            {
                Time++;
            }

            public void ZoomIn(GraphicalPlanet planet)
            {
                IsZoomedIn = true;
                SelectedPlanet = planet;
            }
            
            public void ZoomOut()
            {
                IsZoomedIn = false;
                SelectedPlanet = null;
            }

            public void ToggleOnOrbit()
            {
                ShowOrbits = true;
            }

            public void ToggleOffOrbit()
            {
                ShowOrbits = false;
            }

            public void ToggleOnNames()
            {
                ShowNames = true;
            }

            public void ToggleOffNames()
            {
                ShowNames = false;
            }

            public void DrawNames(ICanvas canvas)
            {
                if (ShowNames){
                    foreach (var planet in Planets)
                    {
                        canvas.FontColor = Colors.White;
                        canvas.DrawString(planet.Name, (float)planet.DrawingPosition.X-25, (float)planet.DrawingPosition.Y,50,50, HorizontalAlignment.Center, VerticalAlignment.Center);
                    }
                }
                else 
                return;
            }

            public void DrawOrbits(ICanvas canvas)
            {
                if (ShowOrbits){
                    foreach (var planet in Planets)
                    {
                        canvas.StrokeColor = Colors.LightGray;
                        canvas.DrawCircle((float)Sun.DrawingPosition.X, (float)Sun.DrawingPosition.Y, (float)planet.OrbitalRadius);
                    }
                }
                else 
                return;
            }

            public void CalculatePositions()
            {
                foreach (var planet in Planets)
                {
                    planet.CalculatePosition(Time, Sun.DrawingPosition);
                }
            }

            public void Draw(ICanvas canvas)
            {
                if (IsZoomedIn)
                {
                    DrawZoomedInPlanet(canvas);
                    DrawInformation(canvas);
                }

                else
                {
                    Sun.Draw(canvas);
                    DrawOrbits(canvas);
                    CalculatePositions();
                    DrawNames(canvas);
                    foreach (var planet in Planets)
                    {   
                        planet.Draw(canvas);
                    }
                }
            }

            public void DrawZoomedInPlanet(ICanvas canvas)
            {
                SelectedPlanet.DrawingSize = 300;
                SelectedPlanet.DrawingPosition = new Point(400, 600);
                SelectedPlanet.Draw(canvas);
                
                foreach (var moon in SelectedPlanet.Moons)
                {
                    //Draw orbit
                    if (ShowOrbits){
                        canvas.StrokeColor = Colors.LightGray;
                        canvas.DrawCircle((float)SelectedPlanet.DrawingPosition.X, (float)SelectedPlanet.DrawingPosition.Y, (float)moon.OrbitalRadius);
                    }
                    if (ShowNames){
                        canvas.FontColor = Colors.White;
                        canvas.FontSize = 20;
                        canvas.DrawString(moon.Name, (float)moon.DrawingPosition.X-25, (float)moon.DrawingPosition.Y,1000,50, HorizontalAlignment.Left, VerticalAlignment.Center);
                    }
                    
                    moon.CalculatePosition(Time, SelectedPlanet.DrawingPosition);
                    moon.Draw(canvas);
                }
            }

            public void DrawInformation(ICanvas canvas)
            {
                if (IsZoomedIn)
                {
                    int infoY = 0;
                    canvas.FontColor = Colors.White;
                    string planetInfo = SelectedPlanet.Name +": \n"+ "Orbital Radius: " + SelectedPlanet.OrbitalRadius + " km\n" + "Orbital Period: " + SelectedPlanet.OrbitalPeriod + " days\n" + "Radius: " + SelectedPlanet.Radius + " km\n" + "Rotational Period: " + SelectedPlanet.RotationalPeriod + " hours";
                    canvas.DrawString(planetInfo, 1100, infoY, 1000, 1000, HorizontalAlignment.Left, VerticalAlignment.Top);
                    
                    foreach (var moon in SelectedPlanet.Moons)
                    {
                        infoY += 200;
                        string moonInfo = moon.Name +": \n"+ "Orbital Radius: " + moon.OrbitalRadius + " km\n" + "Orbital Period: " + moon.OrbitalPeriod + " days\n" + "Radius: " + moon.Radius + " km\n" + "Rotational Period: " + moon.RotationalPeriod + " hours";
                        canvas.DrawString(moonInfo, 1100, infoY, 1000, 1000, HorizontalAlignment.Left, VerticalAlignment.Top);
                    }
                }
            }
        }


        public class GraphicalSpaceObject : SpaceObject
        {
            public int DrawingSize { get; set; }

            public Point DrawingPosition { get; set; }
            public Color Color { get; set; }
            public GraphicalSpaceObject(string name, double orbitalRadius, double orbitalPeriod, double radius, double rotationalPeriod,int drawingSize, Color color) : base(name, orbitalRadius, orbitalPeriod, radius, rotationalPeriod) {
                DrawingSize = drawingSize;
                Color = color;
                DrawingPosition = new Point(0, 0);
             }

             public virtual void CalculatePosition(int time, Point center){
                double radian = 2 * Math.PI * time / OrbitalPeriod;
                double x = center.X + OrbitalRadius * Math.Cos(radian);
                double y = center.Y + OrbitalRadius * Math.Sin(radian);
                DrawingPosition = new Point(x, y);
             }
            public virtual void Draw(ICanvas canvas)
            {
                canvas.FillColor = Color;
                canvas.FillCircle((float)DrawingPosition.X, (float)DrawingPosition.Y, DrawingSize);
            }  
        }

        public class GraphicalStar : GraphicalSpaceObject
        {
            public GraphicalStar(string name, double radius, int drawingSize, Color color, Point drawingPosition) : base(name, 0, 0, radius, 0, drawingSize, color) { 
                DrawingPosition = drawingPosition;
            }

            public override void Draw(ICanvas canvas)
            {
                canvas.FillColor = Color;
                canvas.FillCircle((float)DrawingPosition.X, (float)DrawingPosition.Y, DrawingSize);
            }
        }

        public class GraphicalPlanet : GraphicalSpaceObject
        {
            public List<GraphicalMoon> Moons { get; set; } = new List<GraphicalMoon>();
            public GraphicalPlanet(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, int drawingSize, Point drawingPosition) : base(name, orbitalRadius, orbitalPeriod, radius, rotationalPeriod, drawingSize, color) {
                DrawingPosition = drawingPosition;
            }

            public void AddMoon(GraphicalMoon moon)
            {
                Moons.Add(moon);
            }

            public override void Draw(ICanvas canvas)
            {
                canvas.FillColor = Color;
                canvas.FillCircle((float)DrawingPosition.X, (float)DrawingPosition.Y, DrawingSize);
            }
        }

        public class GraphicalMoon : GraphicalPlanet
        {
            public GraphicalMoon(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, int drawingSize, Point drawingPosition) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, drawingSize, drawingPosition) { }
        
            public override void Draw(ICanvas canvas)
            {
                canvas.FillColor = Color;
                canvas.FillCircle((float)DrawingPosition.X, (float)DrawingPosition.Y, DrawingSize);
            }       
        }
    }
}