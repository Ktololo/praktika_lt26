namespace task04
{
    public class Cruiser : ISpaceship
    {
        private int _angle;

        public int Speed => 50;
        public int FirePower => 100;

        public void MoveForward()
        {

        }

        public void Rotate(int angle)
        {
            _angle = (_angle + angle) % 360;
        }

        public void Fire()
        {

        }
    }
}
