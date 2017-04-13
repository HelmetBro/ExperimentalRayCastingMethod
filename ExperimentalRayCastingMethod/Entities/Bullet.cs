using System;
using System.Collections.Generic;
using System.Drawing;
using Harder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace The_E_Project.Entities
{
    public class Bullet
    {
        private const float Speed = 350;
        //private const int _maxBounces = 20;
        private int _currentBounces;
        private Vector2 _velocity;
        private readonly Visual _visual;

        private Size _bulletSize;

        private bool HitObj;
        private bool IsActive;

        private float HitboxAngle;

        public Vector2 Position;
        private readonly bool _levelHasMovingParts;

        private bool HitProjectedPoint;

        private List<Vector2> projectedEdges;
        private List<Vector2> projectedPoints;

        public Bullet(GraphicsDevice graphicsDevice, Vector2 startingPosition, float angle, string textureName, bool levelHasMovingParts)
        {
            HitboxAngle = -angle;

            _bulletSize = new Size(62, 32);

            //TestBullets
            _visual = new Visual
            {
                TextureName = textureName,
                Animations = new List<Animation.Animation>()
            };

            //remember to close the stream when deleting the bullet
            if (_visual.Texture == null)
            {
                using (var stream = TitleContainer.OpenStream("Content/" + textureName + ".png"))
                {
                    _visual.Texture = Texture2D.FromStream(graphicsDevice, stream);
                }
            }

            var spin = new Animation.Animation();

            for (byte k = 0; k < 13; k++) //can use sizeOfSprite here if all animations are same size
                spin.AddFrame(new Rectangle(k*_bulletSize.Width, 0, _bulletSize.Width, _bulletSize.Height),
                    TimeSpan.FromSeconds(0.0384615));

            _visual.IdleAnimation = spin;
            _visual.CurrentAnimation = _visual.IdleAnimation;

            IsActive = true; //add breakpoint here
            _currentBounces = 0;
            Position = startingPosition;
            _levelHasMovingParts = levelHasMovingParts;
            _visual.Angle = angle;

            _velocity = GetDesiredVelocityFromAngle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _visual.SourceRectangle = _visual.CurrentAnimation.CurrentRectangle;

            spriteBatch.Draw(_visual.Texture, Position, _visual.SourceRectangle, _visual.TintColor,
                _visual.Angle, new Vector2(_bulletSize.Width/2, _bulletSize.Height/2), _visual.Scale,
                _visual.Effects, _visual.Layer);
        }

        private Vector2 GetDesiredVelocityFromAngle()
        {
            var desiredVelocity = new Vector2();

            desiredVelocity.Normalize();

            var xVelocity = (float) Math.Cos(_visual.Angle) * Speed;
            var yVelocity = (float) Math.Sin(_visual.Angle) * Speed;

            desiredVelocity = new Vector2(xVelocity, yVelocity);

            return desiredVelocity;
        }

        private void MoveBulletPosition(GameTime gameTime)
        {
            //Position += new Vector2(_velocity.X*(float) gameTime.ElapsedGameTime.TotalSeconds,
            //    _velocity.Y*(float) gameTime.ElapsedGameTime.TotalSeconds);

            Position.X += _velocity.X*(float) gameTime.ElapsedGameTime.TotalSeconds;
            Position.Y += _velocity.Y*(float) gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        private static bool IsBetween(float min, float max, float targetAngle)
        {
            if (max - min >= Math.PI)
                return false;

            return min <= targetAngle && targetAngle <= max;
        }

        public static float NormalizeAngle(float targetAngle)
        {
            return targetAngle > 0 ? targetAngle : 2 * (float)Math.PI + targetAngle;
        }

        private Vector2 FindWhereProjectileHits(out Vector2 closestInterPoint, out float distanceToNextPoint)
        {
            bool goingToHitAnything = false;

            //http://stackoverflow.com/questions/4543506/algorithm-for-intersection-of-2-lines
            //https://www.topcoder.com/community/data-science/data-science-tutorials/geometry-concepts-line-intersection-and-its-applications/

            //for now, iterates over every polygon. not efficient

            var intersectionPoints = new List<Vector2>(); //used as a closestInterPoint
            var validEdges = new List<Vector2>(); //used as a vector

            foreach (var terrain in Run.currentLevel.Entities)
            {
                for (var i = 0; i < terrain.Polygon.Edges.Count; i++)
                {
                    var currentPoint = terrain.Polygon.Points[i];
                    var nextPoint = i + 1 > terrain.Polygon.Edges.Count - 1 ? terrain.Polygon.Points[0] : terrain.Polygon.Points[i + 1];

                    //finds the angle between current position and current closestInterPoint
                    var xLength = currentPoint.X - Position.X;
                    var yLength = currentPoint.Y - Position.Y;
                    var angle1 = NormalizeAngle(-(float)Math.Atan2(yLength, xLength));

                    //finds the angle between current position and next closestInterPoint
                    xLength = nextPoint.X - Position.X;
                    yLength = nextPoint.Y - Position.Y;
                    var angle2 = NormalizeAngle(-(float)Math.Atan2(yLength, xLength));

                    if (angle1 < angle2
                        ? !IsBetween(angle1, angle2, NormalizeAngle(HitboxAngle))
                        : !IsBetween(angle2, angle1, NormalizeAngle(HitboxAngle)))
                        continue;
                    
                    goingToHitAnything = true;

                    //if it reaches here, then currentPoint and nextPoint is a valid edge.
                     validEdges.Add(currentPoint.ToVector2() - nextPoint.ToVector2());

                    //line1 (points of the edges made into a line)
                    float A1;
                    float B1;
                    float C1;

                    Utility.LineFromTwoPoints(currentPoint, nextPoint, out A1, out B1, out C1);

                    //line2 (line from position and angle), first to find point on line according to angle
                    //use FastDistance instead of DistanceTo in the future
                    var pointForNewLine = new Vector2((float)Math.Sin(HitboxAngle), (float)Math.Cos(HitboxAngle)) + Position;

                    //Utility.DistanceTo(Position.ToPoint(), currentPoint)

                    float A2;
                    float B2;
                    float C2;

                    Utility.LineFromTwoPoints(Position.ToPoint(), pointForNewLine.ToPoint(), out A2, out B2, out C2);

                    //adds the intersection closestInterPoint to the array
                    var pseudoPoint = Utility.IntersectionPoint(A1, B1, C1, A2, B2, C2);
                    intersectionPoints.Add(new Vector2(pseudoPoint.X, Activity1.SizeOfDeviceY - pseudoPoint.Y));
                } //for edge loop
            }//foreach polygon loop

            if (!goingToHitAnything)
            {
                //add somthing here, make bullet travel forever in that direction or smth idk later tho
                //bullet travels to end of screen then deletes itself. screensize*2 or screensize + bullet length idk
                closestInterPoint = new Vector2();
                distanceToNextPoint = 0;
                return new Vector2();
            }

            //loop through all intersection points and find closest one
            Vector2 closestVector2 = new Vector2(); //used as a point
            distanceToNextPoint = float.PositiveInfinity;

            //used to locate both the edge and intersection point
            var k = 0;
            for (; k < intersectionPoints.Count - 1; k++)
            {
                var newDistance = Utility.FastDistance(Position, intersectionPoints[k]);

                if (!(newDistance < distanceToNextPoint))
                    continue;

                closestVector2 = intersectionPoints[k];
                distanceToNextPoint = newDistance;
            }

            closestInterPoint = closestVector2;

            return validEdges[k];

        }//method

        private static int GetVerticiesAndCount(Polygon polyEntity, out float[] allPolyX, out float[] allPolyY)
        {
            var countOfVerticies = polyEntity.Points.Count;
            allPolyX = new float[countOfVerticies];
            allPolyY = new float[countOfVerticies];

            for (var i = 0; i < countOfVerticies; i++)
            {
                allPolyX[i] = polyEntity.Points[i].X;
                allPolyY[i] = polyEntity.Points[i].Y;
            }

            return countOfVerticies;

        }

        private static bool PolyCollide(int nvert, IReadOnlyList<float> vertx, IReadOnlyList<float> verty, float testx, float testy)
        {
            int i, j;
            var result = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
                if ((verty[i] > testy != verty[j] > testy) &&
                    (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                    result = !result;
            return result;
        }

        private static Vector2 Retrace(GameTime gameTime, Vector2 velocity, Vector2 position,
            int countOfVerticies, IReadOnlyList<float> allPolyX, IReadOnlyList<float> allPolyY, Polygon polyEntity)
        {
            velocity.Normalize(); // vel already be normalized here, idk check later

            while (PolyCollide(countOfVerticies, allPolyX, allPolyY, position.X, position.Y))
            {
                position += -velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            return WhatEdgeDidItHit(polyEntity, position);
        }

        private static Vector2 WhatEdgeDidItHit(Polygon poly, Vector2 position)
        {
            var hitEdge = false;
            byte tolerance = 0;
            var edgeVectors = new List<Vector2>();

            while (!hitEdge)
            {
                tolerance++;

                for (var i = 0; i < poly.Edges.Count; i++)
                {
                    var currentPoint = poly.Points[i];

                    var nextPoint = i + 1 > poly.Edges.Count - 1 ? poly.Points[0] : poly.Points[i + 1];

                    if (Math.Abs(Utility.DistanceTo(currentPoint, position.ToPoint()) +
                                 Utility.DistanceTo(position.ToPoint(), nextPoint) -
                                 Utility.DistanceTo(currentPoint, nextPoint)) < tolerance)
                    {
                        edgeVectors.Add(Utility.Vector2BetweenTwoPoints(currentPoint, nextPoint));
                        hitEdge = true;
                    }
                } //for loop
            } //while loop

            return Utility.Vector2BetweenMultipleVectors(edgeVectors.ToArray());
        }

        private Vector2 MoveTheoreticalBullet(float distanceToNextPoint)
        {

            var time = 10/distanceToNextPoint;
            //var time = Speed/distanceToNextPoint;
            return new Vector2();

        }

        public void Update(GameTime gameTime)
        {
            if (!_levelHasMovingParts)
            {
                foreach (var terrain in Run.currentLevel.Entities)
                {
                    float[] allPolyX;
                    float[] allPolyY;

                    var countOfVerticies = GetVerticiesAndCount(terrain.Polygon, out allPolyX, out allPolyY);

                    if (PolyCollide(countOfVerticies, allPolyX, allPolyY, Position.X, Position.Y))
                    {
                        var theChosenEdge = Retrace(gameTime, _velocity, Position, countOfVerticies, allPolyX, allPolyY, terrain.Polygon);
                        theChosenEdge.Normalize();

                        _velocity = Vector2.Reflect(_velocity, new Vector2(-theChosenEdge.Y, theChosenEdge.X));

                        _visual.Angle = (float)Math.Atan2(_velocity.Y, _velocity.X);

                        _currentBounces++;
                    }

                    while (PolyCollide(countOfVerticies, allPolyX, allPolyY, Position.X, Position.Y))
                        MoveBulletPosition(gameTime);

                    MoveBulletPosition(gameTime);

                    //later combine while and if statments with a do-while or smth

                }//foreach
            }
            else
            {
                //work on this after state changer + settings.
                //after traveling to next point, if it is currently in a polygon, move until it is out...probably gonna have to steal from there ^
                //might not need ^ after i'm done with this.

                //this is faulted for now
                if (!HitProjectedPoint)
                {
                    //stores all the edges it hits and the distances to the previous points
                    float totalDistance = 0;

                    projectedPoints = new List<Vector2>();
                    projectedEdges = new List<Vector2>();

                    var shortDistances = new List<float>();
                    var currentProjectedPoint = new Vector2();

                    while (totalDistance < Speed)
                    {
                        float distanceToNextPoint;

                        projectedEdges.Add(FindWhereProjectileHits(out currentProjectedPoint, out distanceToNextPoint));
                        projectedPoints.Add(currentProjectedPoint);
                        shortDistances.Add(distanceToNextPoint);

                        totalDistance += distanceToNextPoint;
                    }



                }



                /*
                if ()
                {

                }


                theChosenEdge.Normalize();

                MoveTheoreticalBullet(distanceToNextPoint);

                _velocity = Vector2.Reflect(_velocity, new Vector2(-theChosenEdge.Y, theChosenEdge.X));

                _visual.Angle = (float)Math.Atan2(_velocity.Y, _velocity.X);
                */



            }





















            /*
        Can use if I want to separate idle animation and touching down animation(when sniper aims)


        if (velocity != Vector2.Zero)
        {
            bool movingHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
            if (movingHorizontally)
            {
                if (velocity.X > 0)
                {
                    currentAnimation = walkRight;
                }
                else
                {
                    currentAnimation = walkLeft;
                }
            }
            else
            {
                if (velocity.Y > 0)
                {
                    currentAnimation = walkDown;
                }
                else
                {
                    currentAnimation = walkUp;
                }
            }
        }
        else
        {
            // If the character was walking, we can set the standing animation
            // according to the walking animation that is playing:
            if (currentAnimation == walkRight)
            {
                currentAnimation = standRight;
            }
            else if (currentAnimation == walkLeft)
            {
                currentAnimation = standLeft;
            }
            else if (currentAnimation == walkUp)
            {
                currentAnimation = standUp;
            }
            else if (currentAnimation == walkDown)
            {
                currentAnimation = standDown;
            }
            else if (currentAnimation == null)
            {
                currentAnimation = standDown;
            }

            // if none of the above code hit then the character
            // is already standing, so no need to change the animation.
        }
         */
            _visual.Update(gameTime);
        }
    }
}