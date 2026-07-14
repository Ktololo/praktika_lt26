namespace task04
{
    public class Fighter : ISpaceship
    {
        private int _angle;

        public int Speed => 100;
        public int FirePower => 50;

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
