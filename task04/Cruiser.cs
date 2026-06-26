using System;

namespace task04
{
    public class Cruiser : ISpaceship
    {
        private int _angle;
        public int Speed => 50;
        public int FirePower => 100;
        public void MoveForward()
        {
            Console.WriteLine($"Крейсер движется вперед со скоростью {Speed}");
        }
        public void Rotate(int angle)
        {
            _angle = (_angle + angle) % 360;
            Console.WriteLine($"Крейсер повернут на угол {_angle} градусов");
        }
        public void Fire()
        {
            Console.WriteLine($"Крейсер стреляет мощными ракетами! Мощность: {FirePower}");
        }
    }
}