using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TaskSix_Paint {
    public class FactoryShape {
        
        public Shape createShape(int x, int y, string shape)
        {
            switch (shape) {

                case "Circle":
                    // (Brush, x, y , r)
                    return new Circle(new Brush(Form1.curColor, Form1.brush_w), x, y, Form1.STD_SHAPE_SIZE);

                case "Polygon":
                    // (Brush, x, y , r)
                    return new Polygon(new Brush(Form1.curColor, Form1.brush_w), x, y, Form1.starVertexCount);
                case "GummyObject":
                    return null;//new GummyObject(new Brush(Form1.curColor, 5), x, y, Form1.STD_SHAPE_SIZE);

                case "Star":
                    if (Form1.starVertexCount>4 && Form1.starVertexCount%2==1 )
                        return new RegularStar(new Brush(Form1.curColor, Form1.brush_w), x, y, Form1.starVertexCount);
                    else return null;
                default:
                    return null;
            }
        }

        public Shape createShape(string shape)
        {
            switch (shape) {

                case "Circle":
                    // (Brush, x, y , r)
                    return new Circle();

                case "Polygon":
                    // (Brush, x, y , r)
                    return new Polygon();

                case "RegularStar":
                    return new RegularStar();
                case "ShapeGroup":
                    return new ShapeGroup();
                default:
                    return null;
            }
        }
        public Shape createShape(string shape, Iterator<Shape> _iter)
        {
             return new GummyObject(_iter);
        }

    }
}
