using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace TaskSix_Paint {
    public class GummyObject : VShape, Observable{

        List<Observer> observers;
        //Container<Shape> cont;
        Iterator<Shape> iter;

        public int m_dx = 0, m_dy = 0;

        public GummyObject(Iterator<Shape> _iter) :base(new Brush(Color.Black, 1),0,0)
        {
            R = 0;
            brush.getPen().DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            observers = new List<Observer>();

            iter = _iter;
        }
        public GummyObject(Brush b, int x, int y, int _r, Iterator<Shape> _iter) : base(b, x, y)
        {
            R = _r;
            brush.getPen().DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            observers = new List<Observer>();
       
            iter = _iter;
        }
        
        public override string className()
        {
            return "GummyObject ";
        }

        public override string toString()
        {
            return className() + base.toString();
        }
        public override void save(StreamWriter sw)
        {
            sw.WriteLine(toString());
        }
        
        protected override void notifyXYchanged()
        {
            for (int i = 0; i < observers.Count(); i++) {
                if (!((VShape)observers[i]).isObserver) observers.Clear();
            }
            for (iter.begin(); !iter.eot(); iter.next()) {
                if (iter.getVal() != this && (iter.getVal().collisionEnter((int)(x - R), (int)(y - R)) ||
                    iter.getVal().collisionEnter((int)(x - R), (int)(y + R)) ||
                    iter.getVal().collisionEnter((int)(x + R), (int)(y - R)) ||
                    iter.getVal().collisionEnter((int)(x + R), (int)(y + R)))) {
                    // проверить на "уникальность", чтобы не подписать одну и ту же фигуру
                    if (!observers.Contains(iter.getVal())) {
                        addObserver(iter.getVal());
                    }
                }
            }

            for (int i = 0; i < observers.Count(); i++) {             // цикл по прилипшим объектам
                int _x = ((VShape)observers[i]).x,
                    _y = ((VShape)observers[i]).y,
                    _r = (int)((VShape)observers[i]).R;
                for (iter.begin(); !iter.eot(); iter.next()) {      // проверим на коллиции с ними
                    if (iter.getVal() != this && iter.getVal() != (VShape)observers[i] && 
                        (iter.getVal().collisionEnter((int)(_x - _r), (int)(_y - _r)) ||
                        iter.getVal().collisionEnter((int)(_x - _r), (int)(_y + _r)) ||
                        iter.getVal().collisionEnter((int)(_x + _r), (int)(_y - _r)) ||
                        iter.getVal().collisionEnter((int)(_x + _r), (int)(_y + _r)))) {
                        // проверить на "уникальность", чтобы не подписать одну и ту же фигуру
                        if (!observers.Contains(iter.getVal())) {
                            addObserver(iter.getVal());
                        }
                    }
                }
            }
           
            notifyAll();
        }
        public override void load(StreamReader sr)
        {
            string[] data = sr.ReadLine().Split();
            brush.setColor(System.Drawing.ColorTranslator.FromHtml(data[0]));
            brush.setBrushW(Int32.Parse(data[1]));
            x = Int32.Parse(data[2]);
            y = Int32.Parse(data[3]);
            R = Double.Parse(data[4]);
            angle = float.Parse(data[5]);

        }

        public void notifyAll()
        {
            for(int i = 0; i < observers.Count(); i++) {
                observers[i].onSubjectChanged(this);
            }
        }
        public override void draw(Graphics gr)
        {
            gr.DrawEllipse(brush.getPen(), (float)(x - R), (float)(y - R), (float)(R * 2), (float)(R * 2));
            
        }
        public override void rotation(float k)
        {
            // none
        }
        public override void changeSize(int k)
        {
            /*
            for (int i = 0; i < observers.Count(); i++) {
                int _x = ((VShape)observers[i]).x,
                    _y = ((VShape)observers[i]).y, dx = 0, dy = 0;
                if (_x > x) dx = Form1.STEP;
                if (_x < x) dx = -Form1.STEP;
                if (_y > y) dy = Form1.STEP;
                if (_y < y) dy = -Form1.STEP;
                if(k < 0) { dx *= -1; dy *= -1; }
                if (!((VShape)observers[i]).canMove(dx, dy)) return;
            }
            
            for (int i = 0; i < observers.Count(); i++) {
                int _x = ((VShape)observers[i]).x,
                     _y = ((VShape)observers[i]).y, dx = 0, dy = 0;
      
                double alpha= Math.Atan2(_y - y, _x - x);
                if (alpha < 0) alpha += 180;

                    dx = x+(int)((Math.Sqrt(((_x - x) * (_x - x) + (_y - y) * (_y - y))) + k) * Math.Cos(alpha));
                    dy = y+(int)((Math.Sqrt(((_x - x) * (_x - x) + (_y - y) * (_y - y))) + k) * Math.Sin(alpha));
              
                
                //if (k < 0) { dx *= -1; dy *= -1; }
                ((VShape)observers[i]).hide(Form1.gr);
                ((VShape)observers[i]).x = dx;
                ((VShape)observers[i]).y = dy;
                // ((VShape)observers[i]).move(((VShape)observers[i]).x - dx, ((VShape)observers[i]).y - dy, true);
                ((VShape)observers[i]).show(Form1.gr);
            }*/
            m_dx = 0;
            m_dy = 0;
            base.changeSize(k);
            notifyXYchanged();
        }
        public override void move(int _dx, int _dy)
        {

            if ((x + R + _dx < Form1.WINDOW_WIDTH) &&
                (x + _dx > 145) &&
                (y + R + _dy < Form1.WINDOW_HEIGHT) &&
                (y + _dy > 30)) {

                for (int i = 0; i < observers.Count(); i++) {
                    if (!((VShape)observers[i]).canMove(_dx, _dy)) return;
                }

                x += _dx;
                y += _dy;

                m_dx = _dx;
                m_dy = _dy;
                notifyXYchanged();
            }            
        }

        public void addObserver(Observer obj)
        {
            observers.Add(obj);
        }
    }
}
