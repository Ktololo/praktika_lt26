using System;

namespace task04
{
    public class Fighter : ISpaceship
    {
        private int _angle;
        public int Speed => 100;
        public int FirePower => 50;

        public void MoveForward()
        {
            Console.WriteLine($"Истребитель движется вперед со скоростью {Speed}");
        }
        public void Rotate(int angle)
        {
            _angle = (_angle + angle) % 360;
            Console.WriteLine($"Истребитель повернут на угол {_angle} гр");
        }
        public void Fire()
        {
            Console.WriteLine($"Истребитель стреляет легкими ракетами, Мощность: {FirePower}");
        }
    }
}