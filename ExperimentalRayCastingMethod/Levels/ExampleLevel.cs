using System;
using System.Collections.Generic;
using Harder;
using Microsoft.Xna.Framework;
using The_E_Project.Entities;

namespace The_E_Project.Levels
{
    public class ExampleLevel : BaseLevel
    {
        public ExampleLevel()
        {
            //objects
            Entities = new List<BaseEntity>();

            //does not have moving objects
            Moving = false; //make this into the constructor

            //creating rectangle for sprite
            var rect = new Rectangle(0, 0, 70, Activity1.SizeOfDeviceY);
            //creating physical aspect for object
            var poly1 = Utility.RectToPolygon(rect);
            //adding sprite and physical to new BaseEntity object
            var p1 = new BaseEntity(rect, poly1);

            rect = new Rectangle(Activity1.SizeOfDeviceX - 70, 0, 70, Activity1.SizeOfDeviceY);
            var poly2 = Utility.RectToPolygon(rect);
            var p2 = new BaseEntity(rect, poly2);

            rect = new Rectangle(Activity1.SizeOfDeviceX/2 - 150, Activity1.SizeOfDeviceY/2 - 200, 300, 50);
            var poly3 = Utility.RectToPolygon(rect);
            var p3 = new BaseEntity(rect, poly3);

            rect = new Rectangle(Activity1.SizeOfDeviceX/2 - 150, Activity1.SizeOfDeviceY/2 + 200, 300, 50);
            var poly4 = Utility.RectToPolygon(rect);
            var p4 = new BaseEntity(rect, poly4);

            //test with animations
            var poly5 = new Polygon();
            poly5.AddPoint(new Point(300, 300));
            poly5.AddPoint(new Point(550, 300));
            poly5.AddPoint(new Point(300, 550));
            var helicopter = new BaseEntity(new Rectangle(300, 300, 100, 100), poly5);

            var spin = new Animation.Animation();

            for (byte k = 0; k < 13; k++) //can use sizeOfSprite here if all animations are same size
                spin.AddFrame(new Rectangle(k * 62, 0, 62, 32), TimeSpan.FromSeconds(0.0384615));

            helicopter.AddAnimation(spin, "TestBullets");

            //to here

            //this doesnt work

            //for for objective box thing.
            rect = new Rectangle(170, 100, 100, 100);
            var objectiveBox = new FinalGoal(rect, Utility.RectToPolygon(rect), "objectiveBox", 0);




            //--------------//
            Entities.Add(p1);
            Entities.Add(p2);
            Entities.Add(p3);
            Entities.Add(p4);
            //Entities.Add(helicopter);
            Entities.Add(objectiveBox);
            //Entities.Add(objectiveCircle);
            //--------------//

            foreach (var terrain in Entities)
                terrain.Polygon.BuildEdges();
        }
    }
}